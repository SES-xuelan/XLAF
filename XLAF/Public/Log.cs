using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System;
using System.IO;

namespace XLAF.Public
{
    public class Log
	{

		public static bool debugOn = true;

		private static string debug_file = "debug.log";
		private static string error_file = "error.log";

		static  Log ()
		{
		
		}

		public static void Debug (params object[] objs)
		{
			if (!debugOn)
				return;
			string time = System.DateTime.Now.ToString ("MM-dd HH:mm:ss:fff");
			string line = _GetCodeLineAndFile ();
			string s = time + "|" + line + _ParamsToString (objs);
			UnityEngine.Debug.Log (s);
			ModUtils.WriteToFile (debug_file, s + "\n");
		}

		public static void Error (params object[] objs)
		{
			string time = System.DateTime.Now.ToString ("MM-dd HH:mm:ss:fff");
			string line = _GetCodeLineAndFile (3);
			string s = time + "|" + line + _ParamsToString (objs);
			UnityEngine.Debug.LogError (s);
			ModUtils.WriteToFile (error_file, s + "\n");
		}



		private static string _ParamsToString (params object[] objs)
		{
			string s = "";
			for (int i = 0; i < objs.Length; i++) {
				s = s + objs [i].ToString () + "\t";
			}
			return s;
		}

		private static string _GetCodeLineAndFile (int deep = 2)
		{
			StackTrace insStackTrace = new StackTrace (true);
			StackFrame insStackFrame = insStackTrace.GetFrame (deep);//GetFrame 表示深度
			string filename = Path.GetFileName (insStackFrame.GetFileName ());
			return String.Format ("{0}:{1}|\t ", filename, insStackFrame.GetFileLineNumber ());
		}
	}

}