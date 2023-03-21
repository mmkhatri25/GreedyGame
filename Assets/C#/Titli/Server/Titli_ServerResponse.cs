using UnityEngine;
using SocketIO;
using Titli.Utility;
using Titli.UI;
using Titli.Gameplay;
using UnityEngine.SceneManagement;

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
            socket.On(Events.onleaveRoom, OnDisconnected);
            // socket.On(Events.OnChipMove, OnChipMove);
            // socket.On(Events.OnGameStart, OnGameStart);
            // socket.On(Events.OnAddNewPlayer, OnAddNewPlayer);
            // socket.On(Events.OnPlayerExit, OnPlayerExit);
            socket.On(Events.OnTimerStart, OnTimerStart);
            // socket.On(Events.OnWait, OnWait);
            socket.On(Events.OnTimeUp, OnTimerUp);
            socket.On(Events.OnCurrentTimer, OnCurrentTimer);
            socket.On(Events.OnWinNo, OnWinNo);
            // socket.On(Events.OnBotsData, OnBotsData);
            // socket.On(Events.OnPlayerWin, OnPlayerWin);
            socket.On(Events.OnHistoryRecord, OnHistoryRecord);
            socket.On(Events.OnBetsPlaced, OnBetsPlaced);
            Debug.Log("Listner On");
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
        void OnTimerStart(SocketIOEvent e)
        {
            Titli_Timer.Instance.OnTimerStart(30);
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
           // Debug.Log("start CurrentTimer : " + e.data);
            
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

