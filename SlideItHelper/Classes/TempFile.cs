﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace SlideItHelper.Classes
{ 

	class TempFile
	{
		/* TEMPFILE FUNCTIONS
		 * Graciously lifted from
		 * http://www.daveoncsharp.com/2009/09/how-to-use-temporary-files-in-csharp/
		 */

		public static string CreateTmpFile()
		{
			string fileName = string.Empty;

			try
			{
				// Get the full name of the newly created Temporary file. 
				// Note that the GetTempFileName() method actually creates
				// a 0-byte file and returns the name of the created file.
				fileName = System.IO.Path.GetTempFileName();

				// Craete a FileInfo object to set the file's attributes
				FileInfo fileInfo = new FileInfo(fileName);

				// Set the Attribute property of this file to Temporary. 
				// Although this is not completely necessary, the .NET Framework is able 
				// to optimize the use of Temporary files by keeping them cached in memory.
				fileInfo.Attributes = FileAttributes.Temporary;

				//Console.WriteLine("TEMP file created at: " + fileName);
			}
			catch (Exception ex)
			{
				Console.WriteLine("Unable to create TEMP file or set its attributes: " + ex.Message);
			}

			return fileName;
		}

		public static void UpdateTmpFile(string tmpFile, dynamic data)
		{
			try
			{
				// Write to the temp file.
				StreamWriter streamWriter = File.AppendText(tmpFile);
				streamWriter.WriteLine(data);
				streamWriter.Flush();
				streamWriter.Close();
				//Console.WriteLine("TEMP file updated.");
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error writing to TEMP file: " + ex.Message);
			}
		}

		public static void DeleteTmpFile(string tmpFile)
		{
			try
			{
				// Delete the temp file (if it exists)
				if (File.Exists(tmpFile))
				{
					File.Delete(tmpFile);
					//Console.WriteLine("TEMP file deleted.");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error deleteing TEMP file: " + ex.Message);
			}
		}
	}
}
