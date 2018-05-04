using UnityEditor;
using UnityEngine;
using System.IO;

public class BuildAssetBundle
{

	public static string sourcePath = Application.dataPath + "/Resources";
	const string AssetBundlesOutputPath = "Assets/AssetBundles";

	/// <summary>
	/// 点击后，所有设置了AssetBundle名称的资源会被 分单个打包出来
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

		//根据BuildSetting里面所激活的平台进行打包
		BuildPipeline.BuildAssetBundles (outputPath, 0, EditorUserBuildSettings.activeBuildTarget);

		AssetDatabase.Refresh ();

		Debug.Log ("Build Single succeed");
	}

	/// <summary>
	/// 选择的资源合在一起被打包出来
	/// </summary>
	[MenuItem ("XLAF/AssetBundle/Build Collection")]
	static void BuildAssetBundleCollection ()
	{
		AssetBundleBuild[] buildMap = new AssetBundleBuild[1];
		//打包出来的资源包名字
		buildMap [0].assetBundleName = "all.assetbundle";

		//在Project视图中，选择要打包的对象
		Object[] selects = Selection.GetFiltered (typeof(Object), SelectionMode.DeepAssets);
		string[] enemyAsset = new string[selects.Length];
		for (int i = 0; i < selects.Length; i++) {
			//获得选择 对象的路径
			enemyAsset [i] = AssetDatabase.GetAssetPath (selects [i]);
		}
		buildMap [0].assetNames = enemyAsset;

		BuildPipeline.BuildAssetBundles (Application.dataPath + "/AssetBundles/", buildMap, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);
		//刷新
		AssetDatabase.Refresh ();
		Debug.Log ("Build Collection succeed");
	}



	/// <summary>
	/// 清除所有的AssetBundleName，由于打包方法会将所有设置过AssetBundleName的资源打包，所以自动打包前需要清理
	/// </summary>
	[MenuItem ("XLAF/AssetBundle/ClearAssetBundlesName")]
	static void ClearAssetBundlesName ()
	{
		//获取所有的AssetBundle名称
		string[] abNames = AssetDatabase.GetAllAssetBundleNames ();

		//强制删除所有AssetBundle名称
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

	//自动按照路径设置assetbundle的name
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

		//在代码中给资源设置AssetBundleName
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