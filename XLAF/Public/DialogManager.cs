using UnityEngine;
using System.Collections;
using XLAF.Public;

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





    // Update is called once per frame
    void Update ()
    {
        if (dialogViewRoot == null)
            return;
        SetDialogViewVisible (!(currentDialog == null));
    }

}
