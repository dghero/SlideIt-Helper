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

using Microsoft.Win32;
using System.IO;

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
			JObject jsonObj = GetImgListFromAPI(query);

			JArray jsonArr = (JArray)jsonObj["photos"];
			for (var i = 0; i < jsonArr.Count; i++)
			{
				try
				{
					SearchImage newImg = new SearchImage()
					{
						// When this was with the Unsplash API, the response object was far too large and
						// unwieldy to mock up the object as a class. The response object is much smaller
						// now, but I want to fix this ASAP
						ThumbImgUrl = jsonArr[i]["src"]["tiny"].ToString(), 
						FullImgUrl = jsonArr[i]["src"]["original"].ToString(), 
						Photographer = "Photo by " + jsonArr[i]["photographer"].ToString() + " on Pexels"
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

		private JObject GetImgListFromAPI(string query)
		{
			//Get environment variable from .env file (needs to be in same folder as .exe)
			string accessKey = Environment.GetEnvironmentVariable("PEXELS_ACCESS_KEY");

			JObject jsonObj = new JObject();
			var client = new RestClient("https://api.pexels.com/v1");
			int resultsPerPage = 60;
			string serialQuery = JsonConvert.SerializeObject(query);

			string reqString = 
				"search?query=" + serialQuery
				+ "&per_page=" + resultsPerPage;

			var request = new RestRequest(reqString, Method.GET);
			request.AddHeader("Authorization", accessKey);
				
			////Pixabay API Request
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

			SaveFileDialog sfd = new SaveFileDialog();
			sfd.Filter = "Png Files(*.png)|*.png";

			Nullable<bool> result = sfd.ShowDialog();
			string fileName = "";

			if (result == true)
			{
				fileName = sfd.FileName;
				Size size = new Size(1024, 768);
				RenderTargetBitmap rtb = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Pbgra32);
				displaySlide.Measure(size);
				displaySlide.Arrange(new Rect(size));
				rtb.Render(displaySlide);
				PngBitmapEncoder png = new PngBitmapEncoder();
				png.Frames.Add(BitmapFrame.Create(rtb));
				using (Stream stm = File.Create(fileName))
				{
					png.Save(stm);
				}
			}
			displaySlide.CleanUpImages();
			//this.NavigationService.Navigate(displaySlide);
		}

	}
}
