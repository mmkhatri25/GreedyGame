using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Titli.Utility;
using Shared;
using Titli.UI;

namespace Titli.Gameplay
{
    public class Titli_BetManager : MonoBehaviour
    {
        public static Titli_BetManager Instance;
            Dictionary<Spots, int> betHolder = new Dictionary<Spots, int>();
            private void Awake()
            {
                Instance = this;
            }
            private void Start()
            {
                Debug.Log("Here Bet manager is ready...");
                betHolder.Add(Spots.carrot, 0);
                betHolder.Add(Spots.papaya, 0);
                betHolder.Add(Spots.Cabbage, 0);
                betHolder.Add(Spots.tomato, 0);
                betHolder.Add(Spots.roll, 0);
                betHolder.Add(Spots.hotdog, 0);
                betHolder.Add(Spots.pizza, 0);
                betHolder.Add(Spots.chicken, 0);
                Titli_Timer.Instance.onTimeUp += PostBets;
                Titli_Timer.Instance.onCountDownStart += ClearBet;
            }
            void PostBets()
            {
            }
            void ClearBet()
            {
                betHolder[Spots.carrot] = 0;
                betHolder[Spots.papaya] = 0;
                betHolder[Spots.Cabbage] = 0;
                betHolder[Spots.tomato] = 0;
                betHolder[Spots.roll] = 0;
                betHolder[Spots.hotdog] = 0;
                betHolder[Spots.pizza] = 0;
                betHolder[Spots.chicken] = 0;
            }
            public void AddBets(Spots betType, Chip chipType)
            {
                print("AddBets - place type - " + betType.ToString() + " , chipType "+ chipType);
                betHolder[betType] = GetBetAmount(chipType);
                 Titli_UiHandler.Instance.AddBets(betType);
                
            }
            private int GetBetAmount(Chip chipType)
            {
                print("In GetBetAmount chipType "+ chipType);

                int amount = 0;
                switch (chipType)
                {
                    case Chip.Chip10:
                        amount = 10;
                        break;
                    case Chip.Chip50:
                        amount = 50;
                        break;
                    case Chip.Chip100:
                        amount = 100;
                        break;
                    case Chip.Chip1000:
                        amount = 1000;
                        break;
                    case Chip.Chip10000:
                        amount = 10000;
                        break;
                    default:
                        break;
                }
                print("In GetBetAmount chipType amount = " + amount);

                 return amount;
            }
    }
}
