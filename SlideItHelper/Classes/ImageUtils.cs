using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using RestSharp;
using RestSharp.Extensions;

namespace SlideItHelper.Classes
{
	public class SearchImage
	{
		public string LocalThumbPath { get; set; }
		public string LocalFullPath { get; set; }
		public string Description { get; set; }
		public string ThumbImgUrl { get; set; }
		public string FullImgUrl { get; set; }
		public string Photographer { get; set; }
		public string PhotographerProfile { get; set; }
		public override string ToString()
		{
			return this.Description;
		}

		public string DownloadThumbTempFile()
		{
			return this.LocalThumbPath = this.ApiDownloadImg(this.ThumbImgUrl);
			
		}
		public string DownloadFullTempFile()
		{
			return this.LocalFullPath = this.ApiDownloadImg(this.FullImgUrl);
		}

		private string ApiDownloadImg(string url)
		{
			string file = TempFile.CreateTmpFile();
			var clientSmall = new RestClient(url);
			var requestSmall = new RestRequest("", Method.GET);
			clientSmall.DownloadData(requestSmall).SaveAs(file);
			return file;
		}

		public void DeleteThumbTempFile()
		{
			TempFile.DeleteTmpFile(this.LocalThumbPath);
			this.LocalThumbPath = null;
		}
		public void DeleteFullTempFile()
		{
			TempFile.DeleteTmpFile(this.LocalFullPath);
			this.LocalFullPath = null;
		}
	}
	
}
