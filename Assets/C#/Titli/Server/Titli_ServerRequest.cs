using SocketIO;
using System.Collections;
using UnityEngine;
using Titli.Utility;
using Shared;

namespace Titli.ServerStuff
{
    public class Titli_ServerRequest : Titli_SocketHandler
    {
        public static Titli_ServerRequest instance;
        
        public void Awake()
        {
            socket = GameObject.Find("SocketIOComponents").GetComponent<SocketIOComponent>();
            instance = this;

        }
        public void JoinGame()
        {
            //Debug.Log($"player { PlayerPrefs.GetString("email")} Join game");
            Debug.Log("user id at join game is - " + PlayerPrefs.GetString("userId"));
            Player player = new Player()
            {
                playerId = PlayerPrefs.GetString("userId"),//"nauyaniika@nmsgames.com",  
                userId = PlayerPrefs.GetString("userId"),
                balance = "1000",
                gameId = "4"
            };
            socket.Emit(Events.RegisterPlayer, new JSONObject( Newtonsoft.Json.JsonConvert.SerializeObject(player)) );
        }

        public void LeaveRoom()
        {
            socket.Emit(Events.onleaveRoom);
        }

        // public void OnChipMove(Vector3 position, Chip chip, Spot spot)
        // {
        //     OnChipMove Obj = new OnChipMove()
        //     {
        //         position = position,
        //         playerId = UserDetail.UserId.ToString(),
        //         chip = chip,
        //         spot = spot
        //     };
        //     socket.Emit(Events.OnChipMove, new JSONObject(JsonUtility.ToJson(Obj)));
        // }
        public void OnTest()
        {
            socket.Emit(Events.OnTest);
        }       
        public void OnHistoryRecordGame()
        {
            socket.Emit(Events.OnHistoryRecord);
        }
    }
}
