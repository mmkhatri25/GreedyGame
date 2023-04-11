using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Titli.UI;
using Titli.Utility;
using KhushbuPlugin;
using Shared;
using Newtonsoft.Json;

using UnityEngine.Networking;

namespace Titli.Gameplay
{
    public class Titli_RoundWinningHandler : MonoBehaviour
    {

        [Header("Top 3 Winner Section")]
        [SerializeField] Image Winner1Dp; [SerializeField] Image Winner2Dp; [SerializeField] Image Winner3Dp;
        [SerializeField] Text Winner1Name, Winner2Name, Winner3Name;
        [SerializeField] Text Winner1WinCoins, Winner2WinCoins, Winner3WinCoins; 
        
        //Set top 3 Winners Data 
        void SetWinnersData(List<string> names, List<string> dpUrl, List<double> winAmount)
        {
            for (int i = 0; i < names.Count; i++)
            {
                switch (i)
                {
                    case 0:
                    Winner1Name.text = names[i];
                    Winner1WinCoins.text = winAmount[i].ToString();
                    StartCoroutine ( SetImageFromURL(dpUrl[i], Winner1Dp));
                    break;
                    case 1:
                    Winner2Name.text = names[i];
                    Winner2WinCoins.text = winAmount[i].ToString();
                    StartCoroutine (SetImageFromURL(dpUrl[i], Winner2Dp));
                    break;
                    case 2:
                    Winner3Name.text = names[i];
                    Winner3WinCoins.text = winAmount[i].ToString();
                    StartCoroutine (SetImageFromURL(dpUrl[i], Winner3Dp));
                    break;
                }
                
            }
        }


       //download images
            public static IEnumerator SetImageFromURL(string pictureURL,Image imageView){
            if (pictureURL.Length > 0) {
                WWW www = new WWW(pictureURL);  

                yield return www;
                Texture2D ui_texture = www.texture;
                if (ui_texture != null) {
                    Sprite sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
                    if (sprite != null) {
                        Debug.Log("ProfilePicUrlSet");
                        imageView.overrideSprite = sprite;
                    }
                }
            }
        }

       
    //   public static async Task<Texture2D> GetRemoteTexture ( string url, Image raw , Sprite sprite1 )
    //   { 
    //         using( UnityWebRequest www = UnityWebRequestTexture.GetTexture(url) )
    //         {
    //    // begin request:
    //    var asyncOp = www.SendWebRequest();

    //    // await until it's done: 
    //    while( asyncOp.isDone==false )
    //        await Task.Delay( 1000/30 );//30 hertz
        
    //    // read results:
    //   // if( www.isNetworkError || www.isHttpError )
    //     if( www.result!=UnityWebRequest.Result.Success )// for Unity >= 2020.1
    //    {
    //        // log error:
    //        #if DEBUG
    //        Debug.Log( $"{www.error}, URL:{www.url}" );
    //        #endif
     
    //        return null;
    //    }
    //    else
    //    {
    //                Texture2D tex = DownloadHandlerTexture.GetContent(www);
    //     sprite1 = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
    //        //raw.sprite = tex;
    //        return DownloadHandlerTexture.GetContent(www);
    //    }
    //}
//}



        public static Titli_RoundWinningHandler Instance;
        [SerializeField] List<GameObject> WinningRing;
        public Sprite[] Imgs;
        public Image[] previousWins;
        public List<int> PreviousWinValue;
        bool isTimeUp;
        int win_no;
        public double balance_amt, win_amount, total_bet;
        public GameObject Win_Panel, win_amount_desc, No_Win_Description;
        public Text Win_amount_text, Total_Bet_text, TodayWinText;
        public Image Win_Image,Win_Image_other;
        
       

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            Titli_Timer.Instance.onTimeUp = () => isTimeUp = true;
            Titli_Timer.Instance.onCountDownStart = () => isTimeUp = false;

            //leftDice.SetActive(false);
            //rightDice.SetActive(false);   
            
        }

