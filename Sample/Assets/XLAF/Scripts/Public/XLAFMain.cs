using UnityEngine;
using System.Collections;

/*
unity & android call:

C#:
AndroidJavaClass jc = new AndroidJavaClass ("plugintest.albert.mylibrary.PhoneInfo"); 
string module = jc.CallStatic<string> ("getPhoneModule");

JAVA:
package plugintest.albert.mylibrary;
import xxxxxx
public class PhoneInfo {

public static String getPhoneModule() {
        return Build.MODEL;
    }

}

JAVA code should export as *.aar or *.jar and put aar or jar files to Assets/Plugins/Android is ok.

*/
using XLAF.Private;

namespace XLAF.Public
{
	/// <summary>
	/// XLAF main script.
	/// </summary>
	public class XLAFMain
	{
		
		#region constructed function & initialization

		static XLAFMain ()
		{
			XLAFGameObject = new GameObject ("XLAFGameObject");
			XLAFGameObject.name = "XLAFGameObject";
			GameObject.DontDestroyOnLoad (XLAFGameObject);

			#if UNITY_EDITOR
			#elif UNITY_ANDROID
            //MgrAudio.PreloadAudio("s_click.mp3");
			#endif
			
			//MUST called below
			MgrAssetBundle.Init ();

			// In general, you should not call Init(), you can call  Init() in exceptional case.
			/*
            MgrData.Init ();
            MgrAudio.Init ();
            Log.Init ();
            MgrPopup.Init ();
            MgrScene.Init ();
            MgrFPS.Init ();
            ModDispatcher.Init ();
            ModUtils.Init ();
            ModUIUtils.Init ();
            and so on...
            */
		}

		/// <summary>
		/// call Init() will trigger constructed function, you can call Init() to ensure this class finished initialization
		/// </summary>
		public static void Init ()
		{

		}

		#endregion

		#region public variables

		/// <summary>
		/// Global Gameobject, used for XLAF inner classes in general
		/// </summary>
		public static GameObject XLAFGameObject{ get; set; }

		#endregion

	}
}
