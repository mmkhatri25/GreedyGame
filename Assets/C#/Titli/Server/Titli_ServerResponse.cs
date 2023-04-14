using UnityEngine;
using SocketIO;
using Titli.Utility;
using Titli.UI;
using Titli.Gameplay;
using UnityEngine.SceneManagement;
using System;
//using LunarConsoleEditorInternal;

namespace Titli.ServerStuff
{
    public class Titli_ServerResponse : Titli_SocketHandler
    {
        public static Titli_ServerResponse Instance;
        public Titli_ServerRequest serverRequest;

        private void Awake()
        {
            socket = GameObject.Find("SocketIOComponents").GetComponent<SocketIOComponent>();
            Instance = this;
            
        }
        private void Start()
        {
            
            socket.On("open", OnConnected);
            // socket.On("disconnected", OnDisconnected);
            
            serverRequest.JoinGame();
            
            addSocketListner();
            
        }
        public void addSocketListner()
        {
            //print("here addSocketListner");
            socket.On(Events.onleaveRoom, OnDisconnected);
            // socket.On(Events.OnChipMove, OnChipMove);
            // socket.On(Events.OnGameStart, OnGameStart);
            // socket.On(Events.OnAddNewPlayer, OnAddNewPlayer);
            // socket.On(Events.OnPlayerExit, OnPlayerExit);
            socket.On(Events.OnTimerStart, OnTimerStart);
            socket.On(Events.userDailyWin, OnTimerStart);
            // socket.On(Events.OnWait, OnWait);
            socket.On(Events.OnTimeUp, OnTimerUp);
            socket.On(Events.OnCurrentTimer, OnCurrentTimer);
            socket.On(Events.OnWinNo, OnWinNo);
            // socket.On(Events.OnBotsData, OnBotsData);
            // socket.On(Events.OnPlayerWin, OnPlayerWin);
            socket.On(Events.OnHistoryRecord, OnHistoryRecord);
            socket.On(Events.userWinAmount, OnuserWinAmount);
            socket.On(Events.topWinner, OntopWinner);
            socket.On(Events.winnerList, OnwinnerList);
            
            //Debug.Log("Listner On");
        }
        void OnConnected(SocketIOEvent e)
        {
            print("connected Serer Response");
        }
        void OnDisconnected(SocketIOEvent e)
        {
            print("disconnected"+e.data);
            removeSocketListner();
            
            // SceneManager.UnloadScene("Titli");
            if (Application.isEditor)
            {
                Debug.Log("Scene Unloaded");
            }
            try
            {
                // Application.Quit();
                AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                activity.Call("finish");
            }
            catch( UnityException ex )
            {
                Debug.Log("Exception:" +ex.ToString() + ex.HelpLink + ex.HResult);
            }
            // AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
            // activity.Call("finish");
            // socket.OnDestroy();
            // GameObject.Destroy(socket);
        }

        public void removeSocketListner()
        {
            socket.Off("open", OnConnected);
            socket.Off(Events.onleaveRoom, OnDisconnected);
            socket.Off(Events.OnTimerStart, OnTimerStart);
            // socket.Off(Events.OnWait, OnWait);
            socket.Off(Events.OnTimeUp, OnTimerUp);
            socket.Off(Events.OnCurrentTimer, OnCurrentTimer);
            socket.Off(Events.OnWinNo, OnWinNo);
            socket.Off(Events.OnHistoryRecord, OnHistoryRecord);
            socket.Off(Events.OnBetsPlaced, OnBetsPlaced);
               socket.Off(Events.userWinAmount, OnuserWinAmount);
            socket.Off(Events.topWinner, OntopWinner);
            socket.Off(Events.winnerList, OnwinnerList);
         //   Debug.Log("Listner Off");
        }

        void OnBetsPlaced (SocketIOEvent e)
        {
            Debug.Log("OnBetsPlaced: " +e.data); 
            Titli_UiHandler.Instance.BetRecieveData(e.data);
        }
        // void OnChipMove(SocketIOEvent e)
        // {

