using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using XLAF.Private;

#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Reflection;
using System.Text.RegularExpressions;
using SimpleJSON;

namespace XLAF.Public
{
    /// <summary>
    /// Log with java & object-C in mobile
    /// </summary>
    public class Log
    {
        private class LogLevel
        {
            public static readonly string v = "V";
            public static readonly string d = "D";
            public static readonly string i = "I";
            public static readonly string w = "W";
            public static readonly string e = "E";
        }

        #region private variables

        private static bool _isWriteToFile = false;

        private static bool _isWarnOn = false;
        private static bool _isInfoOn = false;
        private static bool _isDebugOn = false;
        private static bool _isErrorOn = false;
        private static int _deep = -1;

        private static string debug_file;
        private static string error_file;

        #endregion

        #region constructed function & initialization

        static Log()
        {
            debug_file = ModUtils.documentsDirectory + "/debug.log";
            error_file = ModUtils.documentsDirectory + "/error.log";
            int level = MgrData.GetInt(MgrData.sysSettingsName, "XLAF.debug", 0);
            SetDebugLevel(level);
        }


        /// <summary>
        /// call Init() will trigger constructed function, you can call Init() to ensure this class finished initialization
        /// </summary>
        public static void Init()
        {

        }

        #endregion

        #region public functions

        /// <summary>
        /// Gets a value indicating whether this <see cref="XLAF.Public.Log"/> is warn on.
        /// </summary>
        /// <value><c>true</c> if is warn on; otherwise, <c>false</c>.</value>
        public static bool isWarnOn { get { return _isWarnOn; } }

        /// <summary>
        /// Gets a value indicating whether this <see cref="XLAF.Public.Log"/> is info on.
        /// </summary>
        /// <value><c>true</c> if is info on; otherwise, <c>false</c>.</value>
        public static bool isInfoOn { get { return _isInfoOn; } }

        /// <summary>
        /// Gets a value indicating whether this <see cref="XLAF.Public.Log"/> is debug on.
        /// </summary>
        /// <value><c>true</c> if is debug on; otherwise, <c>false</c>.</value>
        public static bool isDebugOn { get { return _isDebugOn; } }

        /// <summary>
        /// Gets a value indicating whether this <see cref="XLAF.Public.Log"/> is error on.
        /// </summary>
        /// <value><c>true</c> if is error on; otherwise, <c>false</c>.</value>
        public static bool isErrorOn { get { return _isErrorOn; } }

        /// <summary>
        /// Gets a value indicating whether this <see cref="XLAF.Public.Log"/> is write to file.
        /// </summary>
        /// <value><c>true</c> if is write to file; otherwise, <c>false</c>.</value>
        public static bool isWriteToFile { get { return _isWriteToFile; } }

        /// <summary>
        /// Gets or sets the deep.<para></para>
        /// when you get then set to -1 (only once effect).
        /// </summary>
        /// <value>The deep, when get then set to -1.</value>
        public static int deep
        {
            get
            {
                int tmp = _deep;
                _deep = -1;
                return tmp;
            }
            set
            {
                _deep = value;
            }
        }

        /// <summary>
        /// Sets the debug level.<para></para>
        /// e.g. <para></para>
        /// set 0xF to enable all log;<para></para>
        /// set 0x0 to close all log; <para></para>
        /// set 0xE means Error closed and others opened.
        /// </summary>
        /// <param name="level">Level, after convert to binary, each byte means isWarnOn，isInfoOn，isDebugOn，isErrorOn</param>
        public static void SetDebugLevel(int level)
        {
            MgrData.Set(MgrData.sysSettingsName, "XLAF.debug", level);
            _isErrorOn = _GetByteInfo(level, 0) == 1;
            _isDebugOn = _GetByteInfo(level, 1) == 1;
            _isInfoOn = _GetByteInfo(level, 2) == 1;
            _isWarnOn = _GetByteInfo(level, 3) == 1;
        }

        /// <summary>
        /// Write to file or not.
        /// </summary>
        /// <param name="isOn">If set to <c>true</c> is on.</param>
        public static void SetWriteToFile(bool isOn)
        {
            _isWriteToFile = isOn;
        }

