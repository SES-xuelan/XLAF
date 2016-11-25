using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace XLAF.Public
{
	public class UIUtils : MonoBehaviour
	{

		public static T getChild<T> (Transform parent, string childName)
		{
			Transform t = parent.FindChild (childName);
			if (t != null) {
				return t.GetComponent<T> ();
			} else {
				Log.Error ("error! find child null");
				return default(T);
			}

		}

		public static T setChild<T> (Transform parent, string childName)
		{
			Transform t = parent.FindChild (childName);
			if (t != null) {
				return t.GetComponent<T> ();
			} else {
				Log.Error ("error! find child null");
				return default(T);
			}

		}
	}

}
