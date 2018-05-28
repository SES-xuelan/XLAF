using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using XLAF.Private;

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
				XLAFInnerLog.Error ("error! find child null");
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
				XLAFInnerLog.Error ("error! find child null");
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
				XLAFInnerLog.Error ("RectTransform is null");
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
				XLAFInnerLog.Error ("RectTransform is null");
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
				XLAFInnerLog.Error ("RectTransform is null");
			ChangeSize (rect, width, height);
		}

		/// <summary>
		/// Bindings the all button click event in <c>parent</c>.
		/// </summary>
		/// <param name="parent">Parent.</param>
		/// <param name="onUIEvent">Event.</param>
		public static void BindingButtonClick (GameObject parent, Action<XLAF_UIEvent> onUIEvent)
		{
			Button[] buttons = parent.GetComponentsInChildren<Button> (true);
			for (int i = 0; i < buttons.Length; i++) {
				AddClick (buttons [i].gameObject, onUIEvent);
			}
		}

		/// <summary>
		/// Adds the click.
		/// </summary>
		/// <param name="go">GameObject.</param>
		/// <param name="callback">Callback.</param>
		public static void AddClick (GameObject go, Action<XLAF_UIEvent> onUIEvent)
		{
			XLAFEventTriggerListener.Get (go).onClick = (GameObject g, PointerEventData e) => {
				XLAF_UIEvent evt = new XLAF_UIEvent ();
				evt.eventData = e;
				evt.target = g;
				evt.phase = Phase.Click;
				onUIEvent (evt);
			};
		}

		/// <summary>
		/// Removes the click.
		/// </summary>
		/// <param name="go">GameObject.</param>
		public static void RemoveClick (GameObject go)
		{
			XLAFEventTriggerListener.Get (go).onClick = null;
		}

		/// <summary>
		/// Adds the touch event.
		/// </summary>
		/// <param name="go">GameObject.</param>
		/// <param name="onUIEvent">On user interface event.</param>
		public static void AddTouchEvent (GameObject go, Action<XLAF_UIEvent> onUIEvent)
		{
			XLAFEventTriggerListener.Get (go).onBeginDrag = (GameObject g, PointerEventData e) => {
				XLAF_UIEvent evt = new XLAF_UIEvent ();
				evt.eventData = e;
				evt.target = g;
				evt.phase = Phase.BeginDrag;
				onUIEvent (evt);
			};
			XLAFEventTriggerListener.Get (go).onDrag = (GameObject g, PointerEventData e) => {
				XLAF_UIEvent evt = new XLAF_UIEvent ();
				evt.eventData = e;
				evt.target = g;
				evt.phase = Phase.Dragging;
				onUIEvent (evt);
			};
			XLAFEventTriggerListener.Get (go).onEndDrag = (GameObject g, PointerEventData e) => {
				XLAF_UIEvent evt = new XLAF_UIEvent ();
				evt.eventData = e;
				evt.target = g;
				evt.phase = Phase.EndDrag;
				onUIEvent (evt);
			};
			XLAFEventTriggerListener.Get (go).onDown = (GameObject g, PointerEventData e) => {
				XLAF_UIEvent evt = new XLAF_UIEvent ();
				evt.eventData = e;
				evt.target = g;
				evt.phase = Phase.Down;
				onUIEvent (evt);
			};
			XLAFEventTriggerListener.Get (go).onUp = (GameObject g, PointerEventData e) => {
				XLAF_UIEvent evt = new XLAF_UIEvent ();
				evt.eventData = e;
				evt.target = g;
				evt.phase = Phase.Up;
				onUIEvent (evt);
			};
		}

		/// <summary>
		/// Removes the touch event.
		/// </summary>
		/// <param name="go">GameObject.</param>
		public static void RemoveTouchEvent (GameObject go)
		{
			XLAFEventTriggerListener.Get (go).onBeginDrag = null;
			XLAFEventTriggerListener.Get (go).onDrag = null;
			XLAFEventTriggerListener.Get (go).onEndDrag = null;
			XLAFEventTriggerListener.Get (go).onDown = null;
			XLAFEventTriggerListener.Get (go).onUp = null;
		}

		#endregion
	}

}
