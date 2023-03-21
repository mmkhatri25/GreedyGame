using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

    public static class LocalPlayer
    {
        private static string _deviceId;
        private static string _balance;
        private static string _profilePic;
        private static string _userName;
        public static string deviceId { get { return _deviceId; } set { _deviceId = value; } }
        public static string balance { get { return _balance; } set { _balance = value; } }
        public static string profilePic { get { return _profilePic; } set { _profilePic = value; } }
        public static string userName { get { return _userName; } set { _userName = value; } }

        public static void LoadGame()
        {
            _deviceId = PlayerPrefs.GetString("deviceId", "1");
            _profilePic = PlayerPrefs.GetString("_profilePic", null);
            _userName = PlayerPrefs.GetString("_userName", string.Concat("Guest:", SystemInfo.deviceUniqueIdentifier.ToString()));
            _balance = PlayerPrefs.GetString("_balance", "10000");
        }
        public static void SaveGame()
        {
            PlayerPrefs.SetString("deviceId", _deviceId);
            PlayerPrefs.SetString("_profilePic", _profilePic);
            PlayerPrefs.SetString("_userName", _userName.ToString());
            PlayerPrefs.SetString("_balance", _balance.ToString());
            PlayerPrefs.Save();
        }
    }
