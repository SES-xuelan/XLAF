using UnityEngine;
using System.Collections;
using SimpleJSON;
using System.Collections.Generic;
using System;

namespace XLAF.Public
{
    /// <summary>
    /// 数据管理
    /// </summary>
    public class MgrData
    {
        static MgrData ()
        {
//            instance = new MgrData ();
            string appfn = ModUtils.documentsDirectory + "/app.jsn";
            string appdef = ModUtils.streamingDirectory + "/app.jsn";
            string sysfn = ModUtils.documentsDirectory + "/sys.jsn";
            string sysdef = ModUtils.streamingDirectory + "/sys.jsn";
            AddSetting (appSettingsName, appfn, appdef);
            AddSetting (sysSettingsName, sysfn, sysdef);

        }

        /// <summary>
        /// 调用Init会触发构造函数，可以用于统一初始化的时候
        /// </summary>
        public static void Init ()
        {

        }


//        private static MgrData instance;

        public readonly static string appSettingsName = "application";
        public readonly static string sysSettingsName = "system";

        private static Dictionary<string,SettingsData> DATA = new Dictionary<string, SettingsData> ();

        private static bool _CheckSettingsName (string settingsName, out SettingsData sd)
        {
            if (DATA.TryGetValue (settingsName, out sd)) {
                return true;
            } else {
                Log.Error (settingsName + " is not added, please call AddSetting before!");
                return false;
            }
        }

        /// <summary>
        /// Adds the setting.
        /// </summary>
        /// <param name="settingsName">Settings name.</param>
        /// <param name="filePathName">File path name.</param>
        /// <param name="defaultFilePathName">Default file path name.</param>
        public static void AddSetting (string settingsName, string filePathName, string defaultFilePathName)
        {
            if (DATA.ContainsKey (settingsName)) {
                Log.Error (settingsName + " already exist!");
                return;
            }

            DATA.Add (settingsName, new SettingsData (filePathName, defaultFilePathName));
        }




        //此处开始，下面的函数名和SettingsData中的函数名一致

        public static void Save (string settingsName)
        {
            SettingsData sd;
            if (!_CheckSettingsName (settingsName, out sd))
                return;

            sd.Save ();
        }

        public static void Set (string settingsName, string key, object value, bool autoSave = true)
        {
            SettingsData sd;
            if (DATA.TryGetValue (settingsName, out sd)) {
                sd.Set (key, value, autoSave);
            } else {
                Log.Error (settingsName + " is not added, please call AddSetting before!");
                return;
            }
        }

        public static string GetString (string settingsName, string key, string defaultValue = "")
        {
            SettingsData sd;
            if (!_CheckSettingsName (settingsName, out sd))
                return defaultValue;
            
            return sd.GetString (key, defaultValue);
        }

        public static int GetInt (string settingsName, string key, int defaultValue = 0)
        {
            SettingsData sd;
            if (!_CheckSettingsName (settingsName, out sd))
                return defaultValue;
            
            return sd.GetInt (key, defaultValue);
        }

        public static float GetFloat (string settingsName, string key, float defaultValue = 0.0f)
        {
            SettingsData sd;
            if (!_CheckSettingsName (settingsName, out sd))
                return defaultValue;

            return sd.GetFloat (key, defaultValue);
        }

        public static double GetDouble (string settingsName, string key, double defaultValue = 0.0d)
        {
            SettingsData sd;
            if (!_CheckSettingsName (settingsName, out sd))
                return defaultValue;

            return sd.GetDouble (key, defaultValue);
        }

        public static bool GetBool (string settingsName, string key, bool defaultValue = false)
        {
            SettingsData sd;
            if (!_CheckSettingsName (settingsName, out sd))
                return defaultValue;

            return sd.GetBool (key, defaultValue);
        }

        public static JSONNode GetJsonNode (string settingsName, string key, JSONNode defaultValue = null)
        {
            SettingsData sd;
            if (!_CheckSettingsName (settingsName, out sd))
                return defaultValue;

            return sd.GetJsonNode (key, defaultValue);
        }


        public static JSONNode GetAll (string settingsName)
        {
            SettingsData sd;
            if (!_CheckSettingsName (settingsName, out sd))
                return null;

            return sd.GetAll ();
        }

        public static void SetAll (string settingsName, JSONNode jsonObj = null)
        {

            SettingsData sd;
            if (!_CheckSettingsName (settingsName, out sd))
                return;

            sd.SetAll (jsonObj);
        }

        public static List<string> GetAllKeys (string settingsName)
        {
            SettingsData sd;
            if (!_CheckSettingsName (settingsName, out sd))
                return null;

            return sd.GetAllKeys ();
        }


        public static string GetDefaultString (string settingsName, string key, string defaultValue = "")
        {
            SettingsData sd;
            if (!_CheckSettingsName (settingsName, out sd))
                return defaultValue;

            return sd.GetDefaultString (key, defaultValue);
        }

        public static int GetDefaultInt (string settingsName, string key, int defaultValue = 0)
        {
            SettingsData sd;
            if (!_CheckSettingsName (settingsName, out sd))
                return defaultValue;

            return sd.GetDefaultInt (key, defaultValue);
        }

        public static float GetDefaultFloat (string settingsName, string key, float defaultValue = 0.0f)
        {
            SettingsData sd;
            if (!_CheckSettingsName (settingsName, out sd))
                return defaultValue;

            return sd.GetDefaultFloat (key, defaultValue);
        }

        public static double GetDefaultDouble (string settingsName, string key, double defaultValue = 0.0d)
        {
            SettingsData sd;
            if (!_CheckSettingsName (settingsName, out sd))
                return defaultValue;

            return sd.GetDefaultDouble (key, defaultValue);
        }

