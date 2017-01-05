using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System;
using SimpleJSON;
using UnityEngine.UI;

namespace XLAF.Public
{
    /// <summary>
    /// 常用工具（函数）
    /// </summary>
    public class ModUtils
    {
        /// <summary>
        /// Time stamp.
        /// 计算时间差的，毫秒级，主要用于测试性能时使用
        /// </summary>
        public class TimeStamp
        {

            private Stopwatch sw;

            public TimeStamp ()
            {
                sw = new Stopwatch ();
            }

            public void Start ()
            {
                this.sw.Start ();
            }

            public string Stop ()
            {
                this.sw.Stop ();
                return this.sw.ElapsedMilliseconds.ToString ();
            }
        }

        static ModUtils ()
        {
            Log.PrintInfo ("documentsDirectory", ModUtils.documentsDirectory);
            Log.PrintInfo ("temporaryDirectory", ModUtils.temporaryDirectory);
            Log.PrintInfo ("streamingDirectory", ModUtils.streamingDirectory);
        }

        /// <summary>
        /// 调用Init会触发构造函数，可以用于统一初始化的时候
        /// </summary>
        public static void Init ()
        {

        }


        /// <summary>
        /// Writes string to file.
        /// </summary>
        /// <param name="filePathName">File path name.</param>
        /// <param name="content">Content.</param>
        /// <param name="overWrite">If set to <c>true</c> over write.</param>
        public static void WriteToFile (string filePathName, string content, bool overWrite = false)
        {
            StreamWriter sw;
            FileInfo fi = new FileInfo (filePathName);
            if (!fi.Exists || overWrite) {
                sw = fi.CreateText ();
            } else {
                sw = fi.AppendText ();
            }
            sw.Write (content);
            sw.Close ();
            sw.Dispose ();
        }

        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="filePathName">File path name.</param>
        public static void DeleteFile (string filePathName)
        {
            File.Delete (filePathName); 
        }

        /// <summary>
        /// Reads the file.
        /// </summary>
        /// <returns>The file contents.</returns>
        /// <param name="filePathName">File path name.</param>
        public static string ReadFile (string filePathName)
        {     
            FileInfo t = new FileInfo (filePathName);          
            if (!t.Exists) {
                return null;
            }
            StreamReader sr = null;    
            sr = File.OpenText (filePathName);
            string str = sr.ReadToEnd (); 
            sr.Close ();
            sr.Dispose ();
            return str;
        }

        /// <summary>
        /// Writes the json object to file.
        /// </summary>
        /// <param name="filePathName">File path name.</param>
        /// <param name="json">Json object.</param>
        public static void WriteJsonToFile (string filePathName, JSONNode json)
        {
            WriteToFile (filePathName, json.ToString (), true);
        }

        /// <summary>
        /// Reads the json from file.
        /// </summary>
        /// <returns>The JSONNode from file.</returns>
        /// <param name="filePathName">File path name.</param>
        /// <param name="def">Default JSONNode (return this if filePathName not exist)</param>
        public static JSONNode ReadJsonFromFile (string filePathName, JSONNode def = null)
        {
            string str = ModUtils.ReadFile (filePathName);
            if (str != null) {
                JSONNode jsn = JSONNode.Parse (str);
                return jsn;
            } else {
                return def;
            }
        }

        /// <summary>
        /// 项目目录下的StreamingAssets目录
        /// 例如：/Users/champion_yuan/Desktop/unityAlbert/wordAlbert/Assets/StreamingAssets
        /// </summary>
        /// <value>The streaming directory.</value>
        public static string streamingDirectory {
            get {
                return  Application.streamingAssetsPath;
            }
        }

        /// <summary>
        /// documents目录，在手机上会被iCloud和google自动备份
        /// </summary>
        /// <value>The documents directory.</value>
        public static string documentsDirectory {
            get {
                return Application.persistentDataPath;
            }
        }

        /// <summary>
        /// temporary目录，在手机上会不会被iCloud和google自动备份，清理缓存的时候该目录可能会被清除.
        /// </summary>
        /// <value>The temporary directory.</value>
        public static string temporaryDirectory {
            get {
                return Application.temporaryCachePath;
            }
        }

        #if UNITY_ANDROID
        /// <summary>
        /// Android外置储存卡的位置
        /// </summary>
        /// <value>The SD card path.</value>
        public static string SDCardPath  { get { return "/sdcard"; } }

        /// <summary>
        /// Android内置储存卡的位置
        /// </summary>
        /// <value>The storage path.</value>
        public static string storagePath { get { return "/storage/emulated/0"; } }
        #endif


        public static void ShowAlert (string title, string message, string okLabel, string cancelLabel, string neutralLabel, 
                                      Action actionOK, Action actionCancel, Action actionNeutral)
        {
            Log.Debug (title, message);

        }

        public static void ShowAlert (string title, string message, string okLabel, string cancelLabel, 
                                      Action actionOK, Action actionCancel)
        {
            Log.Debug (title, message);
        }


        public static void ShowAlert (string title, string message, string okLabel, Action actionOK)
        {
            Log.Debug (title, message);
        }

        public static void BindingButtonEvent (GameObject parent, Action<UIEvent> OnUIEvent)
        {
            //绑定button的click事件
            Button[] buttons = parent.GetComponentsInChildren<Button> (true);
            for (int i = 0; i < buttons.Length; i++) {
                Button b = buttons [i];
                b.onClick.AddListener (() => {
                    UIEvent e = new UIEvent ();
                    e.target = b.gameObject;
                    e.targetType = "button";
                    e.phase = TouchPhase.Ended;
                    OnUIEvent (e);
                });
            }
            //!!TODO!! 可以继续绑定其他的事件
        }

        public static void ReplaceButtonEvent (GameObject parent, Action<UIEvent> OnUIEvent)
        {
            //绑定button的click事件
            Button[] buttons = parent.GetComponentsInChildren<Button> (true);
            for (int i = 0; i < buttons.Length; i++) {
                Button b = buttons [i];
                b.onClick.RemoveAllListeners ();
                b.onClick.AddListener (() => {
                    UIEvent e = new UIEvent ();
                    e.target = b.gameObject;
                    e.targetType = "button";
                    e.phase = TouchPhase.Ended;
                    OnUIEvent (e);
                });
            }
            //!!TODO!! 可以继续绑定其他的事件
        }

        public static void BindingUIEvents (GameObject parent, Action<UIEvent> OnUIEvent)
        {
            BindingButtonEvent (parent, OnUIEvent);
        }
        public static void ReplaceUIEvents (GameObject parent, Action<UIEvent> OnUIEvent)
        {
            ReplaceButtonEvent (parent, OnUIEvent);
        }


        public static int Character2Ascii (string character)
        {
            if (character.Length == 1) {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding ();
                int asciiCode = (int)asciiEncoding.GetBytes (character) [0];
                return asciiCode;
            } else {
                throw new Exception ("character is not valid.");
            }
        }


        public static string Ascii2Character (int asciiCode)
        {
            if (asciiCode >= 0 && asciiCode <= 255) {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding ();
                byte[] byteArray = new byte[]{ (byte)asciiCode };
                string character = asciiEncoding.GetString (byteArray);
                return character;
            } else {
                throw new Exception ("ASCII code is not valid.");
            }
        }

    }


	


}