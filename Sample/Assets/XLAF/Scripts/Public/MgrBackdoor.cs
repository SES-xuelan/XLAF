using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace XLAF.Public
{
	/// <summary>
	/// backdoor manager.<para></para>
	/// click  BACKDOOR_CLICK_TIMES in BACKDOOR_OUT_OF_SECONDS will show backdoor.<para></para>
	/// !!WARNNING!!<para></para>
	/// ONLY use back door for debug!!!
	/// </summary>
	public class MgrBackdoor:MonoBehaviour
	{
		private static readonly int BACKDOOR_CLICK_TIMES = 30;
		private static readonly int BACKDOOR_OUT_OF_SECONDS = 10;

		#region private variables

		private static Action<bool,string> callback;
		private static int clickedTimes = 0;

		private static MgrBackdoor instance = null;

		private bool isRunning = false;

		#endregion

		#region constructed function & initialization

		static MgrBackdoor ()
		{
			instance = XLAFMain.XLAFGameObject.AddComponent<MgrBackdoor> ();
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
		/// Sets the backdoor.
		/// Callback<isHandled,cmdString>
		/// </summary>
		/// <param name="transform">Transform.</param>
		/// <param name="callback">Callback<isHandled,cmd>.</param>
		public static void SetBackdoor (Transform transform, Action<bool,string> callback)
		{
			SetBackdoor (transform.gameObject, callback);
		}

		/// <summary>
		/// Sets the backdoor.
		/// Callback<isHandled,cmdString>
		/// </summary>
		/// <param name="transform">Transform.</param>
		/// <param name="callback">Callback<isHandled,cmd>.</param>
		public static void SetBackdoor (GameObject gameObject, Action<bool,string> callback)
		{
			XLAFEventTriggerListener.Get (gameObject).onClick = OnClick;
			MgrBackdoor.callback = callback;
		}

		#endregion

		#region private functions

		private static void OnClick (GameObject go)
		{
//			Log.Debug ("backdoor click!");
			clickedTimes++;
			if (clickedTimes == 1) {
				instance.TimeOutBegan ();
			}
			if (clickedTimes >= BACKDOOR_CLICK_TIMES) {
				instance.TimeOutEnded ();
				//after backdoor shown, click 2 times will show backdoor again
				clickedTimes = BACKDOOR_CLICK_TIMES - 2;
				MgrDialog.ShowDialog ("XLAFBackdoor", MgrBackdoor.callback, SceneAnimation.none);
			}
		}

		private void TimeOutBegan ()
		{
			if (isRunning) {
				return;
			}
			isRunning = true;
			Invoke ("TimeOutEnded", MgrBackdoor.BACKDOOR_OUT_OF_SECONDS);
		}

		private void TimeOutEnded ()
		{
			if (!isRunning) {
				return;
			}
			Log.Debug ("TimeOutEnded");
			isRunning = false;
			MgrBackdoor.clickedTimes = 0;
		}

		#endregion
	}

}