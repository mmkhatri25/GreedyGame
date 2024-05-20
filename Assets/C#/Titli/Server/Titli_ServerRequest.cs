using SocketIO;
using System.Collections;
using UnityEngine;
using Titli.Utility;
using Shared;
using System;
using UnityEngine.SceneManagement;

namespace Titli.ServerStuff
{
    public class Titli_ServerRequest : Titli_SocketHandler
    {
        public static Titli_ServerRequest instance;
        public static Action onJoinGame;

        private void OnEnable()
        {
            onJoinGame += JoinGame;
        }
        private void OnDisable()
        {
            onJoinGame -= JoinGame;

        }
        public void Awake()
        {
            socket = GameObject.Find("SocketIOComponents").GetComponent<SocketIOComponent>();
            instance = this;

        }
        private void Update()
        {
            if (socket.MyScoketDisconnected)
            {
                Debug.Log("JoinGame on reconnect");

                socket.MyScoketDisconnected = false;
                StopAllCoroutines();
                Titli_ServerResponse.Instance.removeSocketListener();
                SceneManager.LoadScene(0);
                JoinGame();
            }
        }
        public void JoinGame()
        {
            //Debug.Log($"player { PlayerPrefs.GetString("email")} Join game");
            //Debug.Log("user id at join game is - " + PlayerPrefs.GetString("userId"));
            Player player = new Player()
            {
                playerId = PlayerPrefs.GetString("userId"),//"nauyaniika@nmsgames.com",  
                userId = PlayerPrefs.GetString("userId"),
                balance = "1000",
                gameId = "4"
            };
            Debug.Log("On RegisterPlayer "+ player.playerId + " , "+ player.userId + " , "+ player.balance);

            socket.Emit("RegisterPlayer", new JSONObject( Newtonsoft.Json.JsonConvert.SerializeObject(player)) );
            //socket.Emit(Events.OnCurrentTimer);

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
