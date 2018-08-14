using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XLAF.Public
{
	public class MgrTextureSheet : MonoBehaviour
	{

		#region private variables

		private static MgrTextureSheet _instance = null;

		private Dictionary<string ,Object[]> sheets;

		#endregion

		#region public variables

		public static MgrTextureSheet instance {
			get {
				return _instance;  
			}
		}

		#endregion

		#region constructed function & initialization

		static  MgrTextureSheet ()
		{
			_instance = XLAFMain.XLAFGameObject.AddComponent<MgrTextureSheet> ();  
			instance.sheets = new Dictionary<string, Object[]> ();
		}


		/// <summary>
		/// call Init() will trigger constructed function, you can call Init() to ensure this class finished initialization
		/// </summary>
		public static void Init ()
		{

		}

		#endregion

		/// <summary>
		/// Loads the sprite form a sheet.
		/// </summary>
		/// <returns>The sprite.</returns>
		/// <param name="sheetPath">Sheet path.</param>
		/// <param name="spriteName">Sprite name.</param>
		public Sprite LoadSprite (string sheetPath, string spriteName)
		{
			Sprite sprite = FindSpriteFormCache (sheetPath, spriteName);  
			if (sprite == null) {
				Object[] sprites = Resources.LoadAll (sheetPath);  
				sheets.Add (sheetPath, sprites);
				sprite = FindSpriteFormSheet (sprites, spriteName);  
			}  
			return sprite;  
		}

		/// <summary>
		/// Deletes the sheet cache.
		/// </summary>
		/// <param name="sheetPath">Sheet path.</param>
		public void DeleteSheetCache (string sheetPath)
		{  
			if (sheets.ContainsKey (sheetPath)) {  
				sheets.Remove (sheetPath);  
			}  
		}

		/// <summary>
		/// Finds the sprite form cache.
		/// </summary>
		/// <returns>The sprite form cache.</returns>
		/// <param name="sheetPath">Sheet path.</param>
		/// <param name="spriteName">Sprite name.</param>
		private Sprite FindSpriteFormCache (string sheetPath, string spriteName)
		{  
			if (sheets.ContainsKey (sheetPath)) {  
				Object[] sprites = sheets [sheetPath];  
				Sprite sprite = FindSpriteFormSheet (sprites, spriteName);  
				return sprite;  
			}  
			return null;  
		}

		/// <summary>
		/// Finds the sprite form sheet.
		/// </summary>
		/// <returns>The sprite form sheet.</returns>
		/// <param name="sprites">Sprites.</param>
		/// <param name="spriteName">Sprite name.</param>
		private Sprite FindSpriteFormSheet (Object[] sprites, string spriteName)
		{  
			for (int i = 0; i < sprites.Length; i++) {  
				if (sprites [i].GetType () == typeof(UnityEngine.Sprite)) {  
					if (sprites [i].name == spriteName) {  
						return (Sprite)sprites [i];  
					}  
				}  
			}  
			Log.Warning ("Sprite:" + spriteName + ";not found in sheets!");  
			return null;  
		}
	}
}
