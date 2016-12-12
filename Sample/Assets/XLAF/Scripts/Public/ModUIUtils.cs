﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace XLAF.Public
{
    /// <summary>
    /// UI工具
    /// </summary>
    public class ModUIUtils
    {

        /// <summary>
        /// 调用Init会触发构造函数，可以用于统一初始化的时候
        /// </summary>
        public static void Init ()
        {

        }

        /// <summary>
        /// Gets the child form a transform.
        /// </summary>
        /// <returns>The child.</returns>
        /// <param name="parent">Transform parent.</param>
        /// <param name="childName">Child name.</param>
        /// <typeparam name="T">The type of the child.</typeparam>
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

//        public static T setChild<T> (Transform parent, string childName)
//        {
//            //todo
//        }
    }

}