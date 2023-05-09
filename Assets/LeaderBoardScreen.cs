using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SocketIO;
using Titli.Utility;
using Titli.ServerStuff;
using System;
using Newtonsoft.Json;
namespace Titli.Gameplay
{

    [Serializable]
    public class Playernewdata
    {
        public string userId;
    }
    public class LeaderBoardScreen : MonoBehaviour
    {
        public Transform m_ContentContainer, m_ContentContainerDaily;
        public GameObject m_ItemPrefab;
        public Text playerName, winAmountText;
        public Button DailyButton, WeeklyButton;
        public Text selfName, selfAmount, selfRank;
        public Image selfDP;
        
        //last winner data
        public Text lastwinnerprize;
        public Text lastwinneramount;
        public Image lastWinerprofile_pic;
        public Text LastWinnername;
        public RootDailyUsers winData;


        private void OnEnable()
        {
            StartCoroutine(showTopwinners());
        }

        IEnumerator showTopwinners()
        {
            yield return new WaitForSeconds(2f);
            Playernewdata player = new Playernewdata() { userId = PlayerPrefs.GetString("userId") };
            Titli_ServerRequest.instance.socket.Emit(Events.winnerList, new JSONObject(Newtonsoft.Json.JsonConvert.SerializeObject(player)), HandleAction);
           
           
        }

        void HandleAction(JSONObject obj)
        {
            string mystr = obj.ToString().Substring(1, obj.ToString().Length - 2);
            print("winData lists  - " + mystr);
            
            JSONObject abc = obj;
            winData = JsonUtility.FromJson<RootDailyUsers>(mystr);
            //ShowDailyList();
            //ShowDailyList();
            SetupLastWinner();

        }
        public void ShowWeeklyList()
        {
            Playernewdata player = new Playernewdata() { userId = PlayerPrefs.GetString("userId") };
            Titli_ServerRequest.instance.socket.Emit(Events.winnerList, new JSONObject(Newtonsoft.Json.JsonConvert.SerializeObject(player)), HandleAction);
           
            PopulateRankItems(m_ContentContainer, winData.weekly);
        }
        public void ShowDailyList()
        {
            Playernewdata player = new Playernewdata() { userId = PlayerPrefs.GetString("userId") };
            Titli_ServerRequest.instance.socket.Emit(Events.winnerList, new JSONObject(Newtonsoft.Json.JsonConvert.SerializeObject(player)), HandleAction);
           
            DailyPopulateRankItems(m_ContentContainerDaily, winData.daily);
        }


        void PopulateRankItems(Transform m_transform, List<WeeklyTopUsers> root)
        {
            foreach (Transform child in m_transform) { Destroy(child.gameObject); }
             if (root.Count<=0)
              SetPlayerRanking((0).ToString(), true);
             else
              SetPlayerRanking((root.Count+1).ToString(), true);
              
           
             for (int i = 0; i < root.Count; i++)
            {
                var item_go = Instantiate(m_ItemPrefab);
                if (PlayerPrefs.GetString("userId") == root[i].userId)
                {
                    SetPlayerRanking((i + 1).ToString(), true);
                    print("exists... " + root[i].userId);
                }
                    // else
                    //SetPlayerRanking(root.Count+"+", true);
                item_go.transform.SetParent(m_transform);
                item_go.transform.localScale = Vector2.one;
                item_go.GetComponent<RankitemSetup>().Username.text = root[i].name;
                item_go.GetComponent<RankitemSetup>().winAmount.text = root[i].amount.ToString();
                item_go.GetComponent<RankitemSetup>().UserRank.text = (i + 1).ToString();
                StartCoroutine(SetImageFromURL(root[i].profile_pic, item_go.GetComponent<RankitemSetup>().dp));
            }
        }
        void DailyPopulateRankItems(Transform m_transform, List<DailyTopusers> root)
        {
            foreach (Transform child in m_transform) { Destroy(child.gameObject); }
            if (root.Count<=0)
              SetPlayerRanking((0).ToString(), false);
            else
              SetPlayerRanking((root.Count+1).ToString(), false);
              
            for (int i = 0; i < root.Count; i++)
            {
                if (PlayerPrefs.GetString("userId") == root[i].userId)
                    SetPlayerRanking((i + 1).ToString(), false);
                    //else
                    //SetPlayerRanking(root.Count+"+", false);
                    
                    

                var item_go = Instantiate(m_ItemPrefab);
                item_go.transform.SetParent(m_transform);
                item_go.transform.localScale = Vector2.one;
                item_go.GetComponent<RankitemSetup>().Username.text = root[i].name;
                item_go.GetComponent<RankitemSetup>().winAmount.text = root[i].amount.ToString();
                item_go.GetComponent<RankitemSetup>().UserRank.text = (i + 1).ToString();
                StartCoroutine(SetImageFromURL(root[i].profile_pic, item_go.GetComponent<RankitemSetup>().dp));
            }

        }
        public static IEnumerator SetImageFromURL(string pictureURL, Image imageView)
        {
            if (pictureURL.Length > 0)
            {
                WWW www = new WWW(pictureURL);
                yield return www;
                Texture2D ui_texture = www.texture;
                if (ui_texture != null) {
                    Sprite sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
                    if (sprite != null)
                    {
                        Debug.Log("ProfilePicUrlSet");
                        imageView.overrideSprite = sprite;
                    }
                }
            }
        }
        
        

