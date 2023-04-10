using System;
using System.Collections;
using UnityEngine;
using Titli.Utility;
using Newtonsoft.Json;
using System.Collections.Generic;
using Titli.UI;
using Titli.ServerStuff;
using UnityEngine.UI;
using KhushbuPlugin;
using TMPro;

namespace Titli.Gameplay
{
    public class Titli_Timer : MonoBehaviour
    {
        public static Titli_Timer Instance;
        public GameObject waitForBetScreen;
        int bettingTime = 30;
        int timeUpTimer = 5;
        // int waitTimer = 3;
        public Action onTimeUp;
        public Action onCountDownStart;
        public Action startCountDown;
        public static gameState gamestate;
       public TMP_Text countdownTxt, waittext;
        public Text todaywin;
        // [SerializeField] TMP_Text messageTxt;
        private void Awake()
        {
            Instance = this;
            
            //Titli_ServerRequest.instance.socket.Emit(Events.winnerList);
        }
        void onWinnerListREceived()
        {

        }
        void Start()
        {
            // Titli_UiHandler.Instance.ShowMessage("please wait for next round...");
            gamestate = gameState.cannotBet;
            // onTimeUp?.Invoke();
            // onTimeUp();
            if(is_a_FirstRound)
            {
                print("this is fists time  - "+ is_a_FirstRound);
                // Titli_CardController.Instance._winNo = true;
                // for(int i = 0; i < Titli_CardController.Instance.TableObjs.Count; i++)
                // {
                //     Titli_CardController.Instance.TableObjs[i].GetComponent<BoxCollider2D>().enabled = false;
                // }
            }
        }    

        public void OnCurrentTime(object data = null)
        {
            // is_a_FirstRound = true;
            if (is_a_FirstRound)
            {
            waitForBetScreen.SetActive(true);

            }
            else
            {
waitForBetScreen.SetActive(false);
            onTimeUp();
            // Titli_CardController.Instance._winNo = true;
            // Titli_UiHandler.Instance.ShowMessage("please wait for next round...");
            InitialData init = new InitialData();
            try
            {
                init = Utility.Utility.GetObjectOfType<InitialData>(data.ToString());
                // Titli_UiHandler.Instance.UpDateBalance(float.Parse(cr.balance));
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }
            if (init.gametimer > 5)
            {
                Titli_UiHandler.Instance.lessthanFiveSec = false;
                //Debug.Log("Game Timer");
               // Debug.Log("here timer is moreeee 555 - "+ init.gametimer);
                
                OnTimerStart(init.gametimer);
                waitForBetScreen.SetActive(false);
                
            }
            else if (init.gametimer < 5)
            {
                //if(is_a_FirstRound)
                //waitForBetScreen.SetActive(true);
                //else
                //waitForBetScreen.SetActive(false);
                
                Titli_UiHandler.Instance.lessthanFiveSec = true;
                onTimeUp();
            //    Debug.Log("here timer is lesssssss - "+ init.gametimer);
                StartCoroutine(currentTimer(init.gametimer));
            }
                todaywin.text = init.currentWin.ToString();

            }
            
            
        }

        IEnumerator currentTimer(int currentGametimer)
        {
            for (int i = currentGametimer; i >= 0; i--)
            {
                // Titli_UiHandler.Instance.ShowMessage("please wait for next round... " + i.ToString() );
                yield return new WaitForSecondsRealtime(1f);
            }
        }

        public void OnTimerStart(int time)
        {
            // if (is_a_FirstRound)
            // {
            //     Titli_UiHandler.Instance.HideMessage();
            // }
            // is_a_FirstRound = false;
            Titli_UiHandler.Instance.ResetUi();
            Titli_CardController.Instance._startCardBlink = false;
            Titli_CardController.Instance._canPlaceBet = true;
            // Titli_UiHandler.Instance.ResetUi();
            StartCoroutine(timerStartCountdown(time));
            for(int i = 0; i < Titli_CardController.Instance.TableObjs.Count; i++)
            {
                Titli_CardController.Instance.TableObjs[i].GetComponent<BoxCollider2D>().enabled = true;
            }

            // StopCoroutines();
        }

        //this will run once it connected to the server
        //it will carry the time and state of server
        IEnumerator timerStartCountdown(int time)
        {
            onCountDownStart?.Invoke();
            gamestate = gameState.canBet;
            Titli_CardController.Instance._canPlaceBet = true;
            for (int i = time; i >= 0; i--)
            {
                if (i == 1)
                {
                    startCountDown?.Invoke();
                    countdownTxt.text = "wait..";
                    print("here countdown become 0 ...");
                

                }
                else
                {
                    //waittext.gameObject.SetActive(false);
                countdownTxt.text = i.ToString();
                }

                if (i <= 0)
                {
                    waittext.gameObject.SetActive(true);
                    countdownTxt.gameObject.SetActive(false);
                }
                // Debug.Log("Timer:" +i);
                if (i > 5)
                    Titli_Timer.Instance.waitForBetScreen.SetActive(false);
                yield return new WaitForSecondsRealtime(1f);
            }
            
            // Titli_ServerResponse.Instance.TimerUpFunction();
            onTimeUp?.Invoke();

        }


        public void OnTimeUp(object data)
        {
            if (is_a_FirstRound) return;
            
            Titli_CardController.Instance._canPlaceBet = false;
                waitForBetScreen.SetActive(false);
            
            // for(int i = 0; i < Titli_CardController.Instance.TableObjs.Count; i++)
            // {
            //     Titli_CardController.Instance.TableObjs[i].GetComponent<BoxCollider2D>().enabled = false;
            // }
            // StopCoroutines();
            StartCoroutine(TimeUpCountdown());
        }

