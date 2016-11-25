using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace XLAF.Public
{
    public class MgrScene : MonoBehaviour
    {
        public static bool destoryOnSceneChange = false;


        static MgrScene ()
        {
            SCENES = new Dictionary<string, GameObject> ();
            instance = (new GameObject ("MgrScene")).AddComponent<MgrScene> ();

        }



        private static MgrScene instance = null;


        private static bool animating = false;
        private static GameObject currentScene = null;
        private static Transform sceneViewRoot = null;
        private static CanvasGroup sceneViewRootCanvas = null;

        private static float screenWidth;
        private static float screenHeight;

        private static Dictionary<string,GameObject> SCENES;

        public static void SetViewRoot (Transform grp)
        {
            sceneViewRoot = grp;
            sceneViewRootCanvas = sceneViewRoot.transform.GetComponent<CanvasGroup> ();

            screenHeight = Screen.height;
            screenWidth = Screen.width;
        }

        public static Transform GetViewRoot ()
        {
            return sceneViewRoot;
        }

        public static CanvasGroup GetViewRootCanvas ()
        {
            return sceneViewRootCanvas;
        }

        public static GameObject GetCurrentScene ()
        {
            return currentScene;
        }

        public static Dictionary<string,GameObject> GetAllScenes ()
        {
            return SCENES;
        }

        public static bool isAnimating {
            get {
                return animating;
            }
        }

        /// <summary>
        ///  加载scene  用于scene的东西比较多，需要提前加载的情况
        /// </summary>
        /// <param name="sceneName">Scene name.</param>
        public static void LoadScene (string sceneName)
        {

            if (SCENES.ContainsKey (sceneName))
                return;
            
            UnityEngine.Object _prefab = Resources.Load (sceneName);
            GameObject scene = (GameObject)Instantiate (_prefab);
            scene.name = sceneName;
            SCENES.Add (sceneName, scene);
            scene.transform.SetParent (sceneViewRoot, false);
            scene.SetActive (false);
            scene.transform.SetAsFirstSibling ();//置底

        }

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
                currentScene.AddComponent<ignoreUIListener> ();
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

        /////////////////////////////////////////////////// private functions  /////////////////////////////////////////////////////////////////

        private static void _UnloadOldScene (GameObject scene = null)
        {
            if (scene == null)
                return;
		
            if (destoryOnSceneChange) {
                scene.SendMessage ("DestoryScene");
                Destroy (scene.gameObject);
                SCENES.Remove (scene.name);
            } else {
                scene.SetActive (false);
            }
        }

        private static void _LoadNewScene (string sceneName, object data)
        {
            if (currentScene != null) {
                Destroy (currentScene.GetComponent<ignoreUIListener> ());
            }

            if (!SCENES.ContainsKey (sceneName)) {
                UnityEngine.Object _prefab = Resources.Load (sceneName);
                GameObject scene = (GameObject)Instantiate (_prefab);
                scene.name = sceneName;
                currentScene = scene;
                SCENES.Add (sceneName, currentScene);
                currentScene.SendMessage ("CreatScene", data);
            } else {
                currentScene = SCENES [sceneName];
            }
            currentScene.AddComponent<ignoreUIListener> ();
            currentScene.transform.SetParent (sceneViewRoot, false);
            currentScene.SetActive (true);
            currentScene.transform.SetAsLastSibling ();//置顶
            currentScene.SendMessage ("WillEnterScene", data);
        }

        private void _AnimationNone (string sceneName, object data, Action cb)
        {
            GameObject oldScene = currentScene;
            _LoadNewScene (sceneName, data);
            if (oldScene != null) {
                oldScene.SendMessage ("WillExitScene");
                oldScene.SendMessage ("ExitScene");
                _UnloadOldScene (oldScene);
            }
            if (cb != null)
                cb ();
            currentScene.SendMessage ("EnterScene", data);
            Destroy (currentScene.GetComponent<ignoreUIListener> ());
            animating = false;
        }

        private void _AnimationFade (string sceneName, object data, float fadeInTime, float fadeOutTime, Action cb)
        {
            GameObject oldScene = currentScene;
            if (oldScene != null) {
                oldScene.SendMessage ("WillExitScene");
                //恢复到透明度1
                sceneViewRootCanvas.alpha = 1f;
                XLAF_Tween.ValueTo (oldScene, XLAF_Tween.Hash (
                    "from", 1,
                    "to", 0,
                    "time", fadeInTime,
                    "onupdate", (Action<float>)((alpha) => {
                    sceneViewRootCanvas.alpha = alpha;
                }),
                    "oncomplete", (Action)(() => {
                    oldScene.SendMessage ("ExitScene");
                    _UnloadOldScene (oldScene);
                    _LoadNewScene (sceneName, data);
                    XLAF_Tween.ValueTo (currentScene, XLAF_Tween.Hash (
                        "from", 0,
                        "to", 1,
                        "time", fadeOutTime,
                        "onupdate", (Action<float>)((alpha) => {
                        sceneViewRootCanvas.alpha = alpha;
                    }),
                        "oncomplete", (Action)(() => {
                        if (cb != null)
                            cb ();
                        currentScene.SendMessage ("EnterScene", data);
                        Destroy (currentScene.GetComponent<ignoreUIListener> ());
                        animating = false;
                    })
                    ));
                })
                ));
            } else {
                _LoadNewScene (sceneName, data);// load new scene  or   set exise scene
                //show
                XLAF_Tween.ValueTo (currentScene, XLAF_Tween.Hash (
                    "from", 0,
                    "to", 1,
                    "time", fadeOutTime,
                    "onupdate", (Action<float>)((alpha) => {
                    sceneViewRootCanvas.alpha = alpha;
                }),
                    "oncomplete", (Action)(() => {
                    if (cb != null)
                        cb ();
                    currentScene.SendMessage ("EnterScene", data);
                    Destroy (currentScene.GetComponent<ignoreUIListener> ());
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

            float nomalX = currentScene.transform.position.x;
            float nomalY = currentScene.transform.position.y;
            float newSceneStartX = nomalX - screenWidth;

            GameObject oldScene = currentScene;
            _LoadNewScene (sceneName, data);// load new scene  or   set exise scene

            RectTransform tmpRT = currentScene.GetComponent<RectTransform> ();
            tmpRT.position = new Vector3 (newSceneStartX, nomalY);

            oldScene.SendMessage ("WillExitScene");
            XLAF_Tween.MoveTo (currentScene, XLAF_Tween.Hash (
                "x", nomalX,
                "time", newSceneTime,
                "easetype", ease,
                "oncomplete", (Action)(() => {
                oldScene.SendMessage ("ExitScene");
                _UnloadOldScene (oldScene);
                if (cb != null)
                    cb ();
                currentScene.SendMessage ("EnterScene", data);
                Destroy (currentScene.GetComponent<ignoreUIListener> ());
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

            float nomalX = currentScene.transform.position.x;
            float nomalY = currentScene.transform.position.y;
            float newSceneStartX = nomalX + screenWidth;

            GameObject oldScene = currentScene;
            _LoadNewScene (sceneName, data);// load new scene  or   set exise scene



            RectTransform tmpRT = currentScene.GetComponent<RectTransform> ();
            tmpRT.position = new Vector3 (newSceneStartX, nomalY);

            oldScene.SendMessage ("WillExitScene");
            XLAF_Tween.MoveTo (currentScene, XLAF_Tween.Hash (
                "x", nomalX,
                "time", newSceneTime,
                "easetype", ease,
                "oncomplete", (Action)(() => {
                oldScene.SendMessage ("ExitScene");
                _UnloadOldScene (oldScene);
                if (cb != null)
                    cb ();
                currentScene.SendMessage ("EnterScene", data);
                Destroy (currentScene.GetComponent<ignoreUIListener> ());
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

            float nomalX = currentScene.transform.position.x;
            float nomalY = currentScene.transform.position.y;
            float newSceneStartY = nomalY + screenHeight;

            GameObject oldScene = currentScene;
            _LoadNewScene (sceneName, data);// load new scene  or   set exise scene



            RectTransform tmpRT = currentScene.GetComponent<RectTransform> ();
            tmpRT.position = new Vector3 (nomalX, newSceneStartY);

            oldScene.SendMessage ("WillExitScene");
            XLAF_Tween.MoveTo (currentScene, XLAF_Tween.Hash (
                "y", nomalY,
                "time", newSceneTime,
                "easetype", ease,
                "oncomplete", (Action)(() => {
                oldScene.SendMessage ("ExitScene");
                _UnloadOldScene (oldScene);
                if (cb != null)
                    cb ();
                currentScene.SendMessage ("EnterScene", data);
                Destroy (currentScene.GetComponent<ignoreUIListener> ());
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

            float nomalX = currentScene.transform.position.x;
            float nomalY = currentScene.transform.position.y;
            float newSceneStartY = nomalY - screenHeight;

            GameObject oldScene = currentScene;
            _LoadNewScene (sceneName, data);// load new scene  or   set exise scene



            RectTransform tmpRT = currentScene.GetComponent<RectTransform> ();
            tmpRT.position = new Vector3 (nomalX, newSceneStartY);

            oldScene.SendMessage ("WillExitScene");
            XLAF_Tween.MoveTo (currentScene, XLAF_Tween.Hash (
                "y", nomalY,
                "time", newSceneTime,
                "easetype", ease,
                "oncomplete", (Action)(() => {
                oldScene.SendMessage ("ExitScene");
                _UnloadOldScene (oldScene);
                if (cb != null)
                    cb ();
                currentScene.SendMessage ("EnterScene", data);
                Destroy (currentScene.GetComponent<ignoreUIListener> ());
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
            
            float nomalX = currentScene.transform.position.x;
            float nomalY = currentScene.transform.position.y;
            float newSceneStartX = nomalX + screenWidth;
            float oldSceneEndX = nomalX - screenWidth;

            GameObject oldScene = currentScene;
            _LoadNewScene (sceneName, data);// load new scene  or   set exise scene



            RectTransform tmpRT = currentScene.GetComponent<RectTransform> ();
            tmpRT.position = new Vector3 (newSceneStartX, nomalY);

            oldScene.SendMessage ("WillExitScene");
            XLAF_Tween.MoveTo (oldScene, XLAF_Tween.Hash (
                "x", oldSceneEndX,
                "time", newSceneTime,
                "easetype", ease
            ));

            XLAF_Tween.MoveTo (currentScene, XLAF_Tween.Hash (
                "x", nomalX,
                "time", newSceneTime,
                "easetype", ease,
                "oncomplete", (Action)(() => {
                oldScene.SendMessage ("ExitScene");
                _UnloadOldScene (oldScene);
                if (cb != null)
                    cb ();
                currentScene.SendMessage ("EnterScene", data);
                Destroy (currentScene.GetComponent<ignoreUIListener> ());
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

            float nomalX = currentScene.transform.position.x;
            float nomalY = currentScene.transform.position.y;
            float newSceneStartX = nomalX - screenWidth;
            float oldSceneEndX = nomalX + screenWidth;

            GameObject oldScene = currentScene;
            _LoadNewScene (sceneName, data);// load new scene  or   set exise scene

            RectTransform tmpRT = currentScene.GetComponent<RectTransform> ();
            tmpRT.position = new Vector3 (newSceneStartX, nomalY);

            oldScene.SendMessage ("WillExitScene");
            XLAF_Tween.MoveTo (oldScene, XLAF_Tween.Hash (
                "x", oldSceneEndX,
                "time", newSceneTime,
                "easetype", ease
            ));

            XLAF_Tween.MoveTo (currentScene, XLAF_Tween.Hash (
                "x", nomalX,
                "time", newSceneTime,
                "easetype", ease,
                "oncomplete", (Action)(() => {
                oldScene.SendMessage ("ExitScene");
                _UnloadOldScene (oldScene);
                if (cb != null)
                    cb ();
                currentScene.SendMessage ("EnterScene", data);
                Destroy (currentScene.GetComponent<ignoreUIListener> ());
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

            float nomalX = currentScene.transform.position.x;
            float nomalY = currentScene.transform.position.y;
            float newSceneStartY = nomalY - screenHeight;
            float oldSceneEndY = nomalY + screenHeight;


            GameObject oldScene = currentScene;
            _LoadNewScene (sceneName, data);// load new scene  or   set exise scene

            RectTransform tmpRT = currentScene.GetComponent<RectTransform> ();
            tmpRT.position = new Vector3 (nomalX, newSceneStartY);

            oldScene.SendMessage ("WillExitScene");
            XLAF_Tween.MoveTo (oldScene, XLAF_Tween.Hash (
                "y", oldSceneEndY,
                "time", newSceneTime,
                "easetype", ease
            ));

            XLAF_Tween.MoveTo (currentScene, XLAF_Tween.Hash (
                "y", nomalY,
                "time", newSceneTime,
                "easetype", ease,
                "oncomplete", (Action)(() => {
                oldScene.SendMessage ("ExitScene");
                _UnloadOldScene (oldScene);
                if (cb != null)
                    cb ();
                currentScene.SendMessage ("EnterScene", data);
                Destroy (currentScene.GetComponent<ignoreUIListener> ());
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

            float nomalX = currentScene.transform.position.x;
            float nomalY = currentScene.transform.position.y;
            float newSceneStartY = nomalY + screenHeight;
            float oldSceneEndY = nomalY - screenHeight;


            GameObject oldScene = currentScene;
            _LoadNewScene (sceneName, data);// load new scene  or   set exise scene

            RectTransform tmpRT = currentScene.GetComponent<RectTransform> ();
            tmpRT.position = new Vector3 (nomalX, newSceneStartY);

            oldScene.SendMessage ("WillExitScene");
            XLAF_Tween.MoveTo (oldScene, XLAF_Tween.Hash (
                "y", oldSceneEndY,
                "time", newSceneTime,
                "easetype", ease
            ));

            XLAF_Tween.MoveTo (currentScene, XLAF_Tween.Hash (
                "y", nomalY,
                "time", newSceneTime,
                "easetype", ease,
                "oncomplete", (Action)(() => {
                oldScene.SendMessage ("ExitScene");
                _UnloadOldScene (oldScene);
                if (cb != null)
                    cb ();
                currentScene.SendMessage ("EnterScene", data);
                Destroy (currentScene.GetComponent<ignoreUIListener> ());
                animating = false;
            })
            ));
        }

        private void _AnimationZoomIn (string sceneName, object data, float newSceneTime, XLAF_Tween.EaseType easeType, Action cb)
        {
            XLAF_Tween.EaseType ease = (easeType == XLAF_Tween.EaseType.defaultType) ? XLAF_Tween.EaseType.easeOutBack : easeType;

            GameObject oldScene = currentScene;
            _LoadNewScene (sceneName, data);// load new scene  or   set exise scene
            currentScene.transform.localScale = new Vector3 (1f, 1f);
            oldScene.SendMessage ("WillExitScene");
            XLAF_Tween.ScaleFrom (currentScene, XLAF_Tween.Hash (
                "scale", new Vector3 (0f, 0f),
                "time", newSceneTime,
                "easetype", ease,
                "oncomplete", (Action)(() => {
                oldScene.SendMessage ("ExitScene");
                _UnloadOldScene (oldScene);
                if (cb != null)
                    cb ();
                currentScene.SendMessage ("EnterScene", data);
                Destroy (currentScene.GetComponent<ignoreUIListener> ());
                animating = false;
            })
            ));

        }

        private void _AnimationZoomOut (string sceneName, object data, float newSceneTime, XLAF_Tween.EaseType easeType, Action cb)
        {
            XLAF_Tween.EaseType ease = (easeType == XLAF_Tween.EaseType.defaultType) ? XLAF_Tween.EaseType.easeInBack : easeType;

            GameObject oldScene = currentScene;
            _LoadNewScene (sceneName, data);// load new scene  or   set exise scene
            currentScene.transform.localScale = new Vector3 (1f, 1f);
            oldScene.SendMessage ("WillExitScene");
            oldScene.transform.SetAsLastSibling ();//旧的scene在最上
            XLAF_Tween.ScaleTo (oldScene, XLAF_Tween.Hash (
                "scale", new Vector3 (0f, 0f),
                "time", newSceneTime,
                "easetype", ease,
                "oncomplete", (Action)(() => {
                oldScene.SendMessage ("ExitScene");
                _UnloadOldScene (oldScene);
                if (!destoryOnSceneChange) {
                    oldScene.transform.localScale = new Vector3 (1f, 1f);
                }
                if (cb != null)
                    cb ();
                currentScene.SendMessage ("EnterScene", data);
                Destroy (currentScene.GetComponent<ignoreUIListener> ());
                animating = false;
            })
            ));
        }

        ////////////////////////////////////////////////////  others  ////////////////////////////////////////////////////////////////////////////


    }

    /// <summary>
    /// AddComponent<ignoreUIListener> ()之后就不响应界面的事件了
    /// Destroy (currentScene.GetComponent<ignoreUIListener> ());之后就继续响应界面的事件了
    /// 
    /// 界面切换期间不响应事件，所以add上，界面切换完毕响应事件，所以destory它
    /// </summary>
    public class ignoreUIListener : MonoBehaviour ,ICanvasRaycastFilter
    {
        public bool IsFocus = false;

        public bool IsRaycastLocationValid (Vector2 sp, Camera eventCamera)
        {
            return IsFocus;
        }
    }


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

