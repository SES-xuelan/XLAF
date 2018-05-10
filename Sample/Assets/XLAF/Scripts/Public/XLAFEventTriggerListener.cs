using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace XLAF.Public
{
	/// <summary>
	/// Event trigger listener.<para></para>
	/// You can add other EventTrigger.
	/// </summary>
	public class XLAFEventTriggerListener : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
	{
		public delegate void VoidDelegate (GameObject go);

		public VoidDelegate onClick;
		public VoidDelegate onDown;
		public VoidDelegate onEnter;
		public VoidDelegate onExit;
		public VoidDelegate onUp;

		static public XLAFEventTriggerListener Get (GameObject go)
		{
			XLAFEventTriggerListener listener = go.GetComponent<XLAFEventTriggerListener> ();
			if (listener == null)
				listener = go.AddComponent<XLAFEventTriggerListener> ();
			return listener;
		}

		public void OnPointerClick (PointerEventData eventData)
		{
			if (onClick != null)
				onClick (gameObject);
		}

		public void OnPointerDown (PointerEventData eventData)
		{
			if (onDown != null)
				onDown (gameObject);
		}

		public void OnPointerEnter (PointerEventData eventData)
		{
			if (onEnter != null)
				onEnter (gameObject);
		}

		public void OnPointerExit (PointerEventData eventData)
		{
			if (onExit != null)
				onExit (gameObject);
		}

		public void OnPointerUp (PointerEventData eventData)
		{
			if (onUp != null)
				onUp (gameObject);
		}
	}
}