using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace XLAF.Public
{
	/// <summary>
	/// backdoor manager.
	/// 后门，在BACKDOOR_OUT_OF_SECONDS秒内点击BACKDOOR_CLICK_TIMES次则显示后门
	/// </summary>
	public class MgrBackdoor:MonoBehaviour
	{
		private static readonly int BACKDOOR_CLICK_TIMES = 30;
		private static readonly int BACKDOOR_OUT_OF_SECONDS = 10;

		private static Action<bool,string> callback;
		private static int clickedTimes = 0;

		private static MgrBackdoor instance = null;

		private bool isRunning = false;

		static MgrBackdoor ()
		{
			instance = XLAFMain.XLAFGameObject.AddComponent<MgrBackdoor> ();
		}

		/// <summary>
		/// Sets the backdoor.
		/// Callback<isHandled,cmd>
		/// </summary>
		/// <param name="transform">Transform.</param>
		/// <param name="callback">Callback<isHandled,cmd>.</param>
		public static void SetBackdoor (Transform transform, Action<bool,string> callback)
		{
			SetBackdoor (transform.gameObject, callback);
		}

		/// <summary>
		/// Sets the backdoor.
		/// Callback<isHandled,cmd>
		/// </summary>
		/// <param name="transform">Transform.</param>
		/// <param name="callback">Callback<isHandled,cmd>.</param>
		public static void SetBackdoor (GameObject gameObject, Action<bool,string> callback)
		{
			XLAFEventTriggerListener.Get (gameObject).onClick = OnClick;
			MgrBackdoor.callback = callback;
		}

		private static void OnClick (GameObject go)
		{
//			Log.Debug ("backdoor click!");
			clickedTimes++;
			if (clickedTimes == 1) {
				instance.TimeOutBegan ();
			}
			if (clickedTimes >= BACKDOOR_CLICK_TIMES) {
				instance.TimeOutEnded ();
				clickedTimes = BACKDOOR_CLICK_TIMES - 2;//点出来过后门之后，点击2次就出现
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
	}

}