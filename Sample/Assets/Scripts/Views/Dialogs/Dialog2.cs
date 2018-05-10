using UnityEngine;
using System.Collections;
using XLAF.Public;

public class Dialog2 : Storyboard
{

    void Start ()
    {
	
    }

    void Update ()
    {
	
    }



    public override void OnUIEvent (UIEvent e)
    {
        //        Log.Debug ("OnUIEvent", e);
        if (e.phase == TouchPhase.Ended) {
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
//        MgrDialog.HideDialog ("Dialog1", SceneAnimation.fade, null);
        MgrDialog.HideTop ();
//        MgrDialog.ShowDialog ("Dialog1", "55892", SceneAnimation.fade, 1f, () => {
//            Log.Debug ("Dialog1 Done~");
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
