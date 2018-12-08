using SlideItHelper.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Diagnostics;

using RestSharp;
using RestSharp.Extensions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

using System.Json;


namespace SlideItHelper
{
	/// <summary>
	/// Interaction logic for AddImagesForm.xaml
	/// </summary>
	public partial class AddImagesForm : Page
	{
		private string slideTitle;
		private RichTextBox slideContent;
		private List<SearchImage> images = new List<SearchImage>();

		public AddImagesForm(string searchTerm, string slideTitle, RichTextBox slideContent)
		{
			this.slideTitle = slideTitle;
			this.slideContent = slideContent;
			InitializeComponent();
			GetImages(searchTerm);
		}

		private void GetImages(string query)
		{
			JObject jsonObj = GetUnsplashImgList(query);

			JArray jsonArr = (JArray)jsonObj["results"];
			for (var i = 0; i < jsonArr.Count; i++)
			{
				try
				{
					/* Add image profile to list. Note that Unsplash's user policy requires crediting
					 * Unsplash, the original photographer, and linking their profile. Unsplash
					 * further requires you to use direct hotlinks for all images, and that all links
					 * to Unsplash use the parameter "?utm_source=your_app_name&utm_medium=referral".
					 * 
					 * See https://medium.com/unsplash/unsplash-api-guidelines-attribution-4d433941d777
					 * for more details.
					 */
					//// TODO: ADD REFERRAL PARAMETER when out of development. Check desired format
					//// for parameter
					SearchImage newImg = new SearchImage()
					{
						Description = jsonObj["results"][i]["description"].ToString(),
						ThumbImgUrl = jsonObj["results"][i]["urls"]["small"].ToString(), // + "?utm_source=SlideIt%20Helper&utm_medium=referral"
						FullImgUrl = jsonObj["results"][i]["urls"]["regular"].ToString(), // + "?utm_source=SlideIt%20Helper&utm_medium=referral"
						Photographer = jsonObj["results"][i]["user"]["name"].ToString(), // + "?utm_source=SlideIt%20Helper&utm_medium=referral"
						PhotographerProfile = jsonObj["results"][i]["user"]["links"]["html"].ToString() // + "?utm_source=SlideIt%20Helper&utm_medium=referral"
					};
					newImg.DownloadThumbTempFile();

					images.Add(newImg);
				}
				catch (Exception err)
				{
					Console.WriteLine("Error adding image to list: " + err.Message);
				}
			}
			imageOptions.ItemsSource = images;
		}

		private JObject GetUnsplashImgList(string query)
		{
			//Get environment variable from .env file (needs to be in same folder as .exe)
			string access_key = Environment.GetEnvironmentVariable("UNSPLASH_ACCESS_KEY");

			JObject jsonObj = new JObject();
			var client = new RestClient("https://api.unsplash.com");
			int resultsPerPage = 3;
			string serialQuery = JsonConvert.SerializeObject(query);
			string reqString = "search/photos?query=" + serialQuery
				+ "&per_page=" + resultsPerPage
				+ "&client_id=" + access_key;
			var request = new RestRequest(reqString, Method.GET);

			////Unsplash API Request
			try
			{
				IRestResponse response = client.Execute(request);
				var content = response.Content;
				jsonObj = JObject.Parse(content);
			}
			catch (Exception err)
			{
				Console.WriteLine("Error fetching images from web: " + err.Message);
			}

			return jsonObj;
		}

		private void Button_Next(object sender, RoutedEventArgs e)
		{
			List<SearchImage> allImages = new List<SearchImage>();
			List<SearchImage> selectedImages = new List<SearchImage>();

			foreach (SearchImage img in imageOptions.Items) 
			{
				img.DeleteThumbTempFile();
			}
			foreach (SearchImage img in imageOptions.SelectedItems)
			{
				selectedImages.Add(img);
			}

			DisplaySlide displaySlide = new DisplaySlide(this.slideTitle, this.slideContent, selectedImages);
			this.NavigationService.Navigate(displaySlide);
		}

	}
}
