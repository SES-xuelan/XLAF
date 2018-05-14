using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using XLAF.Public;
using System;

public class Tools
{

	[MenuItem ("XLAF/LocalData/ClearAll")]
	static void ClearLocalDataAll ()
	{
		string dataPath = Application.persistentDataPath;
		if (Directory.Exists (dataPath)) {
			Directory.Delete (dataPath, true);
			Debug.Log ("All local data clean");
		} else {
			Debug.Log ("no data to clear");
		}
	}

	[MenuItem ("XLAF/LocalData/ClearCache")]
	static void ClearLocalCache ()
	{
		string path = Application.temporaryCachePath;
		if (Directory.Exists (path)) {
			Directory.Delete (path, true);
			Debug.Log ("Cache data clean");
		} else {
			Debug.Log ("no cache data to clear");
		}
	}

	[MenuItem ("XLAF/TestTools/Compress&Decompress/Compress Selected Folder")]
	static void CompressSelectFolder ()
	{
		//Select resources in "Project" tab
		UnityEngine.Object[] selects = Selection.GetFiltered (typeof(UnityEngine.Object), SelectionMode.Unfiltered);
		//get path
		string selectedFolder = AssetDatabase.GetAssetPath (selects [0]);
		if (string.IsNullOrEmpty (selectedFolder)) {
			Debug.Log ("nothing selected");
		} else {
			ZipCallback cb = new ZipCallback ();
			string outputPath = Path.Combine (Application.streamingAssetsPath, BuildAssetBundle.Platform.GetPlatformFolder (EditorUserBuildSettings.activeBuildTarget));
			ModCompress.Compress (new string[]{ selectedFolder }, outputPath + "/" + DateTime.Now.ToString ("yyyy_MM_dd__hh_mm_ss_fff") + "_with_password.zip", "123", cb); 
		}
	}

	[MenuItem ("XLAF/TestTools/Compress&Decompress/Decompress Selected File")]
	static void DecompressSelectFile ()
	{
		//Select resources in "Project" tab
		UnityEngine.Object[] selects = Selection.GetFiltered (typeof(UnityEngine.Object), SelectionMode.Unfiltered);
		//get path
		string selectedFile = AssetDatabase.GetAssetPath (selects [0]);
		if (string.IsNullOrEmpty (selectedFile)) {
			Debug.Log ("nothing selected");
		} else {
			UnZipCallback cb = new UnZipCallback ();
			string outputPath = Path.Combine (Application.streamingAssetsPath, BuildAssetBundle.Platform.GetPlatformFolder (EditorUserBuildSettings.activeBuildTarget));
			ModCompress.Decompress (selectedFile, outputPath, "123", cb);
		}
	}










	class ZipCallback:ModCompress.ZipCallback
	{
		public override bool OnPreZip (ICSharpCode.SharpZipLib.Zip.ZipEntry entry)
		{
//			Debug.Log ("OnPreZip=>" + entry.ToString ());
			return true;
		}

		public override void OnPostZip (ICSharpCode.SharpZipLib.Zip.ZipEntry entry)
		{
			Debug.Log ("OnPostZip=>" + entry.ToString ());
		}

		public override void OnFinished (bool isSuccess)
		{
			Debug.Log ("<color=red>Compress OnFinished=>" + isSuccess.ToString () + "</color>");
		}
	}

	class UnZipCallback:ModCompress.UnzipCallback
	{
		public override bool OnPreUnzip (ICSharpCode.SharpZipLib.Zip.ZipEntry entry)
		{
			return true;
		}

		public override void OnPostUnzip (ICSharpCode.SharpZipLib.Zip.ZipEntry entry)
		{
			Debug.Log ("OnPostUnzip=>" + entry.ToString ());
		}

		public override void OnFinished (bool isSuccess)
		{
			Debug.Log ("<color=red>Decompress OnFinished=>" + isSuccess.ToString () + "</color>");
		}
	}
}