        public IEnumerator SetWinNumbers(object o)
        {
            yield return new WaitForSeconds(1f);

            InitialData winData = Utility.Utility.GetObjectOfType<InitialData>(o);
            print("currentWin - " + winData.currentWin);

            TodayWinText.text = winData.currentWin + "";

            if (winData.previousWins != null)
            {
                while (winData.previousWins.Count > 9)
                {
                    winData.previousWins.RemoveAt(0);
                }
            }
            //TodayWinText.text = winData.userDailyWin.ToString();
            PreviousWinValue = winData.previousWins;
            PreviousWinValue.Reverse();
            // Debug.Log("Previous win"+ PreviousWinValue.Count);

            // yield return new WaitUntil( () => Imgs.Length == 8 );
            //PreviousWinValue.RemoveAt(0);
            //PreviousWinValue.Add(9);
            for (int i = 0; i < PreviousWinValue.Count; i++)
            {
                previousWins[i].sprite = Imgs[PreviousWinValue[i]];
                previousWins[i].gameObject.SetActive(true);
            }

            //for (int i = 0; i < previousWins.Length; i++)
            //{
            //   // if ( PreviousWinValue[i] > -1 && PreviousWinValue[i] > 9 ) continue;
            //    previousWins[i].sprite = Imgs[PreviousWinValue[i]];
            //    previousWins[i].gameObject.SetActive(true);
            //    // int num = winData.previousWins[i];
            //    // if (num == 0)
            //    // {
            //    //     previousWins[i].sprite = Imgs[0];//dragon
            //    // }
            //    // else if (num == 1)
            //    // {
            //    //     previousWins[i].sprite = Imgs[1];//tie
            //    // }
            //    // else
            //    // {
            //    //     previousWins[i].sprite = Imgs[2];//tiger
            //    // }
            //    // previousWins[i].gameObject.SetActive(true);
            //    //previousWins[i].gameObject.transform.GetChild(0).GetComponent<Text>().text = totalDiceNo.ToString();

            //}
        }
        IEnumerator showtoastforA(DiceWinNos winData)
        {
            yield return null;

            if (winData.data.status != null && winData.data.status == "false")
            {
                Debug.Log(winData.data.status + "  " + winData.data.status  );
                AndroidToastMsg.ShowAndroidToastMessage(" coin not added ");                
            }
            // else if (winData.data.message != null  && winData.data.message.status == 400) 
            // {
            //     Debug.Log(winData.data.message.message);
            //     AndroidToastMsg.ShowAndroidToastMessage(winData.data.message.message);
            // } 
        }
        // IEnumerator showtoastforB(DiceWinNos2 winData2 )
        // {
        //     yield return null;
        //     if (winData2.data.message != null  && winData2.data.message.status == 400) 
        //     {
        //         Debug.Log(winData2.data.message.message);
        //         AndroidToastMsg.ShowAndroidToastMessage(winData2.data.message.message);
        //     }                
            
        // }
        
        // getData(object o)
        // {
        //     try
        //     {
        //         DiceWinNos winData = Utility.Utility.GetObjectOfType<DiceWinNos>(o);
        //         StartCoroutine(showtoastforA(winData));
        //         return winData;
        //     }
        //     catch (System.Exception)
        //     {
        //         DiceWinNos2 winData = Utility.Utility.GetObjectOfType<DiceWinNos2>(o);
        //         StartCoroutine(showtoastforB(winData));
        //         throw;
        //     }
        // }
            public Root player;
        
