using UnityEngine;
using System.Collections;
using System.Diagnostics;
using System;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Reflection;
using System.Text.RegularExpressions;
using SimpleJSON;

namespace XLAF.Public
{
	/// <summary>
	/// Log
	/// </summary>
	public class Log
	{

		private static bool _isWriteToFile = false;

		private static bool _isWarnOn = false;
		private static bool _isInfoOn = false;
		private static bool _isDebugOn = false;
		private static bool _isErrorOn = false;

		private static string debug_file;
		private static string error_file;

		/// <summary>
		/// Gets a value indicating whether this <see cref="XLAF.Public.Log"/> is warn on.
		/// </summary>
		/// <value><c>true</c> if is warn on; otherwise, <c>false</c>.</value>
		public static bool isWarnOn{ get { return _isWarnOn; } }

		/// <summary>
		/// Gets a value indicating whether this <see cref="XLAF.Public.Log"/> is info on.
		/// </summary>
		/// <value><c>true</c> if is info on; otherwise, <c>false</c>.</value>
		public static bool isInfoOn{ get { return _isInfoOn; } }

		/// <summary>
		/// Gets a value indicating whether this <see cref="XLAF.Public.Log"/> is debug on.
		/// </summary>
		/// <value><c>true</c> if is debug on; otherwise, <c>false</c>.</value>
		public static bool isDebugOn { get { return _isDebugOn; } }

		/// <summary>
		/// Gets a value indicating whether this <see cref="XLAF.Public.Log"/> is error on.
		/// </summary>
		/// <value><c>true</c> if is error on; otherwise, <c>false</c>.</value>
		public static bool isErrorOn{ get { return _isErrorOn; } }

		/// <summary>
		/// Gets a value indicating whether this <see cref="XLAF.Public.Log"/> is write to file.
		/// </summary>
		/// <value><c>true</c> if is write to file; otherwise, <c>false</c>.</value>
		public static bool isWriteToFile{ get { return _isWriteToFile; } }

		/// <summary>
		/// Initializes the <see cref="XLAF.Public.Log"/> class.
		/// </summary>
		static  Log ()
		{
			debug_file = ModUtils.documentsDirectory + "/debug.log";
			error_file = ModUtils.documentsDirectory + "/error.log";
		}


		/// <summary>
		/// 调用Init会触发构造函数，可以用于统一初始化的时候
		/// </summary>
		public static void Init ()
		{
			
		}

		/// <summary>
		/// Sets the debug level.
		/// 
		/// 例如：0xF代表开启所有Log; 0x0代表关闭所有Log; 0xE代表Error关闭，其他的开启，具体请看参数说明
		/// </summary>
		/// <param name="level">Level. 转化为2进制后，每一位代表的分别是：isWarnOn，isInfoOn，isDebugOn，isErrorOn</param>
		public static void SetDebugLevel (int level)
		{
			_isErrorOn = _GetByteInfo (level, 0) == 1;
			_isDebugOn = _GetByteInfo (level, 1) == 1;
			_isInfoOn = _GetByteInfo (level, 2) == 1;
			_isWarnOn = _GetByteInfo (level, 3) == 1;
			//			UnityEngine.Debug.LogError (_ParamsToString (isWarnOn, isInfoOn, isDebugOn, isErrorOn));
		}

		public static void SetWriteToFile (bool isOn)
		{
			_isWriteToFile = isOn;
		}

		public static void Debug (params object[] objs)
		{
			if (!_isDebugOn)
				return;
			string time = System.DateTime.Now.ToString ("MM-dd HH:mm:ss:fff");
			string line = _GetCodeLineAndFile ();
			string s = time + "|" + line + _ParamsToString (objs);
			UnityEngine.Debug.Log (s);
			if (_isWriteToFile)
				ModUtils.WriteToFile (debug_file, s + "\n");
		}

		public static void Error (params object[] objs)
		{
			if (!_isErrorOn)
				return;
			
			string time = System.DateTime.Now.ToString ("MM-dd HH:mm:ss:fff");
			string line = _GetCodeLineAndFile (3);
			string s = time + "|" + line + _ParamsToString (objs);
			UnityEngine.Debug.LogError (s);
			if (_isWriteToFile)
				ModUtils.WriteToFile (error_file, s + "\n");
		}

		public static void Warning (params object[] objs)
		{
			string time = System.DateTime.Now.ToString ("MM-dd HH:mm:ss:fff");
			string line = _GetCodeLineAndFile (3);
			string s = time + "|warning|" + line + _ParamsToString (objs);
			UnityEngine.Debug.LogWarning (s);
			if (_isWriteToFile)
				ModUtils.WriteToFile (debug_file, s + "\n");
		}

		public static void Info (params object[] objs)
		{
			if (!_isInfoOn)
				return;
            
			string time = System.DateTime.Now.ToString ("MM-dd HH:mm:ss:fff");
			string line = _GetCodeLineAndFile (3);
			string s = time + "|PrintInfo|" + line + _ParamsToString (objs);
			UnityEngine.Debug.Log (s);
		}

		/// <summary>
		/// Gets the byte info.
		/// </summary>
		/// <returns>The byte info.</returns>
		/// <param name="num">要获取二进制值的数</param>
		/// <param name="index">倒数第一位为0，依次类推</param>
		private static int _GetByteInfo (int num, int index)
		{
			return (num & (0x1 << index)) >> index;
		}

		private static string _ParamsToString (params object[] objs)
		{
			string s = "";
			for (int i = 0; i < objs.Length; i++) {
				if (objs [i] != null) {
					s = s + objs [i].ToString () + "\t";
				} else {
					s = s + "null\t";
				}
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

					//    var listViewStateType = typeof(EditorWindow).Assembly.GetType ("UnityEditor.ListViewState");
					//    fieldInfo = consoleWindowType.GetField ("m_ListView", BindingFlags.Instance | BindingFlags.NonPublic);
					//    object listView = fieldInfo.GetValue (consoleWindowInstance);

					//get row in listViewState
					//    fieldInfo = listViewStateType.GetField ("row", BindingFlags.Instance | BindingFlags.Public);
					//    int row = (int)fieldInfo.GetValue (listView);
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