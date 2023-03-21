using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Titli.UI
{


    public class AndroidExit : MonoBehaviour
    {
        public static AndroidExit instance;
        private void Awake()
        {
            instance = this;
        }
        public GameObject exitPopup;
#if UNITY_ANDROID
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && Titli_UiHandler.Instance.isBetPlaced)
            {
                exitPopup.SetActive(true);
            }
        }
#endif
        public void onExitpopup()
        {
        if(Titli_UiHandler.Instance.isBetPlaced)
            exitPopup.SetActive(true);
        }

        public void onExityes()
        {
            Application.Quit();
        }
        public void onNoExit()
        {
            exitPopup.SetActive(false);
        }
    }
}