        //     // Titli_CardController.Instance.OnOtherPlayerMove((object)e.data);
        // }

        // void OnBotsData(SocketIOEvent e)
        // {
        //     // WOF_BetsHandler.Instance.AddBotsData(e.data);
        // }

        // public void OnWinFunction()
        // {
        //     Debug.Log("WIn function called "  );
        //     // socket.On(Events.OnWinNo, OnWinNo);
        // }

        void OnWinNo(SocketIOEvent e)
        {
            Debug.Log("OnWinNo: "+ e.data);
            Titli_RoundWinningHandler.Instance.OnWin(e.data);
        }

        // void OnGameStart(SocketIOEvent e)
        // {
        //     Debug.Log("OnGameStart " + e.data);
        //     // WOF_ChipController.Instance.OnOtherPlayerMove((object)e.data);
        //     // StartCoroutine(Titli_CardController.Instance.CardsBlink());
        // }
        // void OnAddNewPlayer(SocketIOEvent e)
        // {
        //     Debug.Log("OnAddNewPlayer " + e.data);
        //     // WOF_ChipController.Instance.OnOtherPlayerMove((object)e.data);
        // }
        // void OnPlayerExit(SocketIOEvent e)
        // {
        //     Debug.Log("OnPlayerExit " + e.data);
        //     // WOF_ChipController.Instance.OnOtherPlayerMove((object)e.data);
        // }
        
        
        void OnuserWinAmount(SocketIOEvent e)
        {
            print("OnuserWinAmount - " + e.data);
            RootWin winData = JsonUtility.FromJson<RootWin>(e.data.ToString());
           //  RootWin winData = Utility.Utility.GetObjectOfType<RootWin>(e);
            print("OnuserWinAmount - " + winData.amount);
             
            Titli_RoundWinningHandler.Instance.TodayWinText.text = winData.amount+"";
        }
 [Serializable]
        public class RootWin
    {
            public double amount;
    }
        void OntopWinner(SocketIOEvent e)
        {
            print("OntopWinner - " + e.data);
            setTopWinnerBottom.inst.SetwinnerData(e);

        }
        void OnwinnerList(SocketIOEvent e)
        {
            print("OnwinnerList - " + e.data);

        }
        void OnTimerStart(SocketIOEvent e)
        {
            Titli_Timer.Instance.OnTimerStart(30);
            Titli_Timer.Instance.is_a_FirstRound = false;
            Debug.Log("here timer start - " + e.data);
            Titli_Timer.Instance.waittext.gameObject.SetActive(false);
            Titli_Timer.Instance.countdownTxt.gameObject.SetActive(true);
            // Titli_UiHandler.Instance.HideMessage();
            
        }
        void OnTimerUp(SocketIOEvent e)
        {
          //  Debug.Log("on timeUp " + e.data);
            Titli_Timer.Instance.OnTimeUp((object)e.data);
        }
        // void OnWait(SocketIOEvent e)
        // {
        //     Debug.Log("on wait " + e.data);
        //     // Titli_Timer.Instance.OnWait((object)e.data);
        // }
        void OnCurrentTimer(SocketIOEvent e)
        {
           Debug.Log("OnCurrentTimer : " + e.data);
            
            Titli_Timer.Instance.OnCurrentTime((object)e.data);
            StartCoroutine(Titli_RoundWinningHandler.Instance.SetWinNumbers(e.data));
            // Titli_BotsManager.Instance.UpdateBotData(e.data);
            
            
        }
        // void OnPlayerWin(SocketIOEvent e)
        // {
        //     Debug.Log("win something " + e.data);
        //     // Titli_UiHandler.Instance.OnPlayerWin(e.data);
        // }
        void OnHistoryRecord(SocketIOEvent e)
        {
            Debug.Log("OnHistoryRecord " + e.data);
            // Titli_UiHandler.Instance.ShowHistoryGame(e.data);
        }
    }
}

