using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using XLAF.Public;

public class Scene2 : Storyboard
{
    

	public override void OnUIEvent (UIEvent e)
	{
		if (e.phase == TouchPhase.Ended) {
			if (e.target.name == "Button") {
				btn_click ();
			}
		}
	}

	private void btn_click ()
	{
		MgrAudio.PlaySound ("s_click.mp3");
		MgrScene.GotoScene ("Scene1", 998855, Main.anim, 1f, cb);

	}

	private void cb ()
	{
//        Log.Debug ("Scene1 Done~");

	}



	#region  Storyboard Listeners

	/*
        creat_scene 加载完界面，还未播放动画（只有界面加载的时候，才会触发；读取缓存界面不会触发）
        will_enter_scene 加载完毕scene，即将播放过渡动画
        enter_scene 播放完毕过渡动画

        will_exit_scene 即将播放退出界面的动画
        exit_scene 播放完退出界面的动画
        destory_scene 销毁界面前触发
    */
	public override void CreatScene (object obj)
	{
		Log.Debug ("scene2 creat_scene", obj);
	}

	public override void WillEnterScene (object obj)
	{
		Log.Debug ("scene2 will_enter_scene", obj);
	}

	public override void EnterScene (object obj)
	{
		Log.Debug ("scene2 enter_scene", obj, this.sceneName);
	}

	public override void WillExitScene ()
	{
		Log.Debug ("scene2 will_exit_scene");
	}

	public override void ExitScene ()
	{
		Log.Debug ("scene2 exit_scene");
	}

	public override void DestoryScene ()
	{
		Log.Debug ("scene2 destory_scenee");
	}

	public override void OverlayBegan (string overlaySceneceneName)
	{
		Log.Debug ("scene2 OverlayBegan", overlaySceneceneName);
	}

	public override void OverlayEnded (string overlaySceneceneName)
	{
		Log.Debug ("scene2 OverlayEnded", overlaySceneceneName);
	}

	public override void UpdateLanguage ()
	{
		Log.Debug ("scene2 UpdateLanguage");
	}
	#if UNITY_ANDROID
    public override void AndroidGoBack ()
    {
        Log.Debug ("scene2 AndroidGoBack");
    }
    #endif
	#endregion

}
