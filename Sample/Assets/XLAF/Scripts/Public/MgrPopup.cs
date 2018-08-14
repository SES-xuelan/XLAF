using UnityEngine;
using System.Collections;
using XLAF.Public;
using System.Collections.Generic;
using System;
using XLAF.Private;

namespace XLAF.Public
{
	/// <summary>
	///  Popup manager, the code is similar to <see cref="XLAF.Public.MgrScene"/>
	/// </summary>
	public class MgrPopup : MonoBehaviour
	{
		#region private variables

		private static Transform _popupViewRoot = null;
		private static CanvasGroup _popupViewRootCanvas = null;
		private static bool _popupViewVisiblity = true;
		private static MgrPopup _instance = null;
		private static readonly string _popupPathFormat = "Views/Popups/{0}";
		private static bool _animating = false;

		private static float _screenWidth;
		private static float _screenHeight;

		private static Dictionary<string,SceneObject> _POPUPS;
		private static List<SceneObject> _POPUPS_STACK = new List<SceneObject> ();

		#endregion

		#region constructed function & initialization

		static MgrPopup ()
		{
			_POPUPS = new Dictionary<string, SceneObject> ();
			_instance = XLAFMain.XLAFGameObject.AddComponent<MgrPopup> ();

			_screenHeight = MgrScene.screenHeight;
			_screenWidth = MgrScene.screenWidth;
		}

		/// <summary>
		/// call Init() will trigger constructed function, you can call Init() to ensure this class finished initialization
		/// </summary>
		public static void Init ()
		{

		}

		#endregion

		#region public variables

