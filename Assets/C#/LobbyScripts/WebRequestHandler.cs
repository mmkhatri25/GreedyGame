using System;
using System.Collections;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;

namespace Com.BigWin.WebUtils
{
    public class WebRequestHandler : MonoBehaviour
    {
        public static WebRequestHandler instance;
        private void Awake()
        {
            instance = this;
        }
        public void Get(string url, Action<string, bool> OnRequestProcessed)
        {
            StartCoroutine(GetRequest(url, OnRequestProcessed));
        }
        private IEnumerator GetRequest(string url, Action<string, bool> OnRequestProcessed)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                print("check internet connection");
                // AndroidToastMsg.ShowAndroidToastMessage("check internet connection");
                yield break;
            }
           
            UnityWebRequest request = UnityWebRequest.Get(url);
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log("web request error in Get method with responce code : " + request.responseCode);
                OnRequestProcessed(request.error, false);
            }
            else
            {
                Debug.Log("sending get web request  : " + url + "got response:" + request.downloadHandler.text);
                OnRequestProcessed(request.downloadHandler.text, true);
            }
            request.Dispose();
        }
        public void Post(string url, string json, Action<string, bool> OnRequestProcessed)
        {
            Debug.Log("URL " + url);
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                print("check internet connection");
                // AndroidToastMsg.ShowAndroidToastMessage("check internet connection");
                return;
            }
            Debug.Log(url + " json request: " + json);
            StartCoroutine(PostRequest(url, json, OnRequestProcessed));
        }

        private IEnumerator PostRequest(string url, string json, Action<string, bool> OnRequestProcessed, int attemps = 2)
        {
            Debug.Log("url>>>>>  " + url);
            Debug.Log("PostRequest " + json);
            var request = new UnityWebRequest(url, "POST");
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();
           
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {

                if (attemps == 0)
                {
                    Debug.Log(">>>>>>>>" + request.error);
                    OnRequestProcessed(request.error, false);
                }
                else
                {
                    Debug.Log(">>>>>PostRequest>>>" );
                    StartCoroutine(PostRequest(url, json, OnRequestProcessed, --attemps));
                }
                    
            }
            else
            {
                Debug.Log(url + " json response: " + request.downloadHandler.text);
                OnRequestProcessed(request.downloadHandler.text, true);
            }
            request.Dispose();
        }

        // public IEnumerator buyCoinAPi()
        // {
            // var urlencoded = new URL();
            // urlencoded.append("page", "1");
            // WWWForm data = new WWWForm();
            // data.AddField("page", "1");
            // using ( UnityWebRequest www = UnityWebRequest.Put("https://api-uat.yaravoice.com/api/v1/coin-seller-lis", data.ToString()) )
            // {
            //     www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

            //     yield return www.SendWebRequest();

            //     if (www.result != UnityWebRequest.Result.Success)
            //     {
            //         Debug.Log(www.error);
            //     }
            //     else
            //     {
            //         Debug.Log("Coin Buy " +www.downloadHandler.text );
            //     }
            // }
        // }

        public IEnumerator RegisterAPI(string registerURL, string username, string c_pwd, string mobileNo , string email)
        {
            WWWForm form = new WWWForm();
            form.AddField("username", username);
            form.AddField("email", email);
            form.AddField("password", c_pwd);
            form.AddField("phone", mobileNo);

            using (UnityWebRequest www = UnityWebRequest.Post(registerURL, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    // RegisterScript.Instance.OTPBtn.interactable = false;
                    Reg_Otp_Response reg = JsonConvert.DeserializeObject<Reg_Otp_Response>(www.downloadHandler.text);
                    if (reg.status == 200)
                    {    
                        RegisterScript.Instance.RegisterPanel.SetActive(false);
                        RegisterScript.Instance.ShowMessageText.text = reg.message.ToString();
                        Debug.Log("res message:" +reg.message.ToString());
                        RegisterScript.Instance.Register_btn.gameObject.SetActive(false);
                        RegisterScript.Instance.Otp_Verify.gameObject.SetActive(true);
                    }
                    else if (reg.status == 401)
                    {   
                        RegisterScript.Instance.ShowMessageText.gameObject.SetActive(true);
                        RegisterScript.Instance.ShowMessageText.text = reg.message.ToString();
                        Debug.Log("res message:" +reg.message.ToString() + " Status :" +reg.status.ToString() );                        
                    }
                    // Debug.Log("registered user... "+ www.error);
                    // RegisterScript.Instance.RegisterPanel.SetActive(false);
                }
            }
        }

        public class Reg_Otp_Response
        {
            public int status;
            public bool register;
            public string message;
        }

        public IEnumerator VerifyOTp(string registerURL, string email, string OTP)
        {
            WWWForm form = new WWWForm();
            form.AddField("email", email);
            form.AddField("otp", OTP);

            using (UnityWebRequest www = UnityWebRequest.Post(registerURL, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Reg_Otp_Response reg = JsonConvert.DeserializeObject<Reg_Otp_Response>(www.downloadHandler.text);
                    if (reg.status == 200)
                    {    

                        Debug.Log("res message:" +reg.message.ToString());
                    }
                    Debug.Log("registered ...");
                    
                }
            }
        }

        public IEnumerator LoginAPI(string registerURL, string email, string password)
        {
            WWWForm form = new WWWForm();
            form.AddField("email", email);
            form.AddField("password", password);

            using (UnityWebRequest www = UnityWebRequest.Post(registerURL, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    LoginFormData result = JsonUtility.FromJson<LoginFormData>(www.downloadHandler.text);
                    Debug.Log("result   " + result.status );
                    if(result.status == 200)
                    {
                        Debug.Log("Login successfully.... " + www.downloadHandler.text);
                        PlayerPrefs.SetString("email", email);
                        PlayerPrefs.SetString("password", password);
                        PlayerPrefs.SetFloat("balance", result.user.balance);
                        PlayerPrefs.SetString("username", result.user.username);
                        PlayerPrefs.Save();
                        LoginScript.Instance.ShowLoginUI();
                        HomeScript.Instance.GuestName.text = result.user.username;
                        HomeScript.Instance.ShowHomeUI();
                        Debug.Log("uname  " + result.user.username + result.user.username + result.user.balance);
                    }
                    else
                    {
                        LoginScript.Instance.LoginError();
                    }
                }
            }
        }

        public IEnumerator GetOTPAPI(string OTPURL, string email)
        {
            WWWForm form = new WWWForm();
            form.AddField("email", email);

            using (UnityWebRequest www = UnityWebRequest.Post(OTPURL, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    LoginFormData result = JsonUtility.FromJson<LoginFormData>(www.downloadHandler.text);
                    Debug.Log("result   " + result.status );
                    if(result.status == 200)
                    {
                        Debug.Log("Login successfully.... " + www.downloadHandler.text);
                    }
                }
            }
        }

        public IEnumerator ResetPasswordAPI(string ResetPwdURL, string email, string otp, string new_password)
        {
            Debug.Log("Namita   ResetPwdURL="+ ResetPwdURL + "email ="+  email + "otp ="+ otp + "new_password="+ new_password);
            WWWForm form = new WWWForm();
            form.AddField("email", email);
            form.AddField("otp", otp);
            form.AddField("new_password", new_password);

            using (UnityWebRequest www = UnityWebRequest.Post(ResetPwdURL, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log("Namita" + www.error);
                }
                else
                {
                    LoginFormData result = JsonUtility.FromJson<LoginFormData>(www.downloadHandler.text);
                    Debug.Log("Namita  result   " + result.status );
                    if(result.status == 200)
                    {
                        Debug.Log("Namita Password changed.... " + www.downloadHandler.text);
                        ForgetPasswordScript.Instance.ForgetPasswordPanel.SetActive(false);
                    }
                }
            }
        }

        public IEnumerator WithDrawAPI(string withdrawApiURL, string email, string amount)
        {
            WWWForm form = new WWWForm();
            form.AddField("email", email);
            form.AddField("amount", amount);

            using (UnityWebRequest www = UnityWebRequest.Post(withdrawApiURL, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    POSTApiClass result = JsonUtility.FromJson<POSTApiClass>(www.downloadHandler.text);
                    Debug.Log("WithDrawAPI successfully.... " + www.downloadHandler.text);
                    if(result.status == 200)
                    {
                        WithdrawPanelScript.Instance.SuccessWithdrawal_Panel.SetActive(true);
                    }
                }
            }
        }

        public IEnumerator POSTUPIAPI(string PostApiURL, string actual_name, string upi_address, string emailId)
        {
            WWWForm form = new WWWForm();
            form.AddField("actual_name", actual_name);
            form.AddField("upi_address", upi_address);
            form.AddField("emailId", emailId);

            using (UnityWebRequest www = UnityWebRequest.Post(PostApiURL, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    POSTApiClass result = JsonUtility.FromJson<POSTApiClass>(www.downloadHandler.text);
                    Debug.Log("POSTAPI successfully.... " + www.downloadHandler.text);
                    WithdrawPanelScript.Instance.UPISavebtn();
                }
            }
        }

        public IEnumerator POSTBankAPI(string PostApiURL, string actual_name, string ifsc_code, string account_number, string emailId)
        {
            WWWForm form = new WWWForm();
            form.AddField("emailId", emailId);
            form.AddField("actual_name", actual_name);
            form.AddField("ifsc_code", ifsc_code);
            form.AddField("account_number", account_number);

            using (UnityWebRequest www = UnityWebRequest.Post(PostApiURL, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    POSTBankClass result = JsonUtility.FromJson<POSTBankClass>(www.downloadHandler.text);
                    Debug.Log("POSTAPI successfully.... " + www.downloadHandler.text);
                    if(result.status == 200)
                    {
                        WithdrawPanelScript.Instance.BankSavebtn();
                    }
                }
            }
        }

        public IEnumerator FetchWithdrawDataAPI(string fetchURL, string emailId)
        {
            WWWForm form = new WWWForm();
            form.AddField("emailId", emailId);

            using (UnityWebRequest www = UnityWebRequest.Post(fetchURL, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("FetchAPI successfully.... " + www.downloadHandler.text);
                }
            }
        }

        public IEnumerator FetchBankAPI(string fetchBankURL, string emailId)
        {
            WWWForm form = new WWWForm();
            form.AddField("emailId", emailId);

            using (UnityWebRequest www = UnityWebRequest.Post(fetchBankURL, form))
            {
                yield return www.SendWebRequest();

                if (www.result != UnityWebRequest.Result.Success)
                {
                    Debug.Log(www.error);
                }
                else
                {
                    Debug.Log("FetchAPI successfully.... " + www.downloadHandler.text);
                }
            }
        }

        public IEnumerator GETAPI(string GetApiURL)
        {
            using(UnityWebRequest www = UnityWebRequest.Get(GetApiURL))
            {
                yield return www.SendWebRequest();
                Debug.Log("get  " + www.downloadHandler.text );
            }
        }

        public void DownloadSprite(string url, Action<Sprite> OnDownloadComplete)
        {
            StartCoroutine(LoadFromWeb(url, OnDownloadComplete));
        }

        IEnumerator LoadFromWeb(string url, Action<Sprite> OnDownloadComplete)
        {
            UnityWebRequest webRequest = new UnityWebRequest(url);
            DownloadHandlerTexture textureDownloader = new DownloadHandlerTexture(true);
            webRequest.downloadHandler = textureDownloader;
            yield return webRequest.SendWebRequest();
            if (!(webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError))
            {
                Texture2D texture = textureDownloader.texture;
                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero, 1f);
                OnDownloadComplete(sprite);
            }
            else
            {
                Debug.LogError("failed to download image");
            }
        }
        public void DownloadTexture(string url, Action<Texture> OnDownloadComplete)
        {
            StartCoroutine(LoadFromWeb(url, OnDownloadComplete));
        }
        IEnumerator LoadFromWeb(string url, Action<Texture> OnDownloadComplete)
        {
            UnityWebRequest webRequest = new UnityWebRequest(url);
            DownloadHandlerTexture textureDownloader = new DownloadHandlerTexture(true);
            webRequest.downloadHandler = textureDownloader;
            yield return webRequest.SendWebRequest();
            if (!(webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError))
            {
                OnDownloadComplete(textureDownloader.texture);
            }
            else
            {
                Debug.LogError("failed to download image");
            }
        }
        public int GetVersionCode()
        {
            return FetchVersionCode();
        }
        public static int FetchVersionCode()
        {
            try
            {
                AndroidJavaClass contextCls = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                AndroidJavaObject context = contextCls.GetStatic<AndroidJavaObject>("currentActivity");
                AndroidJavaObject packageMngr = context.Call<AndroidJavaObject>("getPackageManager");
                string packageName = context.Call<string>("getPackageName");
                AndroidJavaObject packageInfo = packageMngr.Call<AndroidJavaObject>("getPackageInfo", packageName, 0);
                return packageInfo.Get<int>("versionCode");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return 2;
            }
        }

        public class LoginFormData
        {
            public int status;
            public string message;
            public user user;
        }
        [Serializable]
        public class user
        {
            public bool login;
            public float balance;
            public LoginProfileData profile;
            // public UserNameClass username;
            public string username;
        }
        [Serializable]
        public class UserNameClass
        {
            public string username;
        }
        public class LoginProfileData
        {
            public string firstname;
            public string lastname;
            public string email;
            public string cashbalance;
            public string safe_balance;
            public string winning_balance;
            public string bonus_amount;
            public string coin_balance;
        }
        public class WithDrawAPIClass
        {
            public int status;
            public string message;
        }
        public class POSTApiClass
        {
            public int status;
            public string message;
        }
        public class POSTBankClass
        {
            public int status;
            public string message;
        }
        public class FetchUPIBankClass
        {
            public int status;
            public string message;
            public FetchData data;
        }
        public class FetchData
        {
            public UPIdataClass UPI;
        }
        public class UPIdataClass
        {
            public int id;
            public string actual_name;
            public string upi_address;
        }
        public class BankdataClass
        {
            public string ifsc_code;
            public string account_number;
            public string actual_name;
        }
    }
}
