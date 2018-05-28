using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using XLAF.Private;

namespace XLAF.Public
{
	/// <summary>
	/// Scene manager, the code is similar to <see cref="XLAF.Public.MgrPopup"/>
	/// </summary>
	public class MgrScene : MonoBehaviour
	{
		#region public variables

		public static bool destroyOnSceneChange = false;

		#endregion

		#region private variables

		private static MgrScene instance = null;
		private static readonly string scenePathFormat = "Views/Scenes/{0}";

		private static bool animating = false;
		private static SceneObject currentScene = null;
		private static Transform sceneViewRoot = null;
		private static CanvasGroup sceneViewRootCanvas = null;

		private static float _screenWidth;
		private static float _screenHeight;

		private static Dictionary<string,SceneObject> SCENES;

		#endregion

		#region public readonly variables

		public static float screenWidth{ get { return _screenWidth; } }

		public static float screenHeight{ get { return _screenHeight; } }

		public static float screenScale{ get { return  (float)_screenHeight / Screen.height; } }

		#endregion

		#region constructed function & initialization

		static MgrScene ()
		{
			SCENES = new Dictionary<string, SceneObject> ();
			instance = XLAFMain.XLAFGameObject.AddComponent<MgrScene> ();

			_screenHeight = Camera.main.orthographicSize * 2;
			float aspectRatio = (float)Screen.width / Screen.height;
			_screenWidth = _screenHeight * aspectRatio;

			XLAFInnerLog.Debug ("screenHeight,screenWidth", screenHeight, screenWidth);
		}

		/// <summary>
		/// call Init() will trigger constructed function, you can call Init() to ensure this class finished initialization
		/// </summary>
		public static void Init ()
		{

		}

		#endregion

		#region public functions

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
		/// Gets a value indicating whether this <see cref="XLAF.Public.MgrScene"/> is scene or popup changing.
		/// </summary>
		/// <value><c>true</c> if is scene or popup changing; otherwise, <c>false</c>.</value>
		public static bool isSceneOrPopupChanging {
			get {
				return animating || MgrPopup.isPopupChanging;
			}
		}

		/// <summary>
		/// Loads the scene, this function will call CreatScene(params) but not call other functions (e.g. WillEnterScene/EnterScene).
		/// </summary>
		/// <param name="sceneName">Scene name.</param>
		/// <param name="data">the data you want to transmit</param>
		public static void LoadScene (string sceneName, object data)
		{
			if (SCENES.ContainsKey (sceneName))
				return;
			SceneObject sceneObj = null;
			if (MgrAssetBundle.HasAssetBundle (sceneName)) {
				sceneObj = new SceneObject (ModUtils.documentsDirectory + MgrAssetBundle.GetAssetBundlePath (sceneName), sceneName);
			} else {
				sceneObj = new SceneObject (string.Format (scenePathFormat, sceneName));
			}
			SCENES.Add (sceneName, sceneObj);
			sceneObj.script.CreatScene (data);
			sceneObj.script.UpdateLanguage ();

			sceneObj.scene.transform.SetParent (sceneViewRoot, false);
			sceneObj.scene.SetActive (false);
			sceneObj.scene.transform.SetAsFirstSibling ();//set to under

		}

		/// <summary>
		/// Loads the scene, this function will call CreatScene(params) but not call other functions (e.g. WillEnterScene/EnterScene).
		/// </summary>
		/// <param name="sceneName">Scene name.</param>
		public static void LoadScene (string sceneName)
		{
			LoadScene (sceneName, "");
		}

		/// <summary>
		/// Destroy the scene.
		/// </summary>
		/// <param name="sceneObj">Scene object.</param>
		/// <param name="destroyImmediate">If set to <c>true</c> destroy immediate.</param>
		public static void DestroyScene (SceneObject sceneObj, bool destroyImmediate = true)
		{
			if (sceneObj == null || sceneObj.script == null)
				return;
			
			string sceneName = sceneObj.script.sceneName;
			sceneObj.script.DestroyScene ();
			SCENES.Remove (sceneName);
			if (destroyImmediate)
				DestroyImmediate (sceneObj.scene.gameObject);
			else
				Destroy (sceneObj.scene.gameObject);
		}

