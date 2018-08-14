using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

namespace XLAF.Public
{
    /// <summary>
    /// Loading界面，用于异步加载
    /// </summary>
    public class MgrLoading : MonoBehaviour
    {
        static MgrLoading ()
        {
            instance = XLAFMain.XLAFGameObject.AddComponent<MgrLoading> ();

        }


        /// <summary>
        /// 调用Init会触发构造函数，可以用于统一初始化的时候
        /// </summary>
        public static void Init ()
        {

        }


        private static MgrLoading instance = null;

        private static float screenWidth = MgrScene.screenWidth;
        private static float screenHeight = MgrScene.screenHeight;
        private static float screenScale = MgrScene.screenScale;



        public static void ShowLoading (string loadingName)
        {
            instance.StartCoroutine (LoadSceneAsync ());
        }

        private static IEnumerator LoadSceneAsync ()
        {
            yield return new WaitForEndOfFrame ();//加上这么一句就可以先显示加载画面然后再进行加载 
            AsyncOperation a = SceneManager.LoadSceneAsync ("aa");
            yield return a;
        }

        public static void Update ()
        {
            
        }

    }

}