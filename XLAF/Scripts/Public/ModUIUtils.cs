using UnityEngine;
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
        public static T GetChild<T> (Transform parent, string childName)
        {
            Transform t = parent.FindChild (childName);
            if (t != null) {
                return t.GetComponent<T> ();
            } else {
                Log.Error ("error! find child null");
                return default(T);
            }

        }


        public static void ChangePos (RectTransform rect, float? x = null, float? y = null)
        {
            float _x = rect.anchoredPosition.x;
            float _y = rect.anchoredPosition.y;
            if (x == null)
                rect.anchoredPosition = new Vector2 (_x, (float)y);
            else if (y == null)
                rect.anchoredPosition = new Vector2 ((float)x, _y);
            else
                rect.anchoredPosition = new Vector2 ((float)x, (float)y);

        }

        public static void ChangePos (Button t, float? x = null, float? y = null)
        {
            RectTransform rect = t.image.rectTransform;
            if (rect == null)
                Log.Error ("RectTransform is null");
            ChangePos (rect, x, y);
        }


        public static void ChangeSize (RectTransform rect, float? width = null, float? height = null)
        {

            float _width = rect.sizeDelta.x;
            float _height = rect.sizeDelta.y;
            if (width == null)
                rect.sizeDelta = new Vector2 (_width, (float)height);
            else if (height == null)
                rect.sizeDelta = new Vector2 ((float)width, _height);
            else
                rect.sizeDelta = new Vector2 ((float)width, (float)height);
        }

        public static void ChangeSize (Button t, float? width = null, float? height = null)
        {
            RectTransform rect = t.GetComponent<RectTransform> ();
            if (rect == null)
                Log.Error ("RectTransform is null");
            ChangeSize (rect, width, height);
        }

        public static void ChangeSize(GameObject t,float? width = null, float? height = null)
        {
            RectTransform rect = t.GetComponent<RectTransform> ();
            if (rect == null)
                Log.Error ("RectTransform is null");
            ChangeSize (rect, width, height);
        }
    }

}
