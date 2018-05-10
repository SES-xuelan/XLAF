using UnityEditor;
using UnityEngine;
using System.IO;
using SimpleJSON;
using XLAF.Public;
using System.Text.RegularExpressions;

public class BuildAssetBundle
{

	public static string sourcePath = Application.dataPath + "/Resources";
	const string AssetBundlesOutputPath = "Assets/StreamingAssets";

	/// <summary>
	/// All resources with AssetBundle named will pack into single file.
	/// </summary>
	[MenuItem ("XLAF/AssetBundle/Build Single")]
	static void BuildAssetBundleSingle ()
	{
		ClearAssetBundlesName ();

		AutoSetAssetBundlesName (sourcePath);

		string outputPath = Path.Combine (AssetBundlesOutputPath, Platform.GetPlatformFolder (EditorUserBuildSettings.activeBuildTarget));
		if (!Directory.Exists (outputPath)) {
			Directory.CreateDirectory (outputPath);
		}


		BuildPipeline.BuildAssetBundles (outputPath, 0, EditorUserBuildSettings.activeBuildTarget);
		GenAssetbundleConfig (outputPath);
		AssetDatabase.Refresh ();

		Debug.Log ("Build Single succeed");
	}

	/// <summary>
	/// Selected resources will pack for ONE file
	/// </summary>
	[MenuItem ("XLAF/AssetBundle/Build Collection")]
	static void BuildAssetBundleCollection ()
	{
		AssetBundleBuild[] buildMap = new AssetBundleBuild[1];
		//export assetBundle name
		buildMap [0].assetBundleName = "all.assetbundle";

		//Select resources in "Project" tab
		Object[] selects = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);
		string[] enemyAsset = new string[selects.Length];
		for (int i = 0; i < selects.Length; i++) {
			//get path
			enemyAsset [i] = AssetDatabase.GetAssetPath (selects [i]);
		}
		buildMap [0].assetNames = enemyAsset;

		string outputPath = Path.Combine (AssetBundlesOutputPath, Platform.GetPlatformFolder (EditorUserBuildSettings.activeBuildTarget));
		if (!Directory.Exists (outputPath)) {
			Directory.CreateDirectory (outputPath);
		}
		BuildPipeline.BuildAssetBundles (outputPath, buildMap, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

		GenAssetbundleConfig (outputPath);

		AssetDatabase.Refresh ();
		Debug.Log ("Build Collection succeed");
	}



	/// <summary>
	/// Clear all AssetBundle Name in project
	/// </summary>
	[MenuItem ("XLAF/AssetBundle/ClearAssetBundlesName")]
	static void ClearAssetBundlesName ()
	{
		//get all AssetBundle names
		string[] abNames = AssetDatabase.GetAllAssetBundleNames ();

		//remove AssetBundle name
		for (int i = 0; i < abNames.Length; i++) {
			AssetDatabase.RemoveAssetBundleName (abNames [i], true);
		}
		Debug.Log ("Clear AssetBundles Name succeed!!");
	}

	/// <summary>
	/// Auto sets the name of the asset bundles.
	/// </summary>
	/// <param name="source">Source.</param>
	[MenuItem ("XLAF/AssetBundle/AutoSetAssetBundlesName")]
	static void AutoSetABName ()
	{
		AutoSetAssetBundlesName (sourcePath);
		Debug.Log ("Auto Set AssetBundles Name succeed!!");
	}

	//Auto sets the name of the asset bundles for path.
	static void AutoSetAssetBundlesName (string source)
	{
		DirectoryInfo folder = new DirectoryInfo (source);
		FileSystemInfo[] files = folder.GetFileSystemInfos ();
		int length = files.Length;
		for (int i = 0; i < length; i++) {
			if (files [i] is DirectoryInfo) {
				AutoSetAssetBundlesName (files [i].FullName);
			} else {
				if (!files [i].Name.EndsWith (".meta")) {
					SetAB (files [i].FullName);
				}
			}
		}
	}

	static void SetAB (string source)
	{
		string _source = Replace (source);
		string _assetPath = "Assets" + _source.Substring (Application.dataPath.Length);
		string _assetPath2 = _source.Substring (Application.dataPath.Length + 1);
		//Debug.Log (_assetPath);

		//set AssetBundle Name
		AssetImporter assetImporter = AssetImporter.GetAtPath (_assetPath);
		string assetName = _assetPath2.Substring (_assetPath2.IndexOf ("/") + 1);
		assetName = assetName.Replace (Path.GetExtension (assetName), ".assetbundle");
		//Debug.Log (assetName);
		assetImporter.assetBundleName = assetName;
	}

	static string Replace (string s)
	{
		return s.Replace ("\\", "/");
	}

	static void GenAssetbundleConfig (string path)
	{
		JSONNode json = GenAssetbundleConfig (path, JSONNode.Parse ("{}"), Replace (path));
//		Debug.Log (json.ToString ());
		ModUtils.WriteJsonToFile (path + "/assetbundle.config", json);
	}

	static JSONNode GenAssetbundleConfig (string path, JSONNode json, string basePath)
	{
		DirectoryInfo folder = new DirectoryInfo (path);
		FileSystemInfo[] files = folder.GetFileSystemInfos ();
		int length = files.Length;
		for (int i = 0; i < length; i++) {
			if (files [i] is DirectoryInfo) {
				GenAssetbundleConfig (files [i].FullName, json, basePath);
			} else {
				if (files [i].Name.EndsWith (".assetbundle.manifest")) {
					string data = ModUtils.ReadFile (files [i].FullName);
					int start = data.IndexOf ("Assets:") + 8;
					int end = data.IndexOf ("Dependencies:");
					string assets = data.Substring (start, end - start).Trim ();
					string[] arr = assets.Split ('\n');
					foreach (string s in arr) {
						if (s.EndsWith (".prefab")) {
							string[] _arr = s.Split ('/');
							string scene = _arr [_arr.Length - 1].Replace (".prefab", "");
							string fullname = Replace (files [i].FullName);
							json [scene] = fullname.Substring (fullname.IndexOf (basePath) + basePath.Length).Replace (".manifest", "");
						}
					}
				}
			}
		}
		return json;
	}

	public class Platform
	{
		public static string GetPlatformFolder (BuildTarget target)
		{
			switch (target) {
			case BuildTarget.Android:
				return "Android";
			case BuildTarget.iOS:
				return "IOS";
			case BuildTarget.WebGL:
				return "WebGL";
			case BuildTarget.StandaloneWindows:
			case BuildTarget.StandaloneWindows64:
				return "Windows";
			case BuildTarget.StandaloneOSXIntel:
			case BuildTarget.StandaloneOSXIntel64:
			case BuildTarget.StandaloneOSXUniversal:
				return "OSX";
			default:
				return null;
			}
		}
	}
}