		/// <summary>
		/// Gets a value indicating whether this <see cref="XLAF.Public.MgrPopup"/> is scene changing.
		/// </summary>
		/// <value><c>true</c> if is scene changing; otherwise, <c>false</c>.</value>
		public static bool isPopupChanging {
			get {
				return _animating;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="XLAF.Public.MgrPopup"/> is scene or popup changing.
		/// </summary>
		/// <value><c>true</c> if is scene or popup changing; otherwise, <c>false</c>.</value>
		public static bool isSceneOrPopupChanging {
			get {
				return _animating || MgrScene.isSceneChanging;
			}
		}

		#endregion

		#region public functions

		/// <summary>
		/// Sets the popup root.
		/// </summary>
		/// <param name="grp">Group.</param>
		public static void SetPopupRoot (Transform grp)
		{
			_popupViewRoot = grp;
			_popupViewRootCanvas = _popupViewRoot.transform.GetComponent<CanvasGroup> ();

		}

		/// <summary>
		/// Gets the popup root.
		/// </summary>
		/// <returns>The popup root.</returns>
		public static Transform GetPopupRoot ()
		{
			return _popupViewRoot;
		}

		/// <summary>
		/// Gets the popup root canvas.
		/// </summary>
		/// <returns>The popup root canvas.</returns>
		public static CanvasGroup GetPopupRootCanvas ()
		{
			return _popupViewRootCanvas;
		}

		/// <summary>
		/// Gets the popup view visible.
		/// </summary>
		/// <returns><c>true</c>, if popup view visible was gotten, <c>false</c> otherwise.</returns>
		public static bool GetPopupViewVisiblity ()
		{
			return _popupViewVisiblity;
		}

		/// <summary>
		/// Sets the popup view visible.
		/// </summary>
		/// <param name="visible">If set to <c>true</c> visible.</param>
		public static void SetPopupViewVisiblity (bool visiblity)
		{
			if (_popupViewVisiblity == visiblity)
				return;

			XLAFInnerLog.Debug ("SetPopupViewVisible", visiblity);
			_popupViewVisiblity = visiblity;
			_popupViewRoot.gameObject.SetActive (visiblity);
		}


		/// <summary>
		/// Gets all popups.
		/// </summary>
		/// <returns>The all popups.</returns>
		public static Dictionary<string,SceneObject> GetAllPopups ()
		{
			return _POPUPS;
		}

		/// <summary>
		/// Loads the popup, this function will call CreateScene(params) but not call other functions (e.g. WillEnterScene/EnterScene).
		/// </summary>
		/// <param name="sceneName">Scene name.</param>
		/// <param name="data">the data you want to transmit</param>
		public static void LoadPopup (string sceneName, object data)
		{
			if (_POPUPS.ContainsKey (sceneName))
				return;

			SceneObject sceneObj = null;
			if (MgrAssetBundle.HasAssetBundle (sceneName)) {
				sceneObj = new SceneObject ("file://"+ModUtils.documentsDirectory + MgrAssetBundle.GetAssetBundlePath (sceneName), sceneName);
			} else {
				sceneObj = new SceneObject (string.Format (_popupPathFormat, sceneName));
			}

			_POPUPS.Add (sceneName, sceneObj);
			sceneObj.script.CreateScene (data);
			sceneObj.script.UpdateLanguage ();

			sceneObj.scene.transform.SetParent (_popupViewRoot, false);
			sceneObj.scene.SetActive (false);
			sceneObj.scene.transform.SetAsFirstSibling ();//set to under 
		}

		/// <summary>
		/// Loads the popup.
		/// this function will call CreateScene(params) but not call other functions (e.g. WillEnterScene/EnterScene) .
		/// </summary>
		/// <param name="sceneName">Scene name.</param>
		public static void LoadPopup (string sceneName)
		{
			LoadPopup (sceneName, "");
		}

		/// <summary>
		/// Gets the popup.
		/// </summary>
		/// <returns>The popup. return null if the popup not exists</returns>
		/// <param name="sceneName">Scene name.</param>
		public static SceneObject GetPopup (string sceneName)
		{
			if (!_POPUPS.ContainsKey (sceneName)) {
				return null;
			}
			return _POPUPS [sceneName];
		}

		/// <summary>
		/// Hides all popups.
		/// </summary>
		public static void HideAll ()
		{
			foreach (string sceneName in _POPUPS.Keys) {
				SceneParams sp = new SceneParams ();
				sp.sceneName = sceneName;
				sp.anim = SceneAnimation.none;
				Hide (sp);
			}
		}

		/// <summary>
		/// Hides the top popup.
		/// </summary>
		public static void HideTop ()
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = GetTop ().sceneName;
			sp.anim = SceneAnimation.none;
			Hide (sp);
		}

		/// <summary>
		/// Gets the top popup.
		/// </summary>
		/// <returns>The top.</returns>
		public static SceneObject GetTop ()
		{
			if (_POPUPS_STACK.Count <= 0) {
				return null;
			}

			return _POPUPS_STACK [_POPUPS_STACK.Count - 1];
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="XLAF.Public.MgrPopup"/> has popup.
		/// </summary>
		/// <value><c>true</c> if has popup; otherwise, <c>false</c>.</value>
		public static bool hasPopup{ get { return _POPUPS_STACK.Count > 0; } }

		/// <summary>
		/// Gets the popup count.
		/// </summary>
		/// <value>The popup count.</value>
		public static int popupCount{ get { return _POPUPS_STACK.Count; } }


		#region public Show functions [override 49 times]

		public static void Show (SceneParams par)
		{
			string sceneName = par.sceneName;
			SceneAnimation anim = par.anim;
			float oldSceneTime = par.oldSceneTime;
			float newSceneTime = par.newSceneTime;
			iTween.EaseType ease = par.ease;
			Action cb = par.cb;
			object data = par.data;
			float bgAlpha = par.bgAlpha;

			XLAFInnerLog.Debug ("Show (SceneParams par)", par.ToString ());

			SceneObject currentPopup = GetPopup (sceneName);
			if (currentPopup != null) {
				currentPopup.DisableUIListener ();
			}
			_animating = true;

			switch (anim) {

			case SceneAnimation.none:
				_instance._AnimationNone (true, sceneName, data, bgAlpha, cb);
				break;
			case SceneAnimation.fade:
				_instance._AnimationFade (true, sceneName, data, bgAlpha, oldSceneTime, newSceneTime, cb);
				break;
			case SceneAnimation.fromRight:
				_instance._AnimationFromRight (true, sceneName, data, bgAlpha, newSceneTime, ease, cb);
				break;
			case SceneAnimation.fromLeft:
				_instance._AnimationFromLeft (true, sceneName, data, bgAlpha, newSceneTime, ease, cb);
				break;
			case SceneAnimation.fromTop:
				_instance._AnimationFromTop (true, sceneName, data, bgAlpha, newSceneTime, ease, cb);
				break;
			case SceneAnimation.fromBottom:
				_instance._AnimationFromBottom (true, sceneName, data, bgAlpha, newSceneTime, ease, cb);
				break;
			case SceneAnimation.slideLeft:
				XLAFInnerLog.Warning ("SceneAnimation.slideLeft should not use in MgrPopup.Show, use fromRight instead");
				_instance._AnimationFromRight (true, sceneName, data, bgAlpha, newSceneTime, ease, cb);
				break;
			case SceneAnimation.slideRight:
				XLAFInnerLog.Warning ("SceneAnimation.slideRight should not use in MgrPopup.Show, use fromLeft instead");
				_instance._AnimationFromLeft (true, sceneName, data, bgAlpha, newSceneTime, ease, cb);
				break;
			case SceneAnimation.slideDown:
				XLAFInnerLog.Warning ("SceneAnimation.slideDown should not use in MgrPopup.Show, use fromTop instead");
				_instance._AnimationFromTop (true, sceneName, data, bgAlpha, newSceneTime, ease, cb);
				break;
			case SceneAnimation.slideUp:
				XLAFInnerLog.Warning ("SceneAnimation.slideUp should not use in MgrPopup.Show, use fromBottom instead");
				_instance._AnimationFromBottom (true, sceneName, data, bgAlpha, newSceneTime, ease, cb);
				break;
			case SceneAnimation.zoomIn:
				_instance._AnimationZoomIn (sceneName, data, bgAlpha, newSceneTime, ease, cb);
				break;
			case SceneAnimation.zoomOut:
				XLAFInnerLog.Warning ("SceneAnimation.zoomOut should not use in MgrPopup.Show, use zoomIn instead");
				_instance._AnimationZoomIn (sceneName, data, bgAlpha, newSceneTime, ease, cb);
				break;
			}

		}

		public static void Show (string sceneName)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			Show (sp);
		}

		// 2 group parameter => 6
		public static void Show (string sceneName, object data)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.data = data;
			Show (sp);
		}

