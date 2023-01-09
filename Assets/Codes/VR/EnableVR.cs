using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Management;
using UnityEngine.XR;

public class EnableVR : MonoBehaviour
{
    public IEnumerator StartXR()
    {
        Debug.Log("Initializing XR...");
        yield return XRGeneralSettings.Instance.Manager.InitializeLoader();

        if (XRGeneralSettings.Instance.Manager.activeLoader == null)
        {
            Debug.LogError("Initializing XR Failed. Check Editor or Player log for details.");
        }
        else
        {
            Debug.Log("Starting XR...");
            XRGeneralSettings.Instance.Manager.StartSubsystems();
        }
    }    

    public void StopXR()
    {
        Debug.Log("Stopping XR...");

        XRGeneralSettings.Instance.Manager.StopSubsystems();
        XRGeneralSettings.Instance.Manager.DeinitializeLoader();
        Debug.Log("XR stopped completely.");
    }

    IEnumerator LoadDevice(string newDevice, bool enable)
    {
        XRSettings.LoadDeviceByName(newDevice);
        yield return null;
        XRSettings.enabled = enable;
    }

    void EnableVR_Co()
    {
        StartCoroutine(LoadDevice("daydream", true));
    }

    void DisableVR_Co()
    {
        StartCoroutine(LoadDevice("", false));
    }

    // Start is called before the first frame update
    void Start()
    {
        //UnityEngine.XR.XRSettings.enabled = true;
        //XRGeneralSettings.Instance.Manager.InitializeLoader();
        //XRGeneralSettings.Instance.Manager.StartSubsystems();

        //StartCoroutine(StartXR());
        StartXR();
        EnableVR_Co();
    }

    // Update is called once per frame
    void Update()
    {
        StartXR();
        EnableVR_Co();
    }
}
