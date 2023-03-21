using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Com.BigWin.WebUtils;
using Newtonsoft.Json;

public class DiamondScript : MonoBehaviour
{
    public static DiamondScript Instance;
    public GameObject SafePanel;


    private void Awake()
    {
        Instance = this;
    }

    public void AddDiamonds()
    {
        bool isGuestLogin = PlayerPrefs.GetString("MobileNum", "") != "";
        string user_ID = isGuestLogin ? PlayerPrefs.GetString("MobileNum", "") : SystemInfo.deviceUniqueIdentifier;
        AddDiamond diamond = new AddDiamond()
        {
            user_id = user_ID,
            free_fire_id = 123456789.ToString(),
            diamond_id = 8.ToString()
        };
        WebRequestHandler.instance.Post(LobbyScripts.Constants.AddDiamondURL,
            JsonConvert.SerializeObject(diamond), OnAddDiamondCompleted);

    }
    void OnAddDiamondCompleted(string response,bool status)
    {
        if (!status)
        {
            Debug.Log("Something went wrong");
            return;
        }
        Debug.Log(response);
        //AddDaimondResponse o = JsonUtility.FromJson<AddDaimondResponse>(response);

    }



    public void ShowDiamondUI()
    {

        SafePanel.SetActive(true);

    }
    public void CloseDiamondUI()
    {
        SafePanel.SetActive(false);
    }

    public class AddDiamond
    {
        public string user_id;
        public string diamond_id;
        public string free_fire_id;
    }

    public class Data
    {
    }

    public class Response
    {
        public bool status { get; set; }
        public string message { get; set; }
        public Data data { get; set; }
    }

    public class AddDaimondResponse
    {
        public Response response { get; set; }
    }

}