		public static void Show (string sceneName, SceneAnimation anim)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			Show (sp);
		}

		public static void Show (string sceneName, float oldSceneTime, float newSceneTime)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			Show (sp);
		}

		public static void Show (string sceneName, float eachSceneTime)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			Show (sp);
		}

		public static void Show (string sceneName, iTween.EaseType ease)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.ease = ease;
			Show (sp);
		}

		public static void Show (string sceneName, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.cb = cb;
			Show (sp);
		}

		// 3 group parameter => 14
		public static void Show (string sceneName, object data, SceneAnimation anim)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.data = data;
			Show (sp);
		}

		public static void Show (string sceneName, object data, float oldSceneTime, float newSceneTime)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.data = data;
			Show (sp);
		}

		public static void Show (string sceneName, object data, float eachSceneTime)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.data = data;
			Show (sp);
		}

		public static void Show (string sceneName, object data, iTween.EaseType ease)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.ease = ease;
			sp.data = data;
			Show (sp);
		}

		public static void Show (string sceneName, object data, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.cb = cb;
			sp.data = data;
			Show (sp);
		}

		public static void Show (string sceneName, SceneAnimation anim, float oldSceneTime, float newSceneTime)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			Show (sp);
		}

		public static void Show (string sceneName, SceneAnimation anim, float eachSceneTime)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			Show (sp);
		}

		public static void Show (string sceneName, SceneAnimation anim, iTween.EaseType ease)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.ease = ease;
			Show (sp);
		}

		public static void Show (string sceneName, SceneAnimation anim, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.cb = cb;
			Show (sp);
		}

		public static void Show (string sceneName, float oldSceneTime, float newSceneTime, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.cb = cb;
			Show (sp);
		}

		public static void Show (string sceneName, float oldSceneTime, float newSceneTime, iTween.EaseType ease)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.ease = ease;
			Show (sp);
		}

		public static void Show (string sceneName, float eachSceneTime, iTween.EaseType ease)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.ease = ease;
			Show (sp);
		}

		public static void Show (string sceneName, float eachSceneTime, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.cb = cb;
			Show (sp);
		}

		public static void Show (string sceneName, iTween.EaseType ease, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.ease = ease;
			sp.cb = cb;
			Show (sp);
		}

		// 4 group parameter => 16
		public static void Show (string sceneName, object data, SceneAnimation anim, float oldSceneTime, float newSceneTime)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.data = data;
			Show (sp);
		}

		public static void Show (string sceneName, object data, SceneAnimation anim, float eachSceneTime)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.data = data;
			Show (sp);
		}

		public static void Show (string sceneName, object data, SceneAnimation anim, iTween.EaseType ease)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.ease = ease;
			sp.data = data;
			Show (sp);
		}

		public static void Show (string sceneName, object data, SceneAnimation anim, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.cb = cb;
			sp.data = data;
			Show (sp);
		}

		public static void Show (string sceneName, object data, float oldSceneTime, float newSceneTime, iTween.EaseType ease)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.ease = ease;
			sp.data = data;
			Show (sp);
		}

		public static void Show (string sceneName, object data, float oldSceneTime, float newSceneTime, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.cb = cb;
			sp.data = data;
			Show (sp);
		}

		public static void Show (string sceneName, object data, float eachSceneTime, iTween.EaseType ease)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.ease = ease;
			sp.data = data;
			Show (sp);
		}

		public static void Show (string sceneName, object data, float eachSceneTime, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.cb = cb;
			sp.data = data;
			Show (sp);
		}

		public static void Show (string sceneName, object data, iTween.EaseType ease, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.ease = ease;
			sp.cb = cb;
			sp.data = data;
			Show (sp);
		}

		public static void Show (string sceneName, SceneAnimation anim, float oldSceneTime, float newSceneTime, iTween.EaseType ease)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.ease = ease;
			Show (sp);
		}

		public static void Show (string sceneName, SceneAnimation anim, float oldSceneTime, float newSceneTime, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.cb = cb;
			Show (sp);
		}

		public static void Show (string sceneName, SceneAnimation anim, float eachSceneTime, iTween.EaseType ease)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.ease = ease;
			Show (sp);
		}

		public static void Show (string sceneName, SceneAnimation anim, float eachSceneTime, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.cb = cb;
			Show (sp);
		}

		public static void Show (string sceneName, SceneAnimation anim, iTween.EaseType ease, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.ease = ease;
			sp.cb = cb;
			Show (sp);
		}

		public static void Show (string sceneName, float oldSceneTime, float newSceneTime, iTween.EaseType ease, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.ease = ease;
			sp.cb = cb;
			Show (sp);
		}

		public static void Show (string sceneName, float eachSceneTime, iTween.EaseType ease, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.ease = ease;
			sp.cb = cb;
			Show (sp);
		}

		// 5 group parameter => 9
		public static void Show (string sceneName, object data, SceneAnimation anim, float oldSceneTime, float newSceneTime, iTween.EaseType ease)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.ease = ease;
			sp.data = data;
			Show (sp);
		}

		public static void Show (string sceneName, object data, SceneAnimation anim, float oldSceneTime, float newSceneTime, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.data = data;
			sp.cb = cb;
			Show (sp);
		}

		public static void Show (string sceneName, object data, SceneAnimation anim, float eachSceneTime, iTween.EaseType ease)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.ease = ease;
			sp.data = data;
			Show (sp);
		}

		public static void Show (string sceneName, object data, SceneAnimation anim, float eachSceneTime, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.data = data;
			sp.cb = cb;
			Show (sp);
		}

		public static void Show (string sceneName, object data, SceneAnimation anim, iTween.EaseType ease, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.ease = ease;
			sp.data = data;
			sp.cb = cb;
			Show (sp);
		}

		public static void Show (string sceneName, object data, float oldSceneTime, float newSceneTime, iTween.EaseType ease, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.ease = ease;
			sp.data = data;
			sp.cb = cb;
			Show (sp);
		}

		public static void Show (string sceneName, object data, float eachSceneTime, iTween.EaseType ease, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.ease = ease;
			sp.data = data;
			sp.cb = cb;
			Show (sp);
		}

		public static void Show (string sceneName, SceneAnimation anim, float oldSceneTime, float newSceneTime, iTween.EaseType ease, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.ease = ease;
			sp.cb = cb;
			Show (sp);
		}

		public static void Show (string sceneName, SceneAnimation anim, float eachSceneTime, iTween.EaseType ease, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.ease = ease;
			sp.cb = cb;
			Show (sp);
		}

		// 6 group parameter => 2
		public static void Show (string sceneName, object data, SceneAnimation anim, float oldSceneTime, float newSceneTime, iTween.EaseType ease, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.ease = ease;
			sp.data = data;
			sp.cb = cb;
			Show (sp);
		}

		public static void Show (string sceneName, object data, SceneAnimation anim, float eachSceneTime, iTween.EaseType ease, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.ease = ease;
			sp.data = data;
			sp.cb = cb;
			Show (sp);
		}

		#endregion


		#region public Hide functions [override 25 times]

		// 1 group parameter => 2
		public static void Hide (SceneParams par)
		{
			string sceneName = par.sceneName;
			SceneAnimation anim = par.anim;
			float oldSceneTime = par.oldSceneTime;
			float newSceneTime = par.newSceneTime;
			iTween.EaseType ease = par.ease;
			Action cb = par.cb;
			object data = par.data;
			float bgAlpha = par.bgAlpha;

			XLAFInnerLog.Debug ("Hide (SceneParams par)", par.ToString ());

			SceneObject currentPopup = GetPopup (sceneName);
			if (currentPopup == null) {
				return;
			}
			currentPopup.DisableUIListener ();

			_animating = true;
			switch (anim) {

			case SceneAnimation.none:
				_instance._AnimationNone (false, sceneName, data, bgAlpha, cb);
				break;
			case SceneAnimation.fade:
				_instance._AnimationFade (false, sceneName, data, bgAlpha, oldSceneTime, newSceneTime, cb);
				break;
			case SceneAnimation.fromRight:
				_instance._AnimationFromRight (false, sceneName, data, bgAlpha, newSceneTime, ease, cb);
				break;
			case SceneAnimation.fromLeft:
				_instance._AnimationFromLeft (false, sceneName, data, bgAlpha, newSceneTime, ease, cb);
				break;
			case SceneAnimation.fromTop:
				_instance._AnimationFromTop (false, sceneName, data, bgAlpha, newSceneTime, ease, cb);
				break;
			case SceneAnimation.fromBottom:
				_instance._AnimationFromBottom (false, sceneName, data, bgAlpha, newSceneTime, ease, cb);
				break;
			case SceneAnimation.slideLeft:
				XLAFInnerLog.Warning ("SceneAnimation.slideLeft should not use in MgrPopup.Hide, use fromRight instead");
				_instance._AnimationFromRight (false, sceneName, data, bgAlpha, newSceneTime, ease, cb);
				break;
			case SceneAnimation.slideRight:
				XLAFInnerLog.Warning ("SceneAnimation.slideRight should not use in MgrPopup.Hide, use fromLeft instead");
				_instance._AnimationFromLeft (false, sceneName, data, bgAlpha, newSceneTime, ease, cb);
				break;
			case SceneAnimation.slideDown:
				XLAFInnerLog.Warning ("SceneAnimation.slideDown should not use in MgrPopup.Hide, use fromTop instead");
				_instance._AnimationFromTop (false, sceneName, data, bgAlpha, newSceneTime, ease, cb);
				break;
			case SceneAnimation.slideUp:
				XLAFInnerLog.Warning ("SceneAnimation.slideUp should not use in MgrPopup.Hide, use fromBottom instead");
				_instance._AnimationFromBottom (false, sceneName, data, bgAlpha, newSceneTime, ease, cb);
				break;
			case SceneAnimation.zoomIn:
				XLAFInnerLog.Warning ("SceneAnimation.zoomIn should not use in MgrPopup.Hide, use zoomOut instead");
				_instance._AnimationZoomOut (sceneName, data, bgAlpha, newSceneTime, ease, cb);
				break;
			case SceneAnimation.zoomOut:
				_instance._AnimationZoomOut (sceneName, data, bgAlpha, newSceneTime, ease, cb);
				break;
			}

		}

		public static void Hide (string sceneName)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			Hide (sp);
		}
		// 2 group parameter => 5
		public static void Hide (string sceneName, SceneAnimation anim)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			Hide (sp);
		}

		public static void Hide (string sceneName, float oldSceneTime, float newSceneTime)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			Hide (sp);
		}

		public static void Hide (string sceneName, float eachSceneTime)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			Hide (sp);
		}

		public static void Hide (string sceneName, iTween.EaseType ease)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.ease = ease;
			Hide (sp);
		}

		public static void Hide (string sceneName, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.cb = cb;
			Hide (sp);
		}

		// 3 group parameter => 9
		public static void Hide (string sceneName, SceneAnimation anim, float oldSceneTime, float newSceneTime)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			Hide (sp);
		}

		public static void Hide (string sceneName, SceneAnimation anim, float eachSceneTime)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			Hide (sp);
		}

		public static void Hide (string sceneName, SceneAnimation anim, iTween.EaseType ease)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.ease = ease;
			Hide (sp);
		}

		public static void Hide (string sceneName, SceneAnimation anim, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.cb = cb;
			Hide (sp);
		}

		public static void Hide (string sceneName, float oldSceneTime, float newSceneTime, iTween.EaseType ease)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.ease = ease;
			Hide (sp);
		}

		public static void Hide (string sceneName, float oldSceneTime, float newSceneTime, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.cb = cb;
			Hide (sp);
		}

		public static void Hide (string sceneName, float eachSceneTime, iTween.EaseType ease)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.ease = ease;
			Hide (sp);
		}

		public static void Hide (string sceneName, float eachSceneTime, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.cb = cb;
			Hide (sp);
		}

		public static void Hide (string sceneName, iTween.EaseType ease, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.ease = ease;
			sp.cb = cb;
			Hide (sp);
		}
		// 4 group parameter => 7

		public static void Hide (string sceneName, SceneAnimation anim, float oldSceneTime, float newSceneTime, iTween.EaseType ease)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.ease = ease;
			Hide (sp);
		}

		public static void Hide (string sceneName, SceneAnimation anim, float eachSceneTime, iTween.EaseType ease)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.ease = ease;
			Hide (sp);
		}

		public static void Hide (string sceneName, SceneAnimation anim, float oldSceneTime, float newSceneTime, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.cb = cb;
			Hide (sp);
		}

		public static void Hide (string sceneName, SceneAnimation anim, float eachSceneTime, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.cb = cb;
			Hide (sp);
		}

		public static void Hide (string sceneName, SceneAnimation anim, iTween.EaseType ease, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.ease = ease;
			sp.cb = cb;
			Hide (sp);
		}

		public static void Hide (string sceneName, float oldSceneTime, float newSceneTime, iTween.EaseType ease, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.ease = ease;
			sp.cb = cb;
			Hide (sp);
		}

		public static void Hide (string sceneName, float eachSceneTime, iTween.EaseType ease, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.ease = ease;
			sp.cb = cb;
			Hide (sp);
		}
		// 5 group parameter => 2

		public static void Hide (string sceneName, SceneAnimation anim, float oldSceneTime, float newSceneTime, iTween.EaseType ease, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = oldSceneTime;
			sp.newSceneTime = newSceneTime;
			sp.ease = ease;
			sp.cb = cb;
			Hide (sp);
		}

		public static void Hide (string sceneName, SceneAnimation anim, float eachSceneTime, iTween.EaseType ease, Action cb)
		{
			SceneParams sp = new SceneParams ();
			sp.sceneName = sceneName;
			sp.anim = anim;
			sp.oldSceneTime = eachSceneTime;
			sp.newSceneTime = eachSceneTime;
			sp.ease = ease;
			sp.cb = cb;
			Hide (sp);
		}

		#endregion

		#endregion

		#region  private functions (animation functions)

		private static void _UnloadOldScene (SceneObject sceneObj = null)
		{
			if (sceneObj == null)
				return;
            
			//scene.SendMessage ("DestroyScene");
			string sceneName = sceneObj.script.sceneName;
			sceneObj.script.DestroyScene ();

			//OverlayEnded event
			SceneObject currScene = MgrScene.GetCurrentScene ();
			if (currScene != null) {
				currScene.script.OverlayEnded (sceneName);
			}

			if (_POPUPS_STACK.Contains (sceneObj)) {
				_POPUPS_STACK.Remove (sceneObj);
			}
			Destroy (sceneObj.scene.gameObject);
			_POPUPS.Remove (sceneName);
			_animating = false;
		}

		private static SceneObject _LoadNewScene (string sceneName, object data, float bgAlpha)
		{
			SceneObject currentPopup = GetPopup (sceneName);
			if (currentPopup != null) {
				currentPopup.EnableUIListener ();
			}
			SceneObject sceneObj;
			if (!_POPUPS.ContainsKey (sceneName)) {
				if (MgrAssetBundle.HasAssetBundle (sceneName)) {
					sceneObj = new SceneObject ("file://"+ModUtils.documentsDirectory + MgrAssetBundle.GetAssetBundlePath (sceneName), sceneName);
				} else {
					sceneObj = new SceneObject (string.Format (_popupPathFormat, sceneName));
				}
				sceneObj.AddPopupBackground (bgAlpha);
				_POPUPS_STACK.Add (sceneObj);
				_POPUPS.Add (sceneName, sceneObj);
				sceneObj.script.CreateScene (data);
				sceneObj.script.UpdateLanguage ();
			} else {
				sceneObj = _POPUPS [sceneName];
			}

			//OverlayBegan event
			SceneObject currScene = MgrScene.GetCurrentScene ();
			if (currScene != null) {
				currScene.script.OverlayBegan (sceneName);
			}

			sceneObj.DisableUIListener ();
			sceneObj.scene.transform.SetParent (_popupViewRoot, false);
			sceneObj.scene.SetActive (true);
			sceneObj.scene.transform.SetAsLastSibling ();//set top
			sceneObj.script.WillEnterScene (data);
			return sceneObj;
		}

		/// <summary>
		/// Animations the none.
		/// </summary>
		/// <param name="isShow">If set to <c>true</c> is show; <c>false</c> is hide. </param>
		/// <param name="sceneName">Scene name.</param>
		/// <param name="data">Data.</param>
		/// <param name="bgAlpha">Background alpha.</param>
		/// <param name="cb">Cb.</param>
		private void _AnimationNone (bool isShow, string sceneName, object data, float bgAlpha, Action cb)
		{
			SceneObject currentPopup;
			if (isShow) {
				currentPopup = _LoadNewScene (sceneName, data, bgAlpha);
				if (cb != null)
					cb ();
				currentPopup.script.EnterScene (data);
				currentPopup.EnableUIListener ();
				_animating = false;
			} else {
				currentPopup = GetPopup (sceneName);
				if (currentPopup == null) {
					return;
				}
				currentPopup.script.WillExitScene ();
				currentPopup.script.ExitScene ();
				_UnloadOldScene (currentPopup);
			}
		}

		private void _AnimationFade (bool isShow, string sceneName, object data, float bgAlpha, float fadeInTime, float fadeOutTime, Action cb)
		{

			SceneObject currentPopup;
			if (isShow) { 
				currentPopup = _LoadNewScene (sceneName, data, bgAlpha);// load new scene  or   set exise scene
				currentPopup.ChangeAlpha (0f);
				iTween.ValueTo (currentPopup.scene, iTween.Hash (
					"from", 0,
					"to", 1,
					"time", fadeOutTime,
					"onupdate", (Action<float>)((alpha) => {
					currentPopup.ChangeAlpha (alpha);
				}),
					"oncomplete", (Action)(() => {
					if (cb != null)
						cb ();
					currentPopup.script.EnterScene (data);
					currentPopup.EnableUIListener ();
					_animating = false;
				})
				));
			} else {
				currentPopup = GetPopup (sceneName);
				if (currentPopup == null) {
					return;
				}
				currentPopup.script.WillExitScene ();
				iTween.ValueTo (currentPopup.scene, iTween.Hash (
					"from", 1,
					"to", 0,
					"time", fadeInTime,
					"onupdate", (Action<float>)((alpha) => {
					currentPopup.ChangeAlpha (alpha);
				}),
					"oncomplete", (Action)(() => {
					currentPopup.script.ExitScene ();
					_UnloadOldScene (currentPopup);
				})
				));
			}
		}

		private void _AnimationFromLeft (bool isShow, string sceneName, object data, float bgAlpha, float newSceneTime, iTween.EaseType easeType, Action cb)
		{
			iTween.EaseType ease = (easeType == iTween.EaseType.defaultType) ? iTween.EaseType.easeOutExpo : easeType;

			SceneObject currentPopup;

			if (isShow) {
				currentPopup = _LoadNewScene (sceneName, data, bgAlpha);// load new scene  or   set exise scene

				float nomalX = currentPopup.scene.transform.position.x;
				float nomalY = currentPopup.scene.transform.position.y;
				float newSceneStartX = nomalX - _screenWidth;

				RectTransform tmpRT = currentPopup.scene.GetComponent<RectTransform> ();
				tmpRT.position = new Vector3 (newSceneStartX, nomalY);
				XLAFInnerLog.Debug ("newSceneStartX", newSceneStartX);
				iTween.MoveTo (currentPopup.scene, iTween.Hash (
					"x", nomalX,
					"time", newSceneTime,
					"easetype", ease,
					"oncomplete", (Action)(() => {
					if (cb != null)
						cb ();
					currentPopup.script.EnterScene (data);
					currentPopup.EnableUIListener ();
					_animating = false;
				})
				));
			} else {
				currentPopup = GetPopup (sceneName);
				if (currentPopup == null) {
					return;
				}
				float nomalX = currentPopup.scene.transform.position.x;
				float newSceneStartX = nomalX - _screenWidth;

				currentPopup.script.WillExitScene ();
				iTween.MoveTo (currentPopup.scene, iTween.Hash (
					"x", newSceneStartX,
					"time", newSceneTime,
					"easetype", ease,
					"oncomplete", (Action)(() => {
					currentPopup.script.ExitScene ();
					_UnloadOldScene (currentPopup);
				})
				));
			}
		}

		private void _AnimationFromRight (bool isShow, string sceneName, object data, float bgAlpha, float newSceneTime, iTween.EaseType easeType, Action cb)
		{
			iTween.EaseType ease = (easeType == iTween.EaseType.defaultType) ? iTween.EaseType.easeOutExpo : easeType;
			SceneObject currentPopup;


			if (isShow) {
				currentPopup = _LoadNewScene (sceneName, data, bgAlpha);// load new scene  or   set exise scene
				float nomalX = currentPopup.scene.transform.position.x;
				float nomalY = currentPopup.scene.transform.position.y;
				float newSceneStartX = nomalX + _screenWidth;
				RectTransform tmpRT = currentPopup.scene.GetComponent<RectTransform> ();
				tmpRT.position = new Vector3 (newSceneStartX, nomalY);
				iTween.MoveTo (currentPopup.scene, iTween.Hash (
					"x", nomalX,
					"time", newSceneTime,
					"easetype", ease,
					"oncomplete", (Action)(() => {
					if (cb != null)
						cb ();
					currentPopup.script.EnterScene (data);
					currentPopup.EnableUIListener ();
					_animating = false;
				})
				));
			} else {
				currentPopup = GetPopup (sceneName);
				if (currentPopup == null) {
					return;
				}
				float nomalX = currentPopup.scene.transform.position.x;
				float newSceneStartX = nomalX - _screenWidth;
				currentPopup.script.WillExitScene ();
				iTween.MoveTo (currentPopup.scene, iTween.Hash (
					"x", newSceneStartX,
					"time", newSceneTime,
					"easetype", ease,
					"oncomplete", (Action)(() => {
					currentPopup.script.ExitScene ();
					_UnloadOldScene (currentPopup);
				})
				));
			}
		}

		private void _AnimationFromTop (bool isShow, string sceneName, object data, float bgAlpha, float newSceneTime, iTween.EaseType easeType, Action cb)
		{

			iTween.EaseType ease = (easeType == iTween.EaseType.defaultType) ? iTween.EaseType.easeOutExpo : easeType;
			SceneObject currentPopup;

			if (isShow) {
				currentPopup = _LoadNewScene (sceneName, data, bgAlpha);// load new scene  or   set exise scene
				float nomalX = currentPopup.scene.transform.position.x;
				float nomalY = currentPopup.scene.transform.position.y;
				float newSceneStartY = nomalY + _screenHeight;
				RectTransform tmpRT = currentPopup.scene.GetComponent<RectTransform> ();
				tmpRT.position = new Vector3 (nomalX, newSceneStartY);
				iTween.MoveTo (currentPopup.scene, iTween.Hash (
					"y", nomalY,
					"time", newSceneTime,
					"easetype", ease,
					"oncomplete", (Action)(() => {
					if (cb != null)
						cb ();
					currentPopup.script.EnterScene (data);
					currentPopup.EnableUIListener ();
					_animating = false;
				})
				));
			} else {

				currentPopup = GetPopup (sceneName);
				if (currentPopup == null) {
					return;
				}
				float nomalY = currentPopup.scene.transform.position.y;
				float newSceneStartY = nomalY + _screenHeight;
				currentPopup.script.WillExitScene ();
				iTween.MoveTo (currentPopup.scene, iTween.Hash (
					"y", newSceneStartY,
					"time", newSceneTime,
					"easetype", ease,
					"oncomplete", (Action)(() => {
					currentPopup.script.ExitScene ();
					_UnloadOldScene (currentPopup);
				})
				));
			}


		}

		private void _AnimationFromBottom (bool isShow, string sceneName, object data, float bgAlpha, float newSceneTime, iTween.EaseType easeType, Action cb)
		{

			iTween.EaseType ease = (easeType == iTween.EaseType.defaultType) ? iTween.EaseType.easeOutExpo : easeType;
			SceneObject currentPopup;

			if (isShow) {
				currentPopup = _LoadNewScene (sceneName, data, bgAlpha);// load new scene  or   set exise scene
				float nomalX = currentPopup.scene.transform.position.x;
				float nomalY = currentPopup.scene.transform.position.y;
				float newSceneStartY = nomalY - _screenHeight;
				RectTransform tmpRT = currentPopup.scene.GetComponent<RectTransform> ();
				tmpRT.position = new Vector3 (nomalX, newSceneStartY);
				iTween.MoveTo (currentPopup.scene, iTween.Hash (
					"y", nomalY,
					"time", newSceneTime,
					"easetype", ease,
					"oncomplete", (Action)(() => {
					if (cb != null)
						cb ();
					currentPopup.script.EnterScene (data);
					currentPopup.EnableUIListener ();
					_animating = false;
				})
				));
			} else {

				currentPopup = GetPopup (sceneName);
				if (currentPopup == null) {
					return;
				}
				float nomalY = currentPopup.scene.transform.position.y;
				float newSceneStartY = nomalY - _screenHeight;
				currentPopup.script.WillExitScene ();
				iTween.MoveTo (currentPopup.scene, iTween.Hash (
					"y", newSceneStartY,
					"time", newSceneTime,
					"easetype", ease,
					"oncomplete", (Action)(() => {
					currentPopup.script.ExitScene ();
					_UnloadOldScene (currentPopup);
				})
				));
			}
		}

		private void _AnimationZoomIn (string sceneName, object data, float bgAlpha, float newSceneTime, iTween.EaseType easeType, Action cb)
		{
            
			iTween.EaseType ease = (easeType == iTween.EaseType.defaultType) ? iTween.EaseType.easeOutBack : easeType;
			SceneObject currentPopup = _LoadNewScene (sceneName, data, bgAlpha);// load new scene  or   set exise scene
			currentPopup.scene.transform.localScale = new Vector3 (1f, 1f);
			iTween.ScaleFrom (currentPopup.scene, iTween.Hash (
				"scale", new Vector3 (0f, 0f),
				"time", newSceneTime,
				"easetype", ease,
				"oncomplete", (Action)(() => {
				if (cb != null)
					cb ();
				currentPopup.script.EnterScene (data);
				currentPopup.EnableUIListener ();
				_animating = false;
			})
			));

		}

		private void _AnimationZoomOut (string sceneName, object data, float bgAlpha, float newSceneTime, iTween.EaseType easeType, Action cb)
		{
			iTween.EaseType ease = (easeType == iTween.EaseType.defaultType) ? iTween.EaseType.easeInBack : easeType;

			SceneObject currentPopup = GetPopup (sceneName);
			if (currentPopup == null) {
				return;
			}
			currentPopup.scene.transform.localScale = new Vector3 (1f, 1f);
			iTween.ScaleTo (currentPopup.scene, iTween.Hash (
				"scale", new Vector3 (0f, 0f),
				"time", newSceneTime,
				"easetype", ease,
				"oncomplete", (Action)(() => {
				if (cb != null)
					cb ();
				currentPopup.script.EnterScene (data);
				currentPopup.EnableUIListener ();
				_animating = false;
			})
			));
		}

		#endregion


		// Update is called once per frame
		void Update ()
		{
			if (_popupViewRoot == null)
				return;
			SetPopupViewVisiblity (hasPopup);
			#if UNITY_ANDROID
			if (Input.GetKeyDown (KeyCode.Escape)) { //android back
				SceneObject curr = MgrPopup.GetTop ();
				if (curr != null) {
					curr.script.AndroidGoBack ();
					return;
				}
			}
			#endif
			MgrScene.Update ();
		}

	}
}