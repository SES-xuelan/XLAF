using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using XLAF.Public;
using UnityEngine.UI;
using System.Collections.Generic;
using System;


/*
ModDispatcher usage：

        ModDispatcher.AddListener ("test", test);
        ModDispatcher.Dispatch (new XLAF_Event ("test", "dattttta"));
        ModDispatcher.Dispatch ("test", "hfeisahu");

        void test (XLAF_Event e)
        {
               XLAFInnerLog.Debug ("test", e, ModDispatcher.HasListener ("test", test));
        }
*/
using XLAF.Private;


namespace XLAF.Public
{
	/// <summary>
	/// Dispatcher mod.
	/// </summary>
	public class ModDispatcher
	{
		#region constructed function & initialization

		static ModDispatcher ()
		{
		}

		/// <summary>
		/// call Init() will trigger constructed function, you can call Init() to ensure this class finished initialization
		/// </summary>
		public static void Init ()
		{
		}

		#endregion

		#region private variables

		private static Dictionary<object, List<XLAF_Event>> listeners = new Dictionary<object, List<XLAF_Event>> ();

		#endregion

		#region public functions

		/// <summary>
		/// Dispatch the data with event name.
		/// </summary>
		/// <param name="eventName">Event name.</param>
		/// <param name="data">Data.</param>
		public static void Dispatch (object eventName, object data = null)
		{
			List<XLAF_Event> list;
			if (!listeners.TryGetValue (eventName, out list)) {
				XLAFInnerLog.Warning ("No callback functions names ", eventName);
				return;
			}
			for (int i = 0; i < list.Count; i++) {
				if (list [i].action != null) {
					list [i].data = data;
					list [i].action (list [i]);
				}
			}
		}

		/// <summary>
		/// Dispatch the XLAF_Event.
		/// </summary>
		/// <param name="e">XLAF_Event.</param>
		public static void Dispatch (XLAF_Event e)
		{
			if (e.name == null) {
				XLAFInnerLog.Error ("Event is not right", e);
				return;
			}
			List<XLAF_Event> list;
			if (!listeners.TryGetValue (e.name, out list)) {
				XLAFInnerLog.Warning ("No callback functions names ", e.name);
				return;
			}
			for (int i = 0; i < list.Count; i++) {
				if (list [i].action != null) {
					list [i].data = e.data;
					list [i].action (list [i]);
				}
			}
		}

		/// <summary>
		/// Adds the listener.
		/// </summary>
		/// <param name="eventName">Event name.</param>
		/// <param name="handler">Handler.</param>
		public static void AddListener (object eventName, Action<XLAF_Event> handler)
		{
			if (!listeners.ContainsKey (eventName)) {
				listeners.Add (eventName, new List<XLAF_Event> ());
			}
			List<XLAF_Event> list = listeners [eventName];
			XLAF_Event e = new XLAF_Event (eventName);
			e.action = handler;
			list.Add (e);
		}

		/// <summary>
		/// Removes the listener.
		/// </summary>
		/// <param name="eventName">Event name.</param>
		/// <param name="handler">Handler.</param>
		public static void RemoveListener (object eventName, Action<XLAF_Event> handler)
		{
			List<XLAF_Event> list;
			if (!listeners.TryGetValue (eventName, out list)) {
				XLAFInnerLog.Warning ("No callback functions names ", eventName);
				return;
			}
			for (int i = 0; i < list.Count; i++) {
				if (list [i].action == handler)
					list.Remove (list [i]);
			}
		}

		/// <summary>
		/// Determines if has listener the specified eventName handler.
		/// </summary>
		/// <returns><c>true</c> if has listener the specified eventName handler; otherwise, <c>false</c>.</returns>
		/// <param name="eventName">Event name.</param>
		/// <param name="handler">Handler.</param>
		public static bool HasListener (object eventName, Action<XLAF_Event> handler)
		{
			List<XLAF_Event> list;
			if (!listeners.TryGetValue (eventName, out list)) {
				return false;
			}
			for (int i = 0; i < list.Count; i++) {
				if (list [i].action == handler)
					return true;
			}
			return false;
		}

		#endregion
	}

}