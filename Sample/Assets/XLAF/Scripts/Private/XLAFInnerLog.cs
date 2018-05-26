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

		#endregion

		#region public functions

		/// <summary>
		/// Show or hide inner log.
		/// </summary>
		/// <param name="showOrHide">If set to <c>true</c> show.</param>
		public static void ShowOrHide (bool showOrHide)
		{
			_isShow = showOrHide;
		}

		/// <summary>
		/// Debug.
		/// </summary>
		/// <param name="objs">Objects.</param>
		public static void Debug (params object[] objs)
		{
			if (!_isShow)
				return;
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
			Log.Info (objs);
		}

		#endregion

	}
}