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
using System.Windows.Markup;

namespace SlideItHelper
{
	/// <summary>
	/// Interaction logic for DisplaySlide.xaml
	/// </summary>
	public partial class DisplaySlide : Page
	{

		private class DisplayImage : SearchImage
		{
			public int ImgIdx { get; set; }
			public int ImgHeight { get; set; }
			public void CopyValsFromParent(SearchImage parent)
			{
				this.LocalThumbPath = parent.LocalThumbPath;
				this.LocalFullPath = parent.LocalFullPath;
				//this.Description = parent.Description;
				this.ThumbImgUrl = parent.ThumbImgUrl;
				this.FullImgUrl = parent.FullImgUrl;
				this.Photographer = parent.Photographer;
				//this.PhotographerProfile = parent.PhotographerProfile;
			}
		}

		List<DisplayImage> displayImages;

		public DisplaySlide(string slideTitle, RichTextBox slideContent, List<SearchImage> images)
		{
			InitializeComponent();
			PopulateTitle(slideTitle);
			PopulateContent(slideContent);
			PopulateImages(images);
		}

		public void CleanUpImages()
		{
			foreach (DisplayImage img in this.displayImages)
			{
				img.DeleteFullTempFile();
			}
		}

		private void PopulateTitle(string titleText)
		{
			displayTitle.Content = titleText;
		}

		private void PopulateContent(RichTextBox slideContent)
		{
			MemoryStream ms = new MemoryStream();
			XamlWriter.Save(slideContent.Document, ms);
			ms.Seek(0, SeekOrigin.Begin);
			displaySlideContent.Document = XamlReader.Load(ms) as FlowDocument;
			displaySlideContent.Document.FontSize = 26;
		}

		private void PopulateImages(List<SearchImage> images)
		{
			displayImages = new List<DisplayImage>();

			int baseHeight = 450;
			for(int i = 0; i < images.Count; i++)
			{
				//imgGrid.RowDefinitions.Add(new RowDefinition());

				images[i].DownloadFullTempFile();
				DisplayImage dispImg = new DisplayImage();
				dispImg.CopyValsFromParent(images[i]);
				dispImg.ImgHeight = baseHeight / images.Count;

				displayImages.Add(dispImg);
			}
			
			stackImages.ItemsSource = displayImages;
			
		}
		
	}
}
