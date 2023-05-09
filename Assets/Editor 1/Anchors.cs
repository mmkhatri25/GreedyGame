using UnityEngine;
using System.Collections;

public class Anchors : MonoBehaviour {
    public static Anchors ins;
    public GameObject top, bottum, left, right, whiteLine;
    public Camera cam;
    /*
     * top: Middle top of the sceen.
     * Middle: Middle Middle Of the Screen.
     * Bottum: Bottum Midle of the Scrren.
     * ALl of these will create and get setted Automaticaolly in awake, Just drag and drop over the ui camera of the game.
    */

    void Awake(){
        //ins = this;
        // Making new game Objects for the anchors.
        /*  top = new GameObject();
          middle = new GameObject();
          bottum = new GameObject();
          left = new GameObject();
          right = new GameObject();*/

        // Assigning the name and the parent.
        /* top.name="Top";
         bottum.name="Bottum";
         middle.name="Middle";
         right.name = "Right";
         left.name = "Left";*/
        /*  top.transform.SetParent(transform);
          bottum.transform.SetParent(transform);
          middle.transform.SetParent(transform);
          left.transform.SetParent(transform);
          right.transform.SetParent(transform);*/

        // Getting the camera component.
 
        // Getting the Point.
        top.transform.position = cam.ViewportToWorldPoint(new Vector3(.5f, 1, 0));
        bottum.transform.position = cam.ViewportToWorldPoint(new Vector3(.5f, 0, 0));
        left.transform.position = cam.ViewportToWorldPoint(new Vector3(0, .5f, 0));
        right.transform.position = cam.ViewportToWorldPoint(new Vector3(1, .5f, 0));
        whiteLine.transform.position = cam.ViewportToWorldPoint(new Vector3(.9f, .5f, 0));

        top.transform.position = new Vector3(top.transform.position.x, top.transform.position.y, top.transform.position.z);
        bottum.transform.position = new Vector3(bottum.transform.position.x, bottum.transform.position.y, bottum.transform.position.z);
        left.transform.position = new Vector3(left.transform.position.x, left.transform.position.y, left.transform.position.z);
        right.transform.position = new Vector3(right.transform.position.x, right.transform.position.y, right.transform.position.z);
        whiteLine.transform.localPosition = new Vector3(whiteLine.transform.localPosition.x, whiteLine.transform.localPosition.y, 0);

    }
}
