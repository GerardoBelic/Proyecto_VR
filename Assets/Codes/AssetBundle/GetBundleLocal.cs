using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GetBundleLocal : MonoBehaviour
{
    void Start()
    {
        //AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "testing"));
        AssetBundle bundle = AssetBundle.LoadFromFile(@"Assets\StreamingAssets\housebundle");
        if (bundle == null)
        {
            print("error");
            return;
        }

        /*Object[] objs = bundle.LoadAllAssets();
        foreach (Object obj in objs)
        {
            Debug.Log(obj.ToString());
            Instantiate(obj);
        }*/

        foreach (string name in bundle.GetAllAssetNames())
        {
            print(name);
        }

        Object obj = bundle.LoadAsset("House");
        var instance = Instantiate(obj);
        instance.name = "House";

        bundle.Unload(false);
    }

    void Update()
    {
        
    }
}
