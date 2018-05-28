using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace XLAF.Public
{
	/// <summary>
	/// Event trigger listener.<para></para>
	/// You can add other EventTrigger.
	/// </summary>
	public class XLAFEventTriggerListener : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler,IBeginDragHandler,IDragHandler,IEndDragHandler
	{
		public delegate void VoidDelegate (GameObject go, PointerEventData eventData);

		public VoidDelegate onClick;
		public VoidDelegate onDown;
		public VoidDelegate onEnter;
		public VoidDelegate onExit;
		public VoidDelegate onUp;
		public VoidDelegate onBeginDrag;
		public VoidDelegate onDrag;
		public VoidDelegate onEndDrag;

		public static  XLAFEventTriggerListener Get (GameObject go)
		{
			XLAFEventTriggerListener listener = go.GetComponent<XLAFEventTriggerListener> ();
			if (listener == null)
				listener = go.AddComponent<XLAFEventTriggerListener> ();
			return listener;
		}

		public void OnPointerClick (PointerEventData eventData)
		{
			if (onClick != null)
				onClick (gameObject, eventData);
		}

		public void OnPointerDown (PointerEventData eventData)
		{
			if (onDown != null)
				onDown (gameObject, eventData);
		}

		public void OnPointerEnter (PointerEventData eventData)
		{
			if (onEnter != null)
				onEnter (gameObject, eventData);
		}

		public void OnPointerExit (PointerEventData eventData)
		{
			if (onExit != null)
				onExit (gameObject, eventData);
		}

		public void OnPointerUp (PointerEventData eventData)
		{
			if (onUp != null)
				onUp (gameObject, eventData);
		}

		public void OnBeginDrag (PointerEventData eventData)
		{
			if (onBeginDrag != null)
				onBeginDrag (gameObject, eventData);
		}

		public void OnDrag (PointerEventData eventData)
		{
			if (onDrag != null)
				onDrag (gameObject, eventData);
		}

		public void OnEndDrag (PointerEventData eventData)
		{
			if (onEndDrag != null)
				onEndDrag (gameObject, eventData);
		}
	}
}