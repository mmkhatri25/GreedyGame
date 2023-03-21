using System.Collections.Generic;
using UnityEngine;
using Titli.UI;
using Titli.Utility;
using Shared;

namespace Titli.Gameplay
{
    public class Titli_ChipSpawner : MonoBehaviour
    {
        public static Titli_ChipSpawner Instance;
            [SerializeField] GameObject[] chips;
            [SerializeField] Transform[] spawnPostions;

            Dictionary<Chip, GameObject> chipContainer =
                new Dictionary<Chip, GameObject>();


            int chipOrderInLayer = 10;

            private void Awake()
            {
                Instance = this;
            }
            public void Start()
            {
                chipContainer.Add(Chip.Chip10, chips[0]);
                chipContainer.Add(Chip.Chip50, chips[1]);
                chipContainer.Add(Chip.Chip100, chips[2]);
                chipContainer.Add(Chip.Chip1000, chips[3]);
                chipContainer.Add(Chip.Chip10000, chips[4]);
                Titli_Timer.Instance.onTimeUp += () => chipOrderInLayer = 10;
            }
            public GameObject Spawn(int positinIndex, Chip chipType, Transform parent)
            {
                var chip = Instantiate(chipContainer[chipType], parent);
                //chip.GetComponent<SpriteRenderer>().sortingOrder = chipOrderInLayer++;
                chip.SetActive(true);
                chip.transform.position = spawnPostions[positinIndex].position;
                return chip;
            }
    }
}
