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
    /// 加密解密
    /// </summary>
    public class ModEncryption
    {
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


        static ModEncryption ()
        {
            _aes_key = MD5Encrypt16 (_aes_key);
            _zzz_key_len = _zzz_key.Length;
            _zzz_key_bytes = Encoding.UTF8.GetBytes (_zzz_key);
        }



        /// <summary>
        /// 调用Init会触发构造函数，可以用于统一初始化的时候
        /// </summary>
        public static void Init ()
        {

        }

        /// <summary>
        /// 16位MD5加密，返回大写字符串
        /// </summary>
        /// <returns>string（大写）</returns>
        /// <param name="str">String.</param>
        public static string MD5Encrypt16 (string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider ();
            string tmp = System.BitConverter.ToString (md5.ComputeHash (Encoding.Default.GetBytes (str)), 4, 8);
            tmp = tmp.Replace ("-", "");
            return tmp;

        }

        /// <summary>
        /// 32位MD5加密，返回大写字符串
        /// </summary>
        /// <returns>string（大写）</returns>
        /// <param name="str">String.</param>
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
        /// Base64加密
        /// </summary>
        /// <returns>string</returns>
        /// <param name="str">String.</param>
        public static string EncodeBase64 (string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes (str.ToCharArray ());
            return Convert.ToBase64String (bytes);

        }

        /// <summary>
        /// Base64解密
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
        /// AES加密算法
        /// </summary>
        /// <param name="str">明文字符串</param>
        /// <returns>将加密后的密文转换为Base64编码，以便显示</returns>
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
                    cipherBytes = ms.ToArray ();//得到加密后的字节数组
                    cs.Close ();
                    ms.Close ();
                }
            }
            return Convert.ToBase64String (cipherBytes);
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="str">密文字符串</param>
        /// <returns>返回解密后的明文字符串</returns>
        public static string DecodeAES (string str)
        {
            byte[] cipherText = Convert.FromBase64String (str);
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
            return Encoding.UTF8.GetString (decryptBytes).Replace ("\0", "");   ///将字符串后尾的'\0'去掉
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

    }
}