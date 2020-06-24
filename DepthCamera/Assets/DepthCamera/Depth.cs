using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

public class Depth : MonoBehaviour
{
    public Material mat;

    // Capture an image from a camera
    public void Capture()
    {
        Camera camera = GetComponent<Camera>();
        RenderTexture activeRenderTexture = RenderTexture.active;
        RenderTexture.active = camera.targetTexture;

        camera.Render();

        try
        {
            Texture2D image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height);
            image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
            image.Apply();
            RenderTexture.active = activeRenderTexture;

            byte[] bytes = image.EncodeToJPG();
            Destroy(image);
            File.WriteAllBytes($"{Application.dataPath}/Capture/depth.png", bytes);
        } catch (NullReferenceException e)
        {
        }
    }

    public void Start()
    {
        GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
    }

    public void OnRenderImage(RenderTexture source, RenderTexture dest)
    {
        Graphics.Blit(source, dest, mat);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)) Capture();
    }
}