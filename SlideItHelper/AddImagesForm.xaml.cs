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

namespace SlideItHelper
{
	/// <summary>
	/// Interaction logic for AddImagesForm.xaml
	/// </summary>
	public partial class AddImagesForm : Page
	{
		private string search_term;

		public AddImagesForm(string search_term)
		{
			this.search_term = search_term;
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Debug.WriteLine(Environment.GetEnvironmentVariable("UNSPLASH_SECRET_KEY"));
			Debug.WriteLine("Search term: " + this.search_term);

			////Test some API stuff first
			//var client = new RestClient("https://picsum.photos");
			//var request = new RestRequest("200/300/?random", Method.GET);

			//IRestResponse response = client.Execute(request);
			//var content = response.Content; // raw content as string

			//client.DownloadData(request).SaveAs("d:/testing/file.png");

			//Debug.WriteLine(content);


			
		}
	}
}
