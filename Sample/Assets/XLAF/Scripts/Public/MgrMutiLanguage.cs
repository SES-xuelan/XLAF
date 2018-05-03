using System.Collections;
using System.Collections.Generic;
using System;
using XLAF.Public;
using SimpleJSON;
using UnityEngine;

public class MgrMutiLanguage
{
    public enum Language
    {
        zh_cn = 0,
        en_us = 1
    }

    private static JSONNode LanguageConfigs = JSONNode.Parse ("");



    private static Language currentLanguage;


    /// <summary>
    /// 调用Init会触发构造函数，可以用于统一初始化的时候
    /// </summary>
    public static void Init ()
    {

    }


    static MgrMutiLanguage ()
    {
        currentLanguage = (Language)MgrData.GetInt (MgrData.appSettingsName, "XLAF.language", 1);
        Log.Debug ("currentLanguage",currentLanguage);
        Load ();

    }

    private static void Load ()
    {
        TextAsset str = Resources.Load<TextAsset> ("Lang/" + currentLanguage.ToString ());
        LanguageConfigs = JSONNode.Parse (str.ToString ());
        Log.Debug ("bytes", LanguageConfigs);

    }

    public static Language GetCurrentLanguage ()
    {
        return currentLanguage;
    }

    /// <summary>
    /// Switchs the language.
    /// </summary>
    public static void SwitchLanguage (Language lang)
    {
        currentLanguage = lang;
        MgrData.Set (MgrData.appSettingsName, "XLAF.language", (int)lang);
        Load ();
        foreach (KeyValuePair<string,SceneObject> kv in MgrScene.GetAllScenes()) {
            kv.Value.script.UpdateLanguage ();
        }
        foreach (KeyValuePair<string,SceneObject> kv in MgrDialog.GetAllDialogs()) {
            kv.Value.script.UpdateLanguage ();
        }
    }

    public static string GetString (string stringKeyName)
    {
        string ret = LanguageConfigs [stringKeyName].Value;
        return ret;
    }

    public static string GetString (string stringKeyName, params object[] args)
    {
        string ret = "";
        try {
            ret = LanguageConfigs [stringKeyName].Value;
            ret = string.Format (ret, args);
        } catch (Exception e) {
            Log.Error ("error in MgrMutiLanguage|GetString:", e);
        }

        return ret;
    }


}
