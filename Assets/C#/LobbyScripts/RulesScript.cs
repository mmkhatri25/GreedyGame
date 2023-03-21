using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RulesScript : MonoBehaviour
{
    public static RulesScript Instance;
    public GameObject RulesPanel;
    public Scrollbar scrollbar;
    private void Awake()
    {
        Instance = this;
    }
    public void ShowRulesUI()
    {
        scrollbar.value = 1;
        RulesPanel.SetActive(true);
    }
    public void CloseRulesUI()
    {
        RulesPanel.SetActive(false);
    }
}
