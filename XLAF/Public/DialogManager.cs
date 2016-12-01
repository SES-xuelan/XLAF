using UnityEngine;
using System.Collections;
using XLAF.Public;

namespace XLAF.Public
{
    /// <summary>
    /// 弹窗管理
    /// </summary>
    public class DialogManager : MonoBehaviour
    {
        static DialogManager ()
        {
            instance = (new GameObject ("DialogManager")).AddComponent<DialogManager> ();
        }


        private static GameObject currentDialog = null;
        private static Transform dialogViewRoot = null;
        private static CanvasGroup dialogViewRootCanvas = null;
        private static bool dialogViewVisible = true;
        private static DialogManager instance = null;


        /// <summary>
        /// 调用Init会触发构造函数，可以用于统一初始化的时候
        /// </summary>
        public static void Init ()
        {

        }


        public static void SetDialogRoot (Transform grp)
        {
            dialogViewRoot = grp;
            dialogViewRootCanvas = dialogViewRoot.transform.GetComponent<CanvasGroup> ();
        }

        public static Transform GetDialogRoot ()
        {
            return dialogViewRoot;
        }

        public static CanvasGroup GetDialogRootCanvas ()
        {
            return dialogViewRootCanvas;
        }

        public static bool GetDialogViewVisible ()
        {
            return dialogViewVisible;
        }

        public static void SetDialogViewVisible (bool visible)
        {
            if (dialogViewVisible == visible)
                return;

            Log.Debug ("SetDialogViewVisible", visible);
            dialogViewVisible = visible;
            dialogViewRoot.gameObject.SetActive (visible);
        }








        public static void test ()
        {
            instance._test ();
        }


        private void _test ()
        {
        
        }

        // Update is called once per frame
        void Update ()
        {
            if (dialogViewRoot == null)
                return;
            SetDialogViewVisible (!(currentDialog == null));
        }

    }
}