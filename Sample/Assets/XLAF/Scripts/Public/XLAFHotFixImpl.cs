using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;
using XLAF.Private;

namespace XLAF.Public
{
	/// <summary>
	/// XLAF hot fix config Base class, developer MUST inherit this class to create new class to enable hot fix 
	/// </summary>
	public class XLAFHotFixImpl
	{
		
		#region constructed function & initialization

		static XLAFHotFixImpl ()
		{

		}

		/// <summary>
		/// call Init() will trigger constructed function, you can call Init() to ensure this class finished initialization
		/// </summary>
		public static void Init ()
		{

		}

		#endregion

		//!!TODO!! 配置热更新地址、文件hash等各类 热更配置

		#region public variables

		/// <summary>
		/// use toLua hot fix or not
		/// </summary>
		public static bool useHotFix{ get; set; }

		#endregion

		#region protected variables

		/// <summary>
		/// filesToDownload<fullPath, url>
		/// </summary>
		protected Dictionary<string,string> filesToDownload = new Dictionary<string, string> ();

		#endregion

		#region virtual functions

		/// <summary>
		/// First run the app, you should copy resources from streaming to document.
		/// </summary>
		/// <param name="callback">Callback<percent, isFinished>.</param>
		public virtual void FirstRun(Action<float,bool> callback){
			callback (1,true);
		}

		/// <summary>
		/// Makes the file hash code.
		/// </summary>
		/// <returns>The file hash code.</returns>
		/// <param name="filePathName">File path name.</param>
		public virtual string MakeFileHash (string filePathName)
		{
			return "";
		}

		/// <summary>
		/// Checks the update from url.
		/// </summary>
		public virtual void CheckUpdate (string url, Action<int> callback)
		{
			/*
				 recommend json:
				{
					ver=100, --server version
					verstr="1.0.0", --server version
					urls={
						full_file_path_name1=file_download_url1,
						full_file_path_name2=file_download_url2,
						...
					}
					hashs={
						full_file_path_name1=file_hash1,
						full_file_path_name2=file_hash2,
						...
					}
				}
			*/
			callback (0);
		}

		/// <summary>
		/// Downloads the resources in <c>filesToDownload</c>.
		/// </summary>
		/// <param name="callback">Callback<percent, isError>.</param>
		public virtual void DownloadResources (Action<float,bool> callback)
		{
			callback (1, true);
		}

		/// <summary>
		/// Makes the local file hashs.
		/// </summary>
		/// <returns>The local file hashs.</returns>
		public virtual Dictionary<string ,string> MakeLocalFileHashs (string sourcePath = null, Dictionary<string,string> dic = null)
		{
			if (dic == null) {
				dic = new Dictionary<string, string> ();
			}
			if (sourcePath == null) {
				sourcePath = ModUtils.documentsDirectory;
			}
			DirectoryInfo folder = new DirectoryInfo (sourcePath);
			FileSystemInfo[] files = folder.GetFileSystemInfos ();
			foreach (FileSystemInfo file in files) {
				if (file is DirectoryInfo && file.Name.ToLower ().Equals ("unity")) {
					//do nothing for unity reporter
				} else if (file is DirectoryInfo) {
					MakeLocalFileHashs (file.FullName, dic);
				} else {
					dic.Add (file.FullName.Replace ("\\", "/").Replace (ModUtils.documentsDirectory + "/", ""), MakeFileHash (file.FullName));
				}
			}
			return dic;
		}

		#endregion

	}
}