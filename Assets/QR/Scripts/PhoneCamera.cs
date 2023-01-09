using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;
using System;
using UnityEngine.SceneManagement;

/// From https://www.youtube.com/watch?v=c6NXkZWXHnc

public class PhoneCamera : MonoBehaviour
{

    private bool camAvailable = false;
    private WebCamTexture backCam;
    private Texture defaultBackground;

    public RawImage background;
    public AspectRatioFitter fit;

    string QrCode = string.Empty;

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

        StartCoroutine(GetQRCode());
    }

    void Update()
    {
        if (!camAvailable)
            return;

        float ratio = (float)backCam.width / (float)backCam.height;
        fit.aspectRatio = ratio;

        float scaleY = backCam.videoVerticallyMirrored ? -1.0f : 1.0f;
        //background.rectTransform.localScale = new Vector3(1.0f, scaleY, 1.0f);
        background.rectTransform.localScale = new Vector3(1.0f, scaleY, 1.0f)*ratio*1.2f;

        int orient = -backCam.videoRotationAngle;
        background.rectTransform.localEulerAngles = new Vector3(0.0f, 0.0f, orient);
    }

    IEnumerator GetQRCode()
    {
        IBarcodeReader barCodeReader = new BarcodeReader();
        backCam.Play();
        var snap = new Texture2D(backCam.width, backCam.height, TextureFormat.ARGB32, false);
        while (string.IsNullOrEmpty(QrCode))
        {
            try
            {
                snap.SetPixels32(backCam.GetPixels32());
                var Result = barCodeReader.Decode(snap.GetRawTextureData(), backCam.width, backCam.height, RGBLuminanceSource.BitmapFormat.ARGB32);
                if (Result != null)
                {
                    QrCode = Result.Text;
                    if (!string.IsNullOrEmpty(QrCode))
                    {
                        Debug.Log("DECODED TEXT FROM QR: " + QrCode);
                        break;
                    }
                }
            }
            catch (Exception ex) { Debug.LogWarning(ex.Message); }
            yield return null;
        }
        backCam.Stop();
        GlobalStateQR.qr_text = QrCode;
        SceneManager.LoadScene("Assets/Scenes/House Edition.unity", LoadSceneMode.Single);
    }
    
    /*private void OnGUI()
    {
        int w = Screen.width, h = Screen.height;

        GUIStyle style = new GUIStyle();

        Rect rect = new Rect(0, 0, w, h );
        style.alignment = TextAnchor.MiddleCenter;
        style.fontSize = h * 2 / 50;
        style.normal.textColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        string text = QrCode;
        GUI.Label(rect, text, style);
    }*/

}
