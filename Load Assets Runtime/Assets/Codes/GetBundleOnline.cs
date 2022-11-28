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
 
    IEnumerator GetAssetBundle() {
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle("https://dl.dropboxusercontent.com/s/7vgn2a3ljdfe58s/cuartonewvegas?dl=0");
        yield return www.SendWebRequest();
 
        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Hola");
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);


            Object obj = bundle.LoadAsset("Cuarto");
            Instantiate(obj);

            bundle.Unload(false);
        }
    }

    

}
