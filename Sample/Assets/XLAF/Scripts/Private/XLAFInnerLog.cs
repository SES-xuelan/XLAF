using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLAF.Public;

namespace XLAF.Private
{
	/// <summary>
	/// Inner log, only use for XLAF.
	/// </summary>
	public class XLAFInnerLog
	{

		#region private variables

		private static bool _isShow = false;

		#endregion

		#region constructed function & initialization

		static  XLAFInnerLog ()
		{
		}

		/// <summary>
		/// call Init() will trigger constructed function, you can call Init() to ensure this class finished initialization
		/// </summary>
		public static void Init ()
		{

		}

		public static void SetShown(bool isShow){
			_isShow = isShow;
		}
		#endregion

		#region public functions

		/// <summary>
		/// Debug.
		/// </summary>
		/// <param name="objs">Objects.</param>
		public static void Debug (params object[] objs)
		{
			if (!_isShow)
				return;
			Log.deep = 3;
			Log.Debug (objs);
		}

		/// <summary>
		/// Error.
		/// </summary>
		/// <param name="objs">Objects.</param>
		public static void Error (params object[] objs)
		{
			if (!_isShow)
				return;
			Log.deep = 4;
			Log.Error (objs);
		}

		/// <summary>
		/// Warning.
		/// </summary>
		/// <param name="objs">Objects.</param>
		public static void Warning (params object[] objs)
		{
			if (!_isShow)
				return;
			Log.deep = 4;
			Log.Warning (objs);
		}

		/// <summary>
		/// Info.
		/// </summary>
		/// <param name="objs">Objects.</param>
		public static void Info (params object[] objs)
		{
			if (!_isShow)
				return;
			Log.deep = 4;
			Log.Info (objs);
		}

		#endregion

	}
}