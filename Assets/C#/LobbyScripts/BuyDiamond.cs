using Com.BigWin.WebUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Web;
using UnityEngine.Networking;
using System;
using System.Net;
using System.IO;

public class BuyDiamond : MonoBehaviour
{

    public Button BuyButton;
    public string diamond_id;
    public Text buy_amt;
    public Text qty;
    public Image DiamondImage;

   
    // Start is called before the first frame update
    private void Start()
    {
       // UserDetail.Balance = 500;
       // DiamondOffer.Instance.BalanceCapital.text = UserDetail.Balance.ToString();
        DiamondOffer.Instance.AddGemsButton.interactable = false;
    }
    void OnBuyButtonClick(int val)
    {
        
        if (UserDetail.Balance > val)
        {
            DiamondOffer.Instance.AddGems.text = val.ToString();
            DiamondOffer.Instance.AddGemsButton.interactable = true;
            DiamondOffer.Instance.AddGemsButton.onClick.AddListener(() => DiamondOffer.Instance.OnAddGemsClick(val));
            
        }
        else
        {
            DiamondOffer.Instance.AddGems.text = "Insufficient Balance";
            DiamondOffer.Instance.AddGemsButton.interactable = false;
        }
    }



    public void SetBuyDetails(string buy_amt,int qty,Sprite url)//Sprite diamond_Image=null)
    {
        this.buy_amt.text = buy_amt;
        this.qty.text = qty.ToString();
        int tempValue = int.Parse(buy_amt);
 
            DiamondImage.sprite = url;
        this.GetComponent<Button>().onClick.AddListener(() => OnBuyButtonClick(tempValue));
       // WebRequestHandler.instance.DownloadSprite(diamond_URL, OnDownloadComplete);
    
        }

    public void DownloadOfferUI(string url)
    {
        StartCoroutine(DownloadImage(url));
    }
   /* IEnumerator downloadImage(string url)
    {
        //  string authorization = authenticate("YourUserName", "YourPass");

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
        request.Method = "GET";

        UnityWebRequest www = UnityWebRequest.Get(url);
        //www.SetRequestHeader("AUTHORIZATION", authorization);

        DownloadHandler handle = www.downloadHandler;

        //Send Request and wait
        yield return www.Send();
        //Download All the bytes
        byte[] bytes = downloadFullData(request);

        //Load Image
        Texture2D texture2d = new Texture2D(8, 8);
        Sprite sprite = null;
        if (texture2d.LoadImage(bytes))
        {
            sprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), Vector2.zero);
        }
        if (sprite != null)
        {
            DiamondImage.GetComponent<Image>().sprite = sprite;
        }
         if (www.isError)
         {

             UnityEngine.Debug.Log("Error while Receiving: " + www.error);
         }
         else
         {
             UnityEngine.Debug.Log("Success");

             //Load Image
             Texture2D texture2d = new Texture2D(8, 8);
             Sprite sprite = null;
             if (texture2d.LoadImage(handle.data))
             {
                 sprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), Vector2.zero);
             }
             if (sprite != null)
             {
                 DiamondImage.GetComponent<Image>().sprite = sprite;
             }
         }
    }*/
    byte[] downloadFullData(HttpWebRequest request)
    {
        using (WebResponse response = request.GetResponse())
        {

            if (response == null)
            {
                return null;
            }

            using (Stream input = response.GetResponseStream())
            {
                byte[] buffer = new byte[16 * 1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while (input.CanRead && (read = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    return ms.ToArray();
                }
            }
        }
    }
    string authenticate(string username, string password)
    {
        string auth = username + ":" + password;
        auth = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
        auth = "Basic " + auth;
        return auth;
    }
    IEnumerator DownloadImage(string url)
    {
        yield return new WaitForSeconds(0);
       

          StartCoroutine(ImageRequest(url, (UnityWebRequest req) =>
          {
              if (req.result == UnityWebRequest.Result.ConnectionError || req.result == UnityWebRequest.Result.ProtocolError)
              {
                  Debug.Log($"{req.error}: {req.downloadHandler.text}");
              }
              else
              {
                  // Get the texture out using a helper downloadhandler
                  Texture2D texture = DownloadHandlerTexture.GetContent(req);
                  // Save it into the Image UI's sprite
                  DiamondImage.GetComponent<Image>().sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
              }
          }));
    }
   


    IEnumerator ImageRequest(string url, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest req = UnityWebRequestTexture.GetTexture(url))
        {
            yield return req.SendWebRequest();
            callback(req);
        }
    }

}
[Serializable]
public class OfferFormData
{
    public string user_id;
    public string diamond_id;
    public string free_fire_id;
    public OfferFormData(string user_id,string diamond_id,string free_fire_id)
    {
        this.user_id = user_id;
        this.diamond_id = "8";
        this.free_fire_id = "123456789";
    }
}

