// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.SceneManagement;
// using UnityEngine.AddressableAssets;
// using UnityEngine.ResourceManagement.AsyncOperations;
// using UnityEngine.ResourceManagement.ResourceProviders;
// using System;

// public class AddressableSceneHandlerScript : MonoBehaviour
// {
//     public static AddressableSceneHandlerScript Instance;
//     private AsyncOperationHandle<SceneInstance> handle;

//     public void SceneLoad(string str){
//         Addressables.LoadSceneAsync(str, LoadSceneMode.Single).Completed += SceneLoadCompleted;
//     }

//     private void SceneLoadCompleted(AsyncOperationHandle<SceneInstance> obj)
//     {
//         if (obj.Status == AsyncOperationStatus.Succeeded)
//         {
//             Debug.Log("Loading Scene Success");
//             handle = obj;
//         }
//         // throw new NotImplementedException();
//     }

//     public void SceneUnLoad()
//     {
//         Addressables.UnloadSceneAsync(handle, true).Completed += op => {
//             if (op.Status == AsyncOperationStatus.Succeeded)
//             {
//                 Debug.Log("Unload Success");
//                 SceneManager.LoadScene("MainScene");
//             }
//         };
//     }

//     // Start is called before the first frame update
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
        
//     }

//     void Awake()
//     {
//         Instance = this;
//         DontDestroyOnLoad(gameObject);
//     }
// }
