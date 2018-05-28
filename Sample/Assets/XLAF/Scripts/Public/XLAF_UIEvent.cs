using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace XLAF.Public
{
	/// <summary>
	/// XLAF user interface event.
	/// </summary>
	public class XLAF_UIEvent
	{
		/// <summary>
		/// Gets or sets the target.
		/// </summary>
		/// <value>The target.</value>
		public GameObject target{ get; set; }

		/// <summary>
		/// Gets or sets the phase.
		/// </summary>
		/// <value>The phase.</value>
		public Phase phase{ get; set; }

		/// <summary>
		/// Gets or sets the event data.
		/// </summary>
		/// <value>The event data.</value>
		public PointerEventData eventData{ get; set; }

		public override string ToString ()
		{
			return string.Format ("[XLAF_UIEvent: target={0}, phase={1}, eventData={2}]", target, phase, eventData);
		}
	}

	public enum Phase
	{
		/// <summary>
		/// The click.
		/// </summary>
		Click,
		/// <summary>
		/// Mouse down.
		/// </summary>
		Down,
		/// <summary>
		/// Mouse up.
		/// </summary>
		Up,
		/// <summary>
		/// Pointer enter.
		/// </summary>
		Enter,
		/// <summary>
		/// Pointer exit.
		/// </summary>
		Exit,
		/// <summary>
		/// Begin drag.
		/// </summary>
		BeginDrag,
		/// <summary>
		/// Dragging.
		/// </summary>
		Dragging,
		/// <summary>
		/// End drag.
		/// </summary>
		EndDrag
	}
}