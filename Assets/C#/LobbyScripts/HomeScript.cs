using Com.BigWin.WebUtils;
using LobbyScripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class HomeScript : MonoBehaviour
{
    public static HomeScript Instance;
    public GameObject HomePanel;
    public Text NameTxt;
    public Text IDTxt;
    public Image Profile;
    public Text Balance;
    AsyncOperation DragonScene;
    AsyncOperation sevenupScene;
    public Image LobbyAnimpnel;
    public Sprite[] Lobbyframe;
    public Text GuestName;
    private void Awake()
    {
        Instance = this;
    }
    void Update()
    {
        if ( Input.GetKeyDown(KeyCode.Escape) )
        {
            if (Application.platform == RuntimePlatform.Android) Application.Quit();
        }
    }
    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        NameTxt.text = PlayerPrefs.GetString("username");
        Balance.text = PlayerPrefs.GetFloat("balance").ToString();
        Debug.Log("Balance"+PlayerPrefs.GetFloat("balance"));
        StartCoroutine(Loading());
    }
    public void Logout()
    {
        PlayerPrefs.DeleteAll();
        LoginScript.Instance.LoginPanel.SetActive(true);
        HomePanel.SetActive(false);
    }
    IEnumerator Loading()
    {
        foreach (var item in Lobbyframe)
        {
            LobbyAnimpnel.sprite = item;
            yield return new WaitForEndOfFrame();
        }
        StartCoroutine(Loading());
    }
    public void ShowHomeUI()
    {
        //DragonScene = SceneManager.LoadSceneAsync("DragonScene");
        //DragonScene.allowSceneActivation = false;
        //sevenupScene = SceneManager.LoadSceneAsync("7updown");
        //sevenupScene.allowSceneActivation = false;
        Balance.text = PlayerPrefs.GetFloat("balance").ToString();
        // Profile.sprite = ProfileScript.Instance.ProfileSprite[UserDetail.ProfileId];
        NameTxt.text = PlayerPrefs.GetString("username");
        // IDTxt.text = UserDetail.ID;
        HomePanel.SetActive(true);

        // User user = new User() { 
        // user_id = UserDetail.UserId, version_code = 1, language = "en" 
        // };
        // WebRequestHandler.instance.Post(Constants.ProfileURL,JsonUtility.ToJson(user), (data, status) => {
        //     if (!status) return;

        //     Profile profile = JsonConvert.DeserializeObject<Profile>(data);
        //     //print("balance is " + profile.response.data.chip_balance);
        //     Balance.text = profile.response.data.chip_balance.ToString();
        //     UserDetail.Balance = int.Parse(profile.response.data.chip_balance.ToString());
        //     print("Profile Data " + data);
        // });

       
    }
    // public AddressableSceneHandlerScript sceneHandler;
    public void TitliBtn(string stringScene)
    {
        // AndroidToastMsg.ShowAndroidToastMessage("Loading");
        // sceneHandler.SceneLoad(stringScene);
        SceneManager.LoadScene("Titli");
        // Addressables.LoadSceneAsync(stringScene, LoadSceneMode.Single);
    }
    // public void ShopBtn()
    // {
    //     // ShopScript.Instance.ShowShopUI();
    // }
    // public void ProfileBtn()
    // {
    //     // ProfileScript.Instance.ShowProfileUI();
    // }
    // public void settingBtn()
    // {
    //     // SettingScript.Instance.ShowSettingUI();
    // }
    // public void SupportBtn()
    // {
    //     // SupportScript.Instance.ShowSupportUI();
    // }
    // public void NoticeBtn()
    // {
    //     // NoticeScript.Instance.ShowNoticeUI();
    // }
    // public void MailBtn()
    // {
    //     // MailScript.Instance.ShowMailUI();
    // }
    // public void ShareBtn()
    // {
    //     // ReferAndEarnScript.Instance.ShowReferAndEarnUI();
    // }
    // public void SafeBtn()
    // {
    //     // SafeScript.Instance.ShowSafeUI();
    // }
    // public void RankBtn()
    // {
    //     // RankScript.Instance.ShowRankUI();
    // }
    // public void WithdrawBtn()
    // {
    //     // WithDrawScript.Instance.ShowWithDrawUI();
    // }

    // public void DiamondBtn()
    // {
    //     //DiamondScript.Instance.ShowDiamondUI();
    //     // DiamondOffer.Instance.ShowDiamondUI();
    // }

    // public void AgentBtn()
    // {
    //     // AgentScript.Instance.ShowDiamondUI();
    // }
    // public static string RandomString(int length)
    // {
    //     string element = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    //     System.Random random = new System.Random();
    //     return new string((from s in Enumerable.Repeat(element, length)
    //                        select s[random.Next(s.Length)]).ToArray());
    // }
}
// public class LobbyData
// {
//     public string playerId;
// }

// public class ProfileData
// {
//     public string username;
//                   public int chip_balance ;
//                   public int safe_balance ;
//                      public int vip_level ;
//          public string invite_friend_code ;
//              public int team_name_updated ;
//                       public string image ;
//     public List<object> refered_to_friend ;
//                      public string gender ;
//               public string referal_bonus ;
//              public bool account_verified ;
//                public string Signup_bonus ;
//              public string referral_bonus ;
//              public int refered_by_status ;
//                public string version_code ;
//                     public string apk_url ;
// }

// public class ProfileResponse
// {
//     public bool status;
//     public string message;
//     public ProfileData data;
// }

// [Serializable]
// public class Profile
// {
//     public ProfileResponse response;
// }

// public class User
// {
//     public int user_id;
//     public int version_code;
//     public string language;
// }
