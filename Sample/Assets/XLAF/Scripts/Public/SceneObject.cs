using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace XLAF.Public {

	/// <summary>
	/// Scene object, UI part. 
	/// </summary>
	public class SceneObject
	{
		//		/// <summary>
		//		/// Loads the bundle.
		//		/// </summary>
		//		/// <returns>The bundle all.</returns>
		//		/// <param name="path">Path.</param>
		//		/// <param name="sceneName">Scene name.</param>
		//		private IEnumerator LoadBundle (string path, string sceneName)
		//		{
		//			WWW bundle = new WWW (path);
		//			yield return bundle;
		//			GameObject scene = (GameObject)UnityEngine.Object.Instantiate (bundle.assetBundle.LoadAsset (sceneName));
		//			initAttr (scene, sceneName);
		//			yield return 1;
		//		}

		/// <summary>
		/// Initializes a new instance of the <see cref="XLAF.Public.SceneObject"/> class with asset bundle.
		/// </summary>
		/// <param name="assetBundleFullPathName">AssetBundle full path name. <para></para>
		/// e.g. /StreamingAssets/Android/all.assetbundle</param>
		/// <param name="sceneName">Scene name.  e.g. Pop1</param>
		public SceneObject (string assetBundleFullPathName, string sceneName)
		{
			//use async load will cause setParent not right
			//MgrCoroutine.DoCoroutine (LoadBundle (assetBundleFullPathName, sceneName));

			WWW bundle = new WWW (assetBundleFullPathName);
			GameObject scene = (GameObject)UnityEngine.Object.Instantiate (bundle.assetBundle.LoadAsset (sceneName));
			bundle.assetBundle.Unload (false);
			this._sceneName = sceneName;
			initAttr (scene, sceneName);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="XLAF.Public.SceneObject"/> class without asset bundle.
		/// </summary>
		/// <param name="fullSceneNamePath">Full scene name path.</param>
		public SceneObject (string fullSceneNamePath)
		{
			string[] tmp = fullSceneNamePath.Split ('/');
			this._sceneName = tmp [tmp.Length - 1];
			UnityEngine.Object _prefab = Resources.Load (fullSceneNamePath);
			Log.Debug (fullSceneNamePath);
			GameObject scene = (GameObject)UnityEngine.Object.Instantiate (_prefab);
			initAttr (scene, _sceneName);
		}

		/// <summary>
		/// Inits the attr.
		/// </summary>
		/// <param name="scene">Scene.</param>
		/// <param name="_sceneName">Scene name.</param>
		private void initAttr (GameObject scene, string _sceneName)
		{
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
		/// Restores the status.
		/// </summary>
		public void RestoreStatus ()
		{
			RectTransform tmpRT = this.scene.GetComponent<RectTransform> ();
			tmpRT.position = new Vector3 (startX, startY);
			ChangeAlpha (startAlpha);
		}

		/// <summary>
		/// Adds the popup background.
		/// </summary>
		/// <param name="bgAlphaValue">Background alpha value.</param>
		public void AddPopupBackground (float bgAlphaValue)
		{
			Image image = this.scene.AddComponent<Image> ();
			image.color = new Color (0, 0, 0, bgAlphaValue);
		}



		/// <summary>
		/// After AddComponent<ignoreUIListener> () the object will NOT trigger touch or click event;
		/// 
		/// After called currentScene.EnableUIListener (); the object will trigger touch or click event;
		///
		/// When scene or popup changing, you MUST add it to object, after finished changing, destory it.
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
}
