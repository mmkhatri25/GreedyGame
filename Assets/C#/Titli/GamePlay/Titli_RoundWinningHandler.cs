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
        void SetWinnersData(List<string> names, List<string> dpUrl, List<int> winAmount)
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
        public float balance_amt, win_amount, total_bet;
        public GameObject Win_Panel, win_amount_desc, No_Win_Description;
        public Text Win_amount_text, Total_Bet_text;
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

            if (winData.previousWins != null)
            {
                while (winData.previousWins.Count > 9 )
                {
                    winData.previousWins.RemoveAt(0);
                }
            }

            PreviousWinValue = winData.previousWins;
            PreviousWinValue.Reverse();
           // Debug.Log("Previous win"+ PreviousWinValue.Count);

            // yield return new WaitUntil( () => Imgs.Length == 8 );

            for (int i = 0; i < previousWins.Length; i++)
            {
                if ( PreviousWinValue[i] > -1 && PreviousWinValue[i] > 7 ) continue;
                previousWins[i].sprite = Imgs[PreviousWinValue[i]];
                previousWins[i].gameObject.SetActive(true);
                // int num = winData.previousWins[i];
                // if (num == 0)
                // {
                //     previousWins[i].sprite = Imgs[0];//dragon
                // }
                // else if (num == 1)
                // {
                //     previousWins[i].sprite = Imgs[1];//tie
                // }
                // else
                // {
                //     previousWins[i].sprite = Imgs[2];//tiger
                // }
                // previousWins[i].gameObject.SetActive(true);
                //previousWins[i].gameObject.transform.GetChild(0).GetComponent<Text>().text = totalDiceNo.ToString();

            }
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
                        if (win_no == 0)
                        {
                            StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.carrot));
                            Debug.Log("  carrot - " + win_amount);

                        }
                        else if (win_no == 1)
                        {
                            StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.papaya));
                            Debug.Log("  papaya - " + win_amount);

                        }
                        else if (win_no == 2)
                        {
                            StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.Cabbage));
                            Debug.Log("  Cabbage - " + win_amount);

                        }
                        else if (win_no == 3)
                        {
                            StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.tomato));
                            Debug.Log("  tomato - " + win_amount);

                        }
                        else if (win_no == 4)
                        {
                            StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.roll));
                            Debug.Log("  roll - " + win_amount);

                        }
                        else if (win_no == 5)
                        {
                            StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.hotdog));
                            Debug.Log("  hotdog - " + win_amount);

                        }
                        else if (win_no == 6)
                        {
                            StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.pizza));
                            Debug.Log("  pizza - " + win_amount);

                        }
                        else if (win_no == 7)
                        {
                            StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.chicken));
                            Debug.Log("  chicken - " + win_amount);

                        }
                        else
                        {
                            Debug.Log("Invalid Win No");
                        }

                        break;
                    }
                    else
                    {
                        print("I am in else part of win....");
                        win_no = player.winNo;
                        //balance_amt = player.Balance;
                        win_amount = 0;

                        if (win_no == 0)
                        {
                            StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.carrot));
                            Debug.Log("  carrot - " + win_amount);

                        }
                        else if (win_no == 1)
                        {
                            StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.papaya));
                            Debug.Log("  papaya - " + win_amount);

                        }
                        else if (win_no == 2)
                        {
                            StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.Cabbage));
                            Debug.Log("  Cabbage - " + win_amount);

                        }
                        else if (win_no == 3)
                        {
                            StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.tomato));
                            Debug.Log("  tomato - " + win_amount);

                        }
                        else if (win_no == 4)
                        {
                            StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.roll));
                            Debug.Log("  roll - " + win_amount);

                        }
                        else if (win_no == 5)
                        {
                            StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.hotdog));
                            Debug.Log("  hotdog - " + win_amount);

                        }
                        else if (win_no == 6)
                        {
                            StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.pizza));
                            Debug.Log("  pizza - " + win_amount);

                        }
                        else if (win_no == 7)
                        {
                            StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.chicken));
                            Debug.Log("  chicken - " + win_amount);

                        }
                        else
                        {
                            Debug.Log("Invalid Win No");
                        }
                    }
                }
            }
            else
            {
                        win_no = player.winNo;
                        //balance_amt = player.Balance;
                        win_amount = 0;

                        if (win_no == 0)
                        {
                            StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.carrot));
                            Debug.Log("  carrot - " + win_amount);

                        }
                        else if (win_no == 1)
                        {
                            StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.papaya));
                            Debug.Log("  papaya - " + win_amount);

                        }
                        else if (win_no == 2)
                        {
                            StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.Cabbage));
                            Debug.Log("  Cabbage - " + win_amount);

                        }
                        else if (win_no == 3)
                        {
                            StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.tomato));
                            Debug.Log("  tomato - " + win_amount);

                        }
                        else if (win_no == 4)
                        {
                            StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.roll));
                            Debug.Log("  roll - " + win_amount);

                        }
                        else if (win_no == 5)
                        {
                            StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.hotdog));
                            Debug.Log("  hotdog - " + win_amount);

                        }
                        else if (win_no == 6)
                        {
                            StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.pizza));
                            Debug.Log("  pizza - " + win_amount);

                        }
                        else if (win_no == 7)
                        {
                            StartCoroutine(ShowWinningRing(WinningRing[win_no], Spots.chicken));
                            Debug.Log("  chicken - " + win_amount);

                        }
            }
            //List<string> abc = new List<string> { "mann", "kappor", "raj"};
            //List<int> wina = new List<int> {123,33434,56656};
            //List<string> dp = new List<string> { "https://www.gstatic.com/webp/gallery3/1.sm.png", "https://www.gstatic.com/webp/gallery3/1.sm.png", "https://fastly.picsum.photos/id/428/200/300.jpg?hmac=yZnpqAvuXjLW6NjhE0OFa2GwK6XcNLPBIrI3yr4yFsk"};

            winnerNames.Clear();
            DpUrl.Clear();
            Winamount.Clear();
            for (int i = 0; i < 3; i++)
            {
                winnerNames.Add(player.userIds[i].user.name);
                DpUrl.Add(player.userIds[i].user.profile_pic);
                Winamount.Add(player.userIds[i].win);
            }

            SetWinnersData(winnerNames, DpUrl, Winamount);

        }

        public List<string> winnerNames;
        public List<string> DpUrl;
        public List<int> Winamount;
        void mySpinComplete(){
            // StartCoroutine( ShowWinningRing(WinningRing[cardNo] ) );
        }

        /* Need to call below function of OnWin once api is integrated */
        // public void OnWin(object o)
        // {
        //     if (WOF_Timer.Instance.is_a_FirstRound) return;
        //     DiceWinNos winData = Utility.Utility.GetObjectOfType<DiceWinNos>(o);
        //     int[] turnArray = { 0, 1, 2, 3, 0, 1, 2, 3, 0, 1, 2, 3 };
        //     System.Random rands = new System.Random();
        //     List<int> rand = turnArray.OrderBy(c => rands.Next()).Select(c => c).ToList();
        //     int No1 = winData.winNo[0] - 1;
        //     int No2 = winData.winNo[1] - 1;
        //     StartCoroutine(cardOpen(No1, No2, rand[0], rand[1]));

        //     for (int i = 0; i < winData.previousWins.Count; i++)
        //     {
        //         int num = winData.previousWins[i];
        //         if (num == 0)
        //         {
        //             previousWins[i].sprite = Imgs[0];//dragon
        //         }
        //         else if (num == 1)
        //         {
        //             previousWins[i].sprite = Imgs[1];//tie
        //         }
        //         else
        //         {
        //             previousWins[i].sprite = Imgs[2];//tiger
        //         }                
        //     }
        //     if (winData.winningSpot == 0)//aniamtion add khusi
        //     {
        //         StartCoroutine(ShowWinningRing(leftRing, Spot.left, o));//dragon
        //     }
        //     else if (winData.winningSpot == 1)
        //     {
        //         StartCoroutine(ShowWinningRing(middleRing, Spot.middle, o));
        //     }
        //     else
        //     {
        //         StartCoroutine(ShowWinningRing(rightRing, Spot.right, o));
        //     }
        // }

        IEnumerator ShowWinningRing( GameObject ring , Spots winnerSpot )
        {
         Titli_Timer.Instance.waitForBetScreen.SetActive(false);
            Debug.Log("111 111 ShowWinningRing");
        
            yield return StartCoroutine(Titli_CardController.Instance.CardsBlink(win_no));

           // Debug.Log("222 ShowWinningRing");
            // yield return new WaitForSeconds(3f);
            var tempColor_1 = ring.transform.parent.gameObject.GetComponent<Image>().color;
            
            
            // Titli_CardController.Instance._cardsImage[win_no].transform.GetChild(6).gameObject.SetActive(true);
            // Titli_CardController.Instance.TakeChipsBack(winnerSpot);

            Win_Image_other.sprite = Win_Image.sprite = Imgs[win_no];
            if (win_amount > 0)
            {
           // Debug.Log("3333  ShowWinningRing - "+ win_amount);
            
                No_Win_Description.SetActive(false);
                Win_amount_text.text = win_amount.ToString();
                print("total bet on received - "+total_bet);
                Total_Bet_text.text = total_bet.ToString();
                win_amount_desc.SetActive(true);
                Win_Panel.SetActive(true);
            }
            else
            {
                win_amount_desc.SetActive(false);
                No_Win_Description.SetActive(true);
                Win_Panel.SetActive(true);
              // Debug.Log("4444  ShowWinningRing - "+ win_amount);
                
            }
            // Win_Panel.SetActive(true);


            ring.SetActive(true);
            // tempColor_1.a = 0.5f;
            // ring.transform.parent.gameObject.GetComponent<Image>().color = tempColor_1;
            yield return new WaitForSeconds(0.8f);
            ring.SetActive(false);
            // tempColor_1.a = 1.0f;
            // ring.transform.parent.gameObject.GetComponent<Image>().color = tempColor_1;
            yield return new WaitForSeconds(0.8f);
            ring.SetActive(true);
            // tempColor_1.a = 0.5f;
            // ring.transform.parent.gameObject.GetComponent<Image>().color = tempColor_1;
            yield return new WaitForSeconds(0.8f);
            ring.SetActive(false);
            // tempColor_1.a = 1.0f;
            // ring.transform.parent.gameObject.GetComponent<Image>().color = tempColor_1;
            yield return new WaitForSeconds(0.8f);
            ring.SetActive(true);
            // tempColor_1.a = 0.5f;
            // ring.transform.parent.gameObject.GetComponent<Image>().color = tempColor_1;
            yield return new WaitForSeconds(2f);
            ring.SetActive(false);
            // tempColor_1.a = 1.0f;
            // ring.transform.parent.gameObject.GetComponent<Image>().color = tempColor_1;
            // Titli_CardController.Instance._winNo = false;

            StartCoroutine(Titli.UI.Titli_UiHandler.Instance.WinAmount(balance_amt, win_amount));
            // Debug.LogError("cardno  " + Titli_UiHandler.Instance.betsholder[WinNo]);
            // if(Titli_UiHandler.Instance.betsholder[WinNo] > 0)
            // {
            //     Titli_UiHandler.Instance.OnPlayerWin();
            // }

            // foreach(var item in Titli_CardController.Instance._cardsImage)
            // {
            //     item.GetComponent<Button>().interactable = true;
            // }

            PreviousWinValue.Reverse();
            while (PreviousWinValue.Count >= previousWins.Length)
            {
                PreviousWinValue.RemoveAt(0);
                // PreviousWinValue.Add(cardNo);
            }
            PreviousWinValue.Add(win_no);
            PreviousWinValue.Reverse();
            
            for(int i = 0;i < PreviousWinValue.Count; i++)
            {
                if ( PreviousWinValue[i] > -1 && PreviousWinValue[i] > 7 ) continue;
                previousWins[i].sprite = Imgs[PreviousWinValue[i]];
                previousWins[i].gameObject.SetActive(true);
            }
            // Titli_Timer.Instance.OnTimerStart();
            // Titli_UiHandler.Instance.ResetUi(); 
            Win_Panel.SetActive(false);
            total_bet = 0;
            Titli_CardController.Instance._cardsImage[win_no].transform.parent.GetChild(6).gameObject.SetActive(false);
        }

        /* Need to call below function of OnWin once api is integrated */

        // IEnumerator ShowWinningRing(GameObject ring, Spot winnerSpot, object o)
        // {
        //     yield return new WaitForSeconds(1f);
        //     WOF_BotsManager.Instance.UpdateBotData(o);
        //     WOF_OnlinePlayerBets.Intsance.OnWin(o);
        //     WOF_ChipController.Instance.TakeChipsBack(winnerSpot);
        //     yield return new WaitForSeconds(2f);
        //     ring.SetActive(false);
        //     if (winnerSpot == Spot.left)
        //     {
        //         StartCoroutine(DragonAnimation());
        //     }
        //     else if (winnerSpot == Spot.middle)
        //     {
        //         StartCoroutine(TieAnimation());
        //     }
        //     else if (winnerSpot == Spot.right)
        //     {
        //         StartCoroutine(TigerAnimation());
        //     }

        //     //BetManager.Instance.WinnerBets(winnerSpot);

        //     yield return new WaitForSeconds(2f);
        //     UtilitySound.Instance.Cardflipsound();
        //     Card1.sprite = TigerBackCard;           
        //     Card2.sprite = ElephantBackCard;

        // }

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
            public int win;
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
