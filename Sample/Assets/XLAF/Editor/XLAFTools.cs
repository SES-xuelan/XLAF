using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using XLAF.Public;
using System;
using System.Threading;

public class XLAFTools
{

    [MenuItem("XLAF/LocalData/ClearAll")]
    static void ClearLocalDataAll()
    {
        string dataPath = Application.persistentDataPath;
        if (Directory.Exists(dataPath))
        {
            Directory.Delete(dataPath, true);
            Debug.Log("All local data clean");
        }
        else
        {
            Debug.Log("no data to clear");
        }
    }

    [MenuItem("XLAF/LocalData/ClearCache")]
    static void ClearLocalCache()
    {
        string path = Application.temporaryCachePath;
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
            Debug.Log("Cache data clean");
        }
        else
        {
            Debug.Log("no cache data to clear");
        }
    }

    [MenuItem("XLAF/LocalData/OpenPersistentDataPath")]
    static void OpenPersistentDataPath()
    {
        System.Diagnostics.Process.Start(Application.persistentDataPath);
    }

    [MenuItem("XLAF/LocalData/OpenStreamingAssetsPath")]
    static void OpenStreamingAssetsPath()
    {
        System.Diagnostics.Process.Start(Application.streamingAssetsPath);
    }

    [MenuItem("XLAF/LuaTools/CopyLuaFilesToStreaming")]
    static void CopyLuaFilesToStreaming()
    {
        // copy  Assets/Lua/*.lua  to  Assets/StreamingAssets/
        // when first open the app, you should copy lua files from StreamingAssets to documentFolder

        string source = Application.dataPath + "/Lua/";
        string dest = Application.streamingAssetsPath + "/Lua/";
        string searchPattern = "*.lua";
        string[] files = Directory.GetFiles(source, searchPattern, SearchOption.AllDirectories);

        for (int i = 0; i < files.Length; i++)
        {
            string str = files[i].Remove(0, source.Length);
            string path = dest + "/" + str;
            string dir = Path.GetDirectoryName(path);
            Directory.CreateDirectory(dir);
            File.Copy(files[i], path, true);
        }
        AssetDatabase.Refresh();
        Debug.Log("CopyLuaFilesToStreaming succeed!");

    }



    class ZipCallback : ModCompress.ZipCallback
    {
        public override bool OnPreZip(ICSharpCode.SharpZipLib.Zip.ZipEntry entry)
        {
            //			Debug.Log ("OnPreZip=>" + entry.ToString ());
            return true;
        }

        public override void OnPostZip(ICSharpCode.SharpZipLib.Zip.ZipEntry entry)
        {
            Debug.Log("OnPostZip=>" + entry.ToString());
        }

        public override void OnFinished(bool isSuccess)
        {
            Debug.Log("<color=red>Compress OnFinished=>" + isSuccess.ToString() + "</color>");
        }
    }

    class UnZipCallback : ModCompress.UnzipCallback
    {
        public override bool OnPreUnzip(ICSharpCode.SharpZipLib.Zip.ZipEntry entry)
        {
            return true;
        }

        public override void OnPostUnzip(ICSharpCode.SharpZipLib.Zip.ZipEntry entry)
        {
            Debug.Log("OnPostUnzip=>" + entry.ToString());
        }

        public override void OnFinished(bool isSuccess)
        {
            Debug.Log("<color=red>Decompress OnFinished=>" + isSuccess.ToString() + "</color>");
        }
    }
}
