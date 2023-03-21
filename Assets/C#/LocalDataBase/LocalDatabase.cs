using System;
using UnityEngine;
using Newtonsoft.Json;
public static class LocalDatabase
{

    static string KEY = "PlayerData";
    public static LocalData data;
    public static void LoadGame()
    {
        bool hasValidData = PlayerPrefs.HasKey(KEY) && !string.IsNullOrEmpty(PlayerPrefs.GetString(KEY))
            && PlayerPrefs.GetString(KEY) != "null";
        if (hasValidData)
        {
            string saveData = PlayerPrefs.GetString(KEY);
            data = JsonConvert.DeserializeObject<LocalData>(saveData);
            return;
        }
        LocalData defautValue = new LocalData()
        {
            mobileNumber = "",
            username = SystemInfo.deviceModel,
            playerId = SystemInfo.deviceUniqueIdentifier
        };
        data = defautValue;
    }
    public static void SaveGame()
    {
        PlayerPrefs.SetString(KEY, JsonConvert.SerializeObject(data));
    }

    public class LocalData
    {
        public string mobileNumber;
        public string username;
        public string playerId;

    }
}
