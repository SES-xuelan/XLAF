using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace XLAF.Public
{
	/// <summary>
	/// scene管理，和MgrDialog代码差不多
	/// </summary>
	public class MgrScene : MonoBehaviour
	{
		public static bool destoryOnSceneChange = false;


		static MgrScene ()
		{
			SCENES = new Dictionary<string, SceneObject> ();
			instance = XLAFMain.XLAFGameObject.AddComponent<MgrScene> ();



			_screenHeight = Camera.main.orthographicSize * 2;
			float aspectRatio = (float)Screen.width / Screen.height;
			_screenWidth = _screenHeight * aspectRatio;

			Log.Debug ("screenHeight,screenWidth", screenHeight, screenWidth);
		}


		/// <summary>
		/// 调用Init会触发构造函数，可以用于统一初始化的时候
		/// </summary>
		public static void Init ()
		{

		}


		private static MgrScene instance = null;
		private static readonly string scenePathFormat = "Views/Scenes/{0}";


		private static bool animating = false;
		private static SceneObject currentScene = null;
		private static Transform sceneViewRoot = null;
		private static CanvasGroup sceneViewRootCanvas = null;

		public static float screenWidth{ get { return _screenWidth; } }

		public static float screenHeight{ get { return _screenHeight; } }

		public static float screenScale{ get { return  (float)_screenHeight / Screen.height; } }

		private static float _screenWidth;
		private static float _screenHeight;

		private static Dictionary<string,SceneObject> SCENES;

		/// <summary>
		/// Sets the view root.
		/// </summary>
		/// <param name="grp">Transform.</param>
		public static void SetViewRoot (Transform grp)
		{
			sceneViewRoot = grp;
			sceneViewRootCanvas = sceneViewRoot.transform.GetComponent<CanvasGroup> ();

		}


		/// <summary>
		/// Gets the view root.
		/// </summary>
		/// <returns>The view root.</returns>
		public static Transform GetViewRoot ()
		{
			return sceneViewRoot;
		}

		/// <summary>
		/// Gets the root.
		/// </summary>
		/// <returns>The root.</returns>
		public static RectTransform GetRoot ()
		{
			return sceneViewRoot.parent.GetComponent<RectTransform> ();
		}

		/// <summary>
		/// Gets the view root canvas.
		/// </summary>
		/// <returns>The view root canvas.</returns>
		public static CanvasGroup GetViewRootCanvas ()
		{
			return sceneViewRootCanvas;
		}

		/// <summary>
		/// Gets the current scene.
		/// </summary>
		/// <returns>The current scene.</returns>
		public static SceneObject GetCurrentScene ()
		{
			return currentScene;
		}

		/// <summary>
		/// Gets all scenes.
		/// </summary>
		/// <returns>The all scenes.</returns>
		public static Dictionary<string,SceneObject> GetAllScenes ()
		{
			return SCENES;
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="XLAF.Public.MgrScene"/> is scene changing.
		/// </summary>
		/// <value><c>true</c> if is scene changing; otherwise, <c>false</c>.</value>
		public static bool isSceneChanging {
			get {
				return animating;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="XLAF.Public.MgrScene"/> is scene or dialog changing.
		/// </summary>
		/// <value><c>true</c> if is scene or dialog changing; otherwise, <c>false</c>.</value>
		public static bool isSceneOrDialogChanging {
			get {
				return animating || MgrDialog.isDialogChanging;
			}
		}

		/// <summary>
		/// Loads the scene.
		/// 用于scene的东西比较多，需要提前加载的情况
		/// </summary>
		/// <param name="sceneName">Scene name.</param>
		/// <param name="data">要传递给scene的数据</param>
		public static void LoadScene (string sceneName, object data)
		{
			if (SCENES.ContainsKey (sceneName))
				return;
			SceneObject sceneObj = null;
			if (ModAssetBundle.HasAssetBundle (sceneName)) {
				sceneObj = new SceneObject (ModUtils.documentsDirectory+ ModAssetBundle.GetAssetBundlePath (sceneName), sceneName);
			} else {
				sceneObj = new SceneObject (string.Format (scenePathFormat, sceneName));
			}
			SCENES.Add (sceneName, sceneObj);
			sceneObj.script.CreatScene (data);
			sceneObj.script.UpdateLanguage ();

			sceneObj.scene.transform.SetParent (sceneViewRoot, false);
			sceneObj.scene.SetActive (false);
			sceneObj.scene.transform.SetAsFirstSibling ();//置底

		}

		/// <summary>
		/// Loads the scene.
		/// 用于scene的东西比较多，需要提前加载的情况
		/// </summary>
		/// <param name="sceneName">Scene name.</param>
		public static void LoadScene (string sceneName)
		{
			LoadScene (sceneName, "");
		}

		/// <summary>
		/// Returns null if the scene object does not exist
		/// </summary>
		/// <returns>The scene.</returns>
		/// <param name="sceneName">Scene name.</param>
		public static SceneObject GetScene (string sceneName)
		{
			if (!SCENES.ContainsKey (sceneName)) {
				return null;
			}
			return SCENES [sceneName];

            
		}

		#region public GotoScene functions [override 49 times]

		// 1 group parameter => 2
		public static void GotoScene (SceneParams par)
		{
			string sceneName = par.sceneName;
			SceneAnimation anim = par.anim;
			float oldSceneTime = par.oldSceneTime;
			float newSceneTime = par.newSceneTime;
			iTween.EaseType ease = par.ease;
			Action cb = par.cb;
			object data = par.data;


			Log.Debug ("GotoScene (SceneParams par)", par.ToString ());

			if (currentScene != null) {
				currentScene.DisableUIListener ();
			}

			animating = true;
			switch (anim) {

			case SceneAnimation.none:
				instance._AnimationNone (sceneName, data, cb);
				break;
			case SceneAnimation.fade:
				instance._AnimationFade (sceneName, data, oldSceneTime, newSceneTime, cb);
				break;
			case SceneAnimation.fromRight:
				instance._AnimationFromRight (sceneName, data, newSceneTime, ease, cb);
				break;
			case SceneAnimation.fromLeft:
				instance._AnimationFromLeft (sceneName, data, newSceneTime, ease, cb);
				break;
			case SceneAnimation.fromTop:
				instance._AnimationFromTop (sceneName, data, newSceneTime, ease, cb);
				break;
			case SceneAnimation.fromBottom:
				instance._AnimationFromBottom (sceneName, data, newSceneTime, ease, cb);
				break;
			case SceneAnimation.slideLeft:
				instance._AnimationSlideLeft (sceneName, data, newSceneTime, ease, cb);
				break;
			case SceneAnimation.slideRight:
				instance._AnimationSlideRight (sceneName, data, newSceneTime, ease, cb);
				break;
			case SceneAnimation.slideDown:
				instance._AnimationSlideDown (sceneName, data, newSceneTime, ease, cb);
				break;
			case SceneAnimation.slideUp:
				instance._AnimationSlideUp (sceneName, data, newSceneTime, ease, cb);
				break;
			case SceneAnimation.zoomIn:
				instance._AnimationZoomIn (sceneName, data, newSceneTime, ease, cb);
				break;
			case SceneAnimation.zoomOut:
				instance._AnimationZoomOut (sceneName, data, newSceneTime, ease, cb);
				break;
			}

		}

		public static void GotoScene (string sceneName)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			GotoScene (sp);
		}

		// 2 group parameter => 6
		public static void GotoScene (string sceneName, object data)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.data = data;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, SceneAnimation anim)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, float oldSceneTime, float newSceneTime)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, float eachSceneTime)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, iTween.EaseType ease)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.ease = ease;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.cb = cb;
			GotoScene (sp);
		}

		// 3 group parameter => 14
		public static void GotoScene (string sceneName, object data, SceneAnimation anim)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.data = data;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, object data, float oldSceneTime, float newSceneTime)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.data = data;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, object data, float eachSceneTime)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.data = data;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, object data, iTween.EaseType ease)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.ease = ease;
			sp.data = data;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, object data, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.cb = cb;
			sp.data = data;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, SceneAnimation anim, float oldSceneTime, float newSceneTime)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, SceneAnimation anim, float eachSceneTime)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, SceneAnimation anim, iTween.EaseType ease)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.ease = ease;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, SceneAnimation anim, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.cb = cb;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, float oldSceneTime, float newSceneTime, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.cb = cb;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, float oldSceneTime, float newSceneTime, iTween.EaseType ease)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.ease = ease;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, float eachSceneTime, iTween.EaseType ease)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.ease = ease;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, float eachSceneTime, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.cb = cb;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, iTween.EaseType ease, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.ease = ease;
			sp.cb = cb;
			GotoScene (sp);
		}

		// 4 group parameter => 16
		public static void GotoScene (string sceneName, object data, SceneAnimation anim, float oldSceneTime, float newSceneTime)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.data = data;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, object data, SceneAnimation anim, float eachSceneTime)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.data = data;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, object data, SceneAnimation anim, iTween.EaseType ease)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.ease = ease;
			sp.data = data;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, object data, SceneAnimation anim, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.cb = cb;
			sp.data = data;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, object data, float oldSceneTime, float newSceneTime, iTween.EaseType ease)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.ease = ease;
			sp.data = data;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, object data, float oldSceneTime, float newSceneTime, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.cb = cb;
			sp.data = data;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, object data, float eachSceneTime, iTween.EaseType ease)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.ease = ease;
			sp.data = data;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, object data, float eachSceneTime, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.cb = cb;
			sp.data = data;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, object data, iTween.EaseType ease, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.ease = ease;
			sp.cb = cb;
			sp.data = data;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, SceneAnimation anim, float oldSceneTime, float newSceneTime, iTween.EaseType ease)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.ease = ease;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, SceneAnimation anim, float oldSceneTime, float newSceneTime, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.cb = cb;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, SceneAnimation anim, float eachSceneTime, iTween.EaseType ease)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.ease = ease;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, SceneAnimation anim, float eachSceneTime, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.cb = cb;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, SceneAnimation anim, iTween.EaseType ease, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.ease = ease;
			sp.cb = cb;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, float oldSceneTime, float newSceneTime, iTween.EaseType ease, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.ease = ease;
			sp.cb = cb;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, float eachSceneTime, iTween.EaseType ease, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.ease = ease;
			sp.cb = cb;
			GotoScene (sp);
		}

		// 5 group parameter => 9
		public static void GotoScene (string sceneName, object data, SceneAnimation anim, float oldSceneTime, float newSceneTime, iTween.EaseType ease)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.ease = ease;
			sp.data = data;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, object data, SceneAnimation anim, float oldSceneTime, float newSceneTime, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.data = data;
			sp.cb = cb;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, object data, SceneAnimation anim, float eachSceneTime, iTween.EaseType ease)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.ease = ease;
			sp.data = data;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, object data, SceneAnimation anim, float eachSceneTime, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.data = data;
			sp.cb = cb;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, object data, SceneAnimation anim, iTween.EaseType ease, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.ease = ease;
			sp.data = data;
			sp.cb = cb;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, object data, float oldSceneTime, float newSceneTime, iTween.EaseType ease, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.ease = ease;
			sp.data = data;
			sp.cb = cb;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, object data, float eachSceneTime, iTween.EaseType ease, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.ease = ease;
			sp.data = data;
			sp.cb = cb;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, SceneAnimation anim, float oldSceneTime, float newSceneTime, iTween.EaseType ease, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.ease = ease;
			sp.cb = cb;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, SceneAnimation anim, float eachSceneTime, iTween.EaseType ease, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.ease = ease;
			sp.cb = cb;
			GotoScene (sp);
		}

		// 6 group parameter => 2
		public static void GotoScene (string sceneName, object data, SceneAnimation anim, float oldSceneTime, float newSceneTime, iTween.EaseType ease, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.ease = ease;
			sp.data = data;
			sp.cb = cb;
			GotoScene (sp);
		}

		public static void GotoScene (string sceneName, object data, SceneAnimation anim, float eachSceneTime, iTween.EaseType ease, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.ease = ease;
			sp.data = data;
			sp.cb = cb;
			GotoScene (sp);
		}

		#endregion

		#region  private functions (anim functions)

		private static void _UnloadOldScene (SceneObject sceneObj = null)
		{
			if (sceneObj == null)
				return;
		
			if (destoryOnSceneChange) {
				//scene.SendMessage ("DestoryScene");
				string sceneName = sceneObj.script.sceneName;
				sceneObj.script.DestoryScene ();
				Destroy (sceneObj.scene.gameObject);
				SCENES.Remove (sceneName);
			} else {
				sceneObj.scene.SetActive (false);
				//如果不销毁的话，恢复到初始状态
				sceneObj.RestoreStatus ();
			}
		}

		private static void _LoadNewScene (string sceneName, object data)
		{
			if (currentScene != null) {
				currentScene.EnableUIListener ();
			}

			if (!SCENES.ContainsKey (sceneName)) {
				SceneObject sceneObj;
				if (ModAssetBundle.HasAssetBundle (sceneName)) {
					sceneObj = new SceneObject (ModUtils.documentsDirectory+ModAssetBundle.GetAssetBundlePath (sceneName), sceneName);
				} else {
					sceneObj = new SceneObject (string.Format (scenePathFormat, sceneName));
				}
				currentScene = sceneObj;
				SCENES.Add (sceneName, sceneObj);
				currentScene.script.CreatScene (data);
				currentScene.script.UpdateLanguage ();
			} else {
				currentScene = SCENES [sceneName];
			}
			currentScene.DisableUIListener ();
			currentScene.scene.transform.SetParent (sceneViewRoot, false);
			currentScene.scene.SetActive (true);
			currentScene.scene.transform.SetAsLastSibling ();//置顶
			//currentScene.SendMessage ("WillEnterScene", data);
			currentScene.script.WillEnterScene (data);
		}

		private void _AnimationNone (string sceneName, object data, Action cb)
		{
			SceneObject oldScene = currentScene;
			_LoadNewScene (sceneName, data);
			if (oldScene != null) {
				oldScene.script.WillExitScene ();
				oldScene.script.ExitScene ();
				_UnloadOldScene (oldScene);
			}
			if (cb != null)
				cb ();
			currentScene.script.EnterScene (data);
			currentScene.EnableUIListener ();
			animating = false;
		}

		private void _AnimationFade (string sceneName, object data, float fadeInTime, float fadeOutTime, Action cb)
		{
			SceneObject oldScene = currentScene;
			if (oldScene != null) {
				oldScene.script.WillExitScene ();
				//恢复到透明度1
				oldScene.ChangeAlpha (1f);
				Log.Debug ("ChangeAlpha to 1");
				iTween.ValueTo (oldScene.scene, iTween.Hash (
					"from", 1,
					"to", 0,
					"time", fadeInTime,
					"onupdate", (Action<float>)((alpha) => {
					oldScene.ChangeAlpha (alpha);
				}),
					"oncomplete", (Action)(() => {
					oldScene.script.ExitScene ();
					_UnloadOldScene (oldScene);
					_LoadNewScene (sceneName, data);
					currentScene.ChangeAlpha (0f);
					iTween.ValueTo (currentScene.scene, iTween.Hash (
						"from", 0,
						"to", 1,
						"time", fadeOutTime,
						"onupdate", (Action<float>)((alpha) => {
						currentScene.ChangeAlpha (alpha);
					}),
						"oncomplete", (Action)(() => {
						if (cb != null)
							cb ();
						currentScene.script.EnterScene (data);
						currentScene.EnableUIListener ();
						animating = false;
					})
					));
				})
				));
			} else {
				_LoadNewScene (sceneName, data);// load new scene  or   set exise scene
				//show
				iTween.ValueTo (currentScene.scene, iTween.Hash (
					"from", 0,
					"to", 1,
					"time", fadeOutTime,
					"onupdate", (Action<float>)((alpha) => {
					currentScene.ChangeAlpha (alpha);
				}),
					"oncomplete", (Action)(() => {
					if (cb != null)
						cb ();
					currentScene.script.EnterScene (data);
					currentScene.EnableUIListener ();
					animating = false;
				})
				));
			}

		}

		private void _AnimationFromLeft (string sceneName, object data, float newSceneTime, iTween.EaseType easeType, Action cb)
		{
			if (currentScene == null) {
				//Log.Error ("You should NOT use this anim in the first storyoard, use fade instead");

				_AnimationNone (sceneName, data, cb);
				return;
			}
			iTween.EaseType ease = (easeType == iTween.EaseType.defaultType) ? iTween.EaseType.easeOutExpo : easeType;

			float nomalX = currentScene.scene.transform.position.x;
			float nomalY = currentScene.scene.transform.position.y;
			float newSceneStartX = nomalX - screenWidth;

			SceneObject oldScene = currentScene;
			_LoadNewScene (sceneName, data);// load new scene  or   set exise scene

			RectTransform tmpRT = currentScene.scene.GetComponent<RectTransform> ();
			tmpRT.position = new Vector3 (newSceneStartX, nomalY);

			oldScene.script.WillExitScene ();
			iTween.MoveTo (currentScene.scene, iTween.Hash (
				"x", nomalX,
				"time", newSceneTime,
				"easetype", ease,
				"oncomplete", (Action)(() => {
				oldScene.script.ExitScene ();
				_UnloadOldScene (oldScene);
				if (cb != null)
					cb ();
				currentScene.script.EnterScene (data);
				currentScene.EnableUIListener ();
				animating = false;
			})
			));
		}

		private void _AnimationFromRight (string sceneName, object data, float newSceneTime, iTween.EaseType easeType, Action cb)
		{
			if (currentScene == null) {
				//Log.Error ("You should NOT use this anim in the first storyoard, use fade instead");

				_AnimationNone (sceneName, data, cb);
				return;
			}
			iTween.EaseType ease = (easeType == iTween.EaseType.defaultType) ? iTween.EaseType.easeOutExpo : easeType;

			float nomalX = currentScene.scene.transform.position.x;
			float nomalY = currentScene.scene.transform.position.y;
			float newSceneStartX = nomalX + screenWidth;

			SceneObject oldScene = currentScene;
			_LoadNewScene (sceneName, data);// load new scene  or   set exise scene



			RectTransform tmpRT = currentScene.scene.GetComponent<RectTransform> ();
			tmpRT.position = new Vector3 (newSceneStartX, nomalY);

			oldScene.script.WillExitScene ();
			iTween.MoveTo (currentScene.scene, iTween.Hash (
				"x", nomalX,
				"time", newSceneTime,
				"easetype", ease,
				"oncomplete", (Action)(() => {
				oldScene.script.ExitScene ();
				_UnloadOldScene (oldScene);
				if (cb != null)
					cb ();
				currentScene.script.EnterScene (data);
				currentScene.EnableUIListener ();
				animating = false;
			})
			));
		}

		private void _AnimationFromTop (string sceneName, object data, float newSceneTime, iTween.EaseType easeType, Action cb)
		{
			if (currentScene == null) {
				//Log.Error ("You should NOT use this anim in the first storyoard, use fade instead");

				_AnimationNone (sceneName, data, cb);
				return;
			}
			iTween.EaseType ease = (easeType == iTween.EaseType.defaultType) ? iTween.EaseType.easeOutExpo : easeType;

			float nomalX = currentScene.scene.transform.position.x;
			float nomalY = currentScene.scene.transform.position.y;
			float newSceneStartY = nomalY + screenHeight;

			SceneObject oldScene = currentScene;
			_LoadNewScene (sceneName, data);// load new scene  or   set exise scene



			RectTransform tmpRT = currentScene.scene.GetComponent<RectTransform> ();
			tmpRT.position = new Vector3 (nomalX, newSceneStartY);

			oldScene.script.WillExitScene ();
			iTween.MoveTo (currentScene.scene, iTween.Hash (
				"y", nomalY,
				"time", newSceneTime,
				"easetype", ease,
				"oncomplete", (Action)(() => {
				oldScene.script.ExitScene ();
				_UnloadOldScene (oldScene);
				if (cb != null)
					cb ();
				currentScene.script.EnterScene (data);
				currentScene.EnableUIListener ();
				animating = false;
			})
			));
		}

		private void _AnimationFromBottom (string sceneName, object data, float newSceneTime, iTween.EaseType easeType, Action cb)
		{
			if (currentScene == null) {
				//Log.Error ("You should NOT use this anim in the first storyoard, use fade instead");

				_AnimationNone (sceneName, data, cb);
				return;
			}
			iTween.EaseType ease = (easeType == iTween.EaseType.defaultType) ? iTween.EaseType.easeOutExpo : easeType;

			float nomalX = currentScene.scene.transform.position.x;
			float nomalY = currentScene.scene.transform.position.y;
			float newSceneStartY = nomalY - screenHeight;

			SceneObject oldScene = currentScene;
			_LoadNewScene (sceneName, data);// load new scene  or   set exise scene



			RectTransform tmpRT = currentScene.scene.GetComponent<RectTransform> ();
			tmpRT.position = new Vector3 (nomalX, newSceneStartY);

			oldScene.script.WillExitScene ();
			iTween.MoveTo (currentScene.scene, iTween.Hash (
				"y", nomalY,
				"time", newSceneTime,
				"easetype", ease,
				"oncomplete", (Action)(() => {
				oldScene.script.ExitScene ();
				_UnloadOldScene (oldScene);
				if (cb != null)
					cb ();
				currentScene.script.EnterScene (data);
				currentScene.EnableUIListener ();
				animating = false;
			})
			));
		}

		private void _AnimationSlideLeft (string sceneName, object data, float newSceneTime, iTween.EaseType easeType, Action cb = null)
		{
			if (currentScene == null) {
				//Log.Error ("You should NOT use this anim in the first storyoard, use fade instead");

				_AnimationNone (sceneName, data, cb);
				return;
			}
			iTween.EaseType ease = (easeType == iTween.EaseType.defaultType) ? iTween.EaseType.easeOutExpo : easeType;
            
			float nomalX = currentScene.scene.transform.position.x;
			float nomalY = currentScene.scene.transform.position.y;
			float newSceneStartX = nomalX + screenWidth;
			float oldSceneEndX = nomalX - screenWidth;

			SceneObject oldScene = currentScene;
			_LoadNewScene (sceneName, data);// load new scene  or   set exise scene



			RectTransform tmpRT = currentScene.scene.GetComponent<RectTransform> ();
			tmpRT.position = new Vector3 (newSceneStartX, nomalY);
			oldScene.script.WillExitScene ();
			iTween.MoveTo (oldScene.scene, iTween.Hash (
				"x", oldSceneEndX,
				"time", newSceneTime,
				"easetype", ease
			));

			iTween.MoveTo (currentScene.scene, iTween.Hash (
				"x", nomalX,
				"time", newSceneTime,
				"easetype", ease,
				"oncomplete", (Action)(() => {
				oldScene.script.ExitScene ();
				_UnloadOldScene (oldScene);
				if (cb != null)
					cb ();
				currentScene.script.EnterScene (data);
				currentScene.EnableUIListener ();
				animating = false;
			})
			));
		}

		private void _AnimationSlideRight (string sceneName, object data, float newSceneTime, iTween.EaseType easeType, Action cb = null)
		{
			if (currentScene == null) {
				//Log.Error ("You should NOT use this anim in the first storyoard, use fade instead");

				_AnimationNone (sceneName, data, cb);
				return;
			}
			iTween.EaseType ease = (easeType == iTween.EaseType.defaultType) ? iTween.EaseType.easeOutExpo : easeType;

			float nomalX = currentScene.scene.transform.position.x;
			float nomalY = currentScene.scene.transform.position.y;
			float newSceneStartX = nomalX - screenWidth;
			float oldSceneEndX = nomalX + screenWidth;

			SceneObject oldScene = currentScene;
			_LoadNewScene (sceneName, data);// load new scene  or   set exise scene

			RectTransform tmpRT = currentScene.scene.GetComponent<RectTransform> ();
			tmpRT.position = new Vector3 (newSceneStartX, nomalY);

			oldScene.script.WillExitScene ();
			iTween.MoveTo (oldScene.scene, iTween.Hash (
				"x", oldSceneEndX,
				"time", newSceneTime,
				"easetype", ease
			));

			iTween.MoveTo (currentScene.scene, iTween.Hash (
				"x", nomalX,
				"time", newSceneTime,
				"easetype", ease,
				"oncomplete", (Action)(() => {
				oldScene.script.ExitScene ();
				_UnloadOldScene (oldScene);
				if (cb != null)
					cb ();
				currentScene.script.EnterScene (data);
				currentScene.EnableUIListener ();
				animating = false;
			})
			));
		}

		private void _AnimationSlideUp (string sceneName, object data, float newSceneTime, iTween.EaseType easeType, Action cb)
		{
			if (currentScene == null) {
				//Log.Error ("You should NOT use this anim in the first storyoard, use fade instead");

				_AnimationNone (sceneName, data, cb);
				return;
			}
			iTween.EaseType ease = (easeType == iTween.EaseType.defaultType) ? iTween.EaseType.easeOutExpo : easeType;

			float nomalX = currentScene.scene.transform.position.x;
			float nomalY = currentScene.scene.transform.position.y;
			float newSceneStartY = nomalY - screenHeight;
			float oldSceneEndY = nomalY + screenHeight;


			SceneObject oldScene = currentScene;
			_LoadNewScene (sceneName, data);// load new scene  or   set exise scene

			RectTransform tmpRT = currentScene.scene.GetComponent<RectTransform> ();
			tmpRT.position = new Vector3 (nomalX, newSceneStartY);

			oldScene.script.WillExitScene ();
			iTween.MoveTo (oldScene.scene, iTween.Hash (
				"y", oldSceneEndY,
				"time", newSceneTime,
				"easetype", ease
			));

			iTween.MoveTo (currentScene.scene, iTween.Hash (
				"y", nomalY,
				"time", newSceneTime,
				"easetype", ease,
				"oncomplete", (Action)(() => {
				oldScene.script.ExitScene ();
				_UnloadOldScene (oldScene);
				if (cb != null)
					cb ();
				currentScene.script.EnterScene (data);
				currentScene.EnableUIListener ();
				animating = false;
			})
			));
		}

		private void _AnimationSlideDown (string sceneName, object data, float newSceneTime, iTween.EaseType easeType, Action cb)
		{
			if (currentScene == null) {
				//Log.Error ("You should NOT use this anim in the first storyoard, use fade instead");
				_AnimationNone (sceneName, data, cb);
				return;
			}
			iTween.EaseType ease = (easeType == iTween.EaseType.defaultType) ? iTween.EaseType.easeOutExpo : easeType;

			float nomalX = currentScene.scene.transform.position.x;
			float nomalY = currentScene.scene.transform.position.y;
			float newSceneStartY = nomalY + screenHeight;
			float oldSceneEndY = nomalY - screenHeight;


			SceneObject oldScene = currentScene;
			_LoadNewScene (sceneName, data);// load new scene  or   set exise scene

			RectTransform tmpRT = currentScene.scene.GetComponent<RectTransform> ();
			tmpRT.position = new Vector3 (nomalX, newSceneStartY);

			oldScene.script.WillExitScene ();
			iTween.MoveTo (oldScene.scene, iTween.Hash (
				"y", oldSceneEndY,
				"time", newSceneTime,
				"easetype", ease
			));

			iTween.MoveTo (currentScene.scene, iTween.Hash (
				"y", nomalY,
				"time", newSceneTime,
				"easetype", ease,
				"oncomplete", (Action)(() => {
				oldScene.script.ExitScene ();
				_UnloadOldScene (oldScene);
				if (cb != null)
					cb ();
				currentScene.script.EnterScene (data);
				currentScene.EnableUIListener ();
				animating = false;
			})
			));
		}

		private void _AnimationZoomIn (string sceneName, object data, float newSceneTime, iTween.EaseType easeType, Action cb)
		{
			iTween.EaseType ease = (easeType == iTween.EaseType.defaultType) ? iTween.EaseType.easeOutBack : easeType;

			SceneObject oldScene = currentScene;
			_LoadNewScene (sceneName, data);// load new scene  or   set exise scene
			currentScene.scene.transform.localScale = new Vector3 (1f, 1f);
			oldScene.script.WillExitScene ();
			iTween.ScaleFrom (currentScene.scene, iTween.Hash (
				"scale", new Vector3 (0f, 0f),
				"time", newSceneTime,
				"easetype", ease,
				"oncomplete", (Action)(() => {
				oldScene.script.ExitScene ();
				_UnloadOldScene (oldScene);
				if (cb != null)
					cb ();
				currentScene.script.EnterScene (data);
				currentScene.EnableUIListener ();
				animating = false;
			})
			));

		}

		private void _AnimationZoomOut (string sceneName, object data, float newSceneTime, iTween.EaseType easeType, Action cb)
		{
			iTween.EaseType ease = (easeType == iTween.EaseType.defaultType) ? iTween.EaseType.easeInBack : easeType;

			SceneObject oldScene = currentScene;
			_LoadNewScene (sceneName, data);// load new scene  or   set exise scene
			currentScene.scene.transform.localScale = new Vector3 (1f, 1f);
			oldScene.script.WillExitScene ();
			oldScene.scene.transform.SetAsLastSibling ();//旧的scene在最上
			iTween.ScaleTo (oldScene.scene, iTween.Hash (
				"scale", new Vector3 (0f, 0f),
				"time", newSceneTime,
				"easetype", ease,
				"oncomplete", (Action)(() => {
				oldScene.script.ExitScene ();
				_UnloadOldScene (oldScene);
				if (!destoryOnSceneChange) {
					oldScene.scene.transform.localScale = new Vector3 (1f, 1f);
				}
				if (cb != null)
					cb ();
				currentScene.script.EnterScene (data);
				currentScene.EnableUIListener ();
				animating = false;
			})
			));
		}

		#endregion


		public static void Update ()
		{
			#if UNITY_ANDROID
			if (MgrDialog.hasDialog) {
				return;
			}
			if (Input.GetKeyDown (KeyCode.Escape)) { //android back
				SceneObject curr = MgrScene.GetCurrentScene ();
				if (curr != null) {
					curr.script.AndroidGoBack ();
				}
			}
			#endif
		}

	}

	/// <summary>
	/// Scene 的动画效果
	/// </summary>
	public enum SceneAnimation
	{
		//无任何效果
		none,
		//渐隐渐现
		fade,
		//从右侧飞过来（覆盖旧的scene）
		fromRight,
		fromLeft,
		fromTop,
		fromBottom,
		//推向左面（不会覆盖旧的scene）
		slideLeft,
		slideRight,
		slideDown,
		slideUp,
		//放大
		zoomIn,
		//缩小
		zoomOut
		//还需要什么效果，可以在下面添加
	}

	/// <summary>
	/// Scene parameters.
	/// </summary>
	public class SceneParams
	{
		public 	string sceneName;
		public 	SceneAnimation anim = SceneAnimation.fade;
		public 	float oldSceneTime = 0.5f;
		public 	float newSceneTime = 0.5f;
		public 	iTween.EaseType ease = iTween.EaseType.defaultType;
		public 	Action cb = null;
		public object data = "";

		/// <summary>
		/// The background alpha.
		/// Only useful for dialog
		/// </summary>
		public float bgAlpha = 0.8f;

		public override string ToString ()
		{
			return  " sceneName:" + sceneName
			+ "\t anim:" + anim.ToString ()
			+ "\t oldSceneTime:" + oldSceneTime
			+ "\t newSceneTime:" + newSceneTime
			+ "\t EaseType:" + ease.ToString ()
			+ "\t bgAlpha:" + bgAlpha.ToString ()
			+ "\t data:" + data.ToString ()
			+ "\t callback is null:" + (cb == null);
		}

	}

   
}

