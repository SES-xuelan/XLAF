using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using XLAF.Public;
using UnityEngine.UI;
using System.Collections.Generic;
using System;


/*
ModDispatcher使用方法：

        ModDispatcher.AddListener ("test", test);
        ModDispatcher.Dispatch (new XLAF_Event ("test", "dattttta"));
        ModDispatcher.Dispatch ("test", "hfeisahu");

        void test (XLAF_Event e)
        {
               Log.Debug ("test", e, ModDispatcher.HasListener ("test", test));
        }
*/

namespace XLAF.Public
{
    /// <summary>
    /// Mod dispatcher.
    /// </summary>
    public class ModDispatcher
    {
        static ModDispatcher ()
        {
        }

        /// <summary>
        /// 调用Init会触发构造函数，可以用于统一初始化的时候
        /// </summary>
        public static void Init ()
        {
        }

        private static Dictionary<object, List<XLAF_Event>> listeners = new Dictionary<object, List<XLAF_Event>> ();


        /// <summary>
        /// Dispatch the data with event name.
        /// </summary>
        /// <param name="eventName">Event name.</param>
        /// <param name="data">Data.</param>
        public static void Dispatch (object eventName, object data = null)
        {
            List<XLAF_Event> list;
            if (!listeners.TryGetValue (eventName, out list)) {
                Log.Warning ("No callback functions names ", eventName);
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
                Log.Error ("Event is not right", e);
                return;
            }
            List<XLAF_Event> list;
            if (!listeners.TryGetValue (e.name, out list)) {
                Log.Warning ("No callback functions names ", e.name);
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
                Log.Warning ("No callback functions names ", eventName);
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
    }

    /// <summary>
    /// Event for ModDispatcher.
    /// </summary>
    public class XLAF_Event
    {
        /// <summary>
        /// event name
        /// </summary>
        /// <value>The name.</value>
        public object name{ get; set; }

        /// <summary>
        /// data to dispatch.
        /// </summary>
        /// <value>The data.</value>
        public object data{ get; set; }

        /// <summary>
        /// the action to callback.
        /// </summary>
        /// <value>The action.</value>
        public Action<XLAF_Event> action{ get; set; }

        public XLAF_Event (object eventName, object data = null)
        {
            this.name = eventName;
            this.data = data;
        }

        public override string ToString ()
        {
            return string.Format ("[XLAF_Event: name={0}, data={1}]", name, data);
        }


    }

}