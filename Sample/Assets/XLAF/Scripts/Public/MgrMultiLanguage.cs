using System.Collections;
using System.Collections.Generic;
using System;
using XLAF.Public;
using SimpleJSON;
using UnityEngine;

namespace XLAF.Public
{
	/// <summary>
	/// Multi language config.
	/// </summary>
	public class MgrMultiLanguage
	{
		#region private variables

		private static readonly string DEFAULT_LANGUAGE = "en_us";
		private static JSONNode LanguageConfigs = JSONNode.Parse ("");

		private static string currentLanguage;

		#endregion

		#region constructed function & initialization

		static MgrMultiLanguage ()
		{
			currentLanguage = MgrData.GetString (MgrData.appSettingsName, "XLAF.language", DEFAULT_LANGUAGE);
			Log.Debug ("currentLanguage", currentLanguage);
			Load ();

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
		/// Gets the current language.
		/// </summary>
		/// <returns>The current language.</returns>
		public static string GetCurrentLanguage ()
		{
			return currentLanguage;
		}

		/// <summary>
		/// Switchs the language.<para></para>
		/// this will call <code>Scenes or Dialog 's UpdateLanguage()</code>
		/// </summary>
		/// <param name="lang">Language json filename (without extension) in <see cref="Resources/Lang/"/> or <see cref="xxxx.assetBundle 's Lang folder"/>. </param>
		public static void SwitchLanguage (string lang)
		{
			currentLanguage = lang;
			MgrData.Set (MgrData.appSettingsName, "XLAF.language", lang);
			Load ();
			foreach (KeyValuePair<string,SceneObject> kv in MgrScene.GetAllScenes()) {
				kv.Value.script.UpdateLanguage ();
			}
			foreach (KeyValuePair<string,SceneObject> kv in MgrDialog.GetAllDialogs()) {
				kv.Value.script.UpdateLanguage ();
			}
		}

		/// <summary>
		/// Gets the string from current language config file.
		/// </summary>
		/// <returns>The string.</returns>
		/// <param name="stringKeyName">String key name.</param>
		public static string GetString (string stringKeyName)
		{
			string ret = LanguageConfigs [stringKeyName].Value;
			return ret;
		}
		/// <summary>
		/// Gets the string from current language config file with placeholder.<para></para>
		/// e.g. <code>"congratulations! you have got {0} points! rank No. {1}"</code>
		/// </summary>
		/// <returns>The string.</returns>
		/// <param name="stringKeyName">String key name.</param>
		/// <param name="args">Arguments.</param>
		public static string GetString (string stringKeyName, params object[] args)
		{
			string ret = "";
			try {
				ret = LanguageConfigs [stringKeyName].Value;
				ret = string.Format (ret, args);
			} catch (Exception e) {
				Log.Error ("error in MgrMutiLanguage|GetString:", e);
			}

			return ret;
		}

		#endregion

		#region private functions

		private static void Load ()
		{
			TextAsset str = Resources.Load<TextAsset> ("Lang/" + currentLanguage);
			LanguageConfigs = JSONNode.Parse (str.ToString ());
			Log.Debug ("bytes", LanguageConfigs);

		}

		#endregion
	}
}