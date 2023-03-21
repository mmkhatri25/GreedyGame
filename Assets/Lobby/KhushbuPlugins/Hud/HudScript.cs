using UnityEngine;
using UnityEngine.UI;

public class HudScript : MonoBehaviour
{

    public static HudScript Instance;
    public GameObject HudPanel;

    private void Awake()
    {
        Instance = this;
    }
    public void HudActivated()
    {
        HudPanel.SetActive(true);
    }
    public void HudDeactivated()
    {
        HudPanel.SetActive(false);
    }
}
