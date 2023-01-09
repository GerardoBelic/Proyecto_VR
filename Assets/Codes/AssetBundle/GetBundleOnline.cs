using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetBundleOnline : MonoBehaviour
{

    void Start() {
        StartCoroutine(GetAssetBundle());
        //GetAssetBundle();
    }
 
    IEnumerator GetAssetBundle()
    {
        UnityWebRequest www;

        if (GlobalStateQR.qr_text != null)
            www = UnityWebRequestAssetBundle.GetAssetBundle(GlobalStateQR.qr_text);
        else
            www = UnityWebRequestAssetBundle.GetAssetBundle("https://drive.google.com/uc?id=1iqEgDayUrFP9Nef17Wfy1PWN3QbEWGXx&export=download");

        yield return www.SendWebRequest();
 
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Hola");
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);


            Object obj = bundle.LoadAsset("House");
            var instance = Instantiate(obj);
            instance.name = "House";

            bundle.Unload(false);
        }
    }

    

}
