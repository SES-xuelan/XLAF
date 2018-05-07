using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XLAF.Public
{
	internal class CoroutineManagerMonoBehaviour : MonoBehaviour
	{
	}

	/* ==============================================================================
		* 功能描述：协程管理器
		* 创建日期：2015/04/29 17:58:09
		* ==============================================================================*/
	public class MgrCoroutine
	{
		private static CoroutineManagerMonoBehaviour _CoroutineManagerMonoBehaviour;

		static MgrCoroutine ()
		{
			var go = new GameObject ();
			go.name = "MgrCoroutine";
			_CoroutineManagerMonoBehaviour = go.AddComponent<CoroutineManagerMonoBehaviour> ();
			GameObject.DontDestroyOnLoad (go);
		}

		public static void DoCoroutine (IEnumerator routine)
		{
			_CoroutineManagerMonoBehaviour.StartCoroutine (routine);
		}

		public static void Init ()
		{
		}
	}

}
