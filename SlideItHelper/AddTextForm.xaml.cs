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
			//TODO: Extract bold text instead of just plaintext
			string searchTerm = this.titleText.Text 
				+ " " 
				+ StringFromRichTextBox(this.contentText);
			System.Diagnostics.Debug.WriteLine("Result of Next_Page(): " + searchTerm);

			AddImagesForm addImagesForm = new AddImagesForm(searchTerm);
			this.NavigationService.Navigate(addImagesForm);
		}

		/* StringFromRichTextBox()
		 * Plucked from 
		 * https://docs.microsoft.com/en-us/dotnet/framework/wpf/controls/how-to-extract-the-text-content-from-a-richtextbox
		 */
		private string StringFromRichTextBox(RichTextBox rtb)
		{
			TextRange textRange = new TextRange(
				// TextPointer to the start of content in the RichTextBox.
				rtb.Document.ContentStart,
				// TextPointer to the end of content in the RichTextBox.
				rtb.Document.ContentEnd
			);

			// The Text property on a TextRange object returns a string
			// representing the plain text content of the TextRange.
			return textRange.Text;
		}
	}
}
