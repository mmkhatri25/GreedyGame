using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Titli.Utility;
using Titli.ServerStuff;
using System;
using UnityEngine.Networking;
using Mosframe;
//namespace Mosframe;
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
        public DynamicScrollView WeeklyDynamicView, DailyDynamicView;
        //public scroll
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
        public GameObject Loadingbg;
        public static LeaderBoardScreen instance;
        private void Awake()
        {
            instance = this;
        }

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
            //print("winData lists  - " + mystr);
            
            JSONObject abc = obj;
            winData = JsonUtility.FromJson<RootDailyUsers>(mystr);
            //ShowDailyList();
            //ShowDailyList();
            SetupLastWinner();

        }
        public void ShowWeeklyList()
        {
            // show loader
            Loadingbg.SetActive(true);
            print("ShowWeeklyList");

            Playernewdata player = new Playernewdata() { userId = PlayerPrefs.GetString("userId") };
            Titli_ServerRequest.instance.socket.Emit(Events.winnerList, new JSONObject(Newtonsoft.Json.JsonConvert.SerializeObject(player)), HandleAction);
           
            PopulateRankItems(m_ContentContainer, winData.weekly);
        }
        public void ShowDailyList()
        {
            // show loader
            print("ShowDailyList");
            Loadingbg.SetActive(true);

            Playernewdata player = new Playernewdata() { userId = PlayerPrefs.GetString("userId") };
            Titli_ServerRequest.instance.socket.Emit(Events.winnerList, new JSONObject(Newtonsoft.Json.JsonConvert.SerializeObject(player)), HandleAction);
           
            DailyPopulateRankItems(m_ContentContainerDaily, winData.daily);
        }


        //void PopulateRankItems(Transform m_transform, List<WeeklyTopUsers> root)
        //{
        //    print("PopulateRankItems");

        //    Loadingbg.SetActive(true);
        //    foreach (Transform child in m_transform) { Destroy(child.gameObject); }
        //     if (root.Count<=0)
        //      SetPlayerRanking((0).ToString(), true);
        //     else
        //      SetPlayerRanking((root.Count+1).ToString(), true);


        //     for (int i = 0; i < root.Count; i++)
        //    {
        //        var item_go = Instantiate(m_ItemPrefab);
        //        if (PlayerPrefs.GetString("userId") == root[i].userId)
        //        {
        //            SetPlayerRanking((i + 1).ToString(), true);
        //            print("exists... " + root[i].userId);
        //        }
        //            // else
        //            //SetPlayerRanking(root.Count+"+", true);
        //        item_go.transform.SetParent(m_transform);
        //        item_go.transform.localScale = Vector2.one;
        //        item_go.GetComponent<RankitemSetup>().Username.text = root[i].name;
        //        item_go.GetComponent<RankitemSetup>().winAmount.text = root[i].amount.ToString();
        //        item_go.GetComponent<RankitemSetup>().UserRank.text = (i + 1).ToString();
        //        //Loadingbg.SetActive(false);
        //        //StartCoroutine(SetImageFromURL(root[i].profile_pic, item_go.GetComponent<RankitemSetup>().dp));
        //    }
        //}
        public List<WeeklyTopUsers> weeklyUsers;
        public ScrollRect scrollRect, scrollRectDaily;

        void PopulateRankItems(Transform m_transform, List<WeeklyTopUsers> root)
        {

            foreach (Transform child in m_transform) { Destroy(child.gameObject); }
            weeklyUsers.Clear();

            isWeekly = true;

            Loadingbg.SetActive(true);
            WeeklyDynamicView.gameObject.SetActive(true);
            DailyDynamicView.gameObject.SetActive(false);
            weeklyUsers = root;
            WeeklyDynamicView.totalItemCount = root.Count;
            print("weekly PopulateRankItems");
            //WeeklyDynamicView.refresh();

            scrollRect.verticalNormalizedPosition = 1f;

            if (root.Count <= 0)
            {
                SetPlayerRanking("0", true);
                Loadingbg.SetActive(false); // Deactivate loading background as there are no records to load
            }
            else
            {
                SetPlayerRanking((root.Count + 1).ToString(), true);

                int loadedCount = 0; // Counter to track loaded images

                for (int i = 0; i < root.Count; i++)
                {
                    //var item_go = Instantiate(m_ItemPrefab);
                    if (PlayerPrefs.GetString("userId") == root[i].userId)
                    {
                        SetPlayerRanking((i + 1).ToString(), true);
                        //print("exists... " + root[i].userId);
                    }

                    //item_go.transform.SetParent(m_transform);
                    //item_go.transform.localScale = Vector2.one;
                    //item_go.GetComponent<RankitemSetup>().Username.text = root[i].name;
                    //item_go.GetComponent<RankitemSetup>().winAmount.text = root[i].amount.ToString();
                    //item_go.GetComponent<RankitemSetup>().UserRank.text = (i + 1).ToString();
                    //loadedCount++;
                    //if (loadedCount == root.Count)
                    //{
                    //    // Deactivate loading background once all images are loaded
                    //    StartCoroutine(waitToCloseLoading());
                    //}
                    //SetImageFromURL(root[i].profile_pic, item_go.GetComponent<RankitemSetup>().dp, () =>
                    //{
                    //    loadedCount++; // Increment the loaded count when an image is loaded
                    //                   // Check if all images have been loaded
                    //    if (loadedCount == root.Count)
                    //    {
                    //        // Deactivate loading background once all images are loaded
                    //        Loadingbg.SetActive(false);
                    //    }
                    //});
                    Loadingbg.SetActive(false);
                }
            }
        }
        IEnumerator waitToCloseLoading()
        {
            yield return new WaitForSeconds(2f);
            Loadingbg.SetActive(false);

        }

        public List<DailyTopusers> dailyUsers;
        public bool isWeekly;
        void DailyPopulateRankItems(Transform m_transform, List<DailyTopusers> root)
        {
            //DailyDynamicView.scrollByItemIndex(0);

            foreach (Transform child in m_transform) { Destroy(child.gameObject); }
            dailyUsers.Clear();
            isWeekly = false;
            Loadingbg.SetActive(true);

            WeeklyDynamicView.gameObject.SetActive(false);
            DailyDynamicView.gameObject.SetActive(true);
            dailyUsers = root;
            DailyDynamicView.totalItemCount = root.Count;
            print("daily PopulateRankItems");
            scrollRectDaily.verticalNormalizedPosition = 1f;
            //WeeklyDynamicView.refresh();

            //foreach (Transform child in m_transform) { Destroy(child.gameObject); }
            //int loadedCount = 0; // Counter to track loaded images

            if (root.Count <= 0)
            {
                SetPlayerRanking("0", false);
                Loadingbg.SetActive(false); // No records to load, deactivate loading background
            }
            else
            {
                SetPlayerRanking((root.Count + 1).ToString(), false);

                for (int i = 0; i < root.Count; i++)
                {
                    if (PlayerPrefs.GetString("userId") == root[i].userId)
                        SetPlayerRanking((i + 1).ToString(), false);

                    //var item_go = Instantiate(m_ItemPrefab);
                    //item_go.transform.SetParent(m_transform);
                    //item_go.transform.localScale = Vector2.one;
                    //item_go.GetComponent<RankitemSetup>().Username.text = root[i].name;
                    //item_go.GetComponent<RankitemSetup>().winAmount.text = root[i].amount.ToString();
                    //item_go.GetComponent<RankitemSetup>().UserRank.text = (i + 1).ToString();
                    //loadedCount++;
                    //if (loadedCount == root.Count)
                    //{
                    //    // Deactivate loading background once all images are loaded
                    //    //Loadingbg.SetActive(false);
                    //    StartCoroutine(waitToCloseLoading());

                    //}
                    // Load image asynchronously
                    //SetImageFromURL(root[i].profile_pic, item_go.GetComponent<RankitemSetup>().dp, () =>
                    //{
                    //    loadedCount++; // Increment the loaded count when an image is loaded
                    //                   // Check if all images have been loaded
                    //    if (loadedCount == root.Count)
                    //    {
                    //        // Deactivate loading background once all images are loaded
                    //        Loadingbg.SetActive(false);
                    //    }
                    //});
                }
                Loadingbg.SetActive(false);

            }
        }
        public void SetImageFromURL(string pictureURL, Image imageView, Action onComplete = null)
        {
            if (!string.IsNullOrEmpty(pictureURL))
            {
                StartCoroutine(DownloadImage(pictureURL, imageView, onComplete));
            }
        }

        private IEnumerator DownloadImage(string pictureURL, Image imageView, Action onComplete)
        {
            using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(pictureURL))
            {
                yield return www.SendWebRequest();

                if (www.result == UnityWebRequest.Result.Success)
                {
                    Texture2D ui_texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
                    Sprite sprite = Sprite.Create(ui_texture, new Rect(0, 0, ui_texture.width, ui_texture.height), new Vector2(0.5f, 0.5f));

                    imageView.sprite = sprite;
                }
                else
                {
                    Debug.Log("Failed to download image: " + www.error);
                }
            }

            onComplete?.Invoke(); // Invoke the onComplete action if provided
        }

        //public IEnumerator SetImageFromURL(string pictureURL, Image imageView, Action onComplete = null)
        //{
        //    if (!string.IsNullOrEmpty(pictureURL))
        //    {
        //        using (WWW www = new WWW(pictureURL))
        //        {
        //            yield return www;

        //            Texture2D ui_texture = www.texture;
        //            if (ui_texture != null)
        //            {
        //                Sprite sprite = Sprite.Create(ui_texture, new Rect(0, 0, ui_texture.width, ui_texture.height), new Vector2(0, 0));
        //                if (sprite != null)
        //                {
        //                    //Debug.Log("ProfilePicUrlSet");
        //                    imageView.overrideSprite = sprite;
        //                }
        //            }
        //        }
        //    }

        //    onComplete?.Invoke(); // Invoke the onComplete action if provided
        //}

        //void DailyPopulateRankItems(Transform m_transform, List<DailyTopusers> root)
        //{
        //    foreach (Transform child in m_transform) { Destroy(child.gameObject); }
        //    if (root.Count<=0)
        //      SetPlayerRanking((0).ToString(), false);
        //    else
        //      SetPlayerRanking((root.Count+1).ToString(), false);

        //    for (int i = 0; i < root.Count; i++)
        //    {
        //        if (PlayerPrefs.GetString("userId") == root[i].userId)
        //            SetPlayerRanking((i + 1).ToString(), false);
        //            //else
        //            //SetPlayerRanking(root.Count+"+", false);



        //        var item_go = Instantiate(m_ItemPrefab);
        //        item_go.transform.SetParent(m_transform);
        //        item_go.transform.localScale = Vector2.one;
        //        item_go.GetComponent<RankitemSetup>().Username.text = root[i].name;
        //        item_go.GetComponent<RankitemSetup>().winAmount.text = root[i].amount.ToString();
        //        item_go.GetComponent<RankitemSetup>().UserRank.text = (i + 1).ToString();
        //        //StartCoroutine(SetImageFromURL(root[i].profile_pic, item_go.GetComponent<RankitemSetup>().dp));
        //       // Loadingbg.SetActive(false);
        //    }

        //}
        //public IEnumerator SetImageFromURL(string pictureURL, Image imageView)
        //{
        //    if (pictureURL.Length > 0)
        //    {
        //        WWW www = new WWW(pictureURL);
        //        yield return www;
        //        Texture2D ui_texture = www.texture;
        //        if (ui_texture != null) {
        //            Sprite sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
        //            if (sprite != null)
        //            {
        //                Debug.Log("ProfilePicUrlSet");
        //                imageView.overrideSprite = sprite;

        //                StartCoroutine(closeLoading());
        //            }
        //        }
        //    }
        //    // hide loader

        //}

        //IEnumerator closeLoading()
        //{
        //    yield return new WaitForSeconds(2f);
        //    Loadingbg.SetActive(false);
        //}


        void SetPlayerRanking(string rank, bool isWeekly)
        {
            //print(isWeekly +" weekly getting rank - " + rank + "--- "+winData.userInfo.name);
            selfName.text = winData.userInfo.name;
            //if (rank > 0)
                selfRank.text = rank.ToString();
            //else
                //selfRank.text = (rank+1)+"+";
                
            if (isWeekly)
                selfAmount.text = winData.weeklyAmount.ToString();
            else
                selfAmount.text = Titli_RoundWinningHandler.Instance.TodayWinText.text;

            SetImageFromURL(winData.userInfo.profile_pic, selfDP);
            //StartCoroutine(SetImageFromURL(winData.userInfo.profile_pic, selfDP));
        }
        public Text thisweektimer, thisWeekPrize;
        public bool isTimerSet;
        void SetupLastWinner()
        {
            if(LastWinnername.gameObject!=null)
                LastWinnername.text = winData.lastWinner.name;
            if (lastwinnerprize.gameObject != null)
                lastwinnerprize.text = winData.lastWinner.prize.ToString();
            if (lastwinneramount.gameObject != null)
                lastwinneramount.text = winData.lastWinner.amount.ToString();

            //StartCoroutine(SetImageFromURL(winData.lastWinner.profile_pic, lastWinerprofile_pic));
            SetImageFromURL(winData.userInfo.profile_pic, selfDP);

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