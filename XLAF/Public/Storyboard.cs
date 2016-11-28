using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace XLAF.Public
{
    //Storyboard是脚本部分

    //SceneObject是界面部分

    public class Storyboard : MonoBehaviour
    {
        private string _sceneName;

        #region Properties

        public string sceneName { get { return this._sceneName; } }

        #endregion

        /// <summary>
        /// Sets the name of the scene.
        /// !WAINNING!
        /// This function is use for SceneObject ONLY.
        /// You should NOT call this function.
        /// </summary>
        /// <param name="name">Name.</param>
        internal void SetSceneName (string name)
        {
            if (string.IsNullOrEmpty (this._sceneName)) {
                this._sceneName = name;
            } else {
                Log.Warning ("You shouldn't change sceneName");
            }
        }


        #region  Storyboard Listeners

        /*
		CreatScene 加载完界面，还未播放动画（只有界面加载的时候，才会触发；读取缓存界面不会触发）
		WillEnterScene 加载完毕scene，即将播放过渡动画
		EnterScene 播放完毕过渡动画

		WillExitScene 即将播放退出界面的动画
		ExitScene 播放完退出界面的动画
		DestoryScene 销毁界面前触发
        
        */
        public virtual void CreatScene (object obj)
        {
        }

        public virtual void WillEnterScene (object obj)
        {
        }

        public virtual void EnterScene (object obj)
        {
        }

        public virtual void WillExitScene ()
        {
        }

        public virtual void ExitScene ()
        {
        }

        public virtual void DestoryScene ()
        {
        }

        public virtual void OverlayBegan (object obj)
        {
        }

        public virtual void OverlayEnded (object obj)
        {
        }

        #if UNITY_ANDROID
        public virtual void AndroidGoBack ()
        {
        }
        #endif

        #endregion

    }

    public class SceneObject
    {
        public GameObject scene;
        public Storyboard script;

        public string sceneName { 
            get {
                return this._sceneName;
            }
        }

        public SceneObject (string sceneName)
        {

            UnityEngine.Object _prefab = Resources.Load (sceneName);
            GameObject scene = (GameObject)UnityEngine.Object.Instantiate (_prefab);
            this.scene = scene;
            this.script = scene.GetComponent<Storyboard> ();
            this._sceneName = sceneName;
            this.script.SetSceneName (sceneName);

        }


        public void EnableUIListener ()
        {
            if (this.scene.GetComponent<ignoreUIListener> () != null) {
                GameObject.Destroy (this.scene.GetComponent<ignoreUIListener> ());
            }
        }

        public void DisableUIListener ()
        {
            if (this.scene.GetComponent<ignoreUIListener> () == null) {
                this.scene.AddComponent<ignoreUIListener> ();
            }
        }






        /// <summary>
        /// AddComponent<ignoreUIListener> ()之后就不响应界面的事件了
        /// currentScene.EnableUIListener ();之后就继续响应界面的事件了
        /// 
        /// 界面切换期间不响应事件，所以add上，界面切换完毕响应事件，所以destory它
        /// </summary>
        private class ignoreUIListener : MonoBehaviour ,ICanvasRaycastFilter
        {
            public bool IsFocus = false;

            public bool IsRaycastLocationValid (Vector2 sp, Camera eventCamera)
            {
                return IsFocus;
            }
        }

        private string _sceneName = "";
    }
}
