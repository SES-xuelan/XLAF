using UnityEngine;
using System.Collections;
using XLAF.Public;
using System.Collections.Generic;
using System;

namespace XLAF.Public
{
    /// <summary>
    /// 弹窗管理，和MgrScene代码差不多
    /// </summary>
    public class MgrDialog : MonoBehaviour
    {
        static MgrDialog ()
        {
            DIALOGS = new Dictionary<string, SceneObject> ();
            instance = (new GameObject ("MgrDialog")).AddComponent<MgrDialog> ();

            screenHeight = Screen.height;
            screenWidth = Screen.width;
        }


        private static Transform dialogViewRoot = null;
        private static CanvasGroup dialogViewRootCanvas = null;
        private static bool dialogViewVisible = true;
        private static MgrDialog instance = null;
        private static readonly string dialogPathFormat = "Dialogs/{0}";
        private static bool animating = false;

        private static float screenWidth;
        private static float screenHeight;

        private static Dictionary<string,SceneObject> DIALOGS;
        private static List<SceneObject> DIALOGS_STACK = new List<SceneObject> ();


        /// <summary>
        /// 调用Init会触发构造函数，可以用于统一初始化的时候
        /// </summary>
        public static void Init ()
        {

        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="XLAF.Public.MgrDialog"/> is scene changing.
        /// </summary>
        /// <value><c>true</c> if is scene changing; otherwise, <c>false</c>.</value>
        public static bool isDialogChanging {
            get {
                return animating;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="XLAF.Public.MgrDialog"/> is scene or dialog changing.
        /// </summary>
        /// <value><c>true</c> if is scene or dialog changing; otherwise, <c>false</c>.</value>
        public static bool isSceneOrDialogChanging {
            get {
                return animating || MgrScene.isSceneChanging;
            }
        }

        /// <summary>
        /// Sets the dialog root.
        /// </summary>
        /// <param name="grp">Group.</param>
        public static void SetDialogRoot (Transform grp)
        {
            dialogViewRoot = grp;
            dialogViewRootCanvas = dialogViewRoot.transform.GetComponent<CanvasGroup> ();

        }

        /// <summary>
        /// Gets the dialog root.
        /// </summary>
        /// <returns>The dialog root.</returns>
        public static Transform GetDialogRoot ()
        {
            return dialogViewRoot;
        }

        /// <summary>
        /// Gets the dialog root canvas.
        /// </summary>
        /// <returns>The dialog root canvas.</returns>
        public static CanvasGroup GetDialogRootCanvas ()
        {
            return dialogViewRootCanvas;
        }

        /// <summary>
        /// Gets the dialog view visible.
        /// </summary>
        /// <returns><c>true</c>, if dialog view visible was gotten, <c>false</c> otherwise.</returns>
        public static bool GetDialogViewVisible ()
        {
            return dialogViewVisible;
        }

        /// <summary>
        /// Sets the dialog view visible.
        /// </summary>
        /// <param name="visible">If set to <c>true</c> visible.</param>
        public static void SetDialogViewVisible (bool visible)
        {
            if (dialogViewVisible == visible)
                return;

            Log.Debug ("SetDialogViewVisible", visible);
            dialogViewVisible = visible;
            dialogViewRoot.gameObject.SetActive (visible);
        }


        /// <summary>
        /// Gets all dialogs.
        /// </summary>
        /// <returns>The all dialogs.</returns>
        public static Dictionary<string,SceneObject> GetAllDialogs ()
        {
            return DIALOGS;
        }

        /// <summary>
        /// Loads the dialog.
        /// 用于dialog的东西比较多，需要提前加载的情况
        /// </summary>
        /// <param name="sceneName">Scene name.</param>
        /// <param name="data">需要传递的数据</param>
        public static void LoadDialog (string sceneName, object data)
        {
            if (DIALOGS.ContainsKey (sceneName))
                return;
            
            SceneObject sceneObj = new SceneObject (string.Format (dialogPathFormat, sceneName));
            DIALOGS.Add (sceneName, sceneObj);
            sceneObj.script.CreatScene (data);
            sceneObj.script.UpdateLanguage ();

            sceneObj.scene.transform.SetParent (dialogViewRoot, false);
            sceneObj.scene.SetActive (false);
            sceneObj.scene.transform.SetAsFirstSibling ();//置底
        }

        /// <summary>
        /// Loads the dialog.
        /// 用于dialog的东西比较多，需要提前加载的情况
        /// </summary>
        /// <param name="sceneName">Scene name.</param>
        public static void LoadDialog (string sceneName)
        {
            LoadDialog (sceneName, "");
        }

        /// <summary>
        /// Gets the dialog.
        /// </summary>
        /// <returns>The dialog. return null if the dialog not exists</returns>
        /// <param name="sceneName">Scene name.</param>
        public static SceneObject GetDialog (string sceneName)
        {
            if (!DIALOGS.ContainsKey (sceneName)) {
                return null;
            }
            return DIALOGS [sceneName];
        }

        /// <summary>
        /// Hides all dialogs.
        /// </summary>
        public static void HideAll ()
        {
            foreach (string sceneName in DIALOGS.Keys) {
                SceneParams sp = new SceneParams ();
                sp.sceneName = sceneName;
                sp.animation = SceneAnimation.none;
                HideDialog (sp);
            }
        }

        /// <summary>
        /// Hides the top dialog.
        /// </summary>
        public static void HideTop ()
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = GetTop ().sceneName;
            sp.animation = SceneAnimation.none;
            HideDialog (sp);
        }

        /// <summary>
        /// Gets the top dialog.
        /// </summary>
        /// <returns>The top.</returns>
        public static SceneObject GetTop ()
        {
            if (DIALOGS_STACK.Count <= 0) {
                return null;
            }

            return DIALOGS_STACK [DIALOGS_STACK.Count - 1];
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="XLAF.Public.MgrDialog"/> has dialog.
        /// </summary>
        /// <value><c>true</c> if has dialog; otherwise, <c>false</c>.</value>
        public static bool hasDialog{ get { return DIALOGS_STACK.Count > 0; } }

        /// <summary>
        /// Gets the dialog count.
        /// </summary>
        /// <value>The dialog count.</value>
        public static int dialogCount{ get { return DIALOGS_STACK.Count; } }


        #region public ShowDialog functions [override 49 times]

        public static void ShowDialog (SceneParams par)
        {
            string sceneName = par.sceneName;
            SceneAnimation animation = par.animation;
            float oldSceneTime = par.oldSceneTime;
            float newSceneTime = par.newSceneTime;
            XLAF_Tween.EaseType ease = par.ease;
            Action cb = par.cb;
            object data = par.data;
            float bgAlpha = par.bgAlpha;

            Log.Debug ("ShowDialog (SceneParams par)", par.ToString ());

            SceneObject currentDialog = GetDialog (sceneName);
            if (currentDialog != null) {
                currentDialog.DisableUIListener ();
            }
            animating = true;

            switch (animation) {

            case SceneAnimation.none:
                instance._AnimationNone (true, sceneName, data, bgAlpha, cb);
                break;
            case SceneAnimation.fade:
                instance._AnimationFade (true, sceneName, data, bgAlpha, oldSceneTime, newSceneTime, cb);
                break;
            case SceneAnimation.fromRight:
                instance._AnimationFromRight (true, sceneName, data, bgAlpha, newSceneTime, ease, cb);
                break;
            case SceneAnimation.fromLeft:
                instance._AnimationFromLeft (true, sceneName, data, bgAlpha, newSceneTime, ease, cb);
                break;
            case SceneAnimation.fromTop:
                instance._AnimationFromTop (true, sceneName, data, bgAlpha, newSceneTime, ease, cb);
                break;
            case SceneAnimation.fromBottom:
                instance._AnimationFromBottom (true, sceneName, data, bgAlpha, newSceneTime, ease, cb);
                break;
            case SceneAnimation.slideLeft:
                Log.Warning ("SceneAnimation.slideLeft should not use in MgrDialog.ShowDialog, use fromRight instead");
                instance._AnimationFromRight (true, sceneName, data, bgAlpha, newSceneTime, ease, cb);
                break;
            case SceneAnimation.slideRight:
                Log.Warning ("SceneAnimation.slideRight should not use in MgrDialog.ShowDialog, use fromLeft instead");
                instance._AnimationFromLeft (true, sceneName, data, bgAlpha, newSceneTime, ease, cb);
                break;
            case SceneAnimation.slideDown:
                Log.Warning ("SceneAnimation.slideDown should not use in MgrDialog.ShowDialog, use fromTop instead");
                instance._AnimationFromTop (true, sceneName, data, bgAlpha, newSceneTime, ease, cb);
                break;
            case SceneAnimation.slideUp:
                Log.Warning ("SceneAnimation.slideUp should not use in MgrDialog.ShowDialog, use fromBottom instead");
                instance._AnimationFromBottom (true, sceneName, data, bgAlpha, newSceneTime, ease, cb);
                break;
            case SceneAnimation.zoomIn:
                instance._AnimationZoomIn (sceneName, data, bgAlpha, newSceneTime, ease, cb);
                break;
            case SceneAnimation.zoomOut:
                Log.Warning ("SceneAnimation.zoomOut should not use in MgrDialog.ShowDialog, use zoomIn instead");
                instance._AnimationZoomIn (sceneName, data, bgAlpha, newSceneTime, ease, cb);
                break;
            }

        }

        public static void ShowDialog (string sceneName)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            ShowDialog (sp);
        }

        // 2 group parameter => 6
        public static void ShowDialog (string sceneName, object data)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.data = data;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, SceneAnimation animation)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, float oldSceneTime, float newSceneTime)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.oldSceneTime = oldSceneTime;
            sp.newSceneTime = newSceneTime;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, float eachSceneTime)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.oldSceneTime = eachSceneTime;
            sp.newSceneTime = eachSceneTime;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, XLAF_Tween.EaseType ease)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.ease = ease;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.cb = cb;
            ShowDialog (sp);
        }

        // 3 group parameter => 14
        public static void ShowDialog (string sceneName, object data, SceneAnimation animation)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.data = data;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, object data, float oldSceneTime, float newSceneTime)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.oldSceneTime = oldSceneTime;
            sp.newSceneTime = newSceneTime;
            sp.data = data;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, object data, float eachSceneTime)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.oldSceneTime = eachSceneTime;
            sp.newSceneTime = eachSceneTime;
            sp.data = data;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, object data, XLAF_Tween.EaseType ease)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.ease = ease;
            sp.data = data;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, object data, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.cb = cb;
            sp.data = data;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, SceneAnimation animation, float oldSceneTime, float newSceneTime)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.oldSceneTime = oldSceneTime;
            sp.newSceneTime = newSceneTime;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, SceneAnimation animation, float eachSceneTime)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.oldSceneTime = eachSceneTime;
            sp.newSceneTime = eachSceneTime;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, SceneAnimation animation, XLAF_Tween.EaseType ease)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.ease = ease;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, SceneAnimation animation, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.cb = cb;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, float oldSceneTime, float newSceneTime, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.oldSceneTime = oldSceneTime;
            sp.newSceneTime = newSceneTime;
            sp.cb = cb;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, float oldSceneTime, float newSceneTime, XLAF_Tween.EaseType ease)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.oldSceneTime = oldSceneTime;
            sp.newSceneTime = newSceneTime;
            sp.ease = ease;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, float eachSceneTime, XLAF_Tween.EaseType ease)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.oldSceneTime = eachSceneTime;
            sp.newSceneTime = eachSceneTime;
            sp.ease = ease;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, float eachSceneTime, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.oldSceneTime = eachSceneTime;
            sp.newSceneTime = eachSceneTime;
            sp.cb = cb;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, XLAF_Tween.EaseType ease, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.ease = ease;
            sp.cb = cb;
            ShowDialog (sp);
        }

        // 4 group parameter => 16
        public static void ShowDialog (string sceneName, object data, SceneAnimation animation, float oldSceneTime, float newSceneTime)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.oldSceneTime = oldSceneTime;
            sp.newSceneTime = newSceneTime;
            sp.data = data;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, object data, SceneAnimation animation, float eachSceneTime)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.oldSceneTime = eachSceneTime;
            sp.newSceneTime = eachSceneTime;
            sp.data = data;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, object data, SceneAnimation animation, XLAF_Tween.EaseType ease)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.ease = ease;
            sp.data = data;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, object data, SceneAnimation animation, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.cb = cb;
            sp.data = data;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, object data, float oldSceneTime, float newSceneTime, XLAF_Tween.EaseType ease)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.oldSceneTime = oldSceneTime;
            sp.newSceneTime = newSceneTime;
            sp.ease = ease;
            sp.data = data;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, object data, float oldSceneTime, float newSceneTime, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.oldSceneTime = oldSceneTime;
            sp.newSceneTime = newSceneTime;
            sp.cb = cb;
            sp.data = data;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, object data, float eachSceneTime, XLAF_Tween.EaseType ease)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.oldSceneTime = eachSceneTime;
            sp.newSceneTime = eachSceneTime;
            sp.ease = ease;
            sp.data = data;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, object data, float eachSceneTime, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.oldSceneTime = eachSceneTime;
            sp.newSceneTime = eachSceneTime;
            sp.cb = cb;
            sp.data = data;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, object data, XLAF_Tween.EaseType ease, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.ease = ease;
            sp.cb = cb;
            sp.data = data;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, SceneAnimation animation, float oldSceneTime, float newSceneTime, XLAF_Tween.EaseType ease)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.oldSceneTime = oldSceneTime;
            sp.newSceneTime = newSceneTime;
            sp.ease = ease;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, SceneAnimation animation, float oldSceneTime, float newSceneTime, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.oldSceneTime = oldSceneTime;
            sp.newSceneTime = newSceneTime;
            sp.cb = cb;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, SceneAnimation animation, float eachSceneTime, XLAF_Tween.EaseType ease)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.oldSceneTime = eachSceneTime;
            sp.newSceneTime = eachSceneTime;
            sp.ease = ease;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, SceneAnimation animation, float eachSceneTime, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.oldSceneTime = eachSceneTime;
            sp.newSceneTime = eachSceneTime;
            sp.cb = cb;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, SceneAnimation animation, XLAF_Tween.EaseType ease, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.ease = ease;
            sp.cb = cb;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, float oldSceneTime, float newSceneTime, XLAF_Tween.EaseType ease, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.oldSceneTime = oldSceneTime;
            sp.newSceneTime = newSceneTime;
            sp.ease = ease;
            sp.cb = cb;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, float eachSceneTime, XLAF_Tween.EaseType ease, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.oldSceneTime = eachSceneTime;
            sp.newSceneTime = eachSceneTime;
            sp.ease = ease;
            sp.cb = cb;
            ShowDialog (sp);
        }

        // 5 group parameter => 9
        public static void ShowDialog (string sceneName, object data, SceneAnimation animation, float oldSceneTime, float newSceneTime, XLAF_Tween.EaseType ease)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.oldSceneTime = oldSceneTime;
            sp.newSceneTime = newSceneTime;
            sp.ease = ease;
            sp.data = data;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, object data, SceneAnimation animation, float oldSceneTime, float newSceneTime, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.oldSceneTime = oldSceneTime;
            sp.newSceneTime = newSceneTime;
            sp.data = data;
            sp.cb = cb;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, object data, SceneAnimation animation, float eachSceneTime, XLAF_Tween.EaseType ease)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.oldSceneTime = eachSceneTime;
            sp.newSceneTime = eachSceneTime;
            sp.ease = ease;
            sp.data = data;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, object data, SceneAnimation animation, float eachSceneTime, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.oldSceneTime = eachSceneTime;
            sp.newSceneTime = eachSceneTime;
            sp.data = data;
            sp.cb = cb;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, object data, SceneAnimation animation, XLAF_Tween.EaseType ease, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.ease = ease;
            sp.data = data;
            sp.cb = cb;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, object data, float oldSceneTime, float newSceneTime, XLAF_Tween.EaseType ease, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.oldSceneTime = oldSceneTime;
            sp.newSceneTime = newSceneTime;
            sp.ease = ease;
            sp.data = data;
            sp.cb = cb;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, object data, float eachSceneTime, XLAF_Tween.EaseType ease, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.oldSceneTime = eachSceneTime;
            sp.newSceneTime = eachSceneTime;
            sp.ease = ease;
            sp.data = data;
            sp.cb = cb;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, SceneAnimation animation, float oldSceneTime, float newSceneTime, XLAF_Tween.EaseType ease, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.oldSceneTime = oldSceneTime;
            sp.newSceneTime = newSceneTime;
            sp.ease = ease;
            sp.cb = cb;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, SceneAnimation animation, float eachSceneTime, XLAF_Tween.EaseType ease, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.oldSceneTime = eachSceneTime;
            sp.newSceneTime = eachSceneTime;
            sp.ease = ease;
            sp.cb = cb;
            ShowDialog (sp);
        }

        // 6 group parameter => 2
        public static void ShowDialog (string sceneName, object data, SceneAnimation animation, float oldSceneTime, float newSceneTime, XLAF_Tween.EaseType ease, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.oldSceneTime = oldSceneTime;
            sp.newSceneTime = newSceneTime;
            sp.ease = ease;
            sp.data = data;
            sp.cb = cb;
            ShowDialog (sp);
        }

        public static void ShowDialog (string sceneName, object data, SceneAnimation animation, float eachSceneTime, XLAF_Tween.EaseType ease, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.oldSceneTime = eachSceneTime;
            sp.newSceneTime = eachSceneTime;
            sp.ease = ease;
            sp.data = data;
            sp.cb = cb;
            ShowDialog (sp);
        }

        #endregion


        #region public HideDialog functions [override 25 times]

        // 1 group parameter => 2
        public static void HideDialog (SceneParams par)
        {
            string sceneName = par.sceneName;
            SceneAnimation animation = par.animation;
            float oldSceneTime = par.oldSceneTime;
            float newSceneTime = par.newSceneTime;
            XLAF_Tween.EaseType ease = par.ease;
            Action cb = par.cb;
            object data = par.data;
            float bgAlpha = par.bgAlpha;

            Log.Debug ("HideDialog (SceneParams par)", par.ToString ());

            SceneObject currentDialog = GetDialog (sceneName);
            if (currentDialog == null) {
                return;
            }
            currentDialog.DisableUIListener ();

            animating = true;
            switch (animation) {

            case SceneAnimation.none:
                instance._AnimationNone (false, sceneName, data, bgAlpha, cb);
                break;
            case SceneAnimation.fade:
                instance._AnimationFade (false, sceneName, data, bgAlpha, oldSceneTime, newSceneTime, cb);
                break;
            case SceneAnimation.fromRight:
                instance._AnimationFromRight (false, sceneName, data, bgAlpha, newSceneTime, ease, cb);
                break;
            case SceneAnimation.fromLeft:
                instance._AnimationFromLeft (false, sceneName, data, bgAlpha, newSceneTime, ease, cb);
                break;
            case SceneAnimation.fromTop:
                instance._AnimationFromTop (false, sceneName, data, bgAlpha, newSceneTime, ease, cb);
                break;
            case SceneAnimation.fromBottom:
                instance._AnimationFromBottom (false, sceneName, data, bgAlpha, newSceneTime, ease, cb);
                break;
            case SceneAnimation.slideLeft:
                Log.Warning ("SceneAnimation.slideLeft should not use in MgrDialog.HideDialog, use fromRight instead");
                instance._AnimationFromRight (false, sceneName, data, bgAlpha, newSceneTime, ease, cb);
                break;
            case SceneAnimation.slideRight:
                Log.Warning ("SceneAnimation.slideRight should not use in MgrDialog.HideDialog, use fromLeft instead");
                instance._AnimationFromLeft (false, sceneName, data, bgAlpha, newSceneTime, ease, cb);
                break;
            case SceneAnimation.slideDown:
                Log.Warning ("SceneAnimation.slideDown should not use in MgrDialog.HideDialog, use fromTop instead");
                instance._AnimationFromTop (false, sceneName, data, bgAlpha, newSceneTime, ease, cb);
                break;
            case SceneAnimation.slideUp:
                Log.Warning ("SceneAnimation.slideUp should not use in MgrDialog.HideDialog, use fromBottom instead");
                instance._AnimationFromBottom (false, sceneName, data, bgAlpha, newSceneTime, ease, cb);
                break;
            case SceneAnimation.zoomIn:
                Log.Warning ("SceneAnimation.zoomIn should not use in MgrDialog.HideDialog, use zoomOut instead");
                instance._AnimationZoomOut (sceneName, data, bgAlpha, newSceneTime, ease, cb);
                break;
            case SceneAnimation.zoomOut:
                instance._AnimationZoomOut (sceneName, data, bgAlpha, newSceneTime, ease, cb);
                break;
            }

        }

        public static void HideDialog (string sceneName)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            HideDialog (sp);
        }
        // 2 group parameter => 5
        public static void HideDialog (string sceneName, SceneAnimation animation)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            HideDialog (sp);
        }

        public static void HideDialog (string sceneName, float oldSceneTime, float newSceneTime)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.oldSceneTime = oldSceneTime;
            sp.newSceneTime = newSceneTime;
            HideDialog (sp);
        }

        public static void HideDialog (string sceneName, float eachSceneTime)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.oldSceneTime = eachSceneTime;
            sp.newSceneTime = eachSceneTime;
            HideDialog (sp);
        }

        public static void HideDialog (string sceneName, XLAF_Tween.EaseType ease)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.ease = ease;
            HideDialog (sp);
        }

        public static void HideDialog (string sceneName, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.cb = cb;
            HideDialog (sp);
        }

        // 3 group parameter => 9
        public static void HideDialog (string sceneName, SceneAnimation animation, float oldSceneTime, float newSceneTime)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.oldSceneTime = oldSceneTime;
            sp.newSceneTime = newSceneTime;
            HideDialog (sp);
        }

        public static void HideDialog (string sceneName, SceneAnimation animation, float eachSceneTime)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.oldSceneTime = eachSceneTime;
            sp.newSceneTime = eachSceneTime;
            HideDialog (sp);
        }

        public static void HideDialog (string sceneName, SceneAnimation animation, XLAF_Tween.EaseType ease)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.ease = ease;
            HideDialog (sp);
        }

        public static void HideDialog (string sceneName, SceneAnimation animation, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.cb = cb;
            HideDialog (sp);
        }

        public static void HideDialog (string sceneName, float oldSceneTime, float newSceneTime, XLAF_Tween.EaseType ease)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.oldSceneTime = oldSceneTime;
            sp.newSceneTime = newSceneTime;
            sp.ease = ease;
            HideDialog (sp);
        }

        public static void HideDialog (string sceneName, float oldSceneTime, float newSceneTime, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.oldSceneTime = oldSceneTime;
            sp.newSceneTime = newSceneTime;
            sp.cb = cb;
            HideDialog (sp);
        }

        public static void HideDialog (string sceneName, float eachSceneTime, XLAF_Tween.EaseType ease)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.oldSceneTime = eachSceneTime;
            sp.newSceneTime = eachSceneTime;
            sp.ease = ease;
            HideDialog (sp);
        }

        public static void HideDialog (string sceneName, float eachSceneTime, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.oldSceneTime = eachSceneTime;
            sp.newSceneTime = eachSceneTime;
            sp.cb = cb;
            HideDialog (sp);
        }

        public static void HideDialog (string sceneName, XLAF_Tween.EaseType ease, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.ease = ease;
            sp.cb = cb;
            HideDialog (sp);
        }
        // 4 group parameter => 7

        public static void HideDialog (string sceneName, SceneAnimation animation, float oldSceneTime, float newSceneTime, XLAF_Tween.EaseType ease)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.oldSceneTime = oldSceneTime;
            sp.newSceneTime = newSceneTime;
            sp.ease = ease;
            HideDialog (sp);
        }

        public static void HideDialog (string sceneName, SceneAnimation animation, float eachSceneTime, XLAF_Tween.EaseType ease)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.oldSceneTime = eachSceneTime;
            sp.newSceneTime = eachSceneTime;
            sp.ease = ease;
            HideDialog (sp);
        }

        public static void HideDialog (string sceneName, SceneAnimation animation, float oldSceneTime, float newSceneTime, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.oldSceneTime = oldSceneTime;
            sp.newSceneTime = newSceneTime;
            sp.cb = cb;
            HideDialog (sp);
        }

        public static void HideDialog (string sceneName, SceneAnimation animation, float eachSceneTime, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.oldSceneTime = eachSceneTime;
            sp.newSceneTime = eachSceneTime;
            sp.cb = cb;
            HideDialog (sp);
        }

        public static void HideDialog (string sceneName, SceneAnimation animation, XLAF_Tween.EaseType ease, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.ease = ease;
            sp.cb = cb;
            HideDialog (sp);
        }

        public static void HideDialog (string sceneName, float oldSceneTime, float newSceneTime, XLAF_Tween.EaseType ease, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.oldSceneTime = oldSceneTime;
            sp.newSceneTime = newSceneTime;
            sp.ease = ease;
            sp.cb = cb;
            HideDialog (sp);
        }

        public static void HideDialog (string sceneName, float eachSceneTime, XLAF_Tween.EaseType ease, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.oldSceneTime = eachSceneTime;
            sp.newSceneTime = eachSceneTime;
            sp.ease = ease;
            sp.cb = cb;
            HideDialog (sp);
        }
        // 5 group parameter => 2

        public static void HideDialog (string sceneName, SceneAnimation animation, float oldSceneTime, float newSceneTime, XLAF_Tween.EaseType ease, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.oldSceneTime = oldSceneTime;
            sp.newSceneTime = newSceneTime;
            sp.ease = ease;
            sp.cb = cb;
            HideDialog (sp);
        }

        public static void HideDialog (string sceneName, SceneAnimation animation, float eachSceneTime, XLAF_Tween.EaseType ease, Action cb)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.oldSceneTime = eachSceneTime;
            sp.newSceneTime = eachSceneTime;
            sp.ease = ease;
            sp.cb = cb;
            HideDialog (sp);
        }

        #endregion


        #region  private functions (animation functions)

        private static void _UnloadOldScene (SceneObject sceneObj = null)
        {
            if (sceneObj == null)
                return;
            
            //scene.SendMessage ("DestoryScene");
            string sceneName = sceneObj.script.sceneName;
            sceneObj.script.DestoryScene ();

            //OverlayEnded event
            SceneObject currScene = MgrScene.GetCurrentScene ();
            if (currScene != null) {
                currScene.script.OverlayEnded (sceneName);
            }

            if (DIALOGS_STACK.Contains (sceneObj)) {
                DIALOGS_STACK.Remove (sceneObj);
            }
            Destroy (sceneObj.scene.gameObject);
            DIALOGS.Remove (sceneName);
        }

        private static SceneObject _LoadNewScene (string sceneName, object data, float bgAlpha)
        {
            SceneObject currentDialog = GetDialog (sceneName);
            if (currentDialog != null) {
                currentDialog.EnableUIListener ();
            }
            SceneObject sceneObj;
            if (!DIALOGS.ContainsKey (sceneName)) {
                sceneObj = new SceneObject (string.Format (dialogPathFormat, sceneName));
                sceneObj.AddDialogBackground (bgAlpha);
                DIALOGS_STACK.Add (sceneObj);
                DIALOGS.Add (sceneName, sceneObj);
                sceneObj.script.CreatScene (data);
                sceneObj.script.UpdateLanguage ();
            } else {
                sceneObj = DIALOGS [sceneName];
            }

            //OverlayBegan event
            SceneObject currScene = MgrScene.GetCurrentScene ();
            if (currScene != null) {
                currScene.script.OverlayBegan (sceneName);
            }

            sceneObj.DisableUIListener ();
            sceneObj.scene.transform.SetParent (dialogViewRoot, false);
            sceneObj.scene.SetActive (true);
            sceneObj.scene.transform.SetAsLastSibling ();//置顶
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
            SceneObject currentDialog;
            if (isShow) {
                currentDialog = _LoadNewScene (sceneName, data, bgAlpha);
                if (cb != null)
                    cb ();
                currentDialog.script.EnterScene (data);
                currentDialog.EnableUIListener ();
                animating = false;
            } else {
                currentDialog = GetDialog (sceneName);
                if (currentDialog == null) {
                    return;
                }
                currentDialog.script.WillExitScene ();
                currentDialog.script.ExitScene ();
                _UnloadOldScene (currentDialog);
            }
        }

        private void _AnimationFade (bool isShow, string sceneName, object data, float bgAlpha, float fadeInTime, float fadeOutTime, Action cb)
        {

            SceneObject currentDialog;
            if (isShow) { 
                currentDialog = _LoadNewScene (sceneName, data, bgAlpha);// load new scene  or   set exise scene
                currentDialog.ChangeAlpha (0f);
                XLAF_Tween.ValueTo (currentDialog.scene, XLAF_Tween.Hash (
                    "from", 0,
                    "to", 1,
                    "time", fadeOutTime,
                    "onupdate", (Action<float>)((alpha) => {
                    currentDialog.ChangeAlpha (alpha);
                }),
                    "oncomplete", (Action)(() => {
                    if (cb != null)
                        cb ();
                    currentDialog.script.EnterScene (data);
                    currentDialog.EnableUIListener ();
                    animating = false;
                })
                ));
            } else {
                currentDialog = GetDialog (sceneName);
                if (currentDialog == null) {
                    return;
                }
                currentDialog.script.WillExitScene ();
                XLAF_Tween.ValueTo (currentDialog.scene, XLAF_Tween.Hash (
                    "from", 1,
                    "to", 0,
                    "time", fadeInTime,
                    "onupdate", (Action<float>)((alpha) => {
                    currentDialog.ChangeAlpha (alpha);
                }),
                    "oncomplete", (Action)(() => {
                    currentDialog.script.ExitScene ();
                    _UnloadOldScene (currentDialog);
                })
                ));
            }
        }

        private void _AnimationFromLeft (bool isShow, string sceneName, object data, float bgAlpha, float newSceneTime, XLAF_Tween.EaseType easeType, Action cb)
        {
            XLAF_Tween.EaseType ease = (easeType == XLAF_Tween.EaseType.defaultType) ? XLAF_Tween.EaseType.easeOutExpo : easeType;

            SceneObject currentDialog;

            if (isShow) {
                currentDialog = _LoadNewScene (sceneName, data, bgAlpha);// load new scene  or   set exise scene

                float nomalX = currentDialog.scene.transform.position.x;
                float nomalY = currentDialog.scene.transform.position.y;
                float newSceneStartX = nomalX - screenWidth;

                RectTransform tmpRT = currentDialog.scene.GetComponent<RectTransform> ();
                tmpRT.position = new Vector3 (newSceneStartX, nomalY);
                XLAF_Tween.MoveTo (currentDialog.scene, XLAF_Tween.Hash (
                    "x", nomalX,
                    "time", newSceneTime,
                    "easetype", ease,
                    "oncomplete", (Action)(() => {
                    if (cb != null)
                        cb ();
                    currentDialog.script.EnterScene (data);
                    currentDialog.EnableUIListener ();
                    animating = false;
                })
                ));
            } else {
                currentDialog = GetDialog (sceneName);
                if (currentDialog == null) {
                    return;
                }
                float nomalX = currentDialog.scene.transform.position.x;
                float newSceneStartX = nomalX - screenWidth;

                currentDialog.script.WillExitScene ();
                XLAF_Tween.MoveTo (currentDialog.scene, XLAF_Tween.Hash (
                    "x", newSceneStartX,
                    "time", newSceneTime,
                    "easetype", ease,
                    "oncomplete", (Action)(() => {
                    currentDialog.script.ExitScene ();
                    _UnloadOldScene (currentDialog);
                })
                ));
            }
        }

        private void _AnimationFromRight (bool isShow, string sceneName, object data, float bgAlpha, float newSceneTime, XLAF_Tween.EaseType easeType, Action cb)
        {
            XLAF_Tween.EaseType ease = (easeType == XLAF_Tween.EaseType.defaultType) ? XLAF_Tween.EaseType.easeOutExpo : easeType;
            SceneObject currentDialog;


            if (isShow) {
                currentDialog = _LoadNewScene (sceneName, data, bgAlpha);// load new scene  or   set exise scene
                float nomalX = currentDialog.scene.transform.position.x;
                float nomalY = currentDialog.scene.transform.position.y;
                float newSceneStartX = nomalX + screenWidth;
                RectTransform tmpRT = currentDialog.scene.GetComponent<RectTransform> ();
                tmpRT.position = new Vector3 (newSceneStartX, nomalY);
                XLAF_Tween.MoveTo (currentDialog.scene, XLAF_Tween.Hash (
                    "x", nomalX,
                    "time", newSceneTime,
                    "easetype", ease,
                    "oncomplete", (Action)(() => {
                    if (cb != null)
                        cb ();
                    currentDialog.script.EnterScene (data);
                    currentDialog.EnableUIListener ();
                    animating = false;
                })
                ));
            } else {
                currentDialog = GetDialog (sceneName);
                if (currentDialog == null) {
                    return;
                }
                float nomalX = currentDialog.scene.transform.position.x;
                float newSceneStartX = nomalX - screenWidth;
                currentDialog.script.WillExitScene ();
                XLAF_Tween.MoveTo (currentDialog.scene, XLAF_Tween.Hash (
                    "x", newSceneStartX,
                    "time", newSceneTime,
                    "easetype", ease,
                    "oncomplete", (Action)(() => {
                    currentDialog.script.ExitScene ();
                    _UnloadOldScene (currentDialog);
                })
                ));
            }
        }

        private void _AnimationFromTop (bool isShow, string sceneName, object data, float bgAlpha, float newSceneTime, XLAF_Tween.EaseType easeType, Action cb)
        {

            XLAF_Tween.EaseType ease = (easeType == XLAF_Tween.EaseType.defaultType) ? XLAF_Tween.EaseType.easeOutExpo : easeType;
            SceneObject currentDialog;

            if (isShow) {
                currentDialog = _LoadNewScene (sceneName, data, bgAlpha);// load new scene  or   set exise scene
                float nomalX = currentDialog.scene.transform.position.x;
                float nomalY = currentDialog.scene.transform.position.y;
                float newSceneStartY = nomalY + screenHeight;
                RectTransform tmpRT = currentDialog.scene.GetComponent<RectTransform> ();
                tmpRT.position = new Vector3 (nomalX, newSceneStartY);
                XLAF_Tween.MoveTo (currentDialog.scene, XLAF_Tween.Hash (
                    "y", nomalY,
                    "time", newSceneTime,
                    "easetype", ease,
                    "oncomplete", (Action)(() => {
                    if (cb != null)
                        cb ();
                    currentDialog.script.EnterScene (data);
                    currentDialog.EnableUIListener ();
                    animating = false;
                })
                ));
            } else {

                currentDialog = GetDialog (sceneName);
                if (currentDialog == null) {
                    return;
                }
                float nomalY = currentDialog.scene.transform.position.y;
                float newSceneStartY = nomalY + screenHeight;
                currentDialog.script.WillExitScene ();
                XLAF_Tween.MoveTo (currentDialog.scene, XLAF_Tween.Hash (
                    "y", newSceneStartY,
                    "time", newSceneTime,
                    "easetype", ease,
                    "oncomplete", (Action)(() => {
                    currentDialog.script.ExitScene ();
                    _UnloadOldScene (currentDialog);
                })
                ));
            }


        }

        private void _AnimationFromBottom (bool isShow, string sceneName, object data, float bgAlpha, float newSceneTime, XLAF_Tween.EaseType easeType, Action cb)
        {

            XLAF_Tween.EaseType ease = (easeType == XLAF_Tween.EaseType.defaultType) ? XLAF_Tween.EaseType.easeOutExpo : easeType;
            SceneObject currentDialog;

            if (isShow) {
                currentDialog = _LoadNewScene (sceneName, data, bgAlpha);// load new scene  or   set exise scene
                float nomalX = currentDialog.scene.transform.position.x;
                float nomalY = currentDialog.scene.transform.position.y;
                float newSceneStartY = nomalY - screenHeight;
                RectTransform tmpRT = currentDialog.scene.GetComponent<RectTransform> ();
                tmpRT.position = new Vector3 (nomalX, newSceneStartY);
                XLAF_Tween.MoveTo (currentDialog.scene, XLAF_Tween.Hash (
                    "y", nomalY,
                    "time", newSceneTime,
                    "easetype", ease,
                    "oncomplete", (Action)(() => {
                    if (cb != null)
                        cb ();
                    currentDialog.script.EnterScene (data);
                    currentDialog.EnableUIListener ();
                    animating = false;
                })
                ));
            } else {

                currentDialog = GetDialog (sceneName);
                if (currentDialog == null) {
                    return;
                }
                float nomalY = currentDialog.scene.transform.position.y;
                float newSceneStartY = nomalY - screenHeight;
                currentDialog.script.WillExitScene ();
                XLAF_Tween.MoveTo (currentDialog.scene, XLAF_Tween.Hash (
                    "y", newSceneStartY,
                    "time", newSceneTime,
                    "easetype", ease,
                    "oncomplete", (Action)(() => {
                    currentDialog.script.ExitScene ();
                    _UnloadOldScene (currentDialog);
                })
                ));
            }
        }

        private void _AnimationZoomIn (string sceneName, object data, float bgAlpha, float newSceneTime, XLAF_Tween.EaseType easeType, Action cb)
        {
            
            XLAF_Tween.EaseType ease = (easeType == XLAF_Tween.EaseType.defaultType) ? XLAF_Tween.EaseType.easeOutBack : easeType;
            SceneObject currentDialog = _LoadNewScene (sceneName, data, bgAlpha);// load new scene  or   set exise scene
            currentDialog.scene.transform.localScale = new Vector3 (1f, 1f);
            XLAF_Tween.ScaleFrom (currentDialog.scene, XLAF_Tween.Hash (
                "scale", new Vector3 (0f, 0f),
                "time", newSceneTime,
                "easetype", ease,
                "oncomplete", (Action)(() => {
                if (cb != null)
                    cb ();
                currentDialog.script.EnterScene (data);
                currentDialog.EnableUIListener ();
                animating = false;
            })
            ));

        }

        private void _AnimationZoomOut (string sceneName, object data, float bgAlpha, float newSceneTime, XLAF_Tween.EaseType easeType, Action cb)
        {
            XLAF_Tween.EaseType ease = (easeType == XLAF_Tween.EaseType.defaultType) ? XLAF_Tween.EaseType.easeInBack : easeType;

            SceneObject currentDialog = GetDialog (sceneName);
            if (currentDialog == null) {
                return;
            }
            currentDialog.scene.transform.localScale = new Vector3 (1f, 1f);
            XLAF_Tween.ScaleTo (currentDialog.scene, XLAF_Tween.Hash (
                "scale", new Vector3 (0f, 0f),
                "time", newSceneTime,
                "easetype", ease,
                "oncomplete", (Action)(() => {
                if (cb != null)
                    cb ();
                currentDialog.script.EnterScene (data);
                currentDialog.EnableUIListener ();
                animating = false;
            })
            ));
        }

        #endregion


        // Update is called once per frame
        void Update ()
        {
            if (dialogViewRoot == null)
                return;
            SetDialogViewVisible (hasDialog);
            #if UNITY_ANDROID
            if (Input.GetKeyDown (KeyCode.Escape)) { //android back
                SceneObject curr = MgrDialog.GetTop ();
                if (curr != null) {
                    curr.script.AndroidGoBack ();
                }
            }
            #endif
        }

    }
}