		/// <summary>
		/// Destroy the scene.
		/// </summary>
		/// <param name="sceneName">Scene name.</param>
		/// <param name="destroyImmediate">If set to <c>true</c> destroy immediate.</param>
		public static void DestroyScene (string sceneName, bool destroyImmediate = true)
		{
			SceneObject sceneObj = GetScene (sceneName);
			if (sceneObj == null)
				return;
			
			sceneObj.script.DestroyScene ();
			SCENES.Remove (sceneName);
			if (destroyImmediate)
				DestroyImmediate (sceneObj.scene.gameObject);
			else
				Destroy (sceneObj.scene.gameObject);
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


			XLAFInnerLog.Debug ("GotoScene (SceneParams par)", par.ToString ());

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


		public static void Update ()
		{
			#if UNITY_ANDROID
			if (MgrPopup.hasPopup) {
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

		#endregion

		#region  private functions (anim functions)

		private static void _UnloadOldScene (SceneObject sceneObj = null)
		{
			if (sceneObj == null)
				return;

			if (destroyOnSceneChange) {
				//scene.SendMessage ("DestroyScene");
				string sceneName = sceneObj.script.sceneName;
				sceneObj.script.DestroyScene ();
				Destroy (sceneObj.scene.gameObject);
				SCENES.Remove (sceneName);
			} else {
				sceneObj.scene.SetActive (false);
				//if not destroy, reset status
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
				if (MgrAssetBundle.HasAssetBundle (sceneName)) {
					sceneObj = new SceneObject (ModUtils.documentsDirectory + MgrAssetBundle.GetAssetBundlePath (sceneName), sceneName);
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
			currentScene.scene.transform.SetAsLastSibling ();//set top
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
				//reset alpha to 1
				oldScene.ChangeAlpha (1f);
				XLAFInnerLog.Debug ("ChangeAlpha to 1");
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
				//XLAFInnerLog.Error ("You should NOT use this anim in the first storyoard, use fade instead");

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
				//XLAFInnerLog.Error ("You should NOT use this anim in the first storyoard, use fade instead");

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
				//XLAFInnerLog.Error ("You should NOT use this anim in the first storyoard, use fade instead");

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
				//XLAFInnerLog.Error ("You should NOT use this anim in the first storyoard, use fade instead");

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
				//XLAFInnerLog.Error ("You should NOT use this anim in the first storyoard, use fade instead");

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
				//XLAFInnerLog.Error ("You should NOT use this anim in the first storyoard, use fade instead");

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
				//XLAFInnerLog.Error ("You should NOT use this anim in the first storyoard, use fade instead");

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
				//XLAFInnerLog.Error ("You should NOT use this anim in the first storyoard, use fade instead");
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
			oldScene.scene.transform.SetAsLastSibling ();//old scene is in top
			iTween.ScaleTo (oldScene.scene, iTween.Hash (
				"scale", new Vector3 (0f, 0f),
				"time", newSceneTime,
				"easetype", ease,
				"oncomplete", (Action)(() => {
					oldScene.script.ExitScene ();
					_UnloadOldScene (oldScene);
					if (!destroyOnSceneChange) {
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

	}
	/// <summary>
	/// Scene animation.
	/// </summary>
	public enum SceneAnimation
	{
		none,
		fade,
		//fly from right(cover current scene)
		fromRight,
		fromLeft,
		fromTop,
		fromBottom,
		//push the scene to left (not cover current scene)
		slideLeft,
		slideRight,
		slideDown,
		slideUp,
		//zoom large
		zoomIn,
		//zoom small
		zoomOut
		//you can add other animation below
	}

   
}

