using UnityEngine;
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
using SimpleJSON;

public class Main : MonoBehaviour
{
	public static SceneAnimation anim = SceneAnimation.zoomOut;

	void Init ()
	{
		Log.SetDebugLevel (0xF);
		XLAFMain.Init ();
		MgrScene.destoryOnSceneChange = false;

		Application.targetFrameRate = 60;
		MgrFPS.ShowFPS ();

//		LogManager.Print ("This is test message!!!");
	}

	// Use this for initialization
	void Start ()
	{
		Init ();

		MgrScene.SetViewRoot (ModUIUtils.GetChild<Transform> (transform, "SceneViewRoot"));
		MgrDialog.SetDialogRoot (ModUIUtils.GetChild<Transform> (transform, "DialogViewRoot"));

		MgrScene.GotoScene ("Scene1", "main", SceneAnimation.fade, 0.3f, () => {
			
			Log.Debug ("main Scene1 Done~",ModUtils.documentsDirectory);


		});
	}
	// Update is called once per frame
	void Update ()
	{
		//		Log.Debug (MgrScene.isSceneChanging ());
	}




}
