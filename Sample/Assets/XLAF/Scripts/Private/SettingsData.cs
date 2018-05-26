using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using XLAF.Public;

namespace XLAF.Private
{
	/// <summary>
	/// Settings Data
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

		/// <summary>
		/// Gets the string in settings, if is null, find in defaule file, if also null, return defaultValue.
		/// </summary>
		/// <returns>The string.</returns>
		/// <param name="key">Key.</param>
		/// <param name="defaultValue">Default value.</param>
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

		/// <summary>
		/// Gets the string from defaule file.
		/// </summary>
		/// <returns>The default string.</returns>
		/// <param name="key">Key.</param>
		/// <param name="defaultValue">Default value.</param>
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