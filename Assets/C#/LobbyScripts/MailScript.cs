using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MailScript : MonoBehaviour
{
    public static MailScript Instance;
    public GameObject MailPanel;

    private void Awake()
    {
        Instance = this;
    }
    public void ShowMailUI()
    {
        MailPanel.SetActive(true);
    }
    public void CloseMailUI()
    {
        MailPanel.SetActive(false);
    }
}
