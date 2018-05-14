using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;

namespace XLAF.Public
{
	/// <summary>
	/// compress & decompress.
	/// </summary>
	public class ModCompress
	{
		#region ZipCallback

		public abstract class ZipCallback
		{
			/// <summary>
			/// callback on pre-zip
			/// </summary>
			/// <param name="entry"></param>
			/// <returns>return true for compress; false for not compress</returns>
			public virtual bool OnPreZip (ZipEntry entry)
			{
				return true;
			}

			/// <summary>
			/// callback when one file compressed
			/// </summary>
			/// <param name="entry"></param>
			public virtual void OnPostZip (ZipEntry entry)
			{
			}

			/// <summary>
			/// callback when finished
			/// </summary>
			/// <param name="isSuccess">true means succeed; false means failed</param>
			public virtual void OnFinished (bool isSuccess)
			{
			}
		}

		#endregion

		#region UnzipCallback

		public abstract class UnzipCallback
		{
			/// <summary>
			/// callback on pre-unzip
			/// </summary>
			/// <param name="entry"></param>
			/// <returns>return true for compress; false for not decompress</returns>
			public virtual bool OnPreUnzip (ZipEntry entry)
			{
				return true;
			}

			/// <summary>
			/// callback when one file decompressed
			/// </summary>
			/// <param name="entry"></param>
			public virtual void OnPostUnzip (ZipEntry entry)
			{
			}

			/// <summary>
			/// callback when finished
			/// </summary>
			/// <param name="isSuccess">true means succeed; false means failed</param>
			public virtual void OnFinished (bool isSuccess)
			{
			}
		}

		#endregion

		#region public functions

		/// <summary>
		///  compress files and/or folders
		/// </summary>
		/// <param name="fileOrDirectoryArray">folder path and file names</param>
		/// <param name="outputPathName">output path file name </param>
		/// <param name="password">password</param>
		/// <param name="zipCallback">ZipCallback</param>
		/// <returns></returns>
		public static bool Compress (string[] fileOrDirectoryArray, string outputPathName, string password = null, ZipCallback zipCallback = null)
		{
			if ((null == fileOrDirectoryArray) || string.IsNullOrEmpty (outputPathName)) {
				if (null != zipCallback)
					zipCallback.OnFinished (false);

				return false;
			}
			ZipOutputStream zipOutputStream = new ZipOutputStream (File.Create (outputPathName));
			zipOutputStream.SetLevel (6);    // default
			if (!string.IsNullOrEmpty (password))
				zipOutputStream.Password = password;

			for (int index = 0; index < fileOrDirectoryArray.Length; ++index) {
				bool result = false;
				string fileOrDirectory = fileOrDirectoryArray [index];
				if (Directory.Exists (fileOrDirectory))
					result = ZipDirectory (fileOrDirectory, string.Empty, zipOutputStream, zipCallback);
				else if (File.Exists (fileOrDirectory))
					result = ZipFile (fileOrDirectory, string.Empty, zipOutputStream, zipCallback);

				if (!result) {
					if (null != zipCallback)
						zipCallback.OnFinished (false);

					return false;
				}
			}

			zipOutputStream.Finish ();
			zipOutputStream.Close ();

			if (null != zipCallback)
				zipCallback.OnFinished (true);

			return true;
		}

		/// <summary>
		/// Decompress
		/// </summary>
		/// <param name="filePathName">file to decompress. file path</param>
		/// <param name="outputPath">output folder</param>
		/// <param name="password">password</param>
		/// <param name="unzipCallback">UnzipCallback</param>
		/// <returns></returns>
		public static bool Decompress (string filePathName, string outputPath, string password = null, UnzipCallback unzipCallback = null)
		{
			if (string.IsNullOrEmpty (filePathName) || string.IsNullOrEmpty (outputPath)) {
				if (null != unzipCallback)
					unzipCallback.OnFinished (false);

				return false;
			}

			try {
				return Decompress (File.OpenRead (filePathName), outputPath, password, unzipCallback);
			} catch (System.Exception e) {
				Debug.LogError (e.ToString ());

				if (null != unzipCallback)
					unzipCallback.OnFinished (false);

				return false;
			}
		}

		/// <summary>
		/// Decompress
		/// </summary>
		/// <param name="fileBytes">file bytes</param>
		/// <param name="outputPath">output path </param>
		/// <param name="password">password</param>
		/// <param name="unzipCallback">UnzipCallback</param>
		/// <returns></returns>
		public static bool Decompress (byte[] fileBytes, string outputPath, string password = null, UnzipCallback unzipCallback = null)
		{
			if ((null == fileBytes) || string.IsNullOrEmpty (outputPath)) {
				if (null != unzipCallback)
					unzipCallback.OnFinished (false);

				return false;
			}

			bool result = Decompress (new MemoryStream (fileBytes), outputPath, password, unzipCallback);
			if (!result) {
				if (null != unzipCallback)
					unzipCallback.OnFinished (false);
			}

			return result;
		}

