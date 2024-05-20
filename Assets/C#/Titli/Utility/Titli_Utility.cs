using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Shared;

namespace Titli.Utility
{
    public static class Utility
    {
        public static string DOMAIN;
        public static string ON_CHIP_MOVE = DOMAIN + "";
        public static string JOIN_GAME = DOMAIN + "";
        public static string ADD_PLAYER = DOMAIN + "";
        public static string ON_TIME_UP = DOMAIN + "";
        public static string ON_COUNTDOWN_START = DOMAIN + "";
        public static string ON_GAME_START = DOMAIN + "";
        public static string ON_PLAYER_EXIT = DOMAIN + "";
        public static JsonSerializerSettings Desersettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            MissingMemberHandling = MissingMemberHandling.Ignore
        };

        public static string playerId;
        public static T GetObjectOfType<T>(object json) where T : class
        {
            T t = null;
            try
            {
                t = JsonConvert.DeserializeObject<T>(json.ToString(), Desersettings);
                return t;
            }
            catch (Exception e)
            {
                Debug.Log(e);
                
            }
            return t;
        }

    };

    public class OnChipMove
    {
        public string playerId;
        public Chip chip;
        public Spot spot;
        public Vector3 position;
    }
    public class CurrentGameData
    {
        public string  playerId;
        public Chip    chip;
        public Spot    spot;
        public Vector3 position;
    }
    public static class Events
    {
        internal static string OnChipMove = "OnChipMove";
        internal static string OnTest = "test";
        internal static string OnPlayerExit = "OnPlayerExit";
        internal static string OnJoinRoom = "OnJoinRoom";
        internal static string OnTimeUp = "OnTimeUp";
        internal static string OnWait = "OnWait";
        internal static string OnTimerStart = "OnTimerStart";
        internal static string OnDrawCompleted = "OnDrawCompleted";
        internal static string OnGameStart= "OnGameStart";
        internal static string OnAddNewPlayer= "OnAddNewPlayer";
        internal static string OnCurrentTimer = "OnCurrentTimer";
        // internal static string SendCurrentRoundInfo = "SendCurrentRoundInfo";
        internal static string RegisterPlayer = "RegisterPlayer";
        
        internal static string OnBetsPlaced = "OnBetsPlaced";
        internal static string OnWinNo = "OnWinNo";
        internal static string OnBotsData = "OnBotsData";
        internal static string OnPlayerWin = "OnPlayerWin";
        internal static string onleaveRoom = "onleaveRoom";
        internal static string OnHistoryRecord = "OnHistoryRecord";
        internal static string userDailyWin = "userDailyWin";
        internal static string userWinAmount = "userWinAmount";// listner
        internal static string topWinner = "topWinner";// top winner on home page
        internal static string winnerList = "winnerList";// winner list daily weekly
       
        
    }
    public enum Spots
    {
        carrot = 0,
        papaya = 1,
        Cabbage = 2,
        tomato = 3,
        roll = 4,
        hotdog = 5,
        pizza = 6,
        chicken = 7,
        veg = 8,
        nonveg =9,
        //roll = 10


    }
}
