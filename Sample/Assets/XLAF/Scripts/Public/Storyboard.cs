using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using XLAF.Private;

namespace XLAF.Public
{
	//Storyboard is script part
	//SceneObject is UI part

	/// <summary>
	/// Storyboard, script part. 
	/// </summary>
	public class Storyboard : MonoBehaviour
	{
		#region private variables

		private string _sceneName;
		private SceneObject _sceneObject = null;
		private float _preventTime = 1f;

		#endregion

		#region public properties

		/// <summary>
		/// Gets the name of the scene.
		/// </summary>
		/// <value>The name of the scene.</value>
		public string sceneName { get { return this._sceneName; } }

		/// <summary>
		/// Gets the scene object.
		/// </summary>
		/// <value>The scene object.</value>
		public SceneObject sceneObject{ get { return _sceneObject; } }

		/// <summary>
		/// Gets or sets the prevent double click time (seconds).
		/// </summary>
		/// <value>The prevent double click time.</value>
		public float preventDoubleClickTime{ get { return _preventTime; } set { _preventTime = value; } }

		#endregion

		/// <summary>
		/// Sets the name of the scene.<para></para>
		/// !WAINNING!<para></para>
		/// This function is use for SceneObject ONLY.
		/// You should NOT call this function.
		/// </summary>
		/// <param name="name">Name.</param>
		internal void SetSceneName (string name)
		{
			if (string.IsNullOrEmpty (this._sceneName)) {
				this._sceneName = name;
			} else {
				XLAFInnerLog.Warning ("You can't change sceneName");
			}
		}

		/// <summary>
		/// Sets the scene object.<para></para>
		/// !WAINNING!<para></para>
		/// This function is use for SceneObject ONLY.
		/// You should NOT call this function.
		/// </summary>
		/// <param name="obj">Object.</param>
		internal void SetSceneObject (SceneObject obj)
		{
			if (_sceneObject == null) {
				this._sceneObject = obj;
			} else {
				XLAFInnerLog.Warning ("You can't change sceneObject");
			}
		}

		#region  Storyboard Listeners

		/*
	    Each storyboard function called moment:
        CreateScene     => after finish load scene before play enter animation(only call after load from prefab or asset bundle, read cache from MgrScene will not called).
        WillEnterScene => after CreateScene, at the begin of play enter animation.
        EnterScene     => at the end of play enter animation.

        WillExitScene  => at the begin of play exit animation.
        ExitScene      => at the end of play exit animation.
        DestroyScene   => when destroy the scene(before destroy).

        OverlayBegan   => when scene overlaid(only XLAF popup).
        OverlayEnded   => when scene overlaid object disappear(only XLAF popup).
        AndroidGoBack  => in Android, user press back button.
        UpdateLanguage => when update language or after CreateScene.
        */
		public virtual void CreateScene (object obj)
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

		public virtual void DestroyScene ()
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

		/// <summary>
		/// user interface event.
		/// </summary>
		/// <param name="e">Event.</param>
		public virtual void OnUIEvent (XLAF_UIEvent e)
		{
            
		}

		/// <summary>
		/// Dispatch the XLAF_Event.
		/// </summary>
		/// <param name="e">Event.</param>
		public virtual void OnXLAFEvent (XLAF_Event e)
		{

		}

		/// <summary>
		/// Auto bind user interface event.
		/// </summary>
		public void BindAllButtonsClickEvent ()
		{
			if (preventDoubleClickTime <= 0) {
				ModUIUtils.BindingButtonClick (gameObject, this.OnUIEvent);
			} else {
				ModUIUtils.BindingButtonClick (gameObject, preventDoubleClickTime, this.OnUIEvent);
			}
		}

	}

}
