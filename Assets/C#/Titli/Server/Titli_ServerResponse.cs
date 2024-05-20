using UnityEngine;
using SocketIO;
using Titli.Utility;
using Titli.UI;
using Titli.Gameplay;
using UnityEngine.SceneManagement;
using System;
using System.Net.Sockets;

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
        private void Update()
        {
            //if(socket.MyScoketDisconnected)
            //{
            //    print("1111 Titli_ServerResponse Removed Listners..." + socket.MyScoketDisconnected);

            //    socket.MyScoketDisconnected = false;

            //    removeSocketListener();
            //}
        }
        private void Start()
        {
            socket.On("open", OnConnected);
            serverRequest.JoinGame();
            addSocketListener();
        }

        public void addSocketListener()
        {
            socket.On(Events.onleaveRoom, OnDisconnected);
            socket.On(Events.OnGameStart, OnGameStart);
            socket.On(Events.RegisterPlayer, OnRegisterPlayer);
            socket.On(Events.OnTimerStart, OnTimerStart);
            socket.On(Events.userDailyWin, OnTimerStart);
            socket.On(Events.OnTimeUp, OnTimerUp);
            socket.On(Events.OnCurrentTimer, OnCurrentTimer);
            socket.On(Events.OnWinNo, OnWinNo);
            socket.On(Events.OnHistoryRecord, OnHistoryRecord);
            socket.On(Events.userWinAmount, OnuserWinAmount);
            socket.On(Events.topWinner, OntopWinner);
            socket.On(Events.winnerList, OnwinnerList);
        }

        void OnConnected(SocketIOEvent e)
        {
            Debug.Log("Titli_ServerResponse On Socket connected " + e.data);
        }

        void OnDisconnected(SocketIOEvent e)
        {
            Debug.Log(" server response On Socket  disconnected" + e.data);
            removeSocketListener();
            if (Application.isEditor)
            {
                Debug.Log("Scene Unloaded");
            }
            try
            {
                AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                activity.Call("finish");
            }
            catch (UnityException ex)
            {
                Debug.Log("Exception:" + ex.ToString() + ex.HelpLink + ex.HResult);
            }
        }

        public void removeSocketListener()
        {
            socket.Off("open", OnConnected);
            socket.Off(Events.onleaveRoom, OnDisconnected);
            socket.Off(Events.OnTimerStart, OnTimerStart);
            socket.Off(Events.OnTimeUp, OnTimerUp);
            socket.Off(Events.OnCurrentTimer, OnCurrentTimer);
            socket.Off(Events.OnWinNo, OnWinNo);
            socket.Off(Events.OnHistoryRecord, OnHistoryRecord);
            socket.Off(Events.OnBetsPlaced, OnBetsPlaced);
            socket.Off(Events.userWinAmount, OnuserWinAmount);
            socket.Off(Events.topWinner, OntopWinner);
            socket.Off(Events.winnerList, OnwinnerList);
            print("2222 Titli_ServerResponse Removed Listners..." + socket.MyScoketDisconnected);

        }

        void OnBetsPlaced(SocketIOEvent e)
        {
            Debug.Log("OnBetsPlaced: " + e.data);
            Titli_UiHandler.Instance.BetRecieveData(e.data);
        }

        void OnWinNo(SocketIOEvent e)
        {
            Debug.Log("OnWinNo: " + e.data);
            Titli_RoundWinningHandler.Instance.OnWin(e.data);
        }

        void OnGameStart(SocketIOEvent e)
        {
            Debug.Log("OnGameStart " + e.data);
        }

        void OnRegisterPlayer(SocketIOEvent e)
        {
            Debug.Log("OnRegisterPlayer " + e.data);
        }

        void OnuserWinAmount(SocketIOEvent e)
        {
            Debug.Log("OnuserWinAmount - " + e.data);
            RootWin winData = JsonUtility.FromJson<RootWin>(e.data.ToString());
            Debug.Log("OnuserWinAmount - " + winData.amount);
            Titli_RoundWinningHandler.Instance.TodayWinText.text = winData.amount + "";
        }

        [Serializable]
        public class RootWin
        {
            public double amount;
        }

        void OntopWinner(SocketIOEvent e)
        {
            setTopWinnerBottom.inst.SetwinnerData(e);
        }

        void OnwinnerList(SocketIOEvent e)
        {
            // Code for handling winner list
        }

        void OnTimerStart(SocketIOEvent e)
        {
            Titli_Timer.Instance.OnTimerStart(30);
            Titli_Timer.Instance.is_a_FirstRound = false;
            Titli_Timer.Instance.waittext.gameObject.SetActive(false);
            Titli_Timer.Instance.countdownTxt.gameObject.SetActive(true);
            //Debug.Log("here game start - " + e.data.ToString());
        }

        void OnTimerUp(SocketIOEvent e)
        {
            Titli_Timer.Instance.OnTimeUp((object)e.data);
        }
        
        void OnCurrentTimer(SocketIOEvent e)
        {
            //currentTimer = (CurrentTimer)JsonUtility.FromJson(e.ToString(), typeof(CurrentTimer));
            //Debug.Log("OnCurrentTimer  - "+ e.data);
            Titli_UiHandler.Instance.OnCurrentTimerReceived(e.data);
            //if(PlayerPrefs.GetInt("RoundNumber", currentTimer.RoundCount) == currentTimer.RoundCount)
            //{
            //    print("1 RoundNumber - "+ currentTimer.RoundCount);
            //}
            //else
            //{
            //    PlayerPrefs.GetInt("RoundNumber", currentTimer.RoundCount);
            //    print("2 RoundNumber - " + currentTimer.RoundCount);

            //}



            Titli_Timer.Instance.OnCurrentTime((object)e.data);
            StartCoroutine(Titli_RoundWinningHandler.Instance.SetWinNumbers(e.data));
        }

        void OnHistoryRecord(SocketIOEvent e)
        {
            Debug.Log("OnHistoryRecord " + e.data);
        }
    }
    
}
