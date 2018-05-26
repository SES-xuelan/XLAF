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

		#region constructed function & initialization

		static MgrAssetBundle ()
		{
			string path = ModUtils.documentsDirectory;
			LoadAssetBundleConfig (path + "/assetbundle.config");
		}

		/// <summary>
		/// call Init() will trigger constructed function, you can call Init() to ensure this class finished initialization
		/// </summary>
		public static void Init ()
		{
		}

		#endregion

		#region public functions

		public static JSONNode  LoadAssetBundleConfig (string filePathName)
		{
			jsonData = null;
			jsonData = ModUtils.ReadJsonFromFile (filePathName, JSONNode.Parse ("{}"));
			return jsonData;
		}

		public static string GetAssetBundlePath (string sceneName)
		{
			if (jsonData == null) {
				XLAFInnerLog.Error ("please call LoadAssetBundleConfig(path) first");
				return "";
			}
			return jsonData [sceneName].Value;
		}

		public static bool HasAssetBundle (string sceneName)
		{
			XLAFInnerLog.Debug ("HasAssetBundle()", jsonData, sceneName, GetAssetBundlePath (sceneName) != "");
			return GetAssetBundlePath (sceneName) != "";
		}

		#endregion
	}
}