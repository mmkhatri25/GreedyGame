using System;
using System.Collections;
using System.Collections.Generic;
using SocketIO;
using UnityEngine;
using UnityEngine.UI;

public class setTopWinnerBottom : MonoBehaviour
{
    // Start is called before the first frame update
    public static setTopWinnerBottom inst;
    public Text WinAmountWeekly, WinAmountDaily, userNameWeekly, userNameDaily;
    public Image WeeklyDp, DailyDp;

    private void Awake()
    {
        inst = this;
    }

    public void SetwinnerData(SocketIOEvent e)
    {
        Root winData = JsonUtility.FromJson<Root>(e.data.ToString());
        WinAmountWeekly.text = winData.weekly.amount.ToString();
        WinAmountDaily.text = winData.daily.amount.ToString();
        userNameWeekly.text = winData.weekly.name.ToString();
        userNameDaily.text = winData.daily.name.ToString();
        StartCoroutine(SetImageFromURL(winData.weekly.profile_pic, WeeklyDp));
        StartCoroutine(SetImageFromURL(winData.daily.profile_pic, DailyDp));

    }

    public static IEnumerator SetImageFromURL(string pictureURL, Image imageView)
    {
        if (pictureURL.Length > 0)
        {
            WWW www = new WWW(pictureURL);

            yield return www;
            Texture2D ui_texture = www.texture;
            if (ui_texture != null)
            {
                Sprite sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
                if (sprite != null)
                {
                    //Debug.Log("ProfilePicUrlSet");
                    imageView.overrideSprite = sprite;
                }
            }
        }
    }
}
    [Serializable]
    public class Daily
    {
        public int amount;
        public string userId;
        public string profile_pic;
        public string name;
        public string uId;
    }
    [Serializable]
    public class Root
    {
        public Weekly weekly;
        public Daily daily;
    }
    [Serializable]
    public class Weekly
    {
        public int amount;
        public string userId;
        public string profile_pic;
        public string name;
        public string uId;
    }


