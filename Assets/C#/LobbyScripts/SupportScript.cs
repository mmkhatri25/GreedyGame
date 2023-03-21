using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportScript : MonoBehaviour
{
    public static SupportScript Instance;
    public GameObject SupportPanel;

    private void Awake()
    {
        Instance = this;
    }
    public void ShowSupportUI()
    {
        SupportPanel.SetActive(true);
    }
    public void CloseSupportUI()
    {
        SupportPanel.SetActive(false);
    }
}
