using Com.BigWin.WebUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForgetPasswordScript : MonoBehaviour
{
    public static ForgetPasswordScript Instance;
    public GameObject ForgetPasswordPanel;
    public InputField EmailId;
    public InputField Password;
    public InputField ReEnterPassword;
    public InputField OTP;
    // string ForgetPasswordURL = "http://3.109.48.186/Backend_code/rummy/WebServices/forgotPassword";
    public static string OTPURL = "http://216.48.182.176:5000/auth/forgotPassword";
    // public static string ResetPwdURL = "http://216.48.182.176:5000/auth/resetPassword";
    public static string ResetPwdURL = "http://216.48.182.176:5000/auth/verifyForgetOTP";
    private void Awake()
    {
        Instance = this;
    }
    public void ShowForgetPasswordUI()
    {
        ForgetPasswordPanel.SetActive(true);
        
    }
    public void CloseForgetPasswordUI()
    {
        ForgetPasswordPanel.SetActive(false);
    }
    // public void OTPVerifyBtn()
    // {
    //     if (!String.IsNullOrEmpty(EmailId.text))
    //     {
    //         StartCoroutine(WebRequestHandler.instance.GetOTPAPI(OTPURL, EmailId.text));
    //     }
    // }
    
    public void SendOTPBtn()
    {
        if (!String.IsNullOrEmpty(EmailId.text))
        {
            StartCoroutine(WebRequestHandler.instance.GetOTPAPI(OTPURL, EmailId.text));
        }
    }
    // private void OnOtpVerifyRequestProcessed(string json, bool success)
    // {
    //     LoginFormRoot responce = JsonUtility.FromJson<LoginFormRoot>(json);
    //     Debug.Log(responce.response.message);
    //     AndroidToastMsg.ShowAndroidToastMessage(responce.response.message);       
    // }
    public void OkBtn()
    {
        // if (!String.IsNullOrEmpty(EmailId.text) &&  !String.IsNullOrEmpty(OTP.text))
        // {
        //     if (Password.text == ReEnterPassword.text)
        //     {
        //         StartCoroutine(WebRequestHandler.instance.ResetPasswordAPI(ResetPwdURL, EmailId.text, Password.text, ReEnterPassword.text));
        //     }
        // }
        
        if (!String.IsNullOrEmpty(EmailId.text) &&  !String.IsNullOrEmpty(OTP.text))
        {
                StartCoroutine(WebRequestHandler.instance.ResetPasswordAPI(ResetPwdURL, EmailId.text, OTP.text, ReEnterPassword.text));
            
        }
    }
    // private void OnForgetPasswordRequestProcessed(string json, bool success)
    // {
    //     LoginFormRoot responce = JsonUtility.FromJson<LoginFormRoot>(json);
    //     Debug.Log(responce.response.message);
    //     AndroidToastMsg.ShowAndroidToastMessage(responce.response.message);
    // }
}
[Serializable]
public class ForgetForm
{
    public string mobile_number;
    public string password;
    public string language;
    public string otp;

    public ForgetForm(string mobile_number, string password, string otp, string language)
    {
        this.mobile_number = mobile_number;
        this.password = password;
        this.language = language;
        this.otp = otp;
    }
}