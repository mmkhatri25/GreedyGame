using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace KhushbuPlugin
{
    public class DefaultValues : MonoBehaviour
    {

        public static int Int(string oldValue, string defaultValue = "0")
        {
            if (oldValue == null || oldValue == "")
            {
                oldValue = defaultValue;
            }
            return Convert.ToInt32(oldValue);
        }

        public static bool Bool(string oldValue, string defaultValue = "false")
        {
            if (oldValue == null || oldValue == "")
            {
                oldValue = defaultValue;
            }
            return Convert.ToBoolean(oldValue.ToLower());
        }

        public static string String(string oldValue, string defaultValue = "")
        {
            if (oldValue == null || oldValue == "")
            {
                oldValue = defaultValue;
            }
            return oldValue;
        }

        public static float Float(string oldValue, string defaultValue = "0")
        {
            if (oldValue == null || oldValue == "")
            {
                oldValue = defaultValue;
            }
            return Convert.ToSingle(oldValue);
        }

        public static double Double(string oldValue, string defaultValue = "0")
        {
            if (oldValue == null || oldValue == "")
            {
                oldValue = defaultValue;
            }
            return Convert.ToDouble(oldValue);
        }

        public static T Custom<T>(string oldValue, T defaultValue)
        {
            if (oldValue == null || oldValue == "")
            {
                return defaultValue;
            }
            else
            {
                return (T)Convert.ChangeType(oldValue, typeof(T));
            }
        }

        public static T CustomList<T>(T oldValue, T defaultValue)
        {
            if (oldValue == null)
            {
                return defaultValue;
            }
            else
            {
                return (T)Convert.ChangeType(oldValue, typeof(T));
            }
        }
    }
}


