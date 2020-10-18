using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCam : MonoBehaviour
{
    public Material material;

    private static int INPUT_SIZE_WIDTH = 1024;
    private static int INPUT_SIZE_HEIGHT = 720;
    private static int FPS = 20;

    // UI
    RawImage rawImage;
    WebCamTexture webCamTexture;

    // Start is called before the first frame update
    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;
        for (int i = 0; i < devices.Length; i++) {
            Debug.Log(devices[i].name);
        }

        // Webカメラの開始
        this.rawImage = GetComponent<RawImage>();
        this.webCamTexture = new WebCamTexture(INPUT_SIZE_WIDTH, INPUT_SIZE_HEIGHT, FPS);

        material.mainTexture = this.webCamTexture;

        this.rawImage.texture = this.webCamTexture;
        this.rawImage.material = material;

        this.webCamTexture.Play();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
