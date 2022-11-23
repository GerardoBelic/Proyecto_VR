using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// From https://www.youtube.com/watch?v=c6NXkZWXHnc

public class PhoneCamera : MonoBehaviour
{

    private bool camAvailable = false;
    private WebCamTexture backCam;
    private Texture defaultBackground;

    public RawImage background;
    public AspectRatioFitter fit;

    void Start()
    {
        defaultBackground = background.texture;
        WebCamDevice[] devices = WebCamTexture.devices;

        if (devices.Length == 0)
            return;


        for (int i = 0; i < devices.Length; ++i)
        {
            if (!devices[i].isFrontFacing)
            {
                backCam = new WebCamTexture(devices[i].name, Screen.width, Screen.height);
            }
        }

        if (backCam == null)
            backCam = new WebCamTexture(devices[0].name, Screen.width, Screen.height);

        backCam.Play();
        background.texture = backCam;

        camAvailable = true;
    }

    void Update()
    {
        if (!camAvailable)
            return;

        float ratio = (float)backCam.width / (float)backCam.height;
        //float ratio = (float)Screen.width / (float)Screen.height;
        fit.aspectRatio = ratio;

        float scaleY = backCam.videoVerticallyMirrored ? -1.0f : 1.0f;
        //background.rectTransform.localScale = new Vector3(1.0f, scaleY, 1.0f);
        background.rectTransform.localScale = new Vector3(1.0f, scaleY, 1.0f)*ratio;

        int orient = -backCam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0.0f, 0.0f, orient);
    }
}
