// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using AndarBahar.Utility;
// using Newtonsoft.Json;
// using System.IO;

// public class Test : MonoBehaviour
// {
//     public AndarBhar_AiBot_Data data;
//     private void Start()
//     {
//         int i=0;
//         string s = "[";
//         foreach (var item in data.GetData())
//         {
//             i++;
//             s += JsonUtility.ToJson(item);
//             s += ",";
//         }
//         s += "]";
//         if (File.Exists("andarbharBotData.txt"))
//         {
//             File.Delete("andarbharBotData.txt");
//         }
//         using (StreamWriter sw = File.CreateText("andarbharBotData.txt"))
//         {
//             sw.WriteLine(s);
//         }

//     }
// }
