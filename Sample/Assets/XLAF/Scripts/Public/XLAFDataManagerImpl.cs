using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XLAF.Public
{
	/// <summary>
	/// XLAF data manager impl you must add a class inherit this class.<para></para>
	/// e.g.<c>public class MgrAppData:DataManager{}</c>
	/// </summary>
	public class XLAFDataManagerImpl
	{
		#region DO NOT MODIFY

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="XLAF.Public.XLAFDataManagerImpl"/> system music.
		/// </summary>
		/// <value><c>true</c> if system music; otherwise, <c>false</c>.</value>
		public static bool systemMusic {
			set {
				MgrData.Set (MgrData.sysSettingsName, "XLAF.music", value);
			}
			get {
				return MgrData.GetBool (MgrData.sysSettingsName, "XLAF.music", true);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="XLAF.Public.XLAFDataManagerImpl"/> system sound.
		/// </summary>
		/// <value><c>true</c> if system sound; otherwise, <c>false</c>.</value>
		public static bool systemSound {
			set {
				MgrData.Set (MgrData.sysSettingsName, "XLAF.sound", value);
			}
			get {
				return MgrData.GetBool (MgrData.sysSettingsName, "XLAF.sound", true);
			}
		}

		/// <summary>
		/// Gets or sets the game language.
		/// </summary>
		/// <value>The game language.</value>
		public static string gameLanguage {
			set {
				MgrData.Set (MgrData.appSettingsName, "XLAF.language", value);
			}
			get {
				return MgrData.GetString (MgrData.appSettingsName, "XLAF.language", "en_us");
			}
		}

		/// <summary>
		/// Gets or sets the get debug level.
		/// </summary>
		/// <value>The get debug level.</value>
		public static int getDebugLevel { 
			get { return MgrData.GetInt (MgrData.sysSettingsName, "XLAF.debug", 0); } 
		}

		public static bool isShowAdmobTestAds { 
			get { return MgrData.GetBool (MgrData.sysSettingsName, "XLAF.admobtestads", false); } 
			set { MgrData.Set (MgrData.sysSettingsName, "XLAF.admobtestads", value); }
		}

		#endregion
	
	}
}