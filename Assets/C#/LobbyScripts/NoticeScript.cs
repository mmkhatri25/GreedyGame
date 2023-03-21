using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeScript : MonoBehaviour
{
    public static NoticeScript Instance;
    public GameObject NoticePanel;

    private void Awake()
    {
        Instance = this;
    }
    public void ShowNoticeUI()
    {
        NoticePanel.SetActive(true);
    }
    public void CloseNoticeUI()
    {
        NoticePanel.SetActive(false);
    }
}
