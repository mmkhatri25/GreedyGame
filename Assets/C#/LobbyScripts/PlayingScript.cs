using KhushbuPlugin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
// using Dragon.ServerStuff;

public class PlayingScript : MonoBehaviour
{
    public static PlayingScript Instance;
    public GameObject LobbyPanel;
    public Image SoundImg;
    public Image MusicImg;
    public Sprite SoundOFF;
    public Sprite SoundON;
    public Sprite MusicOFF;
    public Sprite MusicON;
 
    public Sprite[] BellAnimation;
    public Image RightBellImag;
    public Image LeftBellImag;    
    bool isLoading = true;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        SFXButtons();
        MusicButtons();
        StartCoroutine(LeftBellLoading(0.025f));
        StartCoroutine(RightBellLoading(0.015f));
    }
    public void ShopBtn()
    {
        LobbyPanel.SetActive(false);
        ShopScript.Instance.ShowShopUI();
    }
    public void LobbyBtn()
    {
        if (LobbyPanel.activeSelf)
        {
            LobbyPanel.SetActive(false);
        }
        else
        {
            SFXButtons();
            MusicButtons();
            LobbyPanel.SetActive(true);
        }
    }  
    public void RulesBtn()
    {
        LobbyPanel.SetActive(false);
        RulesScript.Instance.ShowRulesUI();
    }
    public void SFXButtons(bool isChange = false)
    {
        //if (isChange)
        //{
        //    UtilitySound.Instance.ButtonClickSound();
        //    UtilitySound.Instance.ToggleSound();
        //}
        //if (UtilityModel.GetSound())
        //{
        //    SoundImg.sprite = SoundOFF;
        //}
        //else
        //{
        //    SoundImg.sprite = SoundON;
                      
        //}


        if (isChange)
        {
            UtilitySound.Instance.ButtonClickSound();
            UtilitySound.Instance.ToggleSound();
        }
        if (!UtilityModel.GetSound())
        {
            SoundImg.sprite = SoundOFF;
        }
        else
        {
            SoundImg.sprite = SoundON;
        }
    }
    public void MusicButtons(bool isChange = false)
    {
        if (isChange)
        {
            //UtilitySound.Instance.ButtonClickSound();
            UtilitySound.Instance.ToggleMusic();
        }
        if (UtilityModel.GetMusic())
        {
            MusicImg.sprite = MusicOFF;
        }
        else
        {
            MusicImg.sprite = MusicON;
        }
    }
    IEnumerator LeftBellLoading(float value = 0f)
    {
        yield return new WaitForSeconds(value);
        foreach (var item in BellAnimation)
        {
            LeftBellImag.sprite = item;
            yield return new WaitForSeconds(0.1f);
        }
        StartCoroutine(LeftBellLoading());
    }
    IEnumerator RightBellLoading(float value = 0f)
    {
        yield return new WaitForSeconds(value);
        foreach (var item in BellAnimation)
        {
            RightBellImag.sprite = item;
            yield return new WaitForSeconds(0.1f);
        }
        StartCoroutine(RightBellLoading());
    }

}
