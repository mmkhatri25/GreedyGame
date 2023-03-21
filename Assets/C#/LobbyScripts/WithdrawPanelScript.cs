using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Com.BigWin.WebUtils;

public class WithdrawPanelScript : MonoBehaviour
{
    public static WithdrawPanelScript Instance;
    public InputField _upiActualName, _upiAddress, _upiConfirmAddress;
    public InputField _bankActualName, _bankIFSCCode, _bankAccountNumber;
    public Text _userName, _userId;
    public Text _userName_bankAcc, IFSCcode, AccNumber;
    public InputField _withdrawalAmt;
    public GameObject SuccessWithdrawal_Panel;
    public GameObject PaymentMethodPanel, UPIPanel, BankAccountPanel, BankAccountDetail;
    public GameObject NewUPI_OriginalPos, NewBankAcc_OriginalPos, NewUPI_NewPos, NewBankAcc_NewPos;
    public Text UserNameUPI_withdraw,UserIdUPI_withdraw, Usernamebank_withdraw, 
    IFSCcode_withdraw, AccNumber_withdraw;
    string WithdrawAPI = "http://216.48.182.176:5000/user/withdrawRequest";
    string POSTUPI = "http://216.48.182.176:5000/user/UPI";
    string POSTBank = "http://216.48.182.176:5000/user/Bank";
    string FetchUPIBankAPI = "http://216.48.182.176:5000/user/fetchUPIBank";
    string FetchBankAPI = "http://216.48.182.176:5000/user/getfetchBank";

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // FetchUPIBank();
    }

    public void UPIbtn()
    {
        if( !String.IsNullOrEmpty(_upiActualName.text) || !String.IsNullOrEmpty(_upiAddress.text) || !String.IsNullOrEmpty(_upiConfirmAddress.text) )
        {
            Debug.Log("UPI btn   " + _upiActualName.text + "    " + _upiConfirmAddress.text);
            StartCoroutine(WebRequestHandler.instance.POSTUPIAPI(POSTUPI, _upiActualName.text, _upiConfirmAddress.text, PlayerPrefs.GetString("email")));
        }
    }

    public void UPISavebtn()
    {
        PlayerPrefs.SetString("_upiActualName", _upiActualName.text);
        PlayerPrefs.SetString("_upiAddress", _upiAddress.text);
        PlayerPrefs.SetString("_upiConfirmAddress", _upiConfirmAddress.text);
        UPIPanel.SetActive(false);
        PaymentMethodPanel.SetActive(true);
        _userName.text = PlayerPrefs.GetString("_upiActualName");
        _userId.text = PlayerPrefs.GetString("_upiConfirmAddress");

        UserNameUPI_withdraw.text = PlayerPrefs.GetString("_upiActualName");
        UserIdUPI_withdraw.text = PlayerPrefs.GetString("_upiConfirmAddress");

        _upiActualName.text = "";
        _upiAddress.text = "";
        _upiConfirmAddress.text = "";
    }

    public void UPICancelbtn()
    {
        _upiActualName.text = "";
        _upiAddress.text = "";
        _upiConfirmAddress.text = "";
    }

    public void BankBtn()
    {
        if( !String.IsNullOrEmpty(_bankActualName.text) || !String.IsNullOrEmpty(_bankIFSCCode.text) || !String.IsNullOrEmpty(_bankAccountNumber.text) )
        {
            StartCoroutine( WebRequestHandler.instance.POSTBankAPI(POSTBank, _bankActualName.text, _bankIFSCCode.text, _bankAccountNumber.text, PlayerPrefs.GetString("email")));
            // BankSavebtn();
        }
    }

    public void BankSavebtn()
    {
        BankAccountDetail.SetActive(true);
        NewUPI_OriginalPos.SetActive(false);
        NewBankAcc_OriginalPos.SetActive(false);
        NewUPI_NewPos.SetActive(true);
        NewBankAcc_NewPos.SetActive(true);

        PlayerPrefs.SetString("_bankActualName", _bankActualName.text);
        PlayerPrefs.SetString("_bankIFSCCode", _bankIFSCCode.text);
        PlayerPrefs.SetString("_bankAccountNumber", _bankAccountNumber.text);
        _userName_bankAcc.text = PlayerPrefs.GetString("_bankActualName");
        IFSCcode.text = PlayerPrefs.GetString("_bankIFSCCode");
        AccNumber.text = PlayerPrefs.GetString("_bankAccountNumber");

        Usernamebank_withdraw.text = PlayerPrefs.GetString("_bankActualName");
        IFSCcode_withdraw.text = PlayerPrefs.GetString("_bankIFSCCode");
        AccNumber_withdraw.text = PlayerPrefs.GetString("_bankAccountNumber");

        _bankActualName.text = "";
        _bankIFSCCode.text = "";
        _bankAccountNumber.text = "";
    }

    public void BankCancelbtn()
    {
        _bankActualName.text = "";
        _bankIFSCCode.text = "";
        _bankAccountNumber.text = "";
    }

    public void withdrawalBtn()
    {
        if(int.Parse(_withdrawalAmt.text) >= 300)
        {
            if(!String.IsNullOrEmpty(_withdrawalAmt.text))
            {
                StartCoroutine(WebRequestHandler.instance.WithDrawAPI(WithdrawAPI, PlayerPrefs.GetString("email"), _withdrawalAmt.text ));
            }
        }
    }

    public void FetchUPIBank()
    {
        // StartCoroutine(WebRequestHandler.instance.FetchWithdrawDataAPI(FetchUPIBankAPI, PlayerPrefs.GetString("email")));
        StartCoroutine(WebRequestHandler.instance.FetchBankAPI(FetchBankAPI, PlayerPrefs.GetString("email")));
    }

}
