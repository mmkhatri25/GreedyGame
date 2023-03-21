using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Shared;
using UnityEngine;

namespace LobbyScripts.Utility
{
    public static class Utility
    {
        public static T GetObjectOfType<T>(object json) where T : class
        {
            T t = null;
            try
            {
                t = JsonConvert.DeserializeObject<T>(json.ToString());
                return t;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
            return t;
        }
    };

    public static class Events
    {
        internal static string OnUploadImage = "OnUploadImage";
    }
}
