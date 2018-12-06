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

namespace SlideItHelper
{
	/// <summary>
	/// Interaction logic for AddTextInput.xaml
	/// </summary>
	public partial class AddTextInput : Page
	{
		public AddTextInput()
		{
			InitializeComponent();
		}

		private void Next_Page(object sender, RoutedEventArgs e)
		{
			string searchQuery = this.titleText.Text 
				+ " " 
				+ BoldedTextFromRichTextBox(this.contentText);

			AddImagesForm addImagesForm = new AddImagesForm(searchQuery);
			this.NavigationService.Navigate(addImagesForm);
		}

		private string BoldedTextFromRichTextBox(RichTextBox rtb)
		{
			string results = "";
			foreach (Paragraph p in rtb.Document.Blocks)
			{
				foreach (var inline in p.Inlines)
				{
					Debug.WriteLine("Segment:[" + inline + "]");
					if (inline is Bold || inline.FontWeight == FontWeights.Bold)
					{
						TextRange textRange = new TextRange(
							inline.ContentStart,
							inline.ContentEnd
						);
						results += textRange.Text + " ";
					}
				}
			}
			return results;
		}

		/* StringFromRichTextBox()
		 * Plucked from 
		 * https://docs.microsoft.com/en-us/dotnet/framework/wpf/controls/how-to-extract-the-text-content-from-a-richtextbox
		 */
		private string StringFromRichTextBox(RichTextBox rtb)
		{
			TextRange textRange = new TextRange(
				rtb.Document.ContentStart,
				rtb.Document.ContentEnd
			);
			return textRange.Text;
		}
	}
}
