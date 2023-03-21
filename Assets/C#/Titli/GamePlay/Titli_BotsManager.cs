using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Titli.Utility;

namespace Titli.Gameplay
{
    public class Titli_BotsManager : MonoBehaviour
    {
        public static Titli_BotsManager Instance;
        // public WOF_Bot[] bots;
        private void Awake()
        {
            Instance = this;
        }
        public void UpdateBotData(object data)
        {
            // try
            // {
            //     RoundData roundData = Utility.Utility.GetObjectOfType<RoundData>(data);
            //     for (int i = 0; i < bots.Length; i++)
            //     {
            //         bots[i].UpdateData(roundData.botsBetsDetails[i]);
            //     }
            // }
            // catch (Exception e)
            // {
            //     Debug.Log(e.Message);
            // }
        }
    }

    public class BotsBetsDetail
    {
        public string name;
        public int left;
        public int middle;
        public int right;
        public double balance;
        public int win;
        public int avatarNumber;
    }

    public class RoundData
    {
        public string socketId;
        public List<int> previousWins;
        public List<BotsBetsDetail> botsBetsDetails;
        public int RandomWinAmount;
    }
    public class PreviousWin
    {
        public int win_no;
    }
}
