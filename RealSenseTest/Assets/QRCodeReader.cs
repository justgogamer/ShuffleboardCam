using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Events;
using UnityEngine.UI;
using ZXing;
using ZXing.Common;
using ZXing.Multi;
using ZXing.Multi.QrCode;

public class QRCodeReader : MonoBehaviour
{
    [SerializeField]
    private string lastResult;
    [SerializeField]
    private bool logAvailableWebcams;
    [SerializeField]
    private int selectedWebcamIndex;

    private WebCamTexture camTexture;
    private Color32[] cameraColorData;
    private int width, height;
    private Rect screenRect;
    [SerializeField] private DataCollector DataCollector;

    // create a reader with a custom luminance source
    private IBarcodeReader barcodeReader = new BarcodeReader
    {
        AutoRotate = false,
        Options = new ZXing.Common.DecodingOptions
        {
            TryHarder = false
        }
    };

    QRCodeMultiReader multiReader = new QRCodeMultiReader();

    //private Result result;
    private Result[] result;

    private void Start()
    {
        LogWebcamDevices();
        SetupWebcamTexture();
        PlayWebcamTexture();

        lastResult = "http://www.google.com";

        cameraColorData = new Color32[width * height];
        screenRect = new Rect(0, 0, Screen.width, Screen.height);
    }

    private void OnEnable()
    {
        PlayWebcamTexture();
    }

    private void OnDisable()
    {
        if (camTexture != null)
        {
            camTexture.Pause();
        }
    }

    private void Update()
    {
        if (camTexture.isPlaying)
        {
            // decoding from camera image
            camTexture.GetPixels32(cameraColorData); // -> performance heavy method 

            //Texture2D tx2d = GetTexture2dFromWebcam(camTexture);
            //LuminanceSource luminanceSource = new RGBLuminanceSource(tx2d.GetRawTextureData(), camTexture.width, camTexture.height);
            //Destroy(tx2d);
            //Binarizer binarizer = new HybridBinarizer(luminanceSource).createBinarizer(luminanceSource);
            //BinaryBitmap bitmap = new (binarizer);

            result = barcodeReader.DecodeMultiple(cameraColorData, width, height); // -> performance heavy method
            //result = multiReader.decodeMultiple(bitmap); // -> performance heavy method

            if (result != null)
            {
                Debug.Log($"Found something! {result}");
                //lastResult = result.Text + " " + result.BarcodeFormat;
                //print(lastResult);
                DataCollector collector = DataCollector.GetComponent<DataCollector>();
                collector.screenWidth = width;
                collector.screenHeight = height;
                collector.incomingMultipleQRMessages(result);
            }
        }
    }

    private void OnGUI()
    {
        // show camera image on screen
        GUI.DrawTexture(screenRect, camTexture, ScaleMode.ScaleToFit);
        // show decoded text on screen
        GUI.TextField(new Rect(10, 10, 256, 25), lastResult);
    }

    private void OnDestroy()
    {
        camTexture.Stop();
    }

    private void LogWebcamDevices()
    {
        if (logAvailableWebcams)
        {
            WebCamDevice[] devices = WebCamTexture.devices;
            for (int i = 0; i < devices.Length; i++)
            {
                Debug.Log(devices[i].name);
            }
        }
    }

    private void SetupWebcamTexture()
    {
        string selectedWebcamDeviceName = WebCamTexture.devices[selectedWebcamIndex].name;
        camTexture = new WebCamTexture(selectedWebcamDeviceName);
        camTexture.requestedHeight = Screen.height;
        camTexture.requestedWidth = Screen.width;
    }
 
    private void PlayWebcamTexture()
    {
        if (camTexture != null)
        {
            Debug.Log(camTexture.deviceName);
            camTexture.Play();
            width = camTexture.width;
            height = camTexture.height;
        }
    }

    public Texture2D GetTexture2dFromWebcam(WebCamTexture _WebcamTexture)
    {
        Texture2D _CamTexture2D = new Texture2D(_WebcamTexture.width, _WebcamTexture.height);
        _CamTexture2D.SetPixels32(_WebcamTexture.GetPixels32());
        _CamTexture2D.Apply(); //If you want to see the result, you need to apply the changes

        Resources.UnloadUnusedAssets(); //Clean the memory or you will make your pc crash

        return _CamTexture2D;
    }
}