        public void OnWin(object o)
        {
            Titli_Timer.Instance.is_a_FirstRound = false;
             print("here on win - "+ o);
            //DiceWinNos player = (DiceWinNos)JsonUtility.FromJson(o.ToString(), typeof(DiceWinNos));
             player = (Root)JsonUtility.FromJson(o.ToString(), typeof(Root));



            if (player.userIds.Count > 0)
            {
                print("hereee... win");
                for (int i = 0; i < player.userIds.Count; i++)
                {
                    print("I am checking 1 ...." + PlayerPrefs.GetString("userId"));

                    if (player.userIds[i].userId == PlayerPrefs.GetString("userId"))
                    {
                        print("yes I am exists ...." + player.userIds[i].win);

                        win_no = player.winNo;
                        balance_amt = player.userIds[i].balance;
                        win_amount = player.userIds[i].win;
                        total_bet = player.userIds[i].bat;
                        switch (win_no)
                        {
                            case 0:
                                StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.carrot));
                                Debug.Log("  carrot - " + win_amount);
                                break;
                            case 1:
                                StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.papaya));
                                Debug.Log("  papaya - " + win_amount);
                                break;
                            case 2:
                                StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.Cabbage));
                                Debug.Log("  Cabbage - " + win_amount);
                                break;
                            case 3:
                                StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.tomato));
                                Debug.Log("  tomato - " + win_amount);
                                break;
                            case 4:
                                StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.roll));
                                Debug.Log("  roll - " + win_amount);
                                break;
                            case 5:
                                StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.hotdog));
                                Debug.Log("  hotdog - " + win_amount);
                                break;
                            case 6:
                                StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.pizza));
                                Debug.Log("  pizza - " + win_amount);
                                break;
                            case 7:
                                StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.chicken));
                                Debug.Log("  chicken - " + win_amount);
                                break;
                           case 8:
                        StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.veg));
                        Debug.Log("  veg - " + win_amount);
                        break;
                    case 9:
                        StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.nonveg));
                        Debug.Log("  nonveg - " + win_amount);
                        break;
                            default:
                                Debug.Log("Invalid Win No");
                                break;
                        }

                        break;
                    }
                    else
                    {
                        print("I am in else part of win....");
                        win_no = player.winNo;
                        
                        //balance_amt = player.Balance;
                        win_amount = 0;
                        switch (win_no)
                        {
                            case 0:
                                StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.carrot));
                                Debug.Log("  carrot - " + win_amount);
                                break;
                            case 1:
                                StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.papaya));
                                Debug.Log("  papaya - " + win_amount);
                                break;
                            case 2:
                                StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.Cabbage));
                                Debug.Log("  Cabbage - " + win_amount);
                                break;
                            case 3:
                                StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.tomato));
                                Debug.Log("  tomato - " + win_amount);
                                break;
                            case 4:
                                StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.roll));
                                Debug.Log("  roll - " + win_amount);
                                break;
                            case 5:
                                StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.hotdog));
                                Debug.Log("  hotdog - " + win_amount);
                                break;
                            case 6:
                                StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.pizza));
                                Debug.Log("  pizza - " + win_amount);
                                break;
                            case 7:
                                StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.chicken));
                                Debug.Log("  chicken - " + win_amount);
                                break;
                              case 8:
                        StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.veg));
                        Debug.Log("  veg - " + win_amount);
                        break;
                    case 9:
                        StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.nonveg));
                        Debug.Log("  nonveg - " + win_amount);
                        break;
                            default:
                                Debug.Log("Invalid Win No");
                                break;
                        }
                    }
                }
            }
            else
            {

                win_no = player.winNo;
                //print("hereee... win -  "+ player.winNo);
                        //balance_amt = player.Balance;
                win_amount = 0;
                switch (win_no)
                {
                    case 0:
                        StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.carrot));
                        Debug.Log("  carrot - " + win_amount);
                        break;
                    case 1:
                        StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.papaya));
                        Debug.Log("  papaya - " + win_amount);
                        break;
                    case 2:
                        StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.Cabbage));
                        Debug.Log("  Cabbage - " + win_amount);
                        break;
                    case 3:
                        StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.tomato));
                        Debug.Log("  tomato - " + win_amount);
                        break;
                    case 4:
                        StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.roll));
                        Debug.Log("  roll - " + win_amount);
                        break;
                    case 5:
                        StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.hotdog));
                        Debug.Log("  hotdog - " + win_amount);
                        break;
                    case 6:
                        StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.pizza));
                        Debug.Log("  pizza - " + win_amount);
                        break;
                    case 7:
                        StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.chicken));
                        Debug.Log("  chicken - " + win_amount);
                        break;
                    case 8:
                        StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.veg));
                        Debug.Log("  veg - " + win_amount);
                        break;
                    case 9:
                        StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.nonveg));
                        Debug.Log("  nonveg - " + win_amount);
                        break;
                   default:
                       
                        break;
                }
            }

            winnerNames.Clear();
            DpUrl.Clear();
            Winamount.Clear();
            foreach (var item in TopWinner3)
            {
                item.gameObject.SetActive(false);
            }
            noWinText.SetActive(true);
            for (int i = 0; i < player.userIds.Count; i++)
            {
            noWinText.SetActive(false);
                TopWinner3[i].SetActive(true);
                winnerNames.Add(player.userIds[i].user.name);
                DpUrl.Add(player.userIds[i].user.profile_pic);
                Winamount.Add(player.userIds[i].win);
            }

            SetWinnersData(winnerNames, DpUrl, Winamount);

        }
        public GameObject noWinText;
        public List<GameObject> TopWinner3;
        public List<string> winnerNames;
        public List<string> DpUrl;
        public List<double> Winamount;
        void mySpinComplete(){
            // StartCoroutine( ShowWinningRing(WinningRing[cardNo] ) );
        }
        public Button vegPlate, nonVegPlate;
        
        IEnumerator ShowWinningRing( GameObject ring , Spots winnerSpot )
        {
                print("ShowWinningRing - "+winnerSpot);
        
             Titli_Timer.Instance.waitForBetScreen.SetActive(false);
             yield return StartCoroutine(Titli_CardController.Instance.CardsBlink(win_no));
             var tempColor_1 = ring.transform.parent.gameObject.GetComponent<Image>().color;
             Win_Image_other.sprite = Win_Image.sprite = Imgs[win_no];
            if (win_amount > 0)
            {
                No_Win_Description.SetActive(false);
                Win_amount_text.text = win_amount.ToString();
                Total_Bet_text.text = total_bet.ToString();
                win_amount_desc.SetActive(true);
                Win_Panel.SetActive(true);
            }
            else
            {
                win_amount_desc.SetActive(false);
                No_Win_Description.SetActive(true);
                Win_Panel.SetActive(true);
           
            }
 


            ring.SetActive(true); 
            yield return new WaitForSeconds(0.8f);
            ring.SetActive(false);
            
            yield return new WaitForSeconds(0.8f);
            ring.SetActive(true);
            
            yield return new WaitForSeconds(0.8f);
            ring.SetActive(false);
             
            yield return new WaitForSeconds(0.8f);
            ring.SetActive(true);
             
            yield return new WaitForSeconds(2f);
            ring.SetActive(false);
        

            StartCoroutine(Titli.UI.Titli_UiHandler.Instance.WinAmount(balance_amt, win_amount));
            PreviousWinValue.Reverse();
            while (PreviousWinValue.Count >= previousWins.Length)
            {
                PreviousWinValue.RemoveAt(0);
            }
            PreviousWinValue.Add(win_no);
            PreviousWinValue.Reverse();
            
            //for(int i = 0;i < PreviousWinValue.Count; i++)
            //{
            //    if ( PreviousWinValue[i] > -1 && PreviousWinValue[i] > 7 ) continue;
            //    previousWins[i].sprite = Imgs[PreviousWinValue[i]];
            //    previousWins[i].gameObject.SetActive(true);
            //}
            //  PreviousWinValue.RemoveAt(0);
            //PreviousWinValue.Add(9);
            for (int i = 0; i < PreviousWinValue.Count; i++)
            {
                previousWins[i].sprite = Imgs[PreviousWinValue[i]];
                previousWins[i].gameObject.SetActive(true);
            }
            Win_Panel.SetActive(false);
            total_bet = 0;
            //print("6 number card name - "+  Titli_CardController.Instance._cardsImage[win_no].transform.parent.GetChild(6).gameObject.name);
            Titli_CardController.Instance.owlimages[win_no].gameObject.SetActive(false);
            //Titli_CardController.Instance._cardsImage[win_no].transform.parent.GetChild(6).gameObject.SetActive(false);
            
        }
    }

    public class DiceWinNos
    {
        public int winNo, winPoint;
        public List<int> previousWins;
        public float Balance;
        public ClientData data;
    }
    public class ClientData
    {
        public string status;
        [JsonIgnore]
        public string _message { get; set; }
        // public Result result;
        [JsonIgnore]
        public Message message { get; set; }
        [JsonConstructor]
        public ClientData(JsonToken message )
        {
            if (message is JsonToken.String) 
            {
                _message =  message.ToString();
            }
            else
            { 
                // message = (Message)message.ToObject<Message>();
            }
        }
        public ClientData()
        {
        }
    }

    public class Message
    {
        public int status;
        public string message;
        public Result result;
        // [JsonConstructor]
        // public Message(string message )
        // {
        //     this.message = message;
        // }
        // [JsonConstructor]        
        // public Message()
        // {
        // }

    }
    public class Result
    {
        public long currentBalance;
    }
    
    
        [Serializable]
        public class Data
    {
            public bool status;
            public string message;
    }

        [Serializable]
    public class Root
    {
            public int RoundCount;
            public string playerId;
            public List<UserId> userIds;
            public int winNo;
            public List<int> previousWin_single;
            public int winPoint;
            public Data data;
       
    }

        [Serializable]
    public class UserId
    {
            public string userId;
            public int bat;
            public double win;
            public float balance;
        public User user;
    }
    

[Serializable]
    public class User
    {
        public string name;
        public double uId;
        public object id;
        public string profile_pic;
    }

  

}
