using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareToSocialMedia : MonoBehaviour
{
     public void Share()
    {
        StartCoroutine(ShareToWhatsapp());
    }
     
     public void ShareT()
     {
         StartCoroutine(ShareToTelegram());
     }

    private IEnumerator ShareToWhatsapp()
    {
        yield return new WaitForEndOfFrame();
        
        // string bundleId = "com.whatsapp"; // your target bundle id
        // AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        // AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        // AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");
        //
        // //if the app is installed, no errors. Else, doesn't get past next line
        // AndroidJavaObject launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage",bundleId);
        //
        // ca.Call("startActivity",launchIntent);
        //
        // up.Dispose();
        // ca.Dispose();
        // packageManager.Dispose();
        // launchIntent.Dispose();
        //
        // launchIntent.Call<AndroidJavaObject>("putExtra", "9867066683" );
        
        string url = "https://api.whatsapp.com/send?phone="+ "+918588027410" +"&text=" + "";
//Number variable needs to include the country code. If you want to send a message to Guatemala
// and the mobile number is: 12345678, your final string will be:
// https://api.whatsapp.com/send?phone=50212345678&text=hello

        Application.OpenURL(url);
        
       
    }

    private IEnumerator ShareToTelegram()
    {
        yield return new WaitForEndOfFrame();
        
        string bundleId = "org.telegram.messenger"; // your target bundle id
        AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject ca = up.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = ca.Call<AndroidJavaObject>("getPackageManager");
        
        //if the app is installed, no errors. Else, doesn't get past next line
        AndroidJavaObject launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage",bundleId);
        
        ca.Call("startActivity",launchIntent);
        
        up.Dispose();
        ca.Dispose();
        packageManager.Dispose();
        launchIntent.Dispose();
        
        launchIntent.Call<AndroidJavaObject>("putExtra", "+918588027410", "" );
        
        
    }
}
