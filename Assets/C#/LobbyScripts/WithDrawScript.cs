using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WithDrawScript : MonoBehaviour
{
    public static WithDrawScript Instance;
    public GameObject WithDrawPanel;

    private void Awake()
    {
        Instance = this;
    }
    public void ShowWithDrawUI()
    {
        WithDrawPanel.SetActive(true);
    }
    public void CloseWithDrawUI()
    {
        WithDrawPanel.SetActive(false);
    }
}
