using Com.BigWin.WebUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankScript : MonoBehaviour
{
    public static RankScript Instance;
    public GameObject RankPanel;
    string RankURL = "https://jeetogame.in/jeeto_game/WebServices/myRank";
    public Sprite BG;
    public Sprite BG1;
    public Sprite BG2;
    public Sprite BG3;
    public Sprite Crown1;
    public Sprite Crown2;
    public Sprite Crown3;
    public GameObject PlayerPrefab;
    public Transform Basement;

    private void Awake()
    {
        Instance = this;
    }
    public void ShowRankUI()
    {
        RankForm form = new RankForm(UserDetail.UserId.ToString());
        WebRequestHandler.instance.Post(RankURL, JsonUtility.ToJson(form), OnRankRequestProcessed);
        RankPanel.SetActive(true);
    }
    private void OnRankRequestProcessed(string json, bool success)
    {
        RankDataResponce responce = JsonUtility.FromJson<RankDataResponce>(json);
        if (responce.response.status)
        {
            for (int i = 0; i < responce.response.data.Count; i++)
            {
                GameObject gm = Instantiate(PlayerPrefab,Basement);
                gm.transform.GetChild(1).gameObject.SetActive(false);
                gm.GetComponent<Image>().sprite = BG;
                if (responce.response.data[i].rank == 1)
                {
                    gm.GetComponent<Image>().sprite = BG1;
                    //gm.transform.GetChild(0).GetComponent<Image>().sprite = Crown1;//profile
                    gm.transform.GetChild(1).GetComponent<Image>().sprite = Crown1;
                    gm.transform.GetChild(1).gameObject.SetActive(true);
                }
                else if (responce.response.data[i].rank == 2)
                {
                    gm.GetComponent<Image>().sprite = BG2;
                    //gm.transform.GetChild(0).GetComponent<Image>().sprite = Crown1;//profile
                    gm.transform.GetChild(1).GetComponent<Image>().sprite = Crown2;
                    gm.transform.GetChild(1).gameObject.SetActive(true);
                }
                else if (responce.response.data[i].rank == 3)
                {
                    gm.GetComponent<Image>().sprite = BG3;
                    //gm.transform.GetChild(0).GetComponent<Image>().sprite = Crown1;//profile
                    gm.transform.GetChild(1).GetComponent<Image>().sprite = Crown3;
                    gm.transform.GetChild(1).gameObject.SetActive(true);
                }
                gm.transform.GetChild(2).GetComponent<Text>().text = responce.response.data[i].username;
                gm.transform.GetChild(3).GetChild(0).GetComponent<Text>().text = responce.response.data[i].chip_balance;
            }
            //dialogue.Show(responce.message);
        }
    }
    public void CloseRankUI()
    {
        RankPanel.SetActive(false);
    }
}
[Serializable]
public class RankForm
{
    public string user_id;

    public RankForm(string user_id)
    {
        this.user_id = user_id;
    }
}
[Serializable]
public class RankData
{
    public int rank;
    public string username;
    public string chip_balance;

}
[Serializable]
public class RankResponse
{
    public bool status;
    public string message;
    public List<RankData> data;

}
[Serializable]
public class RankDataResponce
{
    public RankResponse response;

}