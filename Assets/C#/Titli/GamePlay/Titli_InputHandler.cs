using Shared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Titli.Utility;
using Titli.Gameplay;

public class Titli_InputHandler : MonoBehaviour
{
    [SerializeField] Titli_CardController cardController;
    public Camera camera;
    private void OnMouseDown()
    {
        //if(Titli_CardController.Instance._canPlaceBet == true)
        //{
        //    ProjectRay();
        //}
        print("on bet hit 11");
        
            ProjectRay();
        
    }
    void ProjectRay()
    {
        print("on bet hit 22 ");
        Vector3 origin = camera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector3.forward * 100);
        if (hit.collider != null)
        {
        print("on bet hit 33");
        
            cardController.OnUserInput(hit.transform, hit.point);
        }
    }
}
