using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Titli.Gameplay;
using Titli.Utility;
using Shared;

namespace Titli.player
{
    public class Titli_MainPlayer : MonoBehaviour
    {
        public static Titli_MainPlayer Instance;
        public GameObject winCanvas;
        Vector3 intialPos;
        Vector3 finalPos;
        public Transform Finaltra;
        public float speed = 1f;

        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            winCanvas.SetActive(false);
            intialPos = winCanvas.transform.position;
            finalPos = Finaltra.position;
        }

        public void winner(int win)
        {
            StartCoroutine(WinAnimation(win));
        }
        IEnumerator WinAnimation(int winamount)
        {
            winCanvas.SetActive(true);
            if (winamount < 0)
            {
                winCanvas.GetComponent<Text>().color = Color.red;
                winCanvas.GetComponent<Text>().text = "-" + winamount.ToString();
            }
            else
            {
                winCanvas.GetComponent<Text>().color = Color.green;
                winCanvas.GetComponent<Text>().text = "+" + winamount.ToString();
            }
            float d = Vector2.Distance(winCanvas.transform.position, finalPos);
            while (d > 0.01f)
            {
                winCanvas.transform.position = Vector2.MoveTowards(winCanvas.transform.position, finalPos, speed * Time.deltaTime);
                d = Vector2.Distance(winCanvas.transform.position, finalPos);
                yield return new WaitForEndOfFrame();
            }
            winCanvas.SetActive(false);
            winCanvas.transform.position = intialPos;
        }
    }
}
