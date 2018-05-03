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
        creat_scene 加载完界面，还未播放动画（只有界面加载的时候，才会触发；读取缓存界面不会触发）
        will_enter_scene 加载完毕scene，即将播放过渡动画
        enter_scene 播放完毕过渡动画

        will_exit_scene 即将播放退出界面的动画
        exit_scene 播放完退出界面的动画
        destory_scene 销毁界面前触发
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

    public override void DestoryScene ()
    {
    }

    #if UNITY_ANDROID
    public override void AndroidGoBack ()
    {
    }
    #endif
    #endregion
}
