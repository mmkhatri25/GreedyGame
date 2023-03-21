using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.Networking;
using System;
using System.IO;
using LobbyScripts;

public enum ImageTypeProfile
{
    PNG,
    JPG
}

public class ImageUploaderProfile : MonoBehaviour
{
    Texture2D imageTexture;
    string fieldName;
    string fileName = "defaultImageName";
    ImageTypeProfile ImageTypeProfile = ImageTypeProfile.PNG;
    string url;

    string userID;

    //Events
    UnityAction<string> OnErrorAction;
    UnityAction<string> OnCompleteAction;

    public static ImageUploaderProfile Initialize()
    {
        return new GameObject("ImageUploaderProfile").AddComponent<ImageUploaderProfile>();
    }

    public ImageUploaderProfile SetUrl(string serverUrl)
    {
        this.url = serverUrl;
        return this;
    }

    public ImageUploaderProfile SetUserID(string userId)
    {
        this.userID = userId;
        return this;
    }

    public ImageUploaderProfile SetTexture(Texture2D texture)
    {
        this.imageTexture = texture;
        return this;
    }

    public ImageUploaderProfile SetFileName(string filename)
    {
        this.fileName = filename;
        return this;
    }

    public ImageUploaderProfile SetFieldName(string fieldName)
    {
        this.fieldName = fieldName;
        return this;
    }

    public ImageUploaderProfile SetType(ImageTypeProfile type)
    {
        this.ImageTypeProfile = type;
        return this;
    }
    //events
    public ImageUploaderProfile OnError(UnityAction<string> action)
    {
        this.OnErrorAction = action;
        return this;
    }

    WWWForm form;
    public ImageUploaderProfile SetForm(WWWForm form)
    {
        this.form = form;
        return this;
    }

    public ImageUploaderProfile OnComplete(UnityAction<string> action)
    {
        this.OnCompleteAction = action;
        return this;
    }

    public void Upload()
    {
        //check/validate fields
        if (url == null)
            Debug.LogError("Url is not assigned, use SetUrl( url ) to set it. ");
        //...other checks...
        //...

        StopAllCoroutines();
        StartCoroutine(StartUploading());
    }

    public void Upload(Action<bool> success)
    {
        //check/validate fields
        if (url == null)
        {
            Debug.LogError("Url is not assigned, use SetUrl( url ) to set it. ");
            return;
        }
        if (form == null)
        {
            Debug.LogError("Form is not assign ");
            return;
        }
        //...other checks...
        //...

        StopAllCoroutines();
        StartCoroutine(StartUploading(success));
    }

    IEnumerator StartUploading()
    {
        WWWForm form = new WWWForm();
        byte[] textureBytes = null;

        //Get a copy of the texture, because we can't access original texure data directly.
        Texture2D imageTexture_copy = GetTextureCopy(imageTexture);

        switch (ImageTypeProfile)
        {
            case ImageTypeProfile.PNG:
                textureBytes = imageTexture_copy.EncodeToPNG();
                break;
            case ImageTypeProfile.JPG:
                textureBytes = imageTexture_copy.EncodeToJPG();
                break;
        }

        //image file extension
        string extension = ImageTypeProfile.ToString().ToLower();

        // form.AddField("user_id", userID);

        form.AddBinaryData(fieldName, textureBytes, fileName + "." + extension, "image/" + extension);

        Debug.Log(" url:  " + url + "  Request  " + userID);

        // WWW w = new WWW(url, form);
        using (UnityWebRequest w = UnityWebRequest.Post(url, form))
        {
            yield return w;

            if (w.error != null)
            {
                //error :
                if (OnErrorAction != null)
                {
                    OnErrorAction(w.error); //or OnErrorAction.Invoke (w.error);
                    Debug.Log("  url:  " + url + "  Error  " + w.error);
                }
            }
            else
            {
                //success
                if (OnCompleteAction != null)
                {
                    OnCompleteAction(w.downloadHandler.text);
                    Debug.Log(" url:  " + url + "  Response  " + w.downloadHandler.text);
                }
                //or OnCompleteAction.Invoke (w.error);
            }
            w.Dispose();
            Destroy(this.gameObject);
        }

    }

    IEnumerator StartUploading(Action<bool> success)
    {

        UnityEngine.Debug.Log("Sending req:" + url);
        string s = Newtonsoft.Json.JsonConvert.SerializeObject(form);
        UnityEngine.Debug.Log(s);
        WWW w = new WWW(this.url, this.form);

        yield return w;

        if (w.error != null)
        {
            //error : 
            if (OnErrorAction != null)
            {
                OnErrorAction(w.error); //or OnErrorAction.Invoke (w.error);
                UnityEngine.Debug.Log(" url:  " + url + "  Error  " + w.error);
                //BackEndData<emptyClass> res = Newtonsoft.Json.JsonConvert.DeserializeObject<BackEndData<emptyClass>>(w.error);
                //AndroidToastMsg.ShowAndroidToastMessage(res.message);

            }
            success(false);
        }
        else
        {

            //success
            if (OnCompleteAction != null)
            {

                OnCompleteAction(w.text);
                UnityEngine.Debug.Log("url:  " + url + "  Response  " + w.text);

                if (w.text != null)
                {
                    try
                    {

                    BackEndData<emptyClass> res = Newtonsoft.Json.JsonConvert.DeserializeObject<BackEndData<emptyClass>>(w.text);
                    UnityEngine.Debug.Log(res.message);

                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.Log(e.Message);
                        UnityEngine.Debug.Log(e.StackTrace);
                    }
                }


            }
            success(true);
            //or OnCompleteAction.Invoke (w.error);
        }
        w.Dispose();
        Destroy(this.gameObject);
    }

    public Texture2D GetTextureCopy(Texture2D source)
    {
        //Create a RenderTexture
        RenderTexture rt = RenderTexture.GetTemporary(
                               source.width,
                               source.height,
                               0,
                               RenderTextureFormat.Default,
                               RenderTextureReadWrite.Linear
                           );

        //Copy source texture to the new render (RenderTexture)
        Graphics.Blit(source, rt);

        //Store the active RenderTexture & activate new created one (rt)
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = rt;

        //Create new Texture2D and fill its pixels from rt and apply changes.
        Texture2D readableTexture = new Texture2D(source.width, source.height);
        readableTexture.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        readableTexture.Apply();

        //activate the (previous) RenderTexture and release texture created with (GetTemporary( ) ..)
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(rt);

        return readableTexture;
    }
}
