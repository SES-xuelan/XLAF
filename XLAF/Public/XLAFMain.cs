using UnityEngine;
using System.Collections;

/*
unity跟android交互：

C#部分:
AndroidJavaClass jc = new AndroidJavaClass ("plugintest.albert.mylibrary.PhoneInfo"); 
string module = jc.CallStatic<string> ("getPhoneModule");

JAVA部分：
package plugintest.albert.mylibrary;
import xxxxxx
public class PhoneInfo {

public static String getPhoneModule() {
        return Build.MODEL;
    }

}

JAVA部分打包为aar或者jar 放到unity项目中的Plugins/Android目录下即可

*/






namespace XLAF.Public
{
    public class XLAFMain
    {
        

        static XLAFMain ()
        {
            //以下这些都可以调用，也可以不调用，主要作用是触发各class的static构造函数，一般情况下不建议调用；个别情况下需要调用

            /*
            MgrData.Init ();
            MgrAudio.Init ();
            Log.Init ();
            MgrDialog.Init ();
            MgrScene.Init ();
            MgrFPS.Init ();
            ModDispatcher.Init ();
            ModUtils.Init ();
            ModUIUtils.Init ();
            */
        }

        /// <summary>
        /// 调用Init会触发构造函数，可以用于统一初始化的时候
        /// </summary>
        public static void Init ()
        {

        }


        //对常用的函数进行进一步封装






        #if UNITY_ANDROID
        public static void ShowToast ()
        {
            
        }
        #endif







    }
}
