using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System;

namespace XLAF.Public
{
    public class ModUtils : MonoBehaviour
	{
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

		private static string basePath;

        static ModUtils ()
		{
			if (Application.platform == RuntimePlatform.Android) {
				basePath = Application.persistentDataPath;
			} else if (Application.platform == RuntimePlatform.IPhonePlayer) {
				basePath = Application.persistentDataPath;
			} else {
				basePath = Application.dataPath;
			}
//			Log.Debug ("basePath:" + basePath);
		}

		public static void WriteToFile (string filePathName, string content)
		{
			StreamWriter sw;
			FileInfo fi = new FileInfo (basePath + "//" + filePathName);
			if (!fi.Exists) {
				sw = fi.CreateText ();
			} else {
				sw = fi.AppendText ();
			}
			sw.Write (content);
			sw.Close ();
			sw.Dispose ();
		}

		public static void DeleteFile (string filePathName)
		{
			File.Delete (basePath + "//" + filePathName); 
		}

		public static string ReadFile (string filePathName)
		{     
			FileInfo t = new FileInfo (basePath + "//" + filePathName);          
			if (!t.Exists) {
				return "error";
			}
			StreamReader sr = null;    
			sr = File.OpenText (basePath + "//" + filePathName);
			string str = sr.ReadToEnd (); 
			sr.Close ();
			sr.Dispose ();
			return str;
		}
	}


	


}