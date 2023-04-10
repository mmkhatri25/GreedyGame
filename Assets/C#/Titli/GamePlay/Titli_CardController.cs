using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Shared;
using KhushbuPlugin;
using Titli.UI;
using Titli.Utility;
// using Titli.ServerStuff;

namespace Titli.Gameplay
{
    public class Titli_CardController : MonoBehaviour
    {
        public static Titli_CardController Instance;
        public List<Image> _cardsImage;
        public List<Image> owlimages;
        public Titli_BetManager betManager;
        // public Transform ChipParent;
        // float chipMovetime = .5f;
        // [SerializeField] float chipMoveTime;
        // [SerializeField] iTween.EaseType easeType;
        public Action<Transform, Vector3> OnUserInput;
        public Dictionary<Spots, Transform> chipHolder = new Dictionary<Spots, Transform>();
        public bool _startCardBlink, _canPlaceBet;
        // // public GameObject CarrotChipPos, PapayaChipPos, CabbageChipPos, ChickenChipPos, MuttonChipPos, ShrimpChipPos, FishChipPos, TomatoChipPos;
        // [SerializeField] Transform chipSecondLastSpot;
        // [SerializeField] Transform chipLastSpot;
        public AudioClip AddChip;
        public AudioSource CoinMove_AudioSource;
        // public IEnumerator CardBlink_coroutine;
        public List<GameObject> TableObjs;

        void Awake()
        {
            Instance = this;

        }
        // Start is called before the first frame update
        void Start()
        {
            _canPlaceBet = false;
            OnUserInput += CreateChip;
            // chipHolder.Add(Spots.carrot, CarrotChipPos.transform);
            // chipHolder.Add(Spots.papaya, PapayaChipPos.transform);
            // chipHolder.Add(Spots.Cabbage, CabbageChipPos.transform);
            // chipHolder.Add(Spots.chicken, ChickenChipPos.transform);
            // chipHolder.Add(Spots.meat, MuttonChipPos.transform);
            // chipHolder.Add(Spots.shrimp, ShrimpChipPos.transform);
            // chipHolder.Add(Spots.Fish, FishChipPos.transform);
            // chipHolder.Add(Spots.tomato, TomatoChipPos.transform);
            _startCardBlink = true;
            // _winNo = false;
        }

        // public void Card_text_blink(int no)
        // {
        //     StartCoroutine(CardsBlink(no));
        // }
        public void anima(int no)
        {
            StartCoroutine(CardsBlink(no));
        }

        public IEnumerator CardsBlink(int winno)
        {
            int round = 0;
            while ( round <= _cardsImage.Count + winno)
            {    for (int i = 0; i < _cardsImage.Count; i++)
                {
                    owlimages[i].gameObject.SetActive(true);
                    //_cardsImage[i].transform.parent.GetChild(6).gameObject.SetActive(true);
                     //print("another 6 number card name - "+ owlimages[i].gameObject.name);
                    
                    yield return new WaitForSeconds(0.25f);
                    if (round == _cardsImage.Count + winno) yield break;
                    //_cardsImage[i].transform.parent.GetChild(6).gameObject.SetActive(false);
                    owlimages[i].gameObject.SetActive(false);
                    
                    round ++;
                }
            }
            // if(_startCardBlink == true)
            // {
            //     foreach(var item in _cardsImage)
            //     {
            //         var tempColor = item.color;
            //         tempColor.a = 0.5f;
            //         item.color = tempColor;
                    
            //         tempColor.a = 1.0f;
            //         item.color = tempColor;
                    
            //     }
            //     // CardBlink_coroutine = CardsBlink();
            //     StartCoroutine(CardsBlink());
            // }
        }

        public void StopCardsBlink()
        {
            _startCardBlink = false;
            // StopCoroutine(CardBlink_coroutine);
        }