        /// <summary>
        /// Sets the XLAF inner log shown.
        /// </summary>
        /// <param name="isShow">If set to <c>true</c> is show.</param>
        public static void SetInnerLogShown(bool isShow)
        {
            XLAFInnerLog.SetShown(isShow);
        }

        /// <summary>
        /// Debug.
        /// </summary>
        /// <param name="objs">Objects.</param>
        public static void Debug(params object[] objs)
        {
            if (!_isDebugOn)
                return;
            string time = System.DateTime.Now.ToString("MM-dd HH:mm:ss:fff");
            int deepth = deep;
            if (deepth == -1)
                deepth = 2;
            string line = _GetCodeLineAndFile(deepth);
            string s = time + "|" + line + _ParamsToString(objs);
            _NativeLog(LogLevel.d, s);
            if (_isWriteToFile)
                ModUtils.WriteToFile(debug_file, s + "\n");
        }

        /// <summary>
        /// Error.
        /// </summary>
        /// <param name="objs">Objects.</param>
        public static void Error(params object[] objs)
        {
            if (!_isErrorOn)
                return;

            string time = System.DateTime.Now.ToString("MM-dd HH:mm:ss:fff");
            int deepth = deep;
            if (deepth == -1)
                deepth = 3;
            string line = _GetCodeLineAndFile(deepth);
            string s = time + "|" + line + _ParamsToString(objs);
            _NativeLog(LogLevel.e, s);
            if (_isWriteToFile)
                ModUtils.WriteToFile(error_file, s + "\n");
        }

        /// <summary>
        /// Warning.
        /// </summary>
        /// <param name="objs">Objects.</param>
        public static void Warning(params object[] objs)
        {
            if (!_isWarnOn)
                return;
            string time = System.DateTime.Now.ToString("MM-dd HH:mm:ss:fff");
            int deepth = deep;
            if (deepth == -1)
                deepth = 3;
            string line = _GetCodeLineAndFile(deepth);
            string s = time + "|warning|" + line + _ParamsToString(objs);
            _NativeLog(LogLevel.w, s);
            if (_isWriteToFile)
                ModUtils.WriteToFile(debug_file, s + "\n");
        }

        /// <summary>
        /// Info.
        /// </summary>
        /// <param name="objs">Objects.</param>
        public static void Info(params object[] objs)
        {
            if (!_isInfoOn)
                return;

            string time = System.DateTime.Now.ToString("MM-dd HH:mm:ss:fff");
            int deepth = deep;
            if (deepth == -1)
                deepth = 3;
            string line = _GetCodeLineAndFile(deepth);
            string s = time + "|PrintInfo|" + line + _ParamsToString(objs);
            _NativeLog(LogLevel.i, s);
            if (_isWriteToFile)
                ModUtils.WriteToFile(debug_file, s + "\n");
        }

        #endregion

        #region private functions

#if UNITY_IPHONE && !UNITY_EDITOR
		[DllImport ("__Internal")]
		static extern void _log (string log);
#endif

        /// <summary>
        /// Natives log java or obj-C.
        /// </summary>
        /// <param name="logLevel">Log level <c>V,D,I,W,E</c>.</param>
        /// <param name="msg">Message.</param>
        private static void _NativeLog(string logLevel, string msg)
        {
#if UNITY_EDITOR
            if (logLevel == LogLevel.v)
            {
                UnityEngine.Debug.Log(msg);
            }
            else if (logLevel == LogLevel.d)
            {
                UnityEngine.Debug.Log(msg);
            }
            else if (logLevel == LogLevel.i)
            {
                UnityEngine.Debug.Log(msg);
            }
            else if (logLevel == LogLevel.w)
            {
                UnityEngine.Debug.LogWarning(msg);
            }
            else if (logLevel == LogLevel.e)
            {
                UnityEngine.Debug.LogError(msg);
            }
#elif UNITY_ANDROID
			AndroidJavaClass jc = new AndroidJavaClass ("plugintest.albert.mylibrary.AndroidLog"); 
			jc.CallStatic (logLevel, msg);
#elif UNITY_IPHONE
			_log (logLevel+" | "+msg);
#else
			if (logLevel == LogLevel.v) {
			UnityEngine.Debug.Log (msg);
			} else if (logLevel == LogLevel.d) {
			UnityEngine.Debug.Log (msg);
			} else if (logLevel == LogLevel.i) {
			UnityEngine.Debug.Log (msg);
			} else if (logLevel == LogLevel.w) {
			UnityEngine.Debug.LogWarning (msg);
			} else if (logLevel == LogLevel.e) {
			UnityEngine.Debug.LogError (msg);
			}
#endif

        }

