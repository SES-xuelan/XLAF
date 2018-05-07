using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

namespace XLAF.Public
{
	public class ModAssetBundle
	{
		private static JSONNode jsonData = null;

		/// <summary>
		/// 调用Init会触发构造函数，可以用于统一初始化的时候
		/// </summary>
		public static void Init ()
		{
		}

		static ModAssetBundle ()
		{
			string path = ModUtils.documentsDirectory;
			LoadAssetBundleConfig (path + "/assetbundle.config");
		}

		public static JSONNode  LoadAssetBundleConfig (string filePathName)
		{
			jsonData = null;
			jsonData = ModUtils.ReadJsonFromFile (filePathName, JSONNode.Parse ("{}"));
			return jsonData;
		}
		 
		public static string GetAssetBundlePath (string sceneName)
		{
			if (jsonData == null) {
				Log.Error ("please call LoadAssetBundleConfig(path) first");
				return "";
			}
			return jsonData [sceneName].Value;
		}

		public static bool HasAssetBundle (string sceneName)
		{
			Log.Debug ("HasAssetBundle()", jsonData, sceneName, GetAssetBundlePath (sceneName) != "");
			return GetAssetBundlePath (sceneName) != "";
		}
	}
}