using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using SimpleJSON;
using XLAF.Public;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class Scene1 : Storyboard
{
    
	public override void OnUIEvent (UIEvent e)
	{
		Log.Debug ("OnUIEvent", e);
		if (e.phase == TouchPhase.Ended) {
			if (e.target.name == "btn1") {

				//        MgrAudio.PlaySound ("s_click");
				MgrScene.GotoScene ("Scene2", "0000123", Main.anim, 1f, () => {
					Log.Debug ("Scene2 Done~");
				});
				//

//                MgrPopup.Show ("Popup1", "55892", SceneAnimation.fade, 1f, () => {
//                    Log.Debug ("Popup1 Done~");
//                });

				#if UNITY_ANDROID && !UNITY_EDITOR
                AndroidJavaClass jc = new AndroidJavaClass ("plugintest.albert.mylibrary.PhoneInfo"); 
                string module = jc.CallStatic<string> ("getPhoneModule");

                AndroidJavaObject telephonyManager = new AndroidJavaObject ("android.telephony.TelephonyManager");
                string did = telephonyManager.Call<string> ("getLine1Number");

//                string did = jc.CallStatic<string> ("getDeviceId");
                string totalMemory = jc.CallStatic<string> ("getTotalMemory");
//                string phoneNumber = jc.CallStatic<string> ("getPhoneNumber");
                string[] cpu = jc.CallStatic<string[]> ("getCpuInfo");
                Log.Debug ("~~~~~:", module, "|", did, "|", totalMemory, "|", cpu [0], "|", cpu [1]);
				#endif
			} else if (e.target.name == "btn2") {
				MgrPopup.Show ("Popup2", "2222", SceneAnimation.fade, 1f, () => {
					Log.Debug ("Popup2 Done~");
				});


			}
		}
	}

	private IEnumerator loadBundleAll (string path)
	{
		WWW bundle = new WWW (path);
		GameObject scene = (GameObject)UnityEngine.Object.Instantiate (bundle.assetBundle.LoadAsset ("Popup1"));
		Log.Debug ("scene:", scene.name);
		bundle.assetBundle.Unload (false);
		yield return 1;
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
		Log.Debug ("scene1 creat_scene", obj);

//		string fn = ModUtils.documentsDirectory + "/test.jsn";
//		string def = ModUtils.streamingDirectory + "/test.jsn";
//		MgrData.AddSetting ("test", fn, def);
		MgrAudio.PlayMusic ("m_start.mp3", 0.5f);
		ModDispatcher.AddListener ("dia2", (XLAF_Event e) => {
			Log.Debug ("dia2:", e.ToString ());
		});

		MgrBackdoor.SetBackdoor (ModUIUtils.GetChild (transform, "lbl_text"), (isHandled, cmd) => {
			Log.Debug (isHandled, cmd);
		});
	}


	public override void WillEnterScene (object obj)
	{
		Log.Debug ("scene1 will_enter_scene", obj);

	}

	public override void EnterScene (object obj)
	{
		Log.Debug ("scene1 enter_scene", obj, this.sceneName);

		Log.Debug (MgrData.ToString ());


	}

	public override void WillExitScene ()
	{
		Log.Debug ("scene1 will_exit_scene");
	}

	public override void ExitScene ()
	{
		Log.Debug ("scene1 exit_scene");
	}

	public override void DestroyScene ()
	{
		Log.Debug ("scene1 destroy_scenee");
	}

	public override void OverlayBegan (string overlaySceneceneName)
	{
		Log.Debug ("scene1 OverlayBegan", overlaySceneceneName);
	}

	public override void OverlayEnded (string overlaySceneceneName)
	{
		Log.Debug ("scene1 OverlayEnded", overlaySceneceneName);
	}

	public override void UpdateLanguage ()
	{
		Log.Debug ("scene1 UpdateLanguage");
	}

	#if UNITY_ANDROID
	public override void AndroidGoBack ()
	{
		Log.Debug ("scene1 AndroidGoBack");
	}
	#endif
	#endregion

}