		/// <summary>
		/// Decompress
		/// </summary>
		/// <param name="inputStream">Zip inputStream</param>
		/// <param name="outputPath">output folder</param>
		/// <param name="password">password</param>
		/// <param name="unzipCallback">UnzipCallback</param>
		/// <returns></returns>
		public static bool Decompress (Stream inputStream, string outputPath, string password = null, UnzipCallback unzipCallback = null)
		{
			if ((null == inputStream) || string.IsNullOrEmpty (outputPath)) {
				if (null != unzipCallback)
					unzipCallback.OnFinished (false);

				return false;
			}

			// create folder
			if (!Directory.Exists (outputPath))
				Directory.CreateDirectory (outputPath);

			// unzip
			ZipEntry entry = null;
			using (ZipInputStream zipInputStream = new ZipInputStream (inputStream)) {
				if (!string.IsNullOrEmpty (password))
					zipInputStream.Password = password;

				while (null != (entry = zipInputStream.GetNextEntry ())) {
					if (string.IsNullOrEmpty (entry.Name))
						continue;

					if ((null != unzipCallback) && !unzipCallback.OnPreUnzip (entry))
						continue;   // filter

					string filePathName = Path.Combine (outputPath, entry.Name);

					// create folder
					if (entry.IsDirectory) {
						Directory.CreateDirectory (filePathName);
						continue;
					}

					// write to file
					try {
						using (FileStream fileStream = File.Create (filePathName)) {
							byte[] bytes = new byte[1024];
							while (true) {
								int count = zipInputStream.Read (bytes, 0, bytes.Length);
								if (count > 0)
									fileStream.Write (bytes, 0, count);
								else {
									if (null != unzipCallback)
										unzipCallback.OnPostUnzip (entry);

									break;
								}
							}
						}
					} catch (System.Exception e) {
						Debug.LogError (e.ToString ());

						if (null != unzipCallback)
							unzipCallback.OnFinished (false);

						return false;
					}
				}
			}

			if (null != unzipCallback)
				unzipCallback.OnFinished (true);

			return true;
		}

		#endregion

		#region private functions

		/// <summary>
		/// compress file
		/// </summary>
		/// <param name="filePathName">file path name</param>
		/// <param name="parentRelPath">parent path</param>
		/// <param name="zipOutputStream">zipOutputStream</param>
		/// <param name="zipCallback">ZipCallback</param>
		/// <returns></returns>
		private static bool ZipFile (string filePathName, string parentRelPath, ZipOutputStream zipOutputStream, ZipCallback zipCallback = null)
		{
			ZipEntry entry = null;
			FileStream fileStream = null;
			try {
				string entryName = parentRelPath + '/' + Path.GetFileName (filePathName);
				entry = new ZipEntry (entryName);
				entry.DateTime = System.DateTime.Now;

				if ((null != zipCallback) && !zipCallback.OnPreZip (entry))
					return true;    // filter

				fileStream = File.OpenRead (filePathName);
				byte[] buffer = new byte[fileStream.Length];
				fileStream.Read (buffer, 0, buffer.Length);
				fileStream.Close ();

				entry.Size = buffer.Length;

				zipOutputStream.PutNextEntry (entry);
				zipOutputStream.Write (buffer, 0, buffer.Length);
			} catch (System.Exception e) {
				Debug.LogError (e.ToString ());
				return false;
			} finally {
				if (null != fileStream) {
					fileStream.Close ();
					fileStream.Dispose ();
				}
			}

			if (null != zipCallback)
				zipCallback.OnPostZip (entry);

			return true;
		}

		/// <summary>
		/// compress folder
		/// </summary>
		/// <param name="path">folder path</param>
		/// <param name="parentRelPath">parent path</param>
		/// <param name="zipOutputStream">zipOutputStream</param>
		/// <param name="zipCallback">ZipCallback</param>
		/// <returns></returns>
		private static bool ZipDirectory (string path, string parentRelPath, ZipOutputStream zipOutputStream, ZipCallback zipCallback = null)
		{
			ZipEntry entry = null;
			try {
				string entryName = Path.Combine (parentRelPath, Path.GetFileName (path) + '/');
				entry = new ZipEntry (entryName);
				entry.DateTime = System.DateTime.Now;
				entry.Size = 0;

				if ((null != zipCallback) && !zipCallback.OnPreZip (entry))
					return true;    // filter

				zipOutputStream.PutNextEntry (entry);
				zipOutputStream.Flush ();

				string[] files = Directory.GetFiles (path);
				for (int index = 0; index < files.Length; ++index)
					ZipFile (files [index], Path.Combine (parentRelPath, Path.GetFileName (path)), zipOutputStream, zipCallback);
			} catch (System.Exception e) {
				Debug.LogError (e.ToString ());
				return false;
			}

			string[] directories = Directory.GetDirectories (path);
			for (int index = 0; index < directories.Length; ++index) {
				if (!ZipDirectory (directories [index], Path.Combine (parentRelPath, Path.GetFileName (path)), zipOutputStream, zipCallback))
					return false;
			}

			if (null != zipCallback)
				zipCallback.OnPostZip (entry);

			return true;
		}

		#endregion
	}
}
