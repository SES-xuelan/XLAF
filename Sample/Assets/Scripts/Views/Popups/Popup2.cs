using UnityEngine;
using System.Collections;
using XLAF.Public;

public class Popup2 : Storyboard
{

    void Start ()
    {
	
    }

    void Update ()
    {
	
    }



    public override void OnUIEvent (XLAF_UIEvent e)
    {
        //        Log.Debug ("OnUIEvent", e);
        if (e.phase == Phase.Click) {
            if (e.target.name == "btn_button") {
                btn_click ();
            }
        }
    }


    private void btn_click ()
    {
        MgrAudio.PlaySound ("s_click.mp3");
//        foreach (string s in MgrScene.GetAllScenes().Keys) {
//            Log.Debug (s);
//        }
//        MgrPopup.Hide ("Popup1", SceneAnimation.fade, null);
        MgrPopup.HideTop ();
//        MgrPopup.Show ("Popup1", "55892", SceneAnimation.fade, 1f, () => {
//            Log.Debug ("Popup1 Done~");
//        });
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

        AndroidGoBack  => in Android, user press back button.
        UpdateLanguage => when update language or after CreatScene.
    */

    public override void CreatScene (object obj)
	{
		BindAllButtonsClickEvent ();
    }

    public override void WillEnterScene (object obj)
    {
    }

    public override void EnterScene (object obj)
    {
        ModDispatcher.Dispatch ("dia2","123546");
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

    #if UNITY_ANDROID
    public override void AndroidGoBack ()
    {
    }
    #endif
    #endregion
}
