using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TestBuncle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Hola");
        //AssetBundle bundle = AssetBundle.LoadFromFile(Path.Combine(Application.streamingAssetsPath, "testing"));
        AssetBundle bundle = AssetBundle.LoadFromFile(@"Assets\StreamingAssets\testing");
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

        Object obj = bundle.LoadAsset("Cube");
        Instantiate(obj);

        bundle.Unload(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
