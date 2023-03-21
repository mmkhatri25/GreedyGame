using Com.BigWin.WebUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileScript : MonoBehaviour
{
    public static ProfileScript Instance;
    public GameObject ProfilePanel;
    public GameObject AvtarPanel;
    public GameObject ChangeNamePanel;
    public Text PhoneTxt;
    public Text CapitalTxt;
    public Text IDTxt;
    public Text NameTxt;
    public GameObject BondObj;
    public Sprite[] ProfileSprite;
    public Image Profile;
    public InputField nameChange;
    string ProfileUrl = "https://jeetogame.in/jeeto_game/WebServices/updateProfile";
    private void Awake()
    {
        Instance = this;
    }
    public void ShowProfileUI()
    {
        PhoneTxt.text = UserDetail.MobileNo;
        CapitalTxt.text = UserDetail.Balance.ToString();
        NameTxt.text = UserDetail.Name;
        IDTxt.text = UserDetail.ID;
        Debug.Log("Mobile NUmber............ " + UserDetail.MobileNo);
        if (UserDetail.MobileNo== null)
        {
            Debug.Log("Mobile NUmber Not NULL");

           BondObj.SetActive(false);
        }
        else
        {
            Debug.Log("Mobile NUmber <<<<<<<<<NULL>>>>>>>>>>");
            BondObj.SetActive(true);
        }
        Profile.sprite = ProfileScript.Instance.ProfileSprite[UserDetail.ProfileId];
        ProfilePanel.SetActive(true);
    }
    public void CloseProfileUI()
    {
        ProfilePanel.SetActive(false);
    }
    public void BondBtn()
    {
        RegisterScript.Instance.ShowRegisterUI();
    }
    public void AvtarBtn()
    {
        AvtarPanel.SetActive(true);
    }
    public void CloseAvtarUI()
    {
        AvtarPanel.SetActive(false);
    }
    public void NameChangeBtn()
    {
        ChangeNamePanel.SetActive(true);
    }
    public void NameChangecloseUI()
    {
        ChangeNamePanel.SetActive(false);
    }
    public void SelectAvtar(int ind)
    {
        ProfileUpdateForm form = new ProfileUpdateForm(UserDetail.UserId.ToString(), ind, "en");
        // WebRequestHandler.instance.Post(ProfileUrl, JsonUtility.ToJson(form), OnProfileRequestProcessed);
    }

    private void OnProfileRequestProcessed(string json, bool success)
    {
        print("Profile Response " + json);
        ProfileUpdateResponce responceData = JsonUtility.FromJson<ProfileUpdateResponce>(json);
        if (responceData.response.status)
        {
            UserDetail.ProfileId = responceData.response.data.profile_id;
            Profile.sprite = ProfileScript.Instance.ProfileSprite[UserDetail.ProfileId];
            HomeScript.Instance.ShowHomeUI();
            //dialogue.Show(responce.message);
            AvtarPanel.SetActive(false);
        }
        else
        {
            // AndroidToastMsg.ShowAndroidToastMessage(responceData.response.message);
        }
    }
    // public void ChangeName()
    // {
    //     if (nameChange.text != "")
    //     {
    //         ProfileUpdateForm form = new ProfileUpdateForm(UserDetail.UserId.ToString(), nameChange.text, "en");
    //         WebRequestHandler.instance.Post(ProfileUrl, JsonUtility.ToJson(form), OnProfileNameRequestProcessed);
    //     }
    // }
    // private void OnProfileNameRequestProcessed(string json, bool success)
    // {
    //     Debug.Log("JSON.. " + json);
    //     ProfileUpdateResponce responceData = JsonUtility.FromJson<ProfileUpdateResponce>(json);
    //     if (responceData.response.status)
    //     {
    //         UserDetail.Name = responceData.response.data.name;
    //         NameTxt.text = UserDetail.Name;
    //         HomeScript.Instance.ShowHomeUI();
    //         //dialogue.Show(responce.message);
    //         ChangeNamePanel.SetActive(false);
    //     }
    //     else
    //     {
    //         // AndroidToastMsg.ShowAndroidToastMessage(responceData.response.message);
    //     }
    // }

   
}
[Serializable]
public class ProfileUpdateForm
{
    public string user_id;
    public string name;
    public string language;
    public int profile_id;

    public ProfileUpdateForm(string user_id, int profile_id, string language)
    {
        this.user_id = user_id;
        // this.name = name;
        this.language = language;
        this.profile_id = profile_id;
    }
    public ProfileUpdateForm(string user_id, string name, string language)
    {
        this.user_id = user_id;
        this.name = name;
        this.language = language;
        // this.profile_id = profile_id;
    }
}
[Serializable]
public class ProfileUpdateData
{
    public string name;
    public int profile_id;
}
[Serializable]
public class Response
{
    public bool status;
    public string message;
    public ProfileUpdateData data;

}
[Serializable]
public class ProfileUpdateResponce
{
    public Response response;

}

