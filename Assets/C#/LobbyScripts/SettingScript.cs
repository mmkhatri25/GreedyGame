using KhushbuPlugin;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingScript : MonoBehaviour
{
    public static SettingScript Instance;
    public GameObject SettingPanel;
    public Image SoundON;
    public Image SoundOFF;
    public Image MusicON;
    public Image MusicOFF;
    public Toggle MusicToggle;

    private void Awake()
    {
        Instance = this;
    }
    public void ShowSettingUI()
    {
        SFXButtons();
        MusicButtons();
        SettingPanel.SetActive(true);
    }
    public void CloseSettingUI()
    {
        UtilitySound.Instance.ButtonClickSound();
        SettingPanel.SetActive(false);
    }
    public void SFXButtons(bool isChange = false)
    {
        if (isChange)
        {
            UtilitySound.Instance.ButtonClickSound();
            UtilitySound.Instance.ToggleSound();
        }
        if (!UtilityModel.GetSound())
        {
            SoundON.color = new Color(255, 255, 255, 0);
            SoundOFF.color = new Color(255, 255, 255, 255);
        }
        else
        {
            SoundON.color = new Color(255, 255, 255, 255);
            SoundOFF.color = new Color(255, 255, 255, 0);
        }
    }
    public void MusicButtons(bool isChange = false)
    {
        if (isChange)
        {
            UtilitySound.Instance.ButtonClickSound();
            UtilitySound.Instance.ToggleMusic();
        }
        if (!UtilityModel.GetMusic())
        {
            MusicToggle.isOn = true;
            MusicON.color = new Color(255,255,255,0);
            MusicOFF.color = new Color(255, 255, 255, 255);
        }
        else
        {
            MusicToggle.isOn = false;
            MusicON.color = new Color(255, 255, 255, 255);
            MusicOFF.color = new Color(255, 255, 255, 0);
        }
    }
    public void LoginBtn()
    {
        UtilitySound.Instance.ButtonClickSound();
        PlayerPrefs.DeleteAll();
        SceneManager.LoadScene("MainScene");
    }
}
