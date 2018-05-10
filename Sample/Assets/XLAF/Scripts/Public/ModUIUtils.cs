using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace XLAF.Public
{
	/// <summary>
	/// UI Tools
	/// </summary>
	public class ModUIUtils
	{

		#region constructed function & initialization

		static ModUIUtils ()
		{
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
		/// Gets the child form a transform.
		/// </summary>
		/// <returns>The child.</returns>
		/// <param name="parent">Transform parent.</param>
		/// <param name="childName">Child name.</param>
		/// <typeparam name="T">The type of the child.</typeparam>
		public static T GetChild<T> (Transform parent, string childName)
		{
			Transform t = parent.Find (childName);
			if (t != null) {
				return t.GetComponent<T> ();
			} else {
				Log.Error ("error! find child null");
				return default(T);
			}

		}

		/// <summary>
		/// Gets the child form a transform.
		/// </summary>
		/// <returns>The child.</returns>
		/// <param name="parent">Transform parent.</param>
		/// <param name="childName">Child name.</param>
		public static Transform GetChild (Transform parent, string childName)
		{
			Transform t = parent.Find (childName);
			if (t != null) {
				return t;
			} else {
				Log.Error ("error! find child null");
				return null;
			}

		}

		/// <summary>
		/// Changes the position.
		/// </summary>
		/// <param name="rect">Rect.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public static void ChangePos (RectTransform rect, float? x = null, float? y = null)
		{
			float _x = rect.anchoredPosition.x;
			float _y = rect.anchoredPosition.y;
			if (x == null)
				rect.anchoredPosition = new Vector2 (_x, (float)y);
			else if (y == null)
				rect.anchoredPosition = new Vector2 ((float)x, _y);
			else
				rect.anchoredPosition = new Vector2 ((float)x, (float)y);

		}
		/// <summary>
		/// Changes the position.
		/// </summary>
		/// <param name="t">button.</param>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public static void ChangePos (Button t, float? x = null, float? y = null)
		{
			RectTransform rect = t.image.rectTransform;
			if (rect == null)
				Log.Error ("RectTransform is null");
			ChangePos (rect, x, y);
		}

		/// <summary>
		/// Changes the size.
		/// </summary>
		/// <param name="rect">Rect.</param>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		public static void ChangeSize (RectTransform rect, float? width = null, float? height = null)
		{

			float _width = rect.sizeDelta.x;
			float _height = rect.sizeDelta.y;
			if (width == null)
				rect.sizeDelta = new Vector2 (_width, (float)height);
			else if (height == null)
				rect.sizeDelta = new Vector2 ((float)width, _height);
			else
				rect.sizeDelta = new Vector2 ((float)width, (float)height);
		}
		/// <summary>
		/// Changes the size.
		/// </summary>
		/// <param name="t">button.</param>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		public static void ChangeSize (Button t, float? width = null, float? height = null)
		{
			RectTransform rect = t.GetComponent<RectTransform> ();
			if (rect == null)
				Log.Error ("RectTransform is null");
			ChangeSize (rect, width, height);
		}
		/// <summary>
		/// Changes the size.
		/// </summary>
		/// <param name="t">gameobject.</param>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		public static void ChangeSize (GameObject t, float? width = null, float? height = null)
		{
			RectTransform rect = t.GetComponent<RectTransform> ();
			if (rect == null)
				Log.Error ("RectTransform is null");
			ChangeSize (rect, width, height);
		}

		#endregion
	}

}
