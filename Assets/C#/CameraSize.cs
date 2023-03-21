using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class CameraSize : MonoBehaviour
{


    public SpriteRenderer rink;

    void Awake()
    {
        float screenRatio = (float)UnityEngine.Screen.width / (float)UnityEngine.Screen.height;
        float targetRatio = rink.bounds.size.x / rink.bounds.size.y;

        if (screenRatio >= targetRatio)
        {
            Camera.main.orthographicSize = rink.bounds.size.y / 2;
        }
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            Camera.main.orthographicSize = rink.bounds.size.y / 2 * differenceInSize;
        }
    }
    private void Start()
    {
        this.GetComponent<Camera>().orthographicSize = rink.bounds.size.x * Screen.height / Screen.width * 0.5f;
        //float heightDiff = OrthographicBounds(this.GetComponent<Camera>()).size.y / 2 - rink.bounds.size.y / 2;
       // this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - heightDiff, this.transform.position.z);

    }
    public Bounds OrthographicBounds(Camera camera)
    {
        float screenAspect = (float)UnityEngine.Screen.width / (float)UnityEngine.Screen.height;
        float cameraHeight = camera.orthographicSize * 2;
        Bounds bounds = new Bounds(
            camera.transform.position,
            new Vector3(cameraHeight * screenAspect, cameraHeight, 0));
        return bounds;
    }
}
