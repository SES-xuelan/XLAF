using UnityEngine;
using System.Collections;
using System.Text;
using System.Security.Cryptography;
using System;
using System.IO;
using System.Collections.Generic;

namespace XLAF.Public
{
	/// <summary>
	/// Encrypt  Decrypt Mod
	/// </summary>
	public class ModEncryption
	{

		#region private variables

		/// <summary>
		/// The aes key, default is 《白金ディスコ》Lyrics.
		/// </summary>
		private static string _aes_key = "Kawatte ku mono kawaranai mono aki ppoi atashi ga hajimete shitta kono e ien o kimi ni chikau yo purachina ureshiinoni purachina setsunaku natte purachina nami daga de chau no wa nande dooshite disukotikku";
		private static byte[] _aes_IV = {
			0x12,
			0x34,
			0x56,
			0x78,
			0x90,
			0xAB,
			0xCD,
			0xEF,
			0x12,
			0x34,
			0x56,
			0x78,
			0x90,
			0xAB,
			0xCD,
			0xEF
		};
		private static string _zzz_key = "Money is any object or record that is generally accepted as payment for goods and services and repayment of debts in a given socio-economic context or country.";
		private static int _zzz_key_len = 0;
		private static byte[] _zzz_key_bytes;

		#endregion

		#region constructed function & initialization

		static ModEncryption ()
		{
			_aes_key = MD5Encrypt16 (_aes_key);
			_zzz_key_len = _zzz_key.Length;
			_zzz_key_bytes = Encoding.UTF8.GetBytes (_zzz_key);
		}

		/// <summary>
		/// call Init() will trigger constructed function, you can call Init() to ensure this class finished initialization
		/// </summary>
		public static void Init ()
		{

		}

		#endregion

		#region public variables

		/// <summary>
		/// Gets or sets the aes key.
		/// </summary>
		/// <value>The aes key.</value>
		public static string aes_key { 
			get {
				return _aes_key;
			} 
			set {
				_aes_key = value;
			} 
		}

		/// <summary>
		/// Gets or sets the aes IV.
		/// </summary>
		/// <value>The aes I.</value>
		public static byte[] aes_IV { 
			get {
				return _aes_IV;
			} 
			set {
				_aes_IV = value;
			} 
		}

		/// <summary>
		/// Gets or sets the zzz key.
		/// </summary>
		/// <value>The zzz key.</value>
		public static string zzz_key { 
			get {
				return _zzz_key;
			} 
			set {
				_zzz_key = value;
			} 
		}

		#endregion

		#region public functions

		/// <summary>
		///  MD5 encode, return 16 upper letters
		/// </summary>
		/// <returns>string(upper)</returns>
		/// <param name="str">String you want to encode.</param>
		public static string MD5Encrypt16 (string str)
		{
			MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider ();
			string tmp = System.BitConverter.ToString (md5.ComputeHash (Encoding.Default.GetBytes (str)), 4, 8);
			tmp = tmp.Replace ("-", "");
			return tmp;

		}

		/// <summary>
		/// 3MD5 encode, return 32 upper letters
		/// </summary>
		/// <returns>string(upper)</returns>
		/// <param name="str">String you want to encode.</param>
		public static string MD5Encrypt32 (string str)
		{
			MD5 md5 = MD5.Create ();
			byte[] s = md5.ComputeHash (Encoding.UTF8.GetBytes (str));
			string res = "";
			for (int i = 0; i < s.Length; i++) {
				res += s [i].ToString ("X");
			}
			return res;

		}


		/// <summary>
		/// Base64 encoder
		/// </summary>
		/// <returns>string</returns>
		/// <param name="str">String.</param>
		public static string EncodeBase64 (string str)
		{
			byte[] bytes = Encoding.UTF8.GetBytes (str.ToCharArray ());
			return Convert.ToBase64String (bytes);

		}

		/// <summary>
		/// Base64 decoder
		/// </summary>
		/// <returns>string</returns>
		/// <param name="str">String.</param>
		public static string DecodeBase64 (string str)
		{
			string res = "";
			try {
				byte[] bytes = Convert.FromBase64String (str);
				res = Encoding.UTF8.GetString (bytes);
			} catch {
				res = str;
			}
			return res;

		}

		/// <summary>
		/// AES encoder
		/// </summary>
		/// <param name="str">string you want to encode.</param>
		/// <returns>encode string with Base64.</returns>
		public static string EncodeAES (string str)
		{
			SymmetricAlgorithm des = Rijndael.Create ();
			byte[] inputBytes = Encoding.UTF8.GetBytes (str);
			des.Key = Encoding.UTF8.GetBytes (_aes_key);
			des.IV = _aes_IV;
			byte[] cipherBytes = null;
			using (MemoryStream ms = new MemoryStream ()) {
				using (CryptoStream cs = new CryptoStream (ms, des.CreateEncryptor (), CryptoStreamMode.Write)) {
					cs.Write (inputBytes, 0, inputBytes.Length);
					cs.FlushFinalBlock ();
					cipherBytes = ms.ToArray ();//get byte array
					cs.Close ();
					ms.Close ();
				}
			}
			return Convert.ToBase64String (cipherBytes);
		}

		/// <summary>
		/// AES decoder
		/// </summary>
		/// <param name="str">string you want decode with Base64.</param>
		/// <returns>string </returns>
		public static string DecodeAES (string base64Str)
		{
			byte[] cipherText = Convert.FromBase64String (base64Str);
			SymmetricAlgorithm des = Rijndael.Create ();
			des.Key = Encoding.UTF8.GetBytes (_aes_key);
			des.IV = _aes_IV;
			byte[] decryptBytes = new byte[cipherText.Length];
			using (MemoryStream ms = new MemoryStream (cipherText)) {
				using (CryptoStream cs = new CryptoStream (ms, des.CreateDecryptor (), CryptoStreamMode.Read)) {
					cs.Read (decryptBytes, 0, decryptBytes.Length);
					cs.Close ();
					ms.Close ();
				}
			}
			return Encoding.UTF8.GetString (decryptBytes).Replace ("\0", "");   ///remove \0
		}

		public static string DecodeZZZ (byte[] bytes)
		{
			string res = "";
			if (Encoding.UTF8.GetString (bytes, 0, 3) == "zzz") {
				List<byte> lst = new List<byte> ();
				for (int i = 3, index = 1; i < bytes.Length; i++,index++) {
					int r = (int)bytes [i];
					int n = index + (int)Mathf.Floor (1.0f * index / _zzz_key_len);
					n = (n - 1) % _zzz_key_len + 1;
					int key = (int)_zzz_key_bytes [n - 1];
//                    Log.Debug ("before:", n, r, key);
					r = r - key - 88;
					r = (r + 256) % 256;
					lst.Add ((byte)r);
				}
				res = Encoding.UTF8.GetString (lst.ToArray ());
			}
			return res;
		}

		#endregion
	}
}