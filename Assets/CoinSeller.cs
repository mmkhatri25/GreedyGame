using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Net;
using UnityEngine.Networking;
using System;
namespace Titli.UI
{
    public class CoinSeller : MonoBehaviour
    {
        public TMP_Text Name, MobileNo;
        public Button redirectcall, redirectMessage;
        [SerializeField]
        private Image Profile;
        string mobile;

        // Start is called before the first frame update
        void Start()
        {


        }

        public void setDetails(string name, string mobileno)
        {
            Name.text = name;
            MobileNo.text = mobileno;
            Debug.Log("Coin selller script " + mobileno + "  Length " + mobileno.Length);
            // int.TryParse( MobileNo.text, out mobile);
            mobile = mobileno;
            // mobile = Convert.ToInt16(mobileno);
            Debug.Log(mobile);
            // mobile = Convert (mobileno);

        }

        public void LoadProfileImage(string url)
        {
            StartCoroutine(loadProfileImageCour(url));
            // using (WebClient web = new WebClient )
            // {
            //     byte[] imageByte;
            //     yield return imageByte = web.DownloadData(imageurl);


            // }
        }

        IEnumerator loadProfileImageCour(string imageurl)
        {
            UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(imageurl);
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture2D = ((DownloadHandlerTexture)webRequest.downloadHandler).texture;
                Profile.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
            }
            else if (webRequest.result != UnityWebRequest.Result.Success || webRequest.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Error : " + webRequest.error);
            }
        }

        public void MessageCoinSeller()
        {
            if (Titli_UiHandler.Instance.isBetPlaced)
            { AndroidExit.instance.onExitpopup(); }
            else
            {
             AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject pluginInstance = new AndroidJavaObject("com.yaravoice.www.testlib.CoinSellerClass", unityActivity);

            if (pluginInstance == null)
            {
                Debug.Log("Plugin Error Null");
            }
            // pluginInstance.Call("CoinSellerClass", unityActivity);
            pluginInstance.Call("whatsApp", mobile);
            }
            
        }

        public void callCoinSeller()
        {
            if (Titli_UiHandler.Instance.isBetPlaced)
            { AndroidExit.instance.onExitpopup(); }
            else
            {
                AndroidJavaClass unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject unityActivity = unityClass.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaObject pluginInstance = new AndroidJavaObject("com.yaravoice.www.testlib.CoinSellerClass", unityActivity);

                if (pluginInstance == null)
                {
                    Debug.Log("Plugin Error Null");
                }
                // pluginInstance.Call("CoinSellerClass", unityActivity);
                pluginInstance.Call("Call", mobile);
            }

        }

    }
}