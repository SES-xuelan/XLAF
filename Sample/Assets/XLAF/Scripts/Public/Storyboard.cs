using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

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

		#endregion

		#region public properties
		/// <summary>
		/// Gets the name of the scene.
		/// </summary>
		/// <value>The name of the scene.</value>
		public string sceneName { get { return this._sceneName; } }

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
				Log.Warning ("You can't change sceneName");
			}
		}

		#region  Storyboard Listeners

		/*
	    Each storyboard function called moment:
        CreatScene     => after finish load scene before play enter animation(only call after load from prefab or asset bundle, read cache from MgrScene will not called).
        WillEnterScene => after CreatScene, at the begin of play enter animation.
        EnterScene     => at the end of play enter animation.

        WillExitScene  => at the begin of play exit animation.
        ExitScene      => at the end of play exit animation.
        DestroyScene   => when destroy the scene(before destroy).

        OverlayBegan   => when scene overlaid(only XLAF popup).
        OverlayEnded   => when scene overlaid object disappear(only XLAF popup).
        AndroidGoBack  => in Android, user press back button.
        UpdateLanguage => when update language or after CreatScene.
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
		public virtual void OnUIEvent (UIEvent e)
		{
            
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
