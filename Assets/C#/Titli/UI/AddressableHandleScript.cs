// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.UI;
// using Titli.UI;
// using UnityEngine.AddressableAssets;
// using UnityEngine.ResourceManagement.AsyncOperations;
// using UnityEngine.AddressableAssets.ResourceLocators;
// using System;

// public class AddressableHandleScript : MonoBehaviour
// {
//     public static AddressableHandleScript Instance;
//     public AsyncOperationHandle<IList<Sprite>> handle, BetHandle;
//     public AssetLabelReference LabelReferenceUi, LabelReferenceBet;
//     public AssetReference owl;
//     public Slider slider;
//     [SerializeField]
//     // public AssetReferenceSprite[] GameUILayout;
//     // [SerializeField]
//     // public AssetReferenceSprite[] ownAnimation;
//     public List<Sprite> gameUI, CenterWheeleSprites;
//     public GameObject panel, OWL, bse;
//     void Awake() {
//         Instance = this;    
//     }
//     void Start()
//     {
//         Debug.Log("Starting");
//         slider.value = Addressables.InitializeAsync().PercentComplete;
//         Addressables.InitializeAsync().Completed += StartAssigning => {
            
//             Debug.Log("Loading and assigning Assets");
//             handle = Addressables.LoadAssetsAsync<Sprite>(
//                 LabelReferenceUi,
//                 Addressables => {
//                     gameUI.Add(Addressables);
//                 });
//             BetHandle = Addressables.LoadAssetsAsync<Sprite>(
//                 LabelReferenceBet, Addressables => {
//                     CenterWheeleSprites.Add(Addressables);
//                 }
//             );
//             // OwlAnimationHandle = Addressables.LoadAssetsAsync<Sprite>(
//             //     labelReferenceOwl, 
//             //     Addressables => {
//             //         owl_Images.Add(Addressables);
//             //     }
//             // );

//             owl.LoadAssetAsync<GameObject>().Completed += (owlPrefab) => {
//                 owl.InstantiateAsync().Completed += (owlPrefab) => {
//                     OWL = owlPrefab.Result;
//                     Instantiate(OWL, bse.transform);
//                 };
//             };

//             // for (int i = 0; i < GameUILayout.Length; i++) //foreach (var item in GameUILayout)
//             // {
//                 // slider.value = GameUILayout[i].LoadAssetAsync<Sprite>().PercentComplete;
//                 // GameUILayout[i].LoadAssetAsync<Sprite>().Completed += (img) => {
//                 //     gameUI.Add(img.Result); 
//                 // };
//                 // Debug.Log("");
//             // }
//             StartCoroutine(Display());
        
//         };
//     }

//     IEnumerator Display()
//     {
//         yield return new WaitUntil( () => gameUI.Count > 10 );
//         Debug.Log("Game List" +gameUI.Count);
//         // BG.sprite = gameUI[0];
//         yield return StartCoroutine(Titli_UiHandler.Instance.StartAssigningAssets());
//         panel.SetActive(false);
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }
// }
