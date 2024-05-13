using System.Collections.ObjectModel;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.Events;
using System.Linq;
using SocketIO;
using Shared;
using LobbyScripts;
using Titli.Utility;
using Titli.player;
using Titli.Gameplay;
using Titli.ServerStuff;
using Com.BigWin.WebUtils;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;
using TMPro;

namespace Titli.UI
{
    public class Titli_UiHandler : MonoBehaviour
    {
        public bool isBetPlaced;
        public bool isInternetGone = false;

        public static Titli_UiHandler Instance;
        public Chip currentChip;
        public Image[] chip_select;
        public Text[] chip_select_text;
        public GameObject[] Bet_Text;
        Double balance;
        int CarrotBets, PapayaBets, CabbageBets, TomatoBets, RollBets, HotDogBets, PizzaBets, ChickenBets;
        public int[] betsholder = new int[8];
        public Text CarrotBetsTxt, PapayaBetsTxt, CabbageBetsTxt, ChickenBetsTxt, PizzaBetsTxt, RollBetsTxt, HotDogBetsTxt, TomatoBetsTxt;
        public Text Win_Amount_text;
        public GameObject Win_Text_base;
        [SerializeField] Text balanceTxt;
        int totalBetsValue;



        void Awake()
        {
            Instance = this;
            Screen.orientation = ScreenOrientation.Portrait;
            Screen.sleepTimeout = SleepTimeout.NeverSleep; 
        }
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(CheckInternetConnection());

            ResetUi();
            AddListeners();
            totalBetsValue = 0;
            balance = ((double)PlayerPrefs.GetFloat("currentBalance"));
            balanceTxt.text = balance.ToString();
      
            // UpdateUi();
            ChipImgSelect(0);
            BgSound.Play();
            musicPlaying = true;
            StartCoroutine(CheckInternet());
        }



