﻿using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System;
using System.IO;
using UnityEditor;
using System.Reflection;
using System.Text.RegularExpressions;

namespace XLAF.Public
{
    public class Log
    {

        public static bool debugOn = true;
        public static bool isWriteToFile = true;

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
            if (isWriteToFile)
                ModUtils.WriteToFile (debug_file, s + "\n");
        }

        public static void Error (params object[] objs)
        {
            string time = System.DateTime.Now.ToString ("MM-dd HH:mm:ss:fff");
            string line = _GetCodeLineAndFile (3);
            string s = time + "|" + line + _ParamsToString (objs);
            UnityEngine.Debug.LogError (s);
            if (isWriteToFile)
                ModUtils.WriteToFile (error_file, s + "\n");
        }

        public static void Warning (params object[] objs)
        {
            string time = System.DateTime.Now.ToString ("MM-dd HH:mm:ss:fff");
            string line = _GetCodeLineAndFile (3);
            string s = time + "|warning|" + line + _ParamsToString (objs);
            UnityEngine.Debug.LogWarning (s);
            if (isWriteToFile)
                ModUtils.WriteToFile (debug_file, s + "\n");
        }

        private static string _ParamsToString (params object[] objs)
        {
            string s = "";
            for (int i = 0; i < objs.Length; i++) {
                if (objs [i] != null)
                    s = s + objs [i].ToString () + "\t";
                else
                    s = s + "null\t";
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


        #if UNITY_EDITOR
        [UnityEditor.Callbacks.OnOpenAssetAttribute (0)]
        public static bool OnOpenAsset (int instanceID, int line)
        {
            string stackTrace = GetStackTrace ();
            if (!string.IsNullOrEmpty (stackTrace) && stackTrace.Contains ("|")) {
                Match matches = Regex.Match (stackTrace, @"\(at (.+)\)", RegexOptions.IgnoreCase);
                string pathLine = "";
                while (matches.Success) {
                    pathLine = matches.Groups [1].Value;
                    if (!pathLine.Contains ("Log.cs")) {
                        int splitIndex = pathLine.LastIndexOf (":");
                        string path = pathLine.Substring (0, splitIndex);
                        line = System.Convert.ToInt32 (pathLine.Substring (splitIndex + 1));
                        string fullPath = Application.dataPath.Substring (0, Application.dataPath.LastIndexOf ("Assets"));
                        fullPath = fullPath + path;
                        if (Application.platform == RuntimePlatform.OSXEditor) {
                            UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal (fullPath, line);
                        } else if (Application.platform == RuntimePlatform.WindowsEditor) {
                            UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal (fullPath.Replace ('/', '\\'), line);
                        } else {
                            UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal (fullPath, line);
                        }
                            
                        break;
                    }
                    matches = matches.NextMatch ();
                }
                return true;
            }
            return false;
        }

        public static string GetStackTrace ()
        {
            var consoleWindowType = typeof(EditorWindow).Assembly.GetType ("UnityEditor.ConsoleWindow");
            FieldInfo fieldInfo = consoleWindowType.GetField ("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
            object consoleWindowInstance = fieldInfo.GetValue (null);
            if (consoleWindowInstance != null) {
                if ((object)EditorWindow.focusedWindow == consoleWindowInstance) {
                    //get listViewState in consuleWindow
                    var listViewStateType = typeof(EditorWindow).Assembly.GetType ("UnityEditor.ListViewState");
                    fieldInfo = consoleWindowType.GetField ("m_ListView", BindingFlags.Instance | BindingFlags.NonPublic);
                    object listView = fieldInfo.GetValue (consoleWindowInstance);
                    //get row in listViewState
                    fieldInfo = listViewStateType.GetField ("row", BindingFlags.Instance | BindingFlags.Public);
                    int row = (int)fieldInfo.GetValue (listView);
                    //get m_ActiveText in consoleWindow
                    fieldInfo = consoleWindowType.GetField ("m_ActiveText", BindingFlags.Instance | BindingFlags.NonPublic);
                    string activeText = fieldInfo.GetValue (consoleWindowInstance).ToString ();
                    return activeText;
                }
            }
            return null;
        }
        #endif

    }

}