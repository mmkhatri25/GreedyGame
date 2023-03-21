using Com.BigWin.WebUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.Networking;

public class ShopScript : MonoBehaviour
{
    public static ShopScript Instance;
    public GameObject ShopPanel;
    public Text ChipTxt;
    string Chips;
    public Text TotalChips;
    public GameObject[] CardSelectionImage;
    public Image EBipc;
    public Image EBrb;
    public Sprite active;
    public Sprite deactive;
    string ShopURL = "https://jeetogame.in/jeeto_game/WebServices/AddChips";
    public Action OnFinishCallback;
    //public UniWebView uniWeb;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        //string URL = "http://3.109.48.186/Backend_code/rummy/Home/addMoney?";
        //string order_id = UserDetail.ID + OrderIdRandomNumber(10000, 99999).ToString();
        //string url = URL + "user_id=1" /*+ UserDetail.UserId*/ + "&" + "amount=" + Chips + "&" + "order_id=" + order_id;
        //OnFinishCallback = Payment;
        //Url = url;
        //StartCoroutine(payStart());
    }
    public void ShowShopUI()
    {
        CardSelectionBtn(0);
        TotalChips.text = UserDetail.Balance.ToString();
        ShopPanel.SetActive(true);
        Chips = "10";
        ChipTxt.text = "Chips : " + Chips;
    }
    public void CloseShopUI()
    {
        ShopForm form = new ShopForm(UserDetail.UserId.ToString(), Chips);
        Debug.Log("Playment Success...");
        WebRequestHandler.instance.Post(ShopURL, JsonUtility.ToJson(form), OnShopRequestProcessed);
        ShopPanel.SetActive(false);
    }
    public void SelectChip(Text Value)
    {
        Chips = Value.text;
        ChipTxt.text = "Chips : " + Chips;
    }
    public void CardSelectionBtn(int ind)
    {
        for (int i = 0; i < CardSelectionImage.Length; i++)
        {
            CardSelectionImage[i].SetActive(false);
        }
        CardSelectionImage[ind].SetActive(true);
    }
    public void EBipcbtn()
    {
        EBipc.sprite = active;
        EBrb.sprite = deactive;
    }
    public void EBrbbtn()
    {
        EBrb.sprite = active;
        EBipc.sprite = deactive;
    }
    public void AddChips()
    {

        //string URL = " https://jeetogame.in/jeeto_game/Home/addMoney?user_id=1";
        string URL = " https://jeetogame.in/jeeto_game/Home/addMoney?user_id=";
        string order_id = UserDetail.ID + OrderIdRandomNumber(10000, 99999).ToString();
        string url = URL + UserDetail.UserId + "&amount=" + Chips + "&order_id=" + order_id;


         if (!Application.isEditor)
         {
           // Application.OpenURL(url);
             OnFinishCallback = Payment;
             Debug.Log("0");
             //if (uniWeb!=null)
             //{
             Debug.Log("1");
             var webViewGameObject = new GameObject("UniWebView");
             UniWebView webView = webViewGameObject.AddComponent<UniWebView>();
             webView.Frame = new Rect(0, 0, Screen.width, Screen.height);
             webView.Load(url);
             webView.Show();
             //new SampleWebView(url, Payment);
             //}

         }
         else
         {
             Application.OpenURL(url);
         }
        //ShopForm form = new ShopForm(UserDetail.UserId.ToString(), Chips);

        //WebRequestHandler.instance.Post(ShopURL, JsonUtility.ToJson(form), OnShopRequestProcessed);

    }
    private void Payment()
    {
        //if (couponData == null)
        //{
       
        //}
        //else
        //{
        //    Debug.LogError("OrderID: " + couponData.order_id + "\nAmount" + amount);
        //    UpdateTransactionForm form = new UpdateTransactionForm(loginData.user_id, int.Parse(couponData.order_id), int.Parse(amount), 22363060, couponData.coupon_code_id, couponData.discount_amount);
        //    Debug.LogError("Payment update\n" + JsonUtility.ToJson(form));
        //    webRequestHandler.Post(UPDATE_TRANSACTION, JsonUtility.ToJson(form), (message, status) =>
        //    {
        //        Debug.LogError("Transaction update response");
        //        Debug.LogError(message);
        //        RefreshMoney();
        //    });
        //}
    }

    private void OnShopRequestProcessed(string json, bool success)
    {
        ShopRes responce = JsonUtility.FromJson<ShopRes>(json);
        if (responce.response.status)
        {
            UserDetail.Balance = responce.response.data.total_chip_balance;
        }
        TotalChips.text = UserDetail.Balance.ToString();
        HomeScript.Instance.ShowHomeUI();
    }
    private readonly System.Random _random = new System.Random();
    public int OrderIdRandomNumber(int min, int max)
    {
        return _random.Next(min, max);
    }
}
[Serializable]
public class ShopForm
{
    public string user_id;
    public string chip_amount;
    public ShopForm(string user_id, string chip_amount)
    {
        this.user_id = user_id;
        this.chip_amount = chip_amount;
    }
}
[Serializable]
public class ShopData
{
    public int user_id;
    public int total_chip_balance;
}
[Serializable]
public class ShopResponse
{
    public bool status;
    public string message;
    public ShopData data;
}
[Serializable]
public class ShopRes
{
    public ShopResponse response;
}