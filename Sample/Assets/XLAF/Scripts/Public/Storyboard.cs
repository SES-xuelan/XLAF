using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace XLAF.Public
{
    //Storyboard是脚本部分

    //SceneObject是界面部分

    /// <summary>
    /// Storyboard. 脚本部分
    /// </summary>
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
                Log.Warning ("You can't change sceneName");
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

        public virtual void OverlayBegan (string overlaySceneceneName)
        {
        }

        public virtual void OverlayEnded (string overlaySceneceneName)
        {
        }

        public virtual void UpdateLanguage ()
        {
        }

        #if UNITY_ANDROID
        public virtual void AndroidGoBack ()
        {
        }
        #endif

        #endregion



        public virtual void OnUIEvent (UIEvent e)
        {
            
        }


    }

    /// <summary>
    /// Scene object. 界面部分
    /// </summary>
    public class SceneObject
    {
        

        public SceneObject (string fullSceneNamePath)
        {
            string[] tmp = fullSceneNamePath.Split ('/');
            this._sceneName = tmp [tmp.Length - 1];////非完整路径

            UnityEngine.Object _prefab = Resources.Load (fullSceneNamePath);
			Log.Debug (fullSceneNamePath);
            GameObject scene = (GameObject)UnityEngine.Object.Instantiate (_prefab);
            scene.name = _sceneName;
            this.scene = scene;
            this.cg = scene.transform.GetComponent<CanvasGroup> ();
            this.script = scene.GetComponent<Storyboard> ();
            this.script.SetSceneName (this._sceneName);
            this._BindingEvents ();

            startX = this.scene.transform.position.x;
            startY = this.scene.transform.position.y;
            startAlpha = 1f;
        }

        public GameObject scene;
        public Storyboard script;

        private CanvasGroup cg;
        private float startX, startY, startAlpha;

        /// <summary>
        /// Gets the name of the scene.(Read only)
        /// </summary>
        /// <value>The name of the scene.</value>
        public string sceneName { 
            get {
                return this._sceneName;
            }
        }


        /// <summary>
        /// Enables the UI listener.
        /// </summary>
        public void EnableUIListener ()
        {
            if (this.scene.GetComponent<ignoreUIListener> () != null) {
                GameObject.Destroy (this.scene.GetComponent<ignoreUIListener> ());
            }
        }

        /// <summary>
        /// Disables the UI listener.
        /// </summary>
        public void DisableUIListener ()
        {
            if (this.scene.GetComponent<ignoreUIListener> () == null) {
                this.scene.AddComponent<ignoreUIListener> ();
            }
        }

        public void ChangeAlpha (float alphaValue)
        {
            this.cg.alpha = alphaValue;
        }

        /// <summary>
        /// 恢复到初始状态
        /// </summary>
        public void RestoreStatus ()
        {
            RectTransform tmpRT = this.scene.GetComponent<RectTransform> ();
            tmpRT.position = new Vector3 (startX, startY);
            ChangeAlpha (startAlpha);
        }

        public void AddDialogBackground (float bgAlphaValue)
        {
            Image image = this.scene.AddComponent<Image> ();
            image.color = new Color (0, 0, 0, bgAlphaValue);
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

        private void _BindingEvents ()
        {
            ModUtils.BindingUIEvents (this.scene, this.script.OnUIEvent);
        }
    }

    public class UIEvent
    {
        public GameObject target;
        public string targetType;
        public TouchPhase phase;

        public override string ToString ()
        {
            string str = "\n";
            str = str + "tatget:" + target.name + "\n";
            str = str + "targetType:" + targetType + "\n";
            str = str + "phase:" + phase.ToString () + "\n";
            return str;
        }
    }
}
