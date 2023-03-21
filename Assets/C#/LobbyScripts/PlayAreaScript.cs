// // using Dragon.Gameplay;
// // using Dragon.player;
// // using Dragon.UI;
// // using Dragon.Utility;
// using Shared;
// using System.Collections;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
// using UnityEngine.UI;

// public class PlayAreaScript : MonoBehaviour
// {
//     public static PlayAreaScript Instance;
//     bool isTimeUp;
//     public Sprite BackCard;
//     public float Times;
//     public Text TimeText;
//     public Text BetValueTxt;
//     public iTween.EaseType easetype;
//     public Transform DragonBasement;
//     public Transform TigerBasement;
//     public Transform TieBasement;
//     public GameObject[] chipimg;
//     public GameObject ChipPrefab;
//     public Transform PrefabBasement;
//     public BetSpots betSpots;
   
//     private void Awake()
//     {
//         Instance = this;
//     }
//     private void Start()
//     {
//         Screen.orientation = ScreenOrientation.LandscapeLeft;
//         Timer.Instance.onTimeUp += () => isTimeUp = true;
//         Timer.Instance.onCountDownStart += () => isTimeUp = false;        
//     }
//     public void DragonBet(Vector3 target)
//     {
//         if (isTimeUp || Timer.Instance.is_a_FirstRound) return;
      
//         MainPlayer.Instance.AddBet(0, UiHandler.Instance.currentChip, Spot.left, target);
//     }
//     public void TigerBet(Vector3 clickPosition)
//     {
//         if (isTimeUp || Timer.Instance.is_a_FirstRound) return;
//         MainPlayer.Instance.AddBet(0, UiHandler.Instance.currentChip, Spot.right, clickPosition);      
//     }
//     public void TieBet(Vector3 clickPosition)
//     {
//         if (isTimeUp || Timer.Instance.is_a_FirstRound) return;
//         MainPlayer.Instance.AddBet(0, UiHandler.Instance.currentChip, Spot.middle, clickPosition);        
//     }
//     public void BetValueSelect(int val)
//     {
//         if (val == 10)
//         {
//             UiHandler.Instance.currentChip = Chip.Chip10;
//         }
//         else if (val == 50)
//         {
//             UiHandler.Instance.currentChip = Chip.Chip50;
//         }
//         else if (val == 100)
//         {
//             UiHandler.Instance.currentChip = Chip.Chip100;
//         }
//         else if (val == 500)
//         {
//             UiHandler.Instance.currentChip = Chip.Chip500;
//         }
//         else if (val == 1000)
//         {
//             UiHandler.Instance.currentChip = Chip.Chip1000;
//         }
//         else if (val == 5000)
//         {
//             UiHandler.Instance.currentChip = Chip.Chip5000;
//         }      
//     }    
//     public void ChipImgSelect(int ind)
//     {
//         for (int i = 0; i < chipimg.Length; i++)
//         {
//             chipimg[i].SetActive(false);
//         }
//         chipimg[ind].SetActive(true);
//     }

// }