        /// <summary>
        /// Gets the byte info.
        /// </summary>
        /// <returns>The byte info.</returns>
        /// <param name="num">number to convert</param>
        /// <param name="index">the index, the last byte index is 0</param>
        private static int _GetByteInfo(int num, int index)
        {
            return (num & (0x1 << index)) >> index;
        }

        /// <summary>
        /// join parameterses to string.
        /// </summary>
        /// <returns>The to string.</returns>
        /// <param name="objs">Objects.</param>
        private static string _ParamsToString(params object[] objs)
        {
            string s = "";
            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i] != null)
                {
                    s = s + objs[i].ToString() + "\t";
                }
                else
                {
                    s = s + "null\t";
                }
            }
            return s;
        }

        /// <summary>
        /// Gets the code line and file name.
        /// </summary>
        /// <returns>The code line and file.</returns>
        /// <param name="deep">depth, default is 2.</param>
        private static string _GetCodeLineAndFile(int deep = 2)
        {
            StackTrace insStackTrace = new StackTrace(true);
            StackFrame insStackFrame = insStackTrace.GetFrame(deep);
            string filename = Path.GetFileName(insStackFrame.GetFileName());
            return String.Format("{0}:{1}|\t ", filename, insStackFrame.GetFileLineNumber());
        }

        #endregion

#if UNITY_EDITOR
        [UnityEditor.Callbacks.OnOpenAssetAttribute(0)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            string stackTrace = GetStackTrace();
            if (!string.IsNullOrEmpty(stackTrace) && stackTrace.Contains("|"))
            {
                Match matches = Regex.Match(stackTrace, @"\(at (.+)\)", RegexOptions.IgnoreCase);
                string pathLine = "";
                while (matches.Success)
                {
                    pathLine = matches.Groups[1].Value;
                    if (!pathLine.Contains("Log.cs"))
                    {
                        int splitIndex = pathLine.LastIndexOf(":");
                        string path = pathLine.Substring(0, splitIndex);
                        line = System.Convert.ToInt32(pathLine.Substring(splitIndex + 1));
                        string fullPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf("Assets"));
                        fullPath = fullPath + path;
                        if (Application.platform == RuntimePlatform.OSXEditor)
                        {
                            UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(fullPath, line);
                        }
                        else if (Application.platform == RuntimePlatform.WindowsEditor)
                        {
                            UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(fullPath.Replace('/', '\\'), line);
                        }
                        else
                        {
                            UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(fullPath, line);
                        }

                        break;
                    }
                    matches = matches.NextMatch();
                }
                return true;
            }
            return false;
        }

        private static string GetStackTrace()
        {
            var consoleWindowType = typeof(EditorWindow).Assembly.GetType("UnityEditor.ConsoleWindow");
            FieldInfo fieldInfo = consoleWindowType.GetField("ms_ConsoleWindow", BindingFlags.Static | BindingFlags.NonPublic);
            object consoleWindowInstance = fieldInfo.GetValue(null);
            if (consoleWindowInstance != null)
            {
                if ((object)EditorWindow.focusedWindow == consoleWindowInstance)
                {
                    //get listViewState in consuleWindow

                    //    var listViewStateType = typeof(EditorWindow).Assembly.GetType ("UnityEditor.ListViewState");
                    //    fieldInfo = consoleWindowType.GetField ("m_ListView", BindingFlags.Instance | BindingFlags.NonPublic);
                    //    object listView = fieldInfo.GetValue (consoleWindowInstance);

                    //get row in listViewState
                    //    fieldInfo = listViewStateType.GetField ("row", BindingFlags.Instance | BindingFlags.Public);
                    //    int row = (int)fieldInfo.GetValue (listView);
                    //get m_ActiveText in consoleWindow
                    fieldInfo = consoleWindowType.GetField("m_ActiveText", BindingFlags.Instance | BindingFlags.NonPublic);
                    string activeText = fieldInfo.GetValue(consoleWindowInstance).ToString();
                    return activeText;
                }
            }
            return null;
        }
#endif


    }

}