using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEditor;
using LobbyScripts;


public class UploadFile : MonoBehaviour 
{

    public Image profileImage;
    string imageString;
    string uploadImageURL = "http://216.48.182.176:5000/uploadImage";
    private Texture2D profileSelectedImage;
    public Image QRcode;
    string imagePath = "";

    public void Start()
    {
        
    }

    public void openFileExplorer()
    {
        // var paths =  EditorUtility.OpenFilePanel("Title", "", "png");
        // if (paths.Length > 0)
        // {
        //     StartCoroutine(OutputRoutine(new System.Uri(paths).AbsoluteUri));
        // }
    }

    // private IEnumerator OutputRoutine(string url)
    // {
    //     var loader = new WWW(url);
    //     yield return loader;
    //     rawImage.texture = loader.texture;
    //     Texture2D rawImageTexture = (Texture2D)rawImage.texture;
    //     byte[] pngData = rawImageTexture.EncodeToPNG();
    //     imageString = Convert.ToBase64String(pngData);
    //     Debug.Log( "pngData  " + pngData + " imageString " + imageString );
    //     // StartCoroutine(_webObj.SendImageToDB( _globalPlayerName.text, imageString, "https://www.interactivearvr.com/ameritex/racinggame_pc/SendImageToDB.php"));
    //     // StartCoroutine(SendImageToDB( "monica", imageString, uploadImageURL));
    //     StartCoroutine(UploadImage(rawImageTexture));
    // }

    // public IEnumerator SendImageToDB(string uname, string img, string uri)
    // {
    //     WWWForm form = new WWWForm();
    //     form.AddField("uname", uname);
    //     form.AddField("img", img);

    //     using (UnityWebRequest www = UnityWebRequest.Post(uri, form))
    //     {
    //         yield return www.SendWebRequest();

    //         if (www.result != UnityWebRequest.Result.Success)
    //         {
    //             Debug.LogError(www.error);
    //         }
    //         else
    //         {
    //             Debug.LogError(www.downloadHandler.text);
    //         }
    //     }
    // }

    // public void OnClickUploadImageButton()
    // {
    //     // NativeGallery.Permission per = NativeGallery.RequestPermission();
 
    //     // probably should do some permissions checking here
 
    //     NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
    //     {
    //         if (path != null)
    //         {
    //             // Create Texture from selected image
 
    //             Texture2D texture = NativeGallery.LoadImageAtPath(path, 2073600, false);
 
    //             if (texture == null) return;
 
    //             StartCoroutine(UploadImage(texture));
    //         }
    //     }, "Select an image", "image/*");
    // }

    // IEnumerator UploadImage(Texture2D theimage)
    // {
    //     var form = new WWWForm();
    //     form.AddBinaryData("image", theimage.EncodeToPNG(), "uploadedimage.png", "image/png");
 
    //     UnityWebRequestAsyncOperation www = UnityWebRequest.Post(uploadImageURL, form).SendWebRequest();

    //     yield return www;

    //     if (www.webRequest.isHttpError || www.webRequest.isNetworkError)
    //     {
    //         Debug.LogError("Error: " + www.webRequest.error);
    //     }
    //     else
    //     {
    //         Debug.LogError("Done!");
    //     }
    // }

    // public void UploadImage()
    // {
    //     StartCoroutine(UploadAFile(uploadImageURL));
    // }

    //Our coroutine takes a screenshot of the game
    // public IEnumerator UploadAFile(string uploadUrl)
    // {

    //     yield return new WaitForEndOfFrame();

    //     int width = rawImage.texture.width;
    //     int height = rawImage.texture.height;
    //     Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
    //     tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
    //     tex.Apply();
    //     //This basically takes a screenshot

    //     byte[] bytes = tex.EncodeToPNG(); //Can also encode to jpg, just make sure to change the file extensions down below
    //     Destroy(tex);

    //     // Create a Web Form, this will be our POST method's data
    //     var form = new WWWForm();
    //     form.AddField("somefield", "somedata");
    //     form.AddBinaryData("image", bytes, "screenshot.png", "image/png");

    // //POST the screenshot to GameSparks
    //     WWW w = new WWW(uploadUrl, form);
    //     yield return w;

    //     if (w.error != null)
    //     {
    //         Debug.Log(w.error);
    //     }
    //     else
    //     {
    //         Debug.Log(w.text);
    //     }
    // }

    // public void UploadImageServer()
    // {
    //     GetImage(512);
    // }

    // public Texture2D GetImage(int size)
    // {
    //     NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
    //     {
    //         if (path != null)
    //         {

    //             profileSelectedImage = GetImageFromGallary(size, path);
    //             if (profileSelectedImage == null)
    //             {
    //                 return;
    //             }
    //             profileImage.overrideSprite = Sprite.Create((Texture2D)profileSelectedImage, new Rect(0, 0, profileSelectedImage.width, profileSelectedImage.height), new Vector2(0.5f, 0.5f));
    //             UploadPROFILEIMAGEAPI(profileSelectedImage);
    //         }
    //     }, "Select a image", "image/png");

    //     return profileSelectedImage;
    // }

    // private void UploadPROFILEIMAGEAPI(Texture2D myTexture)
    // {
    //     ImageUploaderProfile
    //         .Initialize()
    //         .SetUrl(uploadImageURL)
    //         .SetUserID("monica")
    //         .SetTexture(myTexture)
    //         .SetFieldName("image")
    //         .SetFileName("imagename")
    //         .SetType(ImageTypeProfile.PNG)
    //         .OnError(error => Debug.Log(error))
    //         .OnComplete(text => Debug.Log(text))
    //         .Upload();
    // }

    // public void ChangeQRcode(Sprite _qrsprite)
    // {
    //     QRcode.sprite = _qrsprite;
    // }

    public void UpdateImage()
    {
        Debug.Log("Update Image... ");
        GetImageProfile(512);
    }

    private Texture2D GetImageFromGallary(int maxSize, string path)
    {
        Debug.Log("path  " + path);
        return NativeGallery.LoadImageAtPath(path, maxSize, false);
    }

    public void GetImageProfile(int size)
    {
        Debug.Log("GetImagePrfile... ");
        profileSelectedImage = null;
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            if (path != null)
            {
                profileSelectedImage = GetImageFromGallary(size, path);
                imagePath = Application.persistentDataPath + "/testTexture.png";
                if (profileSelectedImage == null)
                {
                    return;
                }
                profileImage.sprite = Sprite.Create((Texture2D)profileSelectedImage,
                    new Rect(0, 0, profileSelectedImage.width, profileSelectedImage.height), new Vector2(0.5f, 0.5f));
                File.Copy(path, imagePath, true);
                WWWForm form = new WWWForm();
                byte[] textureBytes = null;
                //Get a copy of the texture, because we can't access original texure data directly.
                Texture2D imageTexture_copy = GetTextureCopy(profileSelectedImage);
                //image file extension
                string extension = ImageTypeProfile.PNG.ToString().ToLower();
                textureBytes = imageTexture_copy.EncodeToPNG();
                form.AddField("username", "monica");
                form.AddBinaryData("avatar", textureBytes, "image" + "." + extension, "image/" + extension);
                ImageUploaderProfile
            .Initialize()
            .SetUrl(Constants.UPLOAD_PROFILE_URL)
            .SetForm(form)
            .OnError(error => UnityEngine.Debug.Log(error))
            .OnComplete(text => UnityEngine.Debug.Log(text))
            .Upload(status => UnityEngine.Debug.Log("profiel image uploaded with status " + status));

            }
        }, "Select a image", "image/png");
        //return profileSelectedImage;
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