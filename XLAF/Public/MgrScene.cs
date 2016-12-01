using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace XLAF.Public
{
    /// <summary>
    /// scene管理
    /// </summary>
    public class MgrScene : MonoBehaviour
    {
        public static bool destoryOnSceneChange = false;


        static MgrScene ()
        {
            SCENES = new Dictionary<string, SceneObject> ();
            instance = (new GameObject ("MgrScene")).AddComponent<MgrScene> ();

        }


        /// <summary>
        /// 调用Init会触发构造函数，可以用于统一初始化的时候
        /// </summary>
        public static void Init ()
        {

        }


        private static MgrScene instance = null;
        private static readonly string scenePathFormat = "_Scenes/{0}";


        private static bool animating = false;
        private static SceneObject currentScene = null;
        private static Transform sceneViewRoot = null;
        private static CanvasGroup sceneViewRootCanvas = null;

        private static float screenWidth;
        private static float screenHeight;

        private static Dictionary<string,SceneObject> SCENES;

        /// <summary>
        /// Sets the view root.
        /// </summary>
        /// <param name="grp">Transform.</param>
        public static void SetViewRoot (Transform grp)
        {
            sceneViewRoot = grp;
            sceneViewRootCanvas = sceneViewRoot.transform.GetComponent<CanvasGroup> ();

            screenHeight = Screen.height;
            screenWidth = Screen.width;
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
        /// Loads the scene.
        /// 用于scene的东西比较多，需要提前加载的情况
        /// </summary>
        /// <param name="sceneName">Scene name.</param>
        /// <param name="data">要传递给scene的数据</param>
        public static void LoadScene (string sceneName, object data)
        {
            string fullSceneNamePath = string.Format (scenePathFormat, sceneName);
            if (SCENES.ContainsKey (fullSceneNamePath))
                return;
            SceneObject sceneObj = new SceneObject (fullSceneNamePath);
            SCENES.Add (fullSceneNamePath, sceneObj);
            sceneObj.script.CreatScene (data);

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

        #region public GotoScene functions

        // goto the scene
        public static void GotoScene (SceneParams par)
        {
            string sceneName = par.sceneName;
            SceneAnimation animation = par.animation;
            float oldSceneTime = par.oldSceneTime;
            float newSceneTime = par.newSceneTime;
            XLAF_Tween.EaseType ease = par.ease;
            Action cb = par.cb;
            object data = par.data;


            Log.Debug ("GotoScene (SceneParams par)", par.ToString ());

            if (currentScene != null) {
                currentScene.DisableUIListener ();
            }

            animating = true;
            switch (animation) {

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

        public static void GotoScene (string sceneName, object data = null, SceneAnimation animation = SceneAnimation.fade, float oldSceneTime = 0.5f, float newSceneTime = 0.5f, XLAF_Tween.EaseType ease = XLAF_Tween.EaseType.defaultType, Action cb = null)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.oldSceneTime = oldSceneTime;
            sp.newSceneTime = newSceneTime;
            sp.ease = ease;
            sp.cb = cb;
            sp.data = data;
            GotoScene (sp);
        }

        public static void GotoScene (string sceneName, object data = null, SceneAnimation animation = SceneAnimation.fade, float eachSceneTime = 0.5f, XLAF_Tween.EaseType ease = XLAF_Tween.EaseType.defaultType, Action cb = null)
        {
            GotoScene (sceneName, data, animation, eachSceneTime, eachSceneTime, ease, cb);
        }

        public static void GotoScene (string sceneName, object data = null, SceneAnimation animation = SceneAnimation.fade, float eachSceneTime = 0.5f, Action cb = null)
        {
            GotoScene (sceneName, data, animation, eachSceneTime, eachSceneTime, XLAF_Tween.EaseType.defaultType, cb);
        }

        public static void GotoScene (string sceneName, object data = null, SceneAnimation animation = SceneAnimation.fade, float eachSceneTime = 0.5f, XLAF_Tween.EaseType ease = XLAF_Tween.EaseType.defaultType)
        {
            GotoScene (sceneName, data, animation, eachSceneTime, eachSceneTime, XLAF_Tween.EaseType.defaultType);
        }

        public static void GotoScene (string sceneName, object data = null, SceneAnimation animation = SceneAnimation.fade, XLAF_Tween.EaseType ease = XLAF_Tween.EaseType.defaultType, Action cb = null)
        {
            GotoScene (sceneName, data, animation, 0.5f, 0.5f, ease, cb);
        }

        public static void GotoScene (string sceneName, object data = null, SceneAnimation animation = SceneAnimation.fade, Action cb = null)
        {
            GotoScene (sceneName, data, animation, 0.5f, 0.5f, XLAF_Tween.EaseType.defaultType, cb);
        }


        public static void GotoScene (string sceneName, SceneAnimation animation = SceneAnimation.fade, float oldSceneTime = 0.5f, float newSceneTime = 0.5f, XLAF_Tween.EaseType ease = XLAF_Tween.EaseType.defaultType, Action cb = null)
        {
            SceneParams sp = new SceneParams ();
            sp.sceneName = sceneName;
            sp.animation = animation;
            sp.oldSceneTime = oldSceneTime;
            sp.newSceneTime = newSceneTime;
            sp.ease = ease;
            sp.cb = cb;
            GotoScene (sp);
        }

        public static void GotoScene (string sceneName, SceneAnimation animation = SceneAnimation.fade, float eachSceneTime = 0.5f, XLAF_Tween.EaseType ease = XLAF_Tween.EaseType.defaultType, Action cb = null)
        {
            GotoScene (sceneName, animation, eachSceneTime, eachSceneTime, ease, cb);
        }

        public static void GotoScene (string sceneName, SceneAnimation animation = SceneAnimation.fade, float eachSceneTime = 0.5f, Action cb = null)
        {
            GotoScene (sceneName, animation, eachSceneTime, eachSceneTime, XLAF_Tween.EaseType.defaultType, cb);
        }

        public static void GotoScene (string sceneName, SceneAnimation animation = SceneAnimation.fade, float eachSceneTime = 0.5f, XLAF_Tween.EaseType ease = XLAF_Tween.EaseType.defaultType)
        {
            GotoScene (sceneName, animation, eachSceneTime, eachSceneTime, XLAF_Tween.EaseType.defaultType);
        }

        public static void GotoScene (string sceneName, SceneAnimation animation = SceneAnimation.fade, XLAF_Tween.EaseType ease = XLAF_Tween.EaseType.defaultType, Action cb = null)
        {
            GotoScene (sceneName, animation, 0.5f, 0.5f, ease, cb);
        }

        public static void GotoScene (string sceneName, SceneAnimation animation = SceneAnimation.fade, Action cb = null)
        {
            GotoScene (sceneName, animation, 0.5f, 0.5f, XLAF_Tween.EaseType.defaultType, cb);
        }

        #endregion

        #region  private functions (animation functions)

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
            }
        }

        private static void _LoadNewScene (string sceneName, object data)
        {
            if (currentScene != null) {
                currentScene.EnableUIListener ();
            }

            string fullSceneNamePath = string.Format (scenePathFormat, sceneName);
            if (!SCENES.ContainsKey (fullSceneNamePath)) {
                SceneObject sceneObj = new SceneObject (fullSceneNamePath);
                currentScene = sceneObj;
                SCENES.Add (fullSceneNamePath, sceneObj);
                currentScene.script.CreatScene (data);
                //currentScene.SendMessage ("CreatScene", data);
            } else {
                currentScene = SCENES [fullSceneNamePath];
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
                sceneViewRootCanvas.alpha = 1f;
                XLAF_Tween.ValueTo (oldScene.scene, XLAF_Tween.Hash (
                    "from", 1,
                    "to", 0,
                    "time", fadeInTime,
                    "onupdate", (Action<float>)((alpha) => {
                    sceneViewRootCanvas.alpha = alpha;
                }),
                    "oncomplete", (Action)(() => {
                    oldScene.script.ExitScene ();
                    _UnloadOldScene (oldScene);
                    _LoadNewScene (sceneName, data);
                    XLAF_Tween.ValueTo (currentScene.scene, XLAF_Tween.Hash (
                        "from", 0,
                        "to", 1,
                        "time", fadeOutTime,
                        "onupdate", (Action<float>)((alpha) => {
                        sceneViewRootCanvas.alpha = alpha;
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
                XLAF_Tween.ValueTo (currentScene.scene, XLAF_Tween.Hash (
                    "from", 0,
                    "to", 1,
                    "time", fadeOutTime,
                    "onupdate", (Action<float>)((alpha) => {
                    sceneViewRootCanvas.alpha = alpha;
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

        private void _AnimationFromLeft (string sceneName, object data, float newSceneTime, XLAF_Tween.EaseType easeType, Action cb)
        {
            if (currentScene == null) {
                //Log.Error ("You should NOT use this animation in the first storyoard, use fade instead");

                _AnimationNone (sceneName, data, cb);
                return;
            }
            XLAF_Tween.EaseType ease = (easeType == XLAF_Tween.EaseType.defaultType) ? XLAF_Tween.EaseType.easeOutExpo : easeType;

            float nomalX = currentScene.scene.transform.position.x;
            float nomalY = currentScene.scene.transform.position.y;
            float newSceneStartX = nomalX - screenWidth;

            SceneObject oldScene = currentScene;
            _LoadNewScene (sceneName, data);// load new scene  or   set exise scene

            RectTransform tmpRT = currentScene.scene.GetComponent<RectTransform> ();
            tmpRT.position = new Vector3 (newSceneStartX, nomalY);

            oldScene.script.WillExitScene ();
            XLAF_Tween.MoveTo (currentScene.scene, XLAF_Tween.Hash (
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

        private void _AnimationFromRight (string sceneName, object data, float newSceneTime, XLAF_Tween.EaseType easeType, Action cb)
        {
            if (currentScene == null) {
                //Log.Error ("You should NOT use this animation in the first storyoard, use fade instead");

                _AnimationNone (sceneName, data, cb);
                return;
            }
            XLAF_Tween.EaseType ease = (easeType == XLAF_Tween.EaseType.defaultType) ? XLAF_Tween.EaseType.easeOutExpo : easeType;

            float nomalX = currentScene.scene.transform.position.x;
            float nomalY = currentScene.scene.transform.position.y;
            float newSceneStartX = nomalX + screenWidth;

            SceneObject oldScene = currentScene;
            _LoadNewScene (sceneName, data);// load new scene  or   set exise scene



            RectTransform tmpRT = currentScene.scene.GetComponent<RectTransform> ();
            tmpRT.position = new Vector3 (newSceneStartX, nomalY);

            oldScene.script.WillExitScene ();
            XLAF_Tween.MoveTo (currentScene.scene, XLAF_Tween.Hash (
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

        private void _AnimationFromTop (string sceneName, object data, float newSceneTime, XLAF_Tween.EaseType easeType, Action cb)
        {
            if (currentScene == null) {
                //Log.Error ("You should NOT use this animation in the first storyoard, use fade instead");

                _AnimationNone (sceneName, data, cb);
                return;
            }
            XLAF_Tween.EaseType ease = (easeType == XLAF_Tween.EaseType.defaultType) ? XLAF_Tween.EaseType.easeOutExpo : easeType;

            float nomalX = currentScene.scene.transform.position.x;
            float nomalY = currentScene.scene.transform.position.y;
            float newSceneStartY = nomalY + screenHeight;

            SceneObject oldScene = currentScene;
            _LoadNewScene (sceneName, data);// load new scene  or   set exise scene



            RectTransform tmpRT = currentScene.scene.GetComponent<RectTransform> ();
            tmpRT.position = new Vector3 (nomalX, newSceneStartY);

            oldScene.script.WillExitScene ();
            XLAF_Tween.MoveTo (currentScene.scene, XLAF_Tween.Hash (
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

        private void _AnimationFromBottom (string sceneName, object data, float newSceneTime, XLAF_Tween.EaseType easeType, Action cb)
        {
            if (currentScene == null) {
                //Log.Error ("You should NOT use this animation in the first storyoard, use fade instead");

                _AnimationNone (sceneName, data, cb);
                return;
            }
            XLAF_Tween.EaseType ease = (easeType == XLAF_Tween.EaseType.defaultType) ? XLAF_Tween.EaseType.easeOutExpo : easeType;

            float nomalX = currentScene.scene.transform.position.x;
            float nomalY = currentScene.scene.transform.position.y;
            float newSceneStartY = nomalY - screenHeight;

            SceneObject oldScene = currentScene;
            _LoadNewScene (sceneName, data);// load new scene  or   set exise scene



            RectTransform tmpRT = currentScene.scene.GetComponent<RectTransform> ();
            tmpRT.position = new Vector3 (nomalX, newSceneStartY);

            oldScene.script.WillExitScene ();
            XLAF_Tween.MoveTo (currentScene.scene, XLAF_Tween.Hash (
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

        private void _AnimationSlideLeft (string sceneName, object data, float newSceneTime, XLAF_Tween.EaseType easeType, Action cb = null)
        {
            if (currentScene == null) {
                //Log.Error ("You should NOT use this animation in the first storyoard, use fade instead");

                _AnimationNone (sceneName, data, cb);
                return;
            }
            XLAF_Tween.EaseType ease = (easeType == XLAF_Tween.EaseType.defaultType) ? XLAF_Tween.EaseType.easeOutExpo : easeType;
            
            float nomalX = currentScene.scene.transform.position.x;
            float nomalY = currentScene.scene.transform.position.y;
            float newSceneStartX = nomalX + screenWidth;
            float oldSceneEndX = nomalX - screenWidth;

            SceneObject oldScene = currentScene;
            _LoadNewScene (sceneName, data);// load new scene  or   set exise scene



            RectTransform tmpRT = currentScene.scene.GetComponent<RectTransform> ();
            tmpRT.position = new Vector3 (newSceneStartX, nomalY);

            oldScene.script.WillExitScene ();
            XLAF_Tween.MoveTo (oldScene.scene, XLAF_Tween.Hash (
                "x", oldSceneEndX,
                "time", newSceneTime,
                "easetype", ease
            ));

            XLAF_Tween.MoveTo (currentScene.scene, XLAF_Tween.Hash (
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

        private void _AnimationSlideRight (string sceneName, object data, float newSceneTime, XLAF_Tween.EaseType easeType, Action cb = null)
        {
            if (currentScene == null) {
                //Log.Error ("You should NOT use this animation in the first storyoard, use fade instead");

                _AnimationNone (sceneName, data, cb);
                return;
            }
            XLAF_Tween.EaseType ease = (easeType == XLAF_Tween.EaseType.defaultType) ? XLAF_Tween.EaseType.easeOutExpo : easeType;

            float nomalX = currentScene.scene.transform.position.x;
            float nomalY = currentScene.scene.transform.position.y;
            float newSceneStartX = nomalX - screenWidth;
            float oldSceneEndX = nomalX + screenWidth;

            SceneObject oldScene = currentScene;
            _LoadNewScene (sceneName, data);// load new scene  or   set exise scene

            RectTransform tmpRT = currentScene.scene.GetComponent<RectTransform> ();
            tmpRT.position = new Vector3 (newSceneStartX, nomalY);

            oldScene.script.WillExitScene ();
            XLAF_Tween.MoveTo (oldScene.scene, XLAF_Tween.Hash (
                "x", oldSceneEndX,
                "time", newSceneTime,
                "easetype", ease
            ));

            XLAF_Tween.MoveTo (currentScene.scene, XLAF_Tween.Hash (
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

        private void _AnimationSlideUp (string sceneName, object data, float newSceneTime, XLAF_Tween.EaseType easeType, Action cb)
        {
            if (currentScene == null) {
                //Log.Error ("You should NOT use this animation in the first storyoard, use fade instead");

                _AnimationNone (sceneName, data, cb);
                return;
            }
            XLAF_Tween.EaseType ease = (easeType == XLAF_Tween.EaseType.defaultType) ? XLAF_Tween.EaseType.easeOutExpo : easeType;

            float nomalX = currentScene.scene.transform.position.x;
            float nomalY = currentScene.scene.transform.position.y;
            float newSceneStartY = nomalY - screenHeight;
            float oldSceneEndY = nomalY + screenHeight;


            SceneObject oldScene = currentScene;
            _LoadNewScene (sceneName, data);// load new scene  or   set exise scene

            RectTransform tmpRT = currentScene.scene.GetComponent<RectTransform> ();
            tmpRT.position = new Vector3 (nomalX, newSceneStartY);

            oldScene.script.WillExitScene ();
            XLAF_Tween.MoveTo (oldScene.scene, XLAF_Tween.Hash (
                "y", oldSceneEndY,
                "time", newSceneTime,
                "easetype", ease
            ));

            XLAF_Tween.MoveTo (currentScene.scene, XLAF_Tween.Hash (
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

        private void _AnimationSlideDown (string sceneName, object data, float newSceneTime, XLAF_Tween.EaseType easeType, Action cb)
        {
            if (currentScene == null) {
                //Log.Error ("You should NOT use this animation in the first storyoard, use fade instead");
                _AnimationNone (sceneName, data, cb);
                return;
            }
            XLAF_Tween.EaseType ease = (easeType == XLAF_Tween.EaseType.defaultType) ? XLAF_Tween.EaseType.easeOutExpo : easeType;

            float nomalX = currentScene.scene.transform.position.x;
            float nomalY = currentScene.scene.transform.position.y;
            float newSceneStartY = nomalY + screenHeight;
            float oldSceneEndY = nomalY - screenHeight;


            SceneObject oldScene = currentScene;
            _LoadNewScene (sceneName, data);// load new scene  or   set exise scene

            RectTransform tmpRT = currentScene.scene.GetComponent<RectTransform> ();
            tmpRT.position = new Vector3 (nomalX, newSceneStartY);

            oldScene.script.WillExitScene ();
            XLAF_Tween.MoveTo (oldScene.scene, XLAF_Tween.Hash (
                "y", oldSceneEndY,
                "time", newSceneTime,
                "easetype", ease
            ));

            XLAF_Tween.MoveTo (currentScene.scene, XLAF_Tween.Hash (
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

        private void _AnimationZoomIn (string sceneName, object data, float newSceneTime, XLAF_Tween.EaseType easeType, Action cb)
        {
            XLAF_Tween.EaseType ease = (easeType == XLAF_Tween.EaseType.defaultType) ? XLAF_Tween.EaseType.easeOutBack : easeType;

            SceneObject oldScene = currentScene;
            _LoadNewScene (sceneName, data);// load new scene  or   set exise scene
            currentScene.scene.transform.localScale = new Vector3 (1f, 1f);
            oldScene.script.WillExitScene ();
            XLAF_Tween.ScaleFrom (currentScene.scene, XLAF_Tween.Hash (
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

        private void _AnimationZoomOut (string sceneName, object data, float newSceneTime, XLAF_Tween.EaseType easeType, Action cb)
        {
            XLAF_Tween.EaseType ease = (easeType == XLAF_Tween.EaseType.defaultType) ? XLAF_Tween.EaseType.easeInBack : easeType;

            SceneObject oldScene = currentScene;
            _LoadNewScene (sceneName, data);// load new scene  or   set exise scene
            currentScene.scene.transform.localScale = new Vector3 (1f, 1f);
            oldScene.script.WillExitScene ();
            oldScene.scene.transform.SetAsLastSibling ();//旧的scene在最上
            XLAF_Tween.ScaleTo (oldScene.scene, XLAF_Tween.Hash (
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
        public 	SceneAnimation animation = SceneAnimation.fade;
        public 	float oldSceneTime = 0.5f;
        public 	float newSceneTime = 0.5f;
        public 	XLAF_Tween.EaseType ease = XLAF_Tween.EaseType.defaultType;
        public 	Action cb = null;
        public object data = "";

        public override string ToString ()
        {
            return  " sceneName:" + sceneName
            + "\t animation:" + animation.ToString ()
            + "\t oldSceneTime:" + oldSceneTime
            + "\t newSceneTime:" + newSceneTime
            + "\t EaseType:" + ease.ToString ()
            + "\t data:" + data.ToString ()
            + "\t callback is null:" + (cb == null);
        }

    }


}

