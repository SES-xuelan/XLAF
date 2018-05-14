using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

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
}
