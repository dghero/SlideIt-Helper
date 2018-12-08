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
			if (this.LocalThumbPath == null)
			{
				this.LocalThumbPath = this.ApiDownloadImg(this.ThumbImgUrl);
			}
			return this.LocalThumbPath;
		}
		public string DownloadFullTempFile()
		{
			if(this.LocalFullPath == null)
			{
				this.LocalFullPath = this.ApiDownloadImg(this.FullImgUrl);
			}
			return this.LocalFullPath;
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