        IEnumerator TimeUpCountdown(int time = -1)
        {
            gamestate = gameState.cannotBet;
            onTimeUp?.Invoke();
            Titli_CardController.Instance._startCardBlink = true;
            Titli_CardController.Instance._canPlaceBet = false;
            
            // foreach(var item in Titli_CardController.Instance._cardsImage)
            // {
            //     item.GetComponent<Button>().interactable = false;
            // }
            // StartCoroutine(Titli_CardController.Instance.CardsBlink());

            for (int i = time != -1 ? time : timeUpTimer; i >= 0; i--)
            {
                // messageTxt.text = "Time Up";
                countdownTxt.text = i.ToString();
                print("here counting ... " +countdownTxt.text );
                
                yield return new WaitForSecondsRealtime(1f);
            }

        }



        //Local Timer Start countdown
        // IEnumerator TimerStartCountDown(int timer = 25)
        // {
        //     Debug.Log("timer count down start");
        //     gamestate = gameState.canBet;
            
        //     // Titli_CardController.Instance._winNo = true;
        //     for(int i = timer; i >= 0; i--)
        //     {
        //         if (i == 1)
        //         {
        //             startCountDown?.Invoke();
        //         }
        //         messageTxt.text = "Bettting Time";
        //         countdownTxt.text = i.ToString();
        //         yield return new WaitForSecondsRealtime(1f);
        //     }
        //     Titli_CardController.Instance._canPlaceBet = true;
        //     StartCoroutine(TimeUpCountDown());
        //     onTimeUp?.Invoke();
        // }

        

        //Local Timer timeUp Countdown
        // IEnumerator TimeUpCountDown(int timer = 5)
        // {
        //     gamestate = gameState.cannotBet;
        //     onTimeUp?.Invoke();
            
        //     Titli_CardController.Instance._startCardBlink = true;
        //     Titli_CardController.Instance._canPlaceBet = false;
        //     foreach(var item in Titli_CardController.Instance._cardsImage)
        //     {
        //         item.GetComponent<Button>().interactable = false;
        //     }
        //     // Titli_ServerResponse.Instance.OnWinFunction();
        //     Titli_CardController.Instance.CardBlink_coroutine = Titli_CardController.Instance.CardsBlink();
        //     StartCoroutine(Titli_CardController.Instance.CardsBlink());
        //     // Titli_CardController.Instance._winNo = true;
        //     for(int i = timer; i >= 0; i--)
        //     {
        //         if(i == 2)
        //         {
        //             Titli_CardController.Instance._winNo = true;
        //             // Titli_ServerResponse.Instance.OnWinFunction();
        //         }
        //         messageTxt.text = "Time Up";
        //         countdownTxt.text = i.ToString();
        //         yield return new WaitForSecondsRealtime(2f);
        //     }
        // }

        // IEnumerator WaitCountdown(int time = -1)
        // {
        //     gamestate = gameState.wait;
        //     Titli_CardController.Instance._canPlaceBet = false;
        //     for (int i = time != -1 ? time : waitTimer; i >= 0; i--)
        //     {
        //         messageTxt.text = "Wait Time";
        //         countdownTxt.text = i.ToString();
        //         yield return new WaitForSecondsRealtime(1f);
        //     }

        // }


        

        // public void OnTimerStart()
        // {
        //     if (is_a_FirstRound)
        //     {
        //         Titli_UiHandler.Instance.HideMessage();
        //     }
        //     is_a_FirstRound = false;
        //     Titli_CardController.Instance._startCardBlink = false;
        //     Titli_CardController.Instance._canPlaceBet = true;

        //     StopCoroutines();
        //     Stop_CountDown = TimerStartCountDown();
        //     Debug.Log("timer start");
        //     // StartCoroutine(TimerStartCountDown());
        //     StartCoroutine(Stop_CountDown);
        //     // StartCoroutine(Countdown());
        // }

        

        // public void OnTimeUp()
        // {
        //     if (is_a_FirstRound) return;
        //     Titli_CardController.Instance._canPlaceBet = false;
        //     StopCoroutines();
        //     StopCoroutine(Stop_CountDown);
        //     StartCoroutine(onTimeUpcountDown());
        // }

        // public void OnWait(object data)
        // {            
        //     StopCoroutines();
        //     // StartCoroutine(StartDragonAnim());           
        //     if (is_a_FirstRound) return;
        //     // StartCoroutine(WOF_UiHandler.Instance.StartImageAnimation());
        //     StopCoroutines();
        //     StartCoroutine(WaitCountdown());
        // }
        public bool is_a_FirstRound = true;
        

        // public void StopCoroutines()
        // {
        //     StopCoroutine(Countdown());
        //     StopCoroutine(TimeUpCountdown());
        //     StopCoroutine(WaitCountdown());
        // }
    }
    [Serializable]
    public class CurrentTimer
    {
        public gameState gameState;
        public int timer;
        public List<int> lastWins;
        public int LeftBets;
        public int MiddleBets;
        public int RightBets;
    }
    public enum gameState
    {
        canBet = 0,
        cannotBet = 1,
        wait = 2,
    }

    public class InitialData
    {
        public List<int> previousWins;
        public List<BotsBetsDetail> botsBetsDetails;
        public string balance;
        public int gametimer;
        public int userDailyWin;
        public double currentWin;
    }
}
