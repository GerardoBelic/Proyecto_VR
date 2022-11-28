using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GetBundleLocal : MonoBehaviour
{
    void Start()
    {
        //AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "testing"));
        AssetBundle bundle = AssetBundle.LoadFromFile(@"Assets\Temp\cuartonewvegas");
        if (bundle == null)
        {
            return;
        }

        /*Object[] objs = bundle.LoadAllAssets();
        foreach (Object obj in objs)
        {
            Debug.Log(obj.ToString());
            Instantiate(obj);
        }*/

        Object obj = bundle.LoadAsset("Cuarto");
        Instantiate(obj);

        bundle.Unload(false);
    }

    void Update()
    {
        
    }
}
