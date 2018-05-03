using UnityEditor;
using UnityEngine;

public class BuildAssetBundle
{

	/// <summary>
	/// 点击后，所有设置了AssetBundle名称的资源会被 分单个打包出来
	/// </summary>
	[MenuItem ("AssetBundle/Build (Single)")]
	static void Build_AssetBundle ()
	{
		BuildTarget bt =
		#if UNITY_ANDROID
			BuildTarget.Android;
		#elif UNITY_IPHONE
			BuildTarget.iOS;
		#endif
		BuildPipeline.BuildAssetBundles (Application.dataPath + "/AssetBundles/", BuildAssetBundleOptions.None, bt);
		//刷新
		AssetDatabase.Refresh ();
	}

	/// <summary>
	/// 选择的资源合在一起被打包出来
	/// </summary>
	[MenuItem ("AssetBundle/Build (Collection)")]
	static void Build_AssetBundle_Collection ()
	{
		BuildTarget bt =
		#if UNITY_ANDROID
			BuildTarget.Android;
		#elif UNITY_IPHONE
			BuildTarget.iOS;
		#endif
		
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

		BuildPipeline.BuildAssetBundles (Application.dataPath + "/AssetBundles/", buildMap, BuildAssetBundleOptions.None, bt);
		//刷新
		AssetDatabase.Refresh ();
	}
		
	/// <summary>
	/// 清除所有的AssetBundleName，由于打包方法会将所有设置过AssetBundleName的资源打包，所以自动打包前需要清理
	/// </summary>
	[MenuItem ("AssetBundle/ClearAssetBundlesName")]
	static void ClearAssetBundlesName()
	{
		//获取所有的AssetBundle名称
		string[] abNames = AssetDatabase.GetAllAssetBundleNames();

		//强制删除所有AssetBundle名称
		for (int i = 0; i < abNames.Length; i++)
		{
			AssetDatabase.RemoveAssetBundleName(abNames[i], true);
		}
	}
}