
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Com.BigWin.WebUtils;
using Newtonsoft.Json;
using System;
using System.Net;
using System.IO;

public class DiamondOffer : MonoBehaviour
{
    public static DiamondOffer Instance;
    public GameObject SafePanel;
    public Transform BuyDiamondPrefab;
    public Transform Content;
    public Text BalanceCapital;
    public Text AddGems;
    public Button AddGemsButton;
    public Text OfferMessage;

    
    string DiamondOfferUrl = "https://jeetogame.in/jeeto_game_new/WebServices/DiamondOfferDatails";

    string DiamondAddUrl = "https://jeetogame.in/jeeto_game_new/WebServices/AddDiamond";

    public List<Sprite> DiamondImages = new List<Sprite>();

    float currentTime;
    [SerializeField]
    List<string> ImageLink = new List<string>();
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        OfferMessage.gameObject.SetActive(false);
        BalanceCapital.text = UserDetail.Balance.ToString();
        AddGemsButton.interactable = false;
    }



    void CheckTimeForOffer()
    {
        BalanceCapital.text = UserDetail.Balance.ToString();

        BalanceCapital.text = UserDetail.Balance.ToString();
        OfferFormData formData = new OfferFormData(UserDetail.UserId.ToString(), "8", "123456789");
        WebRequestHandler.instance.Post(DiamondAddUrl, JsonUtility.ToJson(formData), OnDownloadComplete);
    }

    private void OnDownloadComplete(string json, bool success)
    {
        OfferDaimondResponse responseData = JsonUtility.FromJson<OfferDaimondResponse>(json);
        
        if (responseData.response.status)
        {
            OfferMessage.gameObject.SetActive(false);
            Debug.Log("Offer Valid " + responseData.response.message);
            DiamondFormData formData = new DiamondFormData(UserDetail.UserId.ToString());

            WebRequestHandler.instance.Post(DiamondOfferUrl, JsonUtility.ToJson(formData), OnDiamondRequestProcessed);
        }
        else
        {

            OfferMessage.gameObject.SetActive(true);
            OfferMessage.text = responseData.response.message;
            Debug.LogError("Offer Not Valid " + responseData.response.message);
        }
    }

    public void AddDiamonds()
    {
        CheckTimeForOffer();
    }
    private void OnDiamondRequestProcessed(string json, bool success)
    {
        
        OnDiamondOfferResponse responseData = JsonUtility.FromJson<OnDiamondOfferResponse>(json);
        
        print("responseData.response.status "+responseData.response.status);
        if (responseData.response.status)
        {
            for(int i = 0; i < responseData.response.data.Count;i++)
            {
                Transform diamondPrefab = Instantiate(BuyDiamondPrefab);
                diamondPrefab.transform.SetParent(Content.transform, false);
                diamondPrefab.GetComponent<BuyDiamond>().
                    SetBuyDetails(responseData.response.data[i].buy_amt,
                    responseData.response.data[i].qty, DiamondImages[i]);
                Debug.Log(responseData.response.data[i].image);
                // For Server Side Images
             // diamondPrefab.GetComponent<BuyDiamond>().DownloadOfferUI(responseData.response.data[i].image.ToString());
             // diamondPrefab.GetComponent<BuyDiamond>().DownloadOfferUI(ImageLink[i]);

            }
        
        }
        else
        {
            // AndroidToastMsg.ShowAndroidToastMessage(responseData.response.message);
        }
    }

    

    public void OnAddGemsClick(int val)
    {

        if (UserDetail.Balance > val)
        {
            UserDetail.Balance -= val;
            DiamondOffer.Instance.BalanceCapital.text = UserDetail.Balance.ToString();
            AddGemsButton.interactable = false;
            DiamondOffer.Instance.AddGems.text = "";
        }
        
    }


    public void ShowDiamondUI()
    {

        SafePanel.SetActive(true);
        AddDiamonds();
    }
    public void CloseDiamondUI()
    {
        SafePanel.SetActive(false);
    }

}



[Serializable]
public class DiamondFormData
{
    public string user_id;

    public DiamondFormData(string user_id)
    {
        this.user_id = user_id;
    }
}
[Serializable]
public class OnDiamondOfferResponse
{
    public ResponseData response;

}

[Serializable]

public class ResponseData
{

    public bool status;
    public string message;
    public List<Data> data=new List<Data>();

}

[System.Serializable]
public class Data
{
    public int diamond_id;
    public string buy_amt;
    public int qty;
    public string image;
}

[Serializable]
public class OfferData
{
}
[Serializable]
public class OfferResponse
{
    public bool status;
    public string message;
    public OfferData data;
}
[Serializable]
public class OfferDaimondResponse
{
    public OfferResponse response;
}

