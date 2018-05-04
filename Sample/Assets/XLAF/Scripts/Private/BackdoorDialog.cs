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

			if (cmd.StartsWith ("setdebug")) {
				isHandled = true;
				string level = cmd.Replace ("setdebug", "").Trim ();
				int val = Convert.ToInt32 (level, 16);
				Log.Debug (val);
				Log.SetDebugLevel (val);
			} else if (cmd == "") {
			}




			if (callback != null) {
				callback.Invoke (isHandled, cmd);
			}
		}

		#region  Storyboard Listeners

		public override void OnUIEvent (UIEvent e)
		{
			if (e.phase == TouchPhase.Ended) {
				if (e.target.name == "btn_ok") {
					InvokeCallback (inputField.text.ToLower ());
					MgrDialog.HideDialog ("XLAFBackdoor", SceneAnimation.none);
				} else if (e.target.name == "btn_cancel") {
					MgrDialog.HideDialog ("XLAFBackdoor", SceneAnimation.none);
				}
			}
		}

		/*
        CreatScene 加载完界面，还未播放动画（只有界面加载的时候，才会触发；读取缓存界面不会触发）
        WillEnterScene 加载完毕scene，即将播放过渡动画
        EnterScene 播放完毕过渡动画

        WillExitScene 即将播放退出界面的动画
        ExitScene 播放完退出界面的动画
        DestoryScene 销毁界面前触发

        OverlayBegan 界面被遮挡时触发，仅计算XLAF框架内的遮挡
        OverlayEnded 界面取消遮挡时触发，仅计算XLAF框架内的遮挡
        AndroidGoBack Android系统下按实体back按钮触发
        UpdateLanguage 更新语言时、CreatScene之后触发，用于更新界面文字
    */
		public override void CreatScene (object obj)
		{
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

		public override void DestoryScene ()
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