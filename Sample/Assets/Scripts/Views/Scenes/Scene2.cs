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
	    Each storyboard function called moment:
        CreatScene     => after finish load scene before play enter animation(only call after load from prefab or asset bundle, read cache from MgrScene will not called).
        WillEnterScene => after CreatScene, at the begin of play enter animation.
        EnterScene     => at the end of play enter animation.

        WillExitScene  => at the begin of play exit animation.
        ExitScene      => at the end of play exit animation.
        DestroyScene   => when destroy the scene(before destroy).

        OverlayBegan   => when scene overlaid(only XLAF popup).
        OverlayEnded   => when scene overlaid object disappear(only XLAF popup).
        AndroidGoBack  => in Android, user press back button.
        UpdateLanguage => when update language or after CreatScene.
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

	public override void DestroyScene ()
	{
		Log.Debug ("scene2 destroy_scenee");
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
