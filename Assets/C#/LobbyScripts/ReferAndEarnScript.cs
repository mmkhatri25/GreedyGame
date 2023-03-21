using Com.BigWin.WebUtils;
using KhushbuPlugin;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ReferAndEarnScript : MonoBehaviour
{
    public static ReferAndEarnScript Instance;
    public GameObject ReferAndEarnPanel;
    private bool isProcessing = false;
    public string packageName = "com.whatsapp";
    private bool isFocus = false;
    private void Awake()
    {
        Instance = this;
    }
    public void ShowReferAndEarnUI()
    {        
        ReferAndEarnPanel.SetActive(true);
    }
 
    public void CloseReferAndEarnUI()
    {
        UtilitySound.Instance.ButtonClickSound();
        ReferAndEarnPanel.SetActive(false);
    }
    public void ShareWhatsAppBtn()
    {
        packageName = "com.whatsapp";
        ShareText();
    }
    public void SharefacebookBtn()
    {
        packageName = "com.facebook.katana";
        ShareText();
    }
    public void CopyTextBtn()
    {
        GUIUtility.systemCopyBuffer = "hello";
    }
    private void ShareText()
    {

#if UNITY_ANDROID
        if (!isProcessing)
        {
            Debug.Log("1");
            //check if app installed
            //if (CheckIfAppInstalled())
            //{
                Debug.Log("2");
                StartCoroutine(ShareTextInAnroid());
            //}
            //else
            //{
            //    //fallback plan
            //    //can either disable the whatsapp share button
            //    //or can a normal share trigger
            //}
        }
#else
		Debug.Log("No sharing set up for this platform.");
#endif
    }   
    public IEnumerator ShareTextInAnroid()
    {
        Debug.Log("3");
        var shareSubject = "I challenge you to beat my high score in Fire Block";
        var shareMessage = "I challenge you to beat my high score in Fire Block. " +
                           "Get the Fire Block app from the link below. \nCheers\n\n" +
                           "http://onelink.to/fireblock";

        isProcessing = true;

        if (!Application.isEditor)
        {
            Debug.Log("4");
            //Create intent for action send
            AndroidJavaClass intentClass =
                new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject =
                new AndroidJavaObject("android.content.Intent");
            intentObject.Call<AndroidJavaObject>
            ("setAction", intentClass.GetStatic<string>("ACTION_SEND"));

            //put text and subject extra
            intentObject.Call<AndroidJavaObject>("setType", "text/plain");
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), shareSubject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), shareMessage);

            //set the package to whatsapp package
            intentObject.Call<AndroidJavaObject>("setPackage", packageName);

            //call createChooser method of activity class
            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity =
                unity.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject chooser =
                intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, "Share your high score");
            currentActivity.Call("startActivity", chooser);
        }

        yield return new WaitUntil(() => isFocus);
        isProcessing = false;
    }
    void OnApplicationFocus(bool focus)
    {
        isFocus = focus;
    }
}
