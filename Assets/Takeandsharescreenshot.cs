using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Takeandsharescreenshot : MonoBehaviour
{
     public void Share()
    {
        StartCoroutine(TakeScreenShotAndShare());
    }

    private IEnumerator TakeScreenShotAndShare()
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        string filePath = System.IO.Path.Combine(Application.temporaryCachePath, "share.png");
        System.IO.File.WriteAllBytes(filePath, ss.EncodeToPNG());

        Destroy(ss);

        new NativeShare().AddFile(filePath)
            .SetSubject("").SetText("").SetUrl("")
            .SetCallback((res, target) => Debug.Log($"result {res}, target app: {target}"))
            .Share();
    }
}