        void CreateChip(Transform bettingSpot, Vector3 target)
        {
            if (!Titli_UiHandler.Instance.IsEnoughBalancePresent()) return;
            Chip chip = Titli_UiHandler.Instance.currentChip;
            Spots spot = bettingSpot.GetComponent<BettingSpot>().spotType;
            Titli_UiHandler.Instance.AddBets(spot);
            // ServerRequest.instance.OnChipMove(target, chip, spot);
            // switch (spot)
            // {
            //     case Spots.carrot:
            //         target = CarrotChipPos.transform.position;
            //         break;
            //     case Spots.papaya:
            //         target = PapayaChipPos.transform.position;
            //         break;
            //     case Spots.Cabbage:
            //         target = CabbageChipPos.transform.position;
            //         break;
            //     case Spots.chicken:
            //         target = ChickenChipPos.transform.position;
            //         break;
            //     case Spots.meat:
            //         target = MuttonChipPos.transform.position;
            //         break;
            //     case Spots.shrimp:
            //         target = ShrimpChipPos.transform.position;
            //         break;
            //     case Spots.Fish:
            //         target = FishChipPos.transform.position;
            //         break;
            //     case Spots.tomato:
            //         target = TomatoChipPos.transform.position;
            //         break;
            //     default:
            //         break;
            // }
            betManager.AddBets(spot, Titli_UiHandler.Instance.currentChip);
            // Titli_UiHandler.Instance.UpDateBets(spot, chip);         // not need as it gives bot data
            // GameObject chipInstance = Titli_ChipSpawner.Instance.Spawn(0, chip, GetChipParent(spot));
            //StartCoroutine(MoveChip(chipInstance, target)); 
        }

        // Transform GetChipParent(Spots betType)
        // {
        //     switch (betType)
        //     {
        //         case Spots.carrot: return CarrotChipPos.transform;
        //         case Spots.papaya: return PapayaChipPos.transform;
        //         case Spots.Cabbage: return CabbageChipPos.transform;
        //         case Spots.chicken: return ChickenChipPos.transform;
        //         case Spots.meat: return MuttonChipPos.transform;
        //         case Spots.shrimp: return ShrimpChipPos.transform;
        //         case Spots.Fish: return FishChipPos.transform;
        //         case Spots.tomato: return TomatoChipPos.transform;
        //     }
        //     return null;
        // }

        // IEnumerator MoveChip(GameObject chip, Vector3 target)
        // {
        //     iTween.MoveTo(chip, iTween.Hash("position", target, "time", chipMovetime, "easetype", easeType));
        //     //var rotat = new Vector3(0f, 0f, UnityEngine.Random.Range(0, 180));
        //     //iTween.RotateBy(gameObject, iTween.Hash("z", UnityEngine.Random.Range(0, 180), "easeType", "easeInOutBack", "loopType", "pingPong", "delay", chipMovetime));
        //     //iTween.RotateTo(chip, iTween.Hash("rotation", rotat, "time", chipMovetime, "easetype", easeType));
        //     yield return new WaitForSeconds(chipMovetime);
        //     // UtilitySound.Instance.addchipsound();
        //     StartCoroutine(PlayAudioClip());
        //     iTween.PunchScale(chip, iTween.Hash("x", .3, "y", 0.3f, "default", .1));
        // }

        public IEnumerator PlayAudioClip()
        {
            yield return new WaitForSeconds(0.1f);
            CoinMove_AudioSource.clip = AddChip;
            CoinMove_AudioSource.Play();
            // CoinMove_AudioSource.Stop();
        }

        // public void TakeChipsBack(Spots winner)
        // {
        //     StartCoroutine(DestroyChips(winner));
        // }
        // IEnumerator DestroyChips(Spots winnerSpot)
        // {
        //     foreach (var item in chipHolder)
        //     {
        //         if (item.Key == winnerSpot) continue;
        //         foreach (Transform child in item.Value)
        //         {
        //             // StartCoroutine(MoveChips(child, chipSecondLastSpot));
        //         }
        //     }
        //     yield return new WaitForSeconds(1);
        //     foreach (Transform child in chipHolder[winnerSpot])
        //     {
        //         // StartCoroutine(MoveChips(child, chipLastSpot));
        //     }
        //     Titli_UiHandler.Instance.ResetUi();
        // }

        // public float waitTime = 2.5f;
        // IEnumerator MoveChips(Transform chip, Transform destinatio)
        // {
        //     iTween.MoveTo(chip.gameObject, iTween.Hash("position", destinatio.position, "time", 2.5f, "easetype", easeType));
        //     yield return new WaitForSeconds(waitTime);
        //     Destroy(chip.gameObject);
        // }

    }
}
