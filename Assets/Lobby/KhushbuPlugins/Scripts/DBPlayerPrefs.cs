using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

#pragma warning disable 0162, 0414
namespace KhushbuPlugin
{
    public class DBPlayerPrefs : ScriptableObject
    {

        private static readonly byte[] Salt =
        new byte[] { 10, 20, 30, 40, 50, 60, 70, 80, 90, 12, 13, 14, 15, 16, 17, 18, 19 };
        

        private static string UTF8ByteArrayToString(byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            string constructedString = encoding.GetString(characters);
            return (constructedString);
        }

        private static byte[] StringToUTF8ByteArray(string pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }

        public static void SetInt(string Key, int Value)
        {
            PlayerPrefs.SetString(Key, Value.ToString());
        }
        public static void SetString(string Key, string Value)
        {
            PlayerPrefs.SetString(Key, Value);
        }
        public static void SetFloat(string Key, float Value)
        {
            PlayerPrefs.SetString(Key, Value.ToString());
        }
        public static void SetBool(string Key, bool Value)
        {
            PlayerPrefs.SetString(Key, Value.ToString());
        }

        public static string GetString(string Key)
        {
            String t = PlayerPrefs.GetString(Key);
            if (t == "") return "";
            return t;
        }
        public static int GetInt(string Key)
        {
            String t = PlayerPrefs.GetString(Key);
            if (t == "") return 0;
            return int.Parse(t);
        }
        public static float GetFloat(string Key)
        {
            String t = PlayerPrefs.GetString(Key);
            if (t == "") return 0;
            return float.Parse(t);
        }
        public static bool GetBool(string Key)
        {
            String t = PlayerPrefs.GetString(Key);
            if (t == "") return false;
            return Boolean.Parse(t);
        }
        public static void DeleteKey(string Key)
        {
            PlayerPrefs.DeleteKey(Key);
        }
        public static void DeleteAll()
        {
            PlayerPrefs.DeleteAll();
        }

        public static void Save()
        {
            PlayerPrefs.Save();
        }

        public static bool HasKey(string Key)
        {
            return PlayerPrefs.HasKey(Key);
        }
    }
}
#pragma warning restore 0162, 0414