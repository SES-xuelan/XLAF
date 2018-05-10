using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XLAF.Public
{
	internal class CoroutineManagerMonoBehaviour : MonoBehaviour
	{
	}

	/// <summary>
	///  coroutine manager. <para></para>
	///  use this, you should not create gameObject manual (this class will create automatic :)).
	/// </summary>
	public class MgrCoroutine
	{
		private static CoroutineManagerMonoBehaviour _CoroutineManagerMonoBehaviour;

		#region constructed function & initialization

		static MgrCoroutine ()
		{
			var go = new GameObject ();
			go.name = "MgrCoroutine";
			_CoroutineManagerMonoBehaviour = go.AddComponent<CoroutineManagerMonoBehaviour> ();
			GameObject.DontDestroyOnLoad (go);
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
		/// Dos the coroutine.<para></para>
		/// Usage:<para></para>
		/// <code>MgrCoroutine.DoCoroutine (IEnumeratorFunc (param1,param2,etc));</code>
		/// </summary>
		/// <param name="routine">Routine.</param>
		public static void DoCoroutine (IEnumerator routine)
		{
			_CoroutineManagerMonoBehaviour.StartCoroutine (routine);
		}

		#endregion
	}

}
