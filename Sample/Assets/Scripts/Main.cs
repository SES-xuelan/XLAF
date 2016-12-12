﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using XLAF.Public;

/*
创建某个类型的第一个实例时,所进行的操作顺序为:
1.静态变量设置为0
2.执行静态变量初始化器
3.执行基类的静态构造函数
4.执行静态构造函数
5.实例变量设置为0
6.执行实例变量初始化器
7.执行基类中合适的实例构造函数
8.执行实例构造函数
*/

public class Main : MonoBehaviour
{
    void Init ()
    {
        MgrScene.destoryOnSceneChange = false;
        Application.targetFrameRate = 60;
        XLAFMain.Init ();
    }

    // Use this for initialization
    void Start ()
    {
        Init ();

        MgrScene.SetViewRoot (ModUIUtils.getChild<Transform> (transform, "SceneViewRoot"));
        MgrDialog.SetDialogRoot (ModUIUtils.getChild<Transform> (transform, "DialogViewRoot"));

        MgrScene.GotoScene ("Scene1", "main", SceneAnimation.fade, 0.3f, () => {

            Log.Debug ("main Scene1 Done~");

        });
    }
    // Update is called once per frame
    void Update ()
    {
        //		Log.Debug (MgrScene.isSceneChanging ());
    }



    public static SceneAnimation anim = SceneAnimation.zoomOut;

}