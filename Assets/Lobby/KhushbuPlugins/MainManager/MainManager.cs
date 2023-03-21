using KhushbuPlugin;
using UnityEngine;
public class MainManager : MonoBehaviour
{
    public static MainManager Instance;
    public GameObject Prefab;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {             
        GameObject gm1 = Instantiate(Resources.Load("SoundManager", typeof(GameObject))) as GameObject;
        gm1.name = "SoundManager";               
    }
}