        public static bool GetDefaultBool (string settingsName, string key, bool defaultValue = false)
        {
            SettingsData sd;
            if (!_CheckSettingsName (settingsName, out sd))
                return defaultValue;

            return sd.GetDefaultBool (key, defaultValue);
        }

        public static JSONNode GetDefaultJsonNode (string settingsName, string key, JSONNode defaultValue = null)
        {
            SettingsData sd;
            if (!_CheckSettingsName (settingsName, out sd))
                return defaultValue;

            return sd.GetDefaultJsonNode (key, defaultValue);
        }



        public new static string ToString ()
        {
            string str = "\n";
            foreach (KeyValuePair<string,SettingsData> kv in DATA) {
                str = str + "key: " + kv.Key + " |value: " + kv.Value.ToString () + " \n";
            }
            return str;
        }
    }

    /// <summary>
    /// 数据基础类
    /// </summary>
    public class SettingsData
    {
        public SettingsData (string filePathName, string defaultFilePathName = null)
        {
            this.filePathName = filePathName;
            this.defaultFilePathName = defaultFilePathName;
            Load ();
            Save ();
        }

        private string filePathName;
        private string defaultFilePathName;
        JSONNode jsonData;
        JSONNode defJsonData;

        private void Load ()
        {
            jsonData = ModUtils.ReadJsonFromFile (filePathName, JSONNode.Parse ("{}"));

            if (!string.IsNullOrEmpty (defaultFilePathName)) {
                defJsonData = ModUtils.ReadJsonFromFile (defaultFilePathName, JSONNode.Parse ("{}"));
            }
        }

        public void Save ()
        {
            ModUtils.WriteJsonToFile (filePathName, jsonData);
        }


        public void Set (string key, object value, bool autoSave = true)
        {
            jsonData [key] = value.ToString ();
            if (autoSave) {
                Save ();
            }
        }

        //获取设置中的值，如果没有的话，查找默认文件中的值，再没有的话，返回defaultValue
        public string GetString (string key, string defaultValue = "")
        {
            string ret = jsonData [key].Value;
            if (string.IsNullOrEmpty (ret)) {
                ret = defJsonData [key].Value;
            }
            if (string.IsNullOrEmpty (ret)) {
                ret = defaultValue;
            }
            return ret;
        }

        public int GetInt (string key, int defaultValue = 0)
        {
            int ret;
            string str = GetString (key);
            if (int.TryParse (str, out ret)) {
                return ret;
            }
            return defaultValue;
        }

        public float GetFloat (string key, float defaultValue = 0.0f)
        {
            float ret;
            string str = GetString (key);
            if (float.TryParse (str, out ret)) {
                return ret;
            }
            return defaultValue;
        }

        public double GetDouble (string key, double defaultValue = 0.0d)
        {
            double ret;
            string str = GetString (key);
            if (double.TryParse (str, out ret)) {
                return ret;
            }
            return defaultValue;
        }

        public bool GetBool (string key, bool defaultValue = false)
        {
            bool ret;
            string str = GetString (key);
            if (bool.TryParse (str, out ret)) {
                return ret;
            }
            return defaultValue;
        }

        public JSONNode GetJsonNode (string key, JSONNode defaultValue = null)
        {
            JSONNode ret = null;
            string str = GetString (key);
            try {
                ret = JSONNode.Parse (str);
            } catch {
            }
            if (ret == null) {
                ret = defaultValue;
            }
            return ret;
        }


        public JSONNode GetAll ()
        {
            JSONNode jn = JSONNode.Parse ("{}");
            foreach (string k in defJsonData.Keys) {
                jn [k] = defJsonData [k].Value;
            }
            foreach (string k in jsonData.Keys) {
                jn [k] = jsonData [k].Value;
            }
            return jn;
        }

        public void SetAll (JSONNode jsonObj = null)
        {
            if (jsonObj == null) {
                jsonData = JSONNode.Parse ("{}");
            } else {
                jsonData = jsonObj;
            }
            Save ();
        }

        public List<string> GetAllKeys ()
        {
            List<string> ret = new List<string> ();
            foreach (string k in jsonData.Keys) {
                ret.Add (k);
            }
            return ret;
        }

        //获取默认文件中的默认值
        public string GetDefaultString (string key, string defaultValue = "")
        {
            string ret = defJsonData [key].Value;
            if (string.IsNullOrEmpty (ret)) {
                ret = defaultValue;
            }
            return ret;
        }

        public int GetDefaultInt (string key, int defaultValue = 0)
        {
            int ret;
            string str = GetDefaultString (key);
            if (int.TryParse (str, out ret)) {
                return ret;
            }
            return defaultValue;
        }

        public float GetDefaultFloat (string key, float defaultValue = 0.0f)
        {
            float ret;
            string str = GetDefaultString (key);
            if (float.TryParse (str, out ret)) {
                return ret;
            }
            return defaultValue;
        }

        public double GetDefaultDouble (string key, double defaultValue = 0.0f)
        {
            double ret;
            string str = GetDefaultString (key);
            if (double.TryParse (str, out ret)) {
                return ret;
            }
            return defaultValue;
        }

        public bool GetDefaultBool (string key, bool defaultValue = false)
        {
            bool ret;
            string str = GetDefaultString (key);
            if (bool.TryParse (str, out ret)) {
                return ret;
            }
            return defaultValue;
        }

        public JSONNode GetDefaultJsonNode (string key, JSONNode defaultValue = null)
        {
            JSONNode ret = null;
            string str = GetDefaultString (key);
            try {
                ret = JSONNode.Parse (str);
            } catch {
            }
            if (ret == null) {
                ret = defaultValue;
            }
            return ret;
        }


        public override string ToString ()
        {
            return jsonData.ToString ();
        }
    }


}