using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using XLAF.Private;

namespace XLAF.Public
{
	/// <summary>
	/// Asset bundle manager.
	/// </summary>
	public class MgrAssetBundle
	{
		#region private variables

		private static JSONNode jsonData = null;

		#endregion

		#region public variables

		/// <summary>
		/// Gets the assetbundle config file.
		/// </summary>
		/// <value>The assetbundle.config file.</value>
		public static string assetbundleConfigFile {
			get { 
				string path = ModUtils.documentsDirectory;
				return path + "/assetbundle.config";
			}
		}

		#endregion

		#region constructed function & initialization

		static MgrAssetBundle ()
		{
			ReloadConfig ();
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
		/// Reloads the config file.
		/// </summary>
		public static void ReloadConfig ()
		{
			jsonData = ModUtils.ReadJsonFromFile (assetbundleConfigFile, JSONNode.Parse ("{}"));
		}

		/// <summary>
		/// Gets the asset bundle path.
		/// </summary>
		/// <returns>The asset bundle path.</returns>
		/// <param name="sceneName">Scene name.</param>
		public static string GetAssetBundlePath (string sceneName)
		{
			if (jsonData == null) {
				XLAFInnerLog.Error ("please call LoadAssetBundleConfig(path) first");
				return "";
			}
			string v = jsonData [sceneName].Value;
			if (string.IsNullOrEmpty (v)) {
				return "";
			}
			if (!v.StartsWith ("/")) {
				v = "/" + v;
			}
			return v;
		}

		/// <summary>
		/// Determines if has asset bundle the specified sceneName.
		/// </summary>
		/// <returns><c>true</c> if has asset bundle the specified sceneName; otherwise, <c>false</c>.</returns>
		/// <param name="sceneName">Scene name.</param>
		public static bool HasAssetBundle (string sceneName)
		{
			XLAFInnerLog.Debug ("HasAssetBundle()", jsonData, sceneName, GetAssetBundlePath (sceneName) != "");
			return GetAssetBundlePath (sceneName) != "";
		}

		#endregion
	}
}