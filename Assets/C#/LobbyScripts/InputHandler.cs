// // using Dragon.Gameplay;
// // using Dragon.Utility;
// using Shared;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.EventSystems;

// public class InputHandler : MonoBehaviour
// {   
//     public Spot spot;
//     public void OnPointerClick(Vector3 target)
//     {
//         switch (spot)
//         {
//             case Spot.left:
//                 PlayAreaScript.Instance.DragonBet(target);                
//                 break;
//             case Spot.middle:
//                 PlayAreaScript.Instance.TieBet(target);
//                 break;
//             case Spot.right:
//                 PlayAreaScript.Instance.TigerBet(target);
//                 break;
//          }
//     }

//     private void Update()
//     {
//         // if (isTimeUp) return;
//         //if (Input.GetMouseButtonDown(0))
//         //{
//         //    ProjectRay();
//         //}
//     }

//     public LayerMask bettingPlaceLM; 
//     public Camera camera;
//     private void OnMouseDown()
//     {
//         ProjectRay();
//     }
//     void ProjectRay()
//     {
//         Vector3 origin = camera.ScreenToWorldPoint(Input.mousePosition);
//         RaycastHit2D hit = Physics2D.Raycast(origin, Vector3.forward * 100);
//         if (hit.collider != null)
//         {
//             //ChipController.Instance.AddData(spot, hit.point);
//             OnPointerClick(hit.point);
//         }
//     }
// }