        private IEnumerator CheckInternetConnection()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);

                if (Application.internetReachability == NetworkReachability.NotReachable)
                {
                    // Show the popup if internet connection is lost
                    isInternetGone = true;
                    pausePanel.SetActive(true);
                    Debug.Log("NotReachable=====");
                }
                else
                {
                    Debug.Log("Reachable=====");
                    if (isInternetGone)
                    {
                        isInternetGone = false;
                        pausePanel.SetActive(false);
                        SceneManager.LoadScene(0);
                    }
                    
                }
            }
        }
       


        bool musicPlaying;
        public GameObject soundOn, soundOff;
        public void soundONOFF()
        {
            if (musicPlaying)
            {
                BgSound.Pause();
                musicPlaying = false;
                soundOff.SetActive(true);
                soundOn.SetActive(false);
            }
            else
            {
                BgSound.UnPause();
                musicPlaying = true;
                soundOff.SetActive(false);
                soundOn.SetActive(true);
            }

        }
        
        private void Update() {

            if( Input.GetKey(KeyCode.Escape) )
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    ExitLobby();
                }
            }
            
            balanceTxt.text = balance.ToString();
            CarrotBetsTxt.text = CarrotBets.ToString();
            PapayaBetsTxt.text = PapayaBets.ToString();
            CabbageBetsTxt.text = CabbageBets.ToString();
            ChickenBetsTxt.text = ChickenBets.ToString();
            TomatoBetsTxt.text = TomatoBets.ToString();
            HotDogBetsTxt.text = HotDogBets.ToString();
            RollBetsTxt.text = RollBets.ToString();
            PizzaBetsTxt.text = PizzaBets.ToString();


        }
        // bool isConnectedToInternet;
        public IEnumerator CheckInternet()
        {
            UnityWebRequest req = new UnityWebRequest("www.google.com");
            yield return req.SendWebRequest();
            if (req.error != null)
            {
                Debug.Log("No Internet ");
                AndroidToastMsg.ShowAndroidToastMessage(req.error);
                 yield return new WaitForSeconds(2f);
                ExitLobby();
            }
            yield return new WaitForSecondsRealtime(5f);
            StartCoroutine(CheckInternet());
        }
        bool isPaused;
        // public void OnApplicationFocus(bool hasFocus)
        // {
        //     isPaused = !hasFocus;
        //     if (Application.isEditor) return;
        //     if (isPaused && Application.platform == RuntimePlatform.Android) 
        //     {
        //         ExitLobby();
        //     }
        // }
        // DateTime time1, time2;
        public GameObject pausePanel;
        public string GreedyGameScene;
        public bool lessthanFiveSec; 
        private void OnApplicationPause(bool pauseStatus) {
            isPaused = pauseStatus;
            // if (Application.isEditor) return;
            if (isPaused)
            {
              //  Titli_UiHandler.Instance.SendBets();
                pausePanel.SetActive(true);
                // time1 = DateTime.Now;
             //   Debug.Log(isPaused);
                StopAllCoroutines();
                ResetUi();
                Titli_ServerResponse.Instance.removeSocketListener();
            }
            if (!isPaused) 
            {
                    SceneManager.LoadScene(0);
               
               // print("resuming here - "+  PlayerPrefs.GetFloat("nowcoins"));
               // balanceTxt.text =  PlayerPrefs.GetFloat("nowcoins").ToString();
               //// Debug.Log(isPaused);
                //// ExitLobby();
                //StopAllCoroutines();
                //// Titli_ServerResponse.Instance.addSocketListner();
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                //pausePanel.SetActive(false);
                
            }
        }

        private void AddListeners()
        {
            Titli_Timer.Instance.startCountDown += () =>
            {
                for(int i = 0; i < Titli_CardController.Instance.TableObjs.Count; i++)
                {
                    Titli_CardController.Instance.TableObjs[i].GetComponent<BoxCollider2D>().enabled = false;
                }
                StartCoroutine(send());
            };
        }

        IEnumerator send()
        {
            //print("here bet sending");
            SendBets();
            yield return new WaitForSeconds(0.5f);
        }

        public void BetValueSelect(int val)
        {
            if (val == 10)
            {
                currentChip = Chip.Chip10;
            }
            else if (val == 50)
            {
                currentChip = Chip.Chip50;
            }
            else if (val == 100)
            {
                currentChip = Chip.Chip100;
            }
            else if (val == 1000)
            {
                currentChip = Chip.Chip1000;
            }
            else if (val == 10000)
            {
                currentChip = Chip.Chip10000;
            }
        }    
        public void ChipImgSelect(int ind)
        {
            for (int i = 0; i < chip_select.Length; i++)
            {
                chip_select[i].gameObject.SetActive(false);
                chip_select_text[i].color = Color.black;
            }
            chip_select[ind].gameObject.SetActive(true);
            chip_select_text[ind].color = Color.white;
        }

        int carrotBet, papyabet, cabagebet, tomatobet, rollbet, hotdogbet, pizzbet, checkenbet;
        public void AddBets(Spots spot)
        {
            print("add bests section... - "+ Titli_Timer.Instance.is_a_FirstRound);
        
            if (Titli_Timer.Instance.is_a_FirstRound)
                return;
            //carrotBet += CarrotBets;
            //papyabet += PapayaBets;
            //cabagebet += CabbageBets;
            //tomatobet += TomatoBets;
            //rollbet += RollBets;
            //hotdogbet += HotDogBets;
            //pizzbet += PizzaBets;
            //checkenbet += ChickenBets;
            //  CarrotBets = 0;
            //PapayaBets = 0;
            //CabbageBets = 0;
            //TomatoBets= 0;
            //RollBets = 0;
            //HotDogBets = 0;
            //PizzaBets = 0;
            //ChickenBets = 0;
            balance -= (float)currentChip;
           // PlayerPrefs.SetFloat("BetChip", (float)currentChip);
            PlayerPrefs.SetFloat("nowcoins", (float)balance);
            totalBetsValue += (int)currentChip;

            switch (spot)
            {
                case Spots.carrot:
                    CarrotBets += (int)currentChip;
                    betsholder[0] = CarrotBets; 
                    //betsholder[0] = carrotBet; 
                   
                     Bet_Text[0].SetActive(true);
                    isBetPlaced = true;
                   // SendBets();
                    break; 
                case Spots.papaya:
                    PapayaBets += (int)currentChip; 
                    betsholder[1] = PapayaBets;
                    //betsholder[1] = papyabet; 
                    
                    Bet_Text[1].SetActive(true);
                    isBetPlaced = true;
                    
                    break;
                case Spots.Cabbage:
                    CabbageBets += (int)currentChip; 
                    betsholder[2] = CabbageBets; 
                    //betsholder[2] = cabagebet; 
                    
                    Bet_Text[2].SetActive(true);
                    isBetPlaced = true;
                    
                    break;
                case Spots.tomato:
                    TomatoBets += (int)currentChip;
                    betsholder[3] = TomatoBets; 
                    //betsholder[3] = tomatobet; 
                    
                    isBetPlaced = true;
                    Bet_Text[3].SetActive(true);
                    break;
                case Spots.roll:
                    RollBets += (int)currentChip;
                    //betsholder[4] = RollBets; 
                    betsholder[4] = rollbet; 
                    
                    Bet_Text[4].SetActive(true);
                    isBetPlaced = true;
                    break;
                case Spots.hotdog:
                    HotDogBets += (int)currentChip;
                    betsholder[5] = HotDogBets;
                    //betsholder[5] = hotdogbet;
                    
                    Bet_Text[5].SetActive(true);
                    isBetPlaced = true;
                    break;
                case Spots.pizza:
                    PizzaBets += (int)currentChip; 
                    betsholder[6] = PizzaBets;
                    //betsholder[6] = pizzbet;
                    
                    Bet_Text[6].SetActive(true);
                    isBetPlaced = true;
                    break;
                case Spots.chicken:
                    ChickenBets += (int)currentChip;
                    betsholder[7] = ChickenBets; 
                    //betsholder[7] = checkenbet; 
                    
                    Bet_Text[7].SetActive(true);
                    isBetPlaced = true;
                    break;
                default:
                    break;
            }
            // UpdateUi();
         //   SendBets();
            
        }

        public GameObject balanceBoxText;
        public bool IsEnoughBalancePresent()
        {
            if (balance - (float)currentChip >= 0)
            {
                return true;
                // balanceBoxText.gameObject.transform.position.x = Mathf.Sin(Time.time * 1f) * 1f;
            }
            else
            {
                StartCoroutine(lowBal());
                return false;
            }
            // return balance - (float)currentChip > 0;
        }
        IEnumerator lowBal()
        {
            balanceBoxText.SetActive(true);
            yield return new WaitForSeconds(1.3f);
            balanceBoxText.SetActive(false);            
            
        }
        public void SendBets()
        {

            Debug.Log("On Send bet - Game Id = "+PlayerPrefs.GetString("gameId") + " And BetChip = "+PlayerPrefs.GetFloat("BetChip"));

            Titli_RoundWinningHandler.Instance.total_bet = betsholder.Sum();
            bet_data data = new bet_data()
            {
                userId = PlayerPrefs.GetString("userId"),
                carrot_total_bets = CarrotBets,
                papaya_total_bets = PapayaBets,
                cabbage_total_bets = CabbageBets,
                tomato_total_bets = TomatoBets,
                roll_total_bets = RollBets,
                hotdog_total_bets = HotDogBets,
                pizza_total_bets = PizzaBets,
                chicken_total_bets = ChickenBets,

                storeId = PlayerPrefs.GetString("storeId"),
                gameID = PlayerPrefs.GetString("gameId")
            };
                //print("CarrotBets - "+CarrotBets);
            
            print("send bete GAME ID - " + data.gameID+ ",   UserID: "+ data.userId +"\n"+"Carrot:" + data.carrot_total_bets + "\n" + "Papaya:" + data.papaya_total_bets + "\n" + "Cabbage:" + data.cabbage_total_bets + "\n" + "Chicken:" + data.chicken_total_bets + "\n" +"Mutton:" + data.pizza_total_bets + "\n" +"Shrimp:" + data.hotdog_total_bets + "\n" +"Fish:" + data.roll_total_bets + "\n" +"Tomato:" + data.tomato_total_bets + "\n" + data.gameID +  "\n" + data.storeId );
            Titli_ServerRequest.instance.socket.Emit(Events.OnBetsPlaced, new JSONObject(JsonConvert.SerializeObject(data)));
            isBetPlaced = false;
       //     print("CarrotBets - "+CarrotBets);
  
            
        }

        public class bet_data
        {
            public string userId, gameID, storeId;
            public int carrot_total_bets, papaya_total_bets, cabbage_total_bets, chicken_total_bets, pizza_total_bets, roll_total_bets, hotdog_total_bets, tomato_total_bets ;
        }

        public void BetRecieveData(object o)
        {
            bet_recieve_data data = Newtonsoft.Json.JsonConvert.DeserializeObject<bet_recieve_data>(o.ToString());
            print("current coins - "+  data.data.result.currentBalance);
            PlayerPrefs.SetFloat("nowcoins",  data.data.result.currentBalance);
            if (balance != data.data.result.currentBalance)
            {
                Debug.Log((double)data.data.result.currentBalance);
                balance = data.data.result.currentBalance;
                // UpdateUi();            
            }
            if (data.data.status == 200)
            {
                Debug.Log("200 - "+data.data.message);
            
               // AndroidToastMsg.ShowAndroidToastMessage(data.data.message);
            }
            if (data.data.status == 400)
            {
                Debug.Log("400 - "+data.data.message);
                AndroidToastMsg.ShowAndroidToastMessage(data.data.message);
            }
        }

        public class bet_recieve_data
        { 
            public int status;
            public string message;
            public DataB data;
        }
        public class DataB
        { 
            public int status;
            public string message;
            public Result result;
        }
        public class Result
        { 
            public float currentBalance;
        }

        public IEnumerator WinAmount(double balance_win, double win_amount)
        {
            if (win_amount <= 0) yield break;
            Debug.Log("Balance before Win Balance"+balance_win);
            Win_Amount_text.text = "+ "+ win_amount.ToString();
            Win_Text_base.SetActive(true);
            balance = balance_win;
            // balance = balance_win;
            Debug.Log("Balance After Add"+balance);
            balanceTxt.text = balance.ToString();
            yield return new WaitForSeconds(2f);
            Win_Text_base.SetActive(false);
            // PlayerPrefs.SetFloat("balance", balance);
            // PlayerPrefs.Save();
        }
        // // public string LeaderBoard_Url = "http://216.48.182.176:4000/user/leaderlist";
        // public string url_leaderboard = "http://216.48.182.176:4000/user/leaderlist";
        // public GameObject content, Leaderboard_panel, target_panel;
        public List<GameObject> content_clones;
        // public void Leaderboard() //btn function
        // {
        //     StartCoroutine(LeaderboardAPI());
        // }

        // public IEnumerator LeaderboardAPI()
        // {
    
        //     using (UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(url_leaderboard))
        //     {
        //         yield return www.SendWebRequest();

        //         if (www.result != UnityEngine.Networking.UnityWebRequest.Result.Success)
        //         {
        //             Debug.Log(www.error);
        //         }
        //         else
        //         {
        //             Debug.Log("Leaderboard Data: " + www.downloadHandler.text);
        //             Leaderboard_get_response res = JsonConvert.DeserializeObject<Leaderboard_get_response>(www.downloadHandler.text);

        //             // for(int i = 0; i< res.data.Length;i++)
        //             // {
        //             //     Debug.Log("deser:"+res.data[i].username.ToString());
        //             // }

        //             Leaderboard_panel.SetActive(true);

                    

                    // content.SetActive(true);
                    // content.transform.GetChild(1).GetComponent<Text>().text = res.data.leaderboard.Where<point>
                    // POSTApiClass result = JsonUtility.FromJson<POSTApiClass>(www.downloadHandler.text);
                    
                    // if(result.status == 200)
                    // {
                    //     WithdrawPanelScript.Instance.SuccessWithdrawal_Panel.SetActive(true);
                    // }
        //         }
        //     }
        // }

        // public class Leaderboard_get_response
        // {
        //     public string message;
        //     public Data_Dictiobnary data;
        // }
        // public class Data_Dictiobnary
        // {
        //     public int status;
        //     public string message;
        //     public Result_List[] result;
        // } 
        // public class Result_List
        // {
        //     public string _id;
        //     public Double total;
        //     public int count;
        //     public userData[] userData;
        // }
        // public class userData
        // {
        //     public string _id, name;
        // }

        public void DestroyList()
        {
            foreach (var item in content_clones)
            {
                Destroy(item);
            }
            content_clones.Clear();
        }


        public void BuyCoin()
        {
            StartCoroutine(buyCoinAPi());
        }

        public GameObject content, target_panel;
        public IEnumerator buyCoinAPi()
        {
            // StartCoroutine(WebRequestHandler.instance.buyCoinAPi());
            WWWForm data = new WWWForm();
            data.AddField("page", "1");
            using ( UnityWebRequest www = UnityWebRequest.Put("https://api-uat.yaravoice.com/api/v1/coin-seller-list", data.ToString()) )
            {
                www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    JsonSerializerSettings Desersettings = new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        MissingMemberHandling = MissingMemberHandling.Ignore
                    };
                    Debug.Log("Coin Buy " +www.downloadHandler.text );
                    coinResponseClass result = JsonConvert.DeserializeObject<coinResponseClass>(www.downloadHandler.text, Desersettings);

                    for (int i = 0; i <result.result.data.Length; i++)  //foreach (var item in res.data)
                    {
                        GameObject clone = Instantiate(content, target_panel.transform);
                        clone.GetComponent<CoinSeller>().setDetails(result.result.data[i].userId.name, result.result.data[i].userId.mobile);
                        Debug.Log("HomeScript"+result.result.data[i].userId.mobile);
                        clone.GetComponent<CoinSeller>().LoadProfileImage(result.result.data[i].userId.profile_pic);
                        // clone.transform.GetChild(1).GetComponent<TMP_Text>().text = result.result.data[i].userId.name;

                        // clone.transform.name = res.data.result[i].userData[i].name;
                        // clone.transform.GetChild(1).GetComponent<Text>().text = res.data.result[i].userData[i].name;
                        // clone.transform.GetChild(2).transform.GetChild(1).GetComponent<Text>().text = res.data.result[i].total.ToString();
                        clone.SetActive(true);
                        content_clones.Add(clone);
                    }

                }
            }
            
        }

        public class coinResponseClass
        {
            public int status;
            public string message;
            public ResultDetails result;
        }
        public class ResultDetails
        {
            public Data[] data;
        }
        public class Data
        {
            public string _id, adharNo, penNo, drivingNo, createdAt, updatedAt;
            public UserId userId;
            public int coinGet, coinSell, __v;
            public bool isActive;
        
        }
        public class UserId
        {
            public string _id, name, profile_pic, id, mobile;
            public int uId;
            public ProfileID profileId;
        }

        public class ProfileID
        {
            public int currentBalance;
        }


        public void RefreshCoins()
        {
            StartCoroutine(GetDataApi("http://216.48.182.176:4000/auth/login", PlayerPrefs.GetString("userId")));
        }
        
        public IEnumerator GetDataApi(string URL, string userid)
        {
            WWWForm form = new WWWForm();
            form.AddField("userId", userid);

            using (UnityWebRequest www = UnityWebRequest.Post(URL, form))
            {
                
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    BalanceREsponse result = Newtonsoft.Json.JsonConvert.DeserializeObject<BalanceREsponse>(www.downloadHandler.text);
                    Debug.Log("result   " + result.status );
                    if(result.status == 200)
                    {
                        Debug.Log("Data Recieved successfully.... " + www.downloadHandler.text);
                        PlayerPrefs.SetFloat("currentBalance", result.result.currentBalance);
                        // PlayerPrefs.SetString("gameId",result.result.gameId);
                        // PlayerPrefs.SetString("storeId",result.result.storeId);
                        // PlayerPrefs.SetString("name",result.result.name);
                        balance = result.result.currentBalance;
                        // SceneManager.LoadScene(1);

                    }
                    else
                    {   
                        Debug.Log("Error Retriving Data");
                        // if (result.status == 400)
                        //     Debug.Log("Status Code"+result.status);

                        // try
                        // {
                        //     // Application.Quit();
                        //     AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity");
                        //     activity.Call("finish");
                        // }
                        // catch( UnityException ex )
                        // {
                        //     Debug.Log("Exception:" +ex.ToString() + ex.HelpLink + ex.HResult);
                        // }
                        // Application.Quit();
                    }   
                }
            }

        }

        public class BalanceREsponse
        {
            public int status;
            public string message;
            public BalanceREsponseResult result;
        }
        public class BalanceREsponseResult
        {
            public float currentBalance;
            public bool isCoinSeller;
            public string name,gameId,storeId;
        }
        public class SendData
        {
            public string userId;
        }
        
        


        // public void RepeatBets()
        // {
        //     for(int i = 0; i < betsholder.Length; i++ )
        //     {
        //         betsholder[i] = PreviousbetHolder[i]; 
        //     }
        //     CarrotBets = betsholder[0];
        //     PapayaBets = betsholder[1];
        //     CabbageBets = betsholder[2];
        //     ChickenBets = betsholder[3];
        //     MuttonBets = betsholder[4];
        //     ShrimpBets = betsholder[5];
        //     FishBets = betsholder[6];
        //     TomatoBets = betsholder[7];
        //     RoseBets = betsholder[8];
        //     ButterflyBets = betsholder[9];
        //     RabbitBets = betsholder[10];
        //     PigeonBets = betsholder[11];
        //     UpdateUi();
        // }

        // public UnityAction OnClickOnDoubleBetBtn()
        // {
        //     return () =>
        //     {
        //         if (betsholder.Sum() == 0)
        //         {
        //             Debug.Log("no bet placed yet");
        //             return;
        //         }

        //         bool isEnoughBalance = balance > betsholder.Sum() * 2;

        //         if (!isEnoughBalance)
        //         {
        //             Debug.Log("not enough balance");
        //             return;
        //         }
        //         balance += betsholder.Sum();

        //         for (int i = 0; i < betsholder.Length; i++)
        //         {
        //             betsholder[i] *= 2;
        //         }

        //         CarrotBetsTxt.text = betsholder[0].ToString() == "0" ? string.Empty : betsholder[0].ToString();
        //         PapayaBetsTxt.text = betsholder[1].ToString() == "0" ? string.Empty : betsholder[1].ToString();
        //         CabbageBetsTxt.text = betsholder[2].ToString() == "0" ? string.Empty : betsholder[2].ToString();
        //         ChickenBetsTxt.text = betsholder[3].ToString() == "0" ? string.Empty : betsholder[3].ToString();
        //         MuttonBetsTxt.text = betsholder[4].ToString() == "0" ? string.Empty : betsholder[4].ToString();
        //         ShrimpBetsTxt.text = betsholder[5].ToString() == "0" ? string.Empty : betsholder[5].ToString();
        //         FishBetsTxt.text = betsholder[6].ToString() == "0" ? string.Empty : betsholder[6].ToString();
        //         TomatoBetsTxt.text = betsholder[7].ToString() == "0" ? string.Empty : betsholder[7].ToString();
        //         RoseBetsTxt.text = betsholder[8].ToString() == "0" ? string.Empty : betsholder[8].ToString();
        //         ButterflyBetsTxt.text = betsholder[9].ToString() == "0" ? string.Empty : betsholder[9].ToString();
        //         RabbitBetsTxt.text = betsholder[10].ToString() == "0" ? string.Empty : betsholder[10].ToString();
        //         PigeonBetsTxt.text = betsholder[11].ToString() == "0" ? string.Empty : betsholder[11].ToString();
                
        //         CarrotBets = betsholder[0];
        //         PapayaBets = betsholder[1];
        //         CabbageBets = betsholder[2];
        //         ChickenBets = betsholder[3];
        //         MuttonBets = betsholder[4];
        //         ShrimpBets = betsholder[5];
        //         FishBets = betsholder[6];
        //         TomatoBets = betsholder[7];
        //         RoseBets = betsholder[8];
        //         ButterflyBets = betsholder[9];
        //         RabbitBets = betsholder[10];
        //         PigeonBets = betsholder[11];

        //         balance -= betsholder.Sum();
        //         UpdateUi();
        //     };
        // }

        // public void ClearBets()
        // {
        //     ResetUi();
        // }

        // public void ClaimSpin()
        // {
        //     Titli_Timer.Instance.OnTimeUp();
        //     // Titli_ServerResponse.Instance.TimerFunction();
        //     Titli_CardController.Instance._startCardBlink = true;
        //     Titli_CardController.Instance._canPlaceBet = false;
        //     Titli_CardController.Instance._winNo = true;
        //     foreach(var item in Titli_CardController.Instance._cardsImage)
        //     {
        //         item.GetComponent<Button>().interactable = false;
        //     }
        //     Titli_CardController.Instance.CardBlink_coroutine = Titli_CardController.Instance.CardsBlink();
        //     StartCoroutine(Titli_CardController.Instance.CardsBlink());
        // }

        // public void UpDateBalance(float amount)
        // {
        //     Debug.Log("balance updated");
        //     // balance = amount;
        //     // balance = 10000f;
        //     UpdateUi();
        // }

        public void ResetUi()
        {
           // Debug.Log("Reset UI");
            CarrotBets = 0;
            PapayaBets = 0;
            CabbageBets = 0;
            ChickenBets = 0;
            PizzaBets = 0;
            RollBets = 0;
            HotDogBets = 0;
            TomatoBets = 0;
            totalBetsValue = 0;
            // for(int i = 0; i < betsholder.Length; i++ )
            // {
            //     PreviousbetHolder[i] = betsholder[i]; 
            // }
            for (int i = 0; i < betsholder.Length; i++)
            {
                betsholder[i] = 0;
            }
            for (int i = 0; i < Bet_Text.Length; i++)
            {
                Bet_Text[i].SetActive(false);
            }
            // UpdateUi();
        }

        // public void UpdateUi()
        // {
        //     balanceTxt.text = balance.ToString();
        //     // totalBetsTxt.text = totalBetsValue.ToString();
        //     CarrotBetsTxt.text = CarrotBets.ToString();
        //     PapayaBetsTxt.text = PapayaBets.ToString();
        //     CabbageBetsTxt.text = CabbageBets.ToString();
        //     ChickenBetsTxt.text = ChickenBets.ToString();
        //     MuttonBetsTxt.text = MuttonBets.ToString();
        //     ShrimpBetsTxt.text = ShrimpBets.ToString();
        //     FishBetsTxt.text = FishBets.ToString();
        //     TomatoBetsTxt.text = TomatoBets.ToString();
        // }

        // public void ShowMessage(string msg)
        // {
        //     messagePopUP.SetActive(true);
        //     msgTxt.text = msg;
        // }

        // public void HideMessage()
        // {
        //     messagePopUP.SetActive(false);
        //     msgTxt.text = string.Empty;
        // }
        public AudioSource BgSound;
        
        public void ExitLobby()
        {
            if (Titli_UiHandler.Instance.isBetPlaced)
            {
                AndroidExit.instance.onExitpopup();

            }
            else
            {
Titli_ServerRequest.instance.LeaveRoom();
            BgSound.Stop();
            }
            
            // sceneHandler.SceneUnLoad();
            // SceneManager.LoadScene("MainScene");
        }

        // public IEnumerator OnPlayerWin()
        // {
        //     yield return new WaitForSeconds(3.0f);
        //     // Win win = Dragon.Utility.Utility.GetObjectOfType<Win>(o);
        //     if (betsholder[Titli_RoundWinningHandler.Instance.WinNo] != 0)
        //     {
        //         // Titli_MainPlayer.Instance.winner(Convert.ToInt32(win.winAmount));
        //         // balance += win.winAmount;
        //         balance = (betsholder[Titli_RoundWinningHandler.Instance.WinNo] * 10) + balance;
        //     }
        //     UpdateUi();
        // }

    }
}
