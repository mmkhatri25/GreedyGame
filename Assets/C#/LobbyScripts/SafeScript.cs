using Com.BigWin.WebUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SafeScript : MonoBehaviour
{
    public static SafeScript Instance;
    public GameObject SafePanel;
    string SafeURL = "https://jeetogame.in/jeeto_game/WebServices/safeLocker";
    public GameObject Arrow;
    public GameObject DepositBG;
    public GameObject TakeoutBG;
    public Text Currentchip;
    public Text Safechip;
    public InputField depositechip;
    private void Awake()
    {
        Instance = this;
    }
    public void ShowSafeUI()
    {
        depositechip.text = "";       
        Safechip.text = UserDetail.Safechip.ToString();
        Currentchip.text = UserDetail.Balance.ToString();
        SafePanel.SetActive(true);
        SafeForm form = new SafeForm(UserDetail.UserId.ToString(), "DEPOSITE", "0");
        WebRequestHandler.instance.Post(SafeURL, JsonUtility.ToJson(form), OnRaferRequestProcessed);
    }
    public void CloseSafeUI()
    {
        SafePanel.SetActive(false);
    }
    public void DepositeBtn()
    {
        depositechip.text = "";
        Arrow.transform.eulerAngles = new Vector3(0, 0, 0);
        DepositBG.SetActive(true);
        TakeoutBG.SetActive(false);

    }
    public void TakeBtn()
    {
        depositechip.text = "";
        Arrow.transform.eulerAngles = new Vector3(0, 0, 180);
        DepositBG.SetActive(false);
        TakeoutBG.SetActive(true);

    }
    public void ResetBtn()
    {
        depositechip.text = "";
    }
    public void YesBtn()
    {
        if (depositechip.text != "")
        {
            if (Arrow.transform.eulerAngles.z == 0f)
            {
                if (UserDetail.Balance >= Convert.ToInt32(depositechip.text))
                {
                    SafeForm form = new SafeForm(UserDetail.UserId.ToString(), "DEPOSITE", depositechip.text);
                    WebRequestHandler.instance.Post(SafeURL, JsonUtility.ToJson(form), OnRaferRequestProcessed);
                }                
            }
            else
            {
                if (UserDetail.Safechip >= Convert.ToInt32(depositechip.text))
                {
                    SafeForm form = new SafeForm(UserDetail.UserId.ToString(), "TAKE", depositechip.text);
                    WebRequestHandler.instance.Post(SafeURL, JsonUtility.ToJson(form), OnRaferRequestProcessed);
                }                    
            }
        }

    }
    private void OnRaferRequestProcessed(string json, bool success)
    {
        SafeRoot responce = JsonUtility.FromJson<SafeRoot>(json);
        if (responce.response.status)
        {
            UserDetail.Safechip = responce.response.data.safe_balance;
            UserDetail.Balance = responce.response.data.chip_balance;
            HomeScript.Instance.Balance.text = UserDetail.Balance.ToString();
            depositechip.text = "";
            Safechip.text = UserDetail.Safechip.ToString();
            Currentchip.text = UserDetail.Balance.ToString();
           
        }
    }
}
[Serializable]
public class SafeForm
{
    public string user_id;
    public string action;
    public string amount;

    public SafeForm(string user_id, string action, string amount)
    {
        this.user_id = user_id;
        this.action = action;
        this.amount = amount;
    }
}
[Serializable]
public class SafeData
{
    public int chip_balance;
    public int safe_balance;
}
[Serializable]
public class SafeResponse
{
    public bool status;
    public string message;
    public SafeData data;
}
[Serializable]
public class SafeRoot
{
    public SafeResponse response;
}
