using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System;
using SimpleJSON;
using UnityEngine.UI;
using XLAF.Private;
using System.Collections.Generic;

namespace XLAF.Public
{
	/// <summary>
	/// Tools
	/// </summary>
	public class ModUtils
	{
		/// <summary>
		/// Time stamp.<para></para>
		/// calculate timestamp in milliseconds, often use for test or debug
		/// </summary>
		public class TimeStamp
		{

			private Stopwatch sw;

			public TimeStamp ()
			{
				sw = new Stopwatch ();
			}

			public void Start ()
			{
				this.sw.Start ();
			}

			public long Stop ()
			{
				this.sw.Stop ();
				return this.sw.ElapsedMilliseconds;
			}
		}

		#region constructed function & initialization

		static ModUtils ()
		{
			XLAFInnerLog.Info ("documentsDirectory", ModUtils.documentsDirectory);
			XLAFInnerLog.Info ("temporaryDirectory", ModUtils.temporaryDirectory);
			XLAFInnerLog.Info ("streamingDirectory", ModUtils.streamingDirectory);
		}

		/// <summary>
		/// call Init() will trigger constructed function, you can call Init() to ensure this class finished initialization
		/// </summary>
		public static void Init ()
		{

		}

		#endregion

		#region public functions

		/// <summary>
		/// Writes string to file.
		/// </summary>
		/// <param name="filePathName">File path name.</param>
		/// <param name="content">Content.</param>
		/// <param name="overWrite">If set to <c>true</c> over write.</param>
		public static void WriteToFile (string filePathName, string content, bool overWrite = false)
		{
			StreamWriter sw;
			FileInfo fi = new FileInfo (filePathName);
			if (!fi.Exists || overWrite) {
				sw = fi.CreateText ();
			} else {
				sw = fi.AppendText ();
			}
			sw.Write (content);
			sw.Close ();
			sw.Dispose ();
		}

		/// <summary>
		/// Deletes the file.
		/// </summary>
		/// <param name="filePathName">File path name.</param>
		public static void DeleteFile (string filePathName)
		{
			File.Delete (filePathName); 
		}

		/// <summary>
		/// Reads the file (don't use this in android, File.Exists often return false).
		/// </summary>
		/// <returns>The file contents.</returns>
		/// <param name="filePathName">File path name.</param>
		public static string ReadFile (string filePathName)
		{       
			if (!File.Exists (filePathName)) {
				return null;
			}
			StreamReader sr = null;    
			sr = File.OpenText (filePathName);
			string str = sr.ReadToEnd (); 
			sr.Close ();
			sr.Dispose ();
			return str;
		}

		/// <summary>
		/// Reads the file by WWW.
		/// </summary>
		/// <returns>The file by WWW.</returns>
		/// <param name="filePathName">File path name.</param>
		public static string ReadFileByWWW (string filePathName)
		{
			if (filePathName.StartsWith ("jar:")) {
				//filePathName = filePathName;
			} else {
				filePathName = "file://" + filePathName;
			}
			WWW www = new WWW (filePathName);
			if (www.error == null) {
				return www.text;
			} else {
				Log.Warning ("error while ReadFileByWWW", www.error);
				return null;
			}
		}

		/// <summary>
		/// Creates the directorys.
		/// </summary>
		/// <returns><c>true</c>, if directorys was created, <c>false</c> otherwise.</returns>
		/// <param name="fullFilePathOrDirectoryPath">Full file path or directory path.</param>
		public static bool CreateDirectorys (string fullFilePathOrDirectoryPath, bool isFile)
		{
			if (isFile) {
				// is file
				FileInfo f = new FileInfo (fullFilePathOrDirectoryPath);
				return CreateDirectorys (f.DirectoryName, false);
			} else {
				//is dirctory
				DirectoryInfo di = new DirectoryInfo (fullFilePathOrDirectoryPath);
				if (!di.Exists) {
					di.Create ();
					return true;
				}
			}
			return false;
		}

		/// <summary>
		/// Writes the json object to file.
		/// </summary>
		/// <param name="filePathName">File path name.</param>
		/// <param name="json">Json object.</param>
		public static void WriteJsonToFile (string filePathName, JSONNode json)
		{
			WriteToFile (filePathName, json.ToString (), true);
		}

		/// <summary>
		/// Reads the json from file (don't use this in android, File.Exists often return false).
		/// </summary>
		/// <returns>The JSONNode from file.</returns>
		/// <param name="filePathName">File path name.</param>
		/// <param name="def">Default JSONNode (return this if filePathName not exist)</param>
		public static JSONNode ReadJsonFromFile (string filePathName, JSONNode def = null)
		{
			string str = ModUtils.ReadFile (filePathName);
			if (str != null) {
				JSONNode jsn = JSONNode.Parse (str);
				return jsn;
			} else {
				return def;
			}
		}

		/// <summary>
		/// Reads the json from file by WWW.
		/// </summary>
		/// <returns>The JSONNode from file.</returns>
		/// <param name="filePathName">File path name.</param>
		/// <param name="def">Default JSONNode (return this if filePathName not exist)</param>
		public static JSONNode ReadJsonFromFileByWWW (string filePathName, JSONNode def = null)
		{
			string str = ModUtils.ReadFileByWWW (filePathName);
			if (str != null) {
				JSONNode jsn = JSONNode.Parse (str);
				return jsn;
			} else {
				return def;
			}
		}

		/// <summary>
		/// StreamingAssets folder<para>></para>
		/// e.g. Assets/StreamingAssets
		/// </summary>
		/// <value>The streaming directory.</value>
		public static string streamingDirectory {
			get {
				return Application.streamingAssetsPath + "/";
			}
		}

