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
using System.IO;

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
		private string searchTerm;
		private List<SearchImage> imagePaths = new List<SearchImage>();

		public AddImagesForm(string searchTerm)
		{
			this.searchTerm = searchTerm;
			InitializeComponent();
			GetImages(searchTerm);
		}

		public class SearchImage
		{
			public string LocalThumbPath { get; set; }
			public string Description { get; set; }
			public string FullImgUrl { get; set; }
			public string Photographer { get; set; }
			public string PhotographerProfile { get; set; }
			public override string ToString()
			{
				return this.Description;
			}
		}

		private void GetImages(string query)
		{
			JObject jsonObj = GetUnsplashImgList(query);

			JArray jsonArr = (JArray)jsonObj["results"];
			for (var i = 0; i < jsonArr.Count; i++)
			{
				try
				{
					string url = jsonObj["results"][i]["urls"]["small"].ToString();

					//fetch thumbnail as a displayable file
					string file = TempFile.CreateTmpFile();
					var clientSmall = new RestClient(url);
					var requestSmall = new RestRequest("", Method.GET);
					clientSmall.DownloadData(requestSmall).SaveAs(file);

					/* Add image profile to list. Note that Unsplash's user policy requires crediting
					 * Unsplash, the original photographer, and linking their profile with a referral
					 * parameter "?utm_source=your_app_name&utm_medium=referral"
					 */
					//TODO: ADD REFERRAL PARAMETER when out of development
					imagePaths.Add(new SearchImage()
					{
						LocalThumbPath = file,
						Description = jsonObj["results"][i]["description"].ToString(),
						FullImgUrl = jsonObj["results"][i]["urls"]["regular"].ToString(),
						Photographer = jsonObj["results"][i]["user"]["name"].ToString(),
						PhotographerProfile = jsonObj["results"][i]["user"]["links"]["html"].ToString() // + "?utm_source=SlideIt%20Helper&utm_medium=referral"
					});
				}
				catch (Exception err)
				{
					Console.WriteLine("Error adding image to list: " + err.Message);
				}
			}
			imageSelections.ItemsSource = imagePaths;
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

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			List<SearchImage> allItems = new List<SearchImage>();
			List<SearchImage> selected = new List<SearchImage>();

			foreach(SearchImage item in imageSelections.Items)
			{
				allItems.Add(item);
			}
			foreach(SearchImage item in imageSelections.SelectedItems)
			{
				selected.Add(item);
			}

			imageSelections.ItemsSource = null;
			foreach (SearchImage item in allItems)
			{
				TempFile.DeleteTmpFile(item.LocalThumbPath);
			}

			Debug.WriteLine("with the Resto");
		}

	}
}
