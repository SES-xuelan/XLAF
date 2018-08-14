using UnityEngine;
using System.Collections;
using XLAF.Public;
using System;
using UnityEngine.UI;

namespace XLAF.Private
{
	public class BackdoorDialog : Storyboard
	{
		private Action<bool,string> callback;
		private InputField inputField;

		private void InvokeCallback (string cmd)
		{
			bool isHandled = false;
			try {

				if (cmd.StartsWith ("setdebug")) {
					isHandled = true;
					string level = cmd.Replace ("setdebug", "").Trim ();
					int val = Convert.ToInt32 (level, 16);
					Log.Debug (val);
					Log.SetDebugLevel (val);
				} else if (cmd == "") {
				}




			} catch (Exception e) {
				XLAFInnerLog.Warning ("Backdoor error!! " + e.ToString ());
			} finally {
				if (callback != null) {
					callback (isHandled, cmd);
				}
			}
		}

		#region  Storyboard Listeners

		public override void OnUIEvent (XLAF_UIEvent e)
		{
			if (e.phase == Phase.Click) {
				if (e.target.name == "btn_ok") {
					InvokeCallback (inputField.text.ToLower ());
					MgrPopup.Hide ("XLAFBackdoor", SceneAnimation.none);
				} else if (e.target.name == "btn_cancel") {
					MgrPopup.Hide ("XLAFBackdoor", SceneAnimation.none);
				}
			}
		}

		/*
	    Each storyboard function called moment:
        CreateScene     => after finish load scene before play enter animation(only call after load from prefab or asset bundle, read cache from MgrScene will not called).
        WillEnterScene => after CreateScene, at the begin of play enter animation.
        EnterScene     => at the end of play enter animation.

        WillExitScene  => at the begin of play exit animation.
        ExitScene      => at the end of play exit animation.
        DestroyScene   => when destroy the scene(before destroy).

        OverlayBegan   => when scene overlaid(only XLAF dialog).
        OverlayEnded   => when scene overlaid object disappear(only XLAF dialog).
        AndroidGoBack  => in Android, user press back button.
        UpdateLanguage => when update language or after CreateScene.
    */
		public override void CreateScene (object obj)
		{
			BindAllButtonsClickEvent ();
			callback = (Action<bool,string>)obj;
			inputField = ModUIUtils.GetChild<InputField> (transform, "cmd");
		}

		public override void WillEnterScene (object obj)
		{
		}

		public override void EnterScene (object obj)
		{
		}

		public override void WillExitScene ()
		{
		}

		public override void ExitScene ()
		{
		}

		public override void DestroyScene ()
		{
		}

		public override void UpdateLanguage ()
		{

		}
		#if UNITY_ANDROID
		public override void AndroidGoBack ()
		{
		}
		#endif
		#endregion
	}
}