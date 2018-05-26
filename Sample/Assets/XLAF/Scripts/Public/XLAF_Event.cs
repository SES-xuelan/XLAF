using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace XLAF.Public
{
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