		/// <summary>
		/// documents  folder<para>></para>
		/// in mobile this folder will backup automatic with iCloud or google
		/// </summary>
		/// <value>The documents directory.</value>
		public static string documentsDirectory {
			get {
				return Application.persistentDataPath;
			}
		}

		/// <summary>
		/// temporary folder <para></para>
		/// in mobile this folder will NOT backup with iCloud or google. <para></para>
		/// when user clear cache, files in this folder will be removed!
		/// </summary>
		/// <value>The temporary directory.</value>
		public static string temporaryDirectory {
			get {
				return Application.temporaryCachePath;
			}
		}

		#if UNITY_ANDROID
		/// <summary>
		/// Android ext sdcard path
		/// </summary>
		/// <value>The SD card path.</value>
		public static string SDCardPath  { get { return "/sdcard"; } }

		/// <summary>
		/// Android inner storage(inner sdcard) path
		/// </summary>
		/// <value>The storage path.</value>
		public static string storagePath { get { return "/storage/emulated/0"; } }

		/// <summary>
		/// Shows the toast.
		/// </summary>
		public static void ShowToast ()
		{
			//!!TODO!!
		}
		#endif

		/// <summary>
		/// Shows the alert.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="message">Message.</param>
		/// <param name="okLabel">Ok label.</param>
		/// <param name="cancelLabel">Cancel label.</param>
		/// <param name="neutralLabel">Neutral label.</param>
		/// <param name="actionOK">Action O.</param>
		/// <param name="actionCancel">Action cancel.</param>
		/// <param name="actionNeutral">Action neutral.</param>
		public static void ShowAlert (string title, string message, string okLabel, string cancelLabel, string neutralLabel, 
		                              Action actionOK, Action actionCancel, Action actionNeutral)
		{
			//!!TODO!!
			XLAFInnerLog.Debug (title, message);

		}

		/// <summary>
		/// Shows the alert.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="message">Message.</param>
		/// <param name="okLabel">Ok label.</param>
		/// <param name="cancelLabel">Cancel label.</param>
		/// <param name="actionOK">Action O.</param>
		/// <param name="actionCancel">Action cancel.</param>
		public static void ShowAlert (string title, string message, string okLabel, string cancelLabel, 
		                              Action actionOK, Action actionCancel)
		{
			//!!TODO!!
			XLAFInnerLog.Debug (title, message);
		}

		/// <summary>
		/// Shows the alert.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="message">Message.</param>
		/// <param name="okLabel">Ok label.</param>
		/// <param name="actionOK">Action O.</param>
		public static void ShowAlert (string title, string message, string okLabel, Action actionOK)
		{
			//!!TODO!!
			XLAFInnerLog.Debug (title, message);
		}

		/// <summary>
		/// Convert character to the ASCII int code.
		/// </summary>
		/// <returns>The ASCII.</returns>
		/// <param name="character">Character.</param>
		public static int Character2Ascii (string character)
		{
			if (character.Length == 1) {
				System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding ();
				int asciiCode = (int)asciiEncoding.GetBytes (character) [0];
				return asciiCode;
			} else {
				throw new Exception ("character is not valid.");
			}
		}

		/// <summary>
		/// Convert ASCII code to the character.
		/// </summary>
		/// <returns>The character.</returns>
		/// <param name="asciiCode">ASCII code.</param>
		public static string Ascii2Character (int asciiCode)
		{
			if (asciiCode >= 0 && asciiCode <= 255) {
				System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding ();
				byte[] byteArray = new byte[]{ (byte)asciiCode };
				string character = asciiEncoding.GetString (byteArray);
				return character;
			} else {
				throw new Exception ("ASCII code is not valid.");
			}
		}

		/// <summary>
		/// Gets the platform lower string.
		/// </summary>
		/// <returns>The platform lower string.</returns>
		public static string GetPlatformLowerString ()
		{
			if (Application.platform == RuntimePlatform.Android) {
				return "android";
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
				return "ios";
			} else if (Application.platform == RuntimePlatform.WindowsPlayer) {
				return "windows";
			} else if (Application.platform == RuntimePlatform.OSXPlayer) {
				return "osx";
			} else if (Application.platform == RuntimePlatform.LinuxPlayer) {
				return "linux";
			} else if (Application.platform == RuntimePlatform.PS4) {
				return "ps4";
			} else if (Application.platform == RuntimePlatform.Switch) {
				return "switch";
			} else if (Application.platform == RuntimePlatform.tvOS) {
				return "tv";
			} else {
				return "unknown";
			}
		}

		#region Timer Delay

		/// <summary>
		/// Delay call.
		/// </summary>
		/// <param name="delaySeconds">Delay seconds.</param>
		/// <param name="callBack">Call back.</param>
		public static vp_Timer.Handle DelayCall (float delaySeconds, Action callback)
		{
			vp_Timer.Handle handle = new vp_Timer.Handle ();
			vp_Timer.In (delaySeconds, () => {
				handle.Cancel ();
				callback ();
			}, 1, handle);
			return handle;
		}

		/// <summary>
		/// Repeat call.
		/// </summary>
		/// <param name="delaySeconds">Delay(seconds).</param>
		/// <param name="repeatTimes">Repeat times, set <c>1</c> for no repeat .</param>
		/// <param name="callBack">Call back.</param>
		public static vp_Timer.Handle RepeatCall (float delaySeconds, int repeatTimes, Action callback)
		{
			vp_Timer.Handle handle = new vp_Timer.Handle ();
			vp_Timer.In (delaySeconds, () => {
				handle.Cancel ();
				callback ();
			}, 1, handle);
			return handle;
		}

		#endregion

		#endregion


	}


	


}