        void SetPlayerRanking(string rank, bool isWeekly)
        {
            print(isWeekly +" weekly getting rank - " + rank + "--- "+winData.userInfo.name);
            selfName.text = winData.userInfo.name;
            //if (rank > 0)
                selfRank.text = rank.ToString();
            //else
                //selfRank.text = (rank+1)+"+";
                
            if (isWeekly)
                selfAmount.text = winData.weeklyAmount.ToString();
            else
                selfAmount.text = Titli_RoundWinningHandler.Instance.TodayWinText.text;
    

            StartCoroutine(SetImageFromURL(winData.userInfo.profile_pic, selfDP));
        }
        public Text thisweektimer, thisWeekPrize;
        public bool isTimerSet;
        void SetupLastWinner()
        {
            LastWinnername.text = winData.lastWinner.name;
            lastwinnerprize.text = winData.lastWinner.prize.ToString();
            lastwinneramount.text = winData.lastWinner.amount.ToString();
            StartCoroutine(SetImageFromURL(winData.lastWinner.profile_pic, lastWinerprofile_pic));
            thisWeekPrize.text = "Prize : "+winData.prize.ToString();
            if (!isTimerSet)
                StartCoroutine(Countdown((int)(winData.time/1000)));
            else
                print("timer is already set "+ isTimerSet);
        }
        private IEnumerator Countdown(int totalSeconds)
        {
         
            isTimerSet = true;
            while (totalSeconds > 0) {
                //TimeSpan t = TimeSpan.FromSeconds(totalSeconds);
                int d = (int)totalSeconds/(60*60*24);
                int h = (int)(totalSeconds % (60*60*24)) / (60*60);
                int m = (int)(totalSeconds % (60*60)) / (60);
                int s = (int)(totalSeconds % (60 ));
                string answer = string.Format("{0:D2}d:{1:D2}h:{2:D2}m:{3:D2}s",
                     d,
                     h,
                     m,
                     s);
                thisweektimer.text = answer;
               // print("totalSeconds - " + totalSeconds);
                yield return new WaitForSeconds(1);
                totalSeconds--;
            }
            isTimerSet = false;
            
        }
    }

    [Serializable]
    public class DailyTopusers
    {
        public int amount;
        public string userId;
        public string profile_pic;
        public string name;
        public string uId;

    }

    [Serializable]
    public class LastWinner
    {
        public int prize;
        public int amount;
        public string userId;
        public string profile_pic;
        public string name;
        public string uId;
    }

    [Serializable]
    public class RootDailyUsers
    {
        public List<WeeklyTopUsers> weekly;
        public List<DailyTopusers> daily;
        public UserInfo userInfo;
        public LastWinner lastWinner;
        public int weeklyAmount;
        public int prize;
        public double currentTime;
        public double endTime;
        public double time;
    }   

    [Serializable]
    public class UserInfo
    {
        public string userId;
        public string profile_pic;
        public string name;
        public string uId;
    }

    [Serializable]
    public class WeeklyTopUsers
    {
        public int amount;
        public string userId;
        public string profile_pic;
        public string name;
        public string uId;
    }
    public class bet_data
    {
        public string ids;
    }
}