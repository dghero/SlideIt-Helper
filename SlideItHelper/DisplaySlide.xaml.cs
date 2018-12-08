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
		public DisplaySlide(string slideTitle, RichTextBox slideContent, List<SearchImage> images)
		{
			InitializeComponent();
			PopulateTitle(slideTitle);
			PopulateContent(slideContent);
		}

		private void PopulateTitle(string titleText)
		{
			if(titleText.Length > 0)
			{
				displayTitle.Content = titleText;
			}
			else
			{
				displayTitle.Content = "Slide"; //or do I want to keep it empty?
			}
		}

		private void PopulateContent(RichTextBox slideContent)
		{
			MemoryStream ms = new MemoryStream();
			XamlWriter.Save(slideContent.Document, ms);
			ms.Seek(0, SeekOrigin.Begin);
			displaySlideContent.Document = XamlReader.Load(ms) as FlowDocument;
		}
		
	}
}
