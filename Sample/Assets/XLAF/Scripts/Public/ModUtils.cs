﻿using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System;
using SimpleJSON;
using UnityEngine.UI;

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
			Log.Info ("documentsDirectory", ModUtils.documentsDirectory);
			Log.Info ("temporaryDirectory", ModUtils.temporaryDirectory);
			Log.Info ("streamingDirectory", ModUtils.streamingDirectory);
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
		/// Reads the file.
		/// </summary>
		/// <returns>The file contents.</returns>
		/// <param name="filePathName">File path name.</param>
		public static string ReadFile (string filePathName)
		{     
			FileInfo t = new FileInfo (filePathName);          
			if (!t.Exists) {
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
		/// Writes the json object to file.
		/// </summary>
		/// <param name="filePathName">File path name.</param>
		/// <param name="json">Json object.</param>
		public static void WriteJsonToFile (string filePathName, JSONNode json)
		{
			WriteToFile (filePathName, json.ToString (), true);
		}

		/// <summary>
		/// Reads the json from file.
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
		/// StreamingAssets folder<para>></para>
		/// e.g. Assets/StreamingAssets
		/// </summary>
		/// <value>The streaming directory.</value>
		public static string streamingDirectory {
			get {
				return  Application.streamingAssetsPath;
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
			Log.Debug (title, message);

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
			Log.Debug (title, message);
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
			Log.Debug (title, message);
		}

		/// <summary>
		/// Bindings the button event.
		/// </summary>
		/// <param name="parent">Parent.</param>
		/// <param name="OnUIEvent">On user interface event.</param>
		public static void BindingButtonEvent (GameObject parent, Action<UIEvent> OnUIEvent)
		{
			Button[] buttons = parent.GetComponentsInChildren<Button> (true);
			for (int i = 0; i < buttons.Length; i++) {
				Button b = buttons [i];
				b.onClick.AddListener (() => {
					UIEvent e = new UIEvent ();
					e.target = b.gameObject;
					e.targetType = "button";
					e.phase = TouchPhase.Ended;
					OnUIEvent (e);
				});
			}
			//!!TODO!! binding other events
		}

		/// <summary>
		/// Replaces the button event (only onClick event now).
		/// </summary>
		/// <param name="parent">Parent.</param>
		/// <param name="OnUIEvent">On user interface event.</param>
		public static void ReplaceButtonEvent (GameObject parent, Action<UIEvent> OnUIEvent)
		{
			Button[] buttons = parent.GetComponentsInChildren<Button> (true);
			for (int i = 0; i < buttons.Length; i++) {
				Button b = buttons [i];
				b.onClick.RemoveAllListeners ();
				b.onClick.AddListener (() => {
					UIEvent e = new UIEvent ();
					e.target = b.gameObject;
					e.targetType = "button";
					e.phase = TouchPhase.Ended;
					OnUIEvent (e);
				});
			}
			//!!TODO!! binding other events
		}

		/// <summary>
		/// Bindings the user interface events.
		/// </summary>
		/// <param name="parent">Parent.</param>
		/// <param name="OnUIEvent">On user interface event.</param>
		public static void BindingUIEvents (GameObject parent, Action<UIEvent> OnUIEvent)
		{
			BindingButtonEvent (parent, OnUIEvent);
		}

		/// <summary>
		/// Replaces the user interface events.
		/// </summary>
		/// <param name="parent">Parent.</param>
		/// <param name="OnUIEvent">On user interface event.</param>
		public static void ReplaceUIEvents (GameObject parent, Action<UIEvent> OnUIEvent)
		{
			ReplaceButtonEvent (parent, OnUIEvent);
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

		#endregion
	}


	


}