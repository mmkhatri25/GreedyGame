using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentScript : MonoBehaviour
{
    public static AgentScript Instance;
    public GameObject SafePanel;
    private void Awake()
    {
        Instance = this;
    }
    public void ShowDiamondUI()
    {

        SafePanel.SetActive(true);

    }
    public void CloseAgentUI()
    {
        SafePanel.SetActive(false);
    }
}
