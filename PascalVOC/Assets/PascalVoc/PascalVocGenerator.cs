using System;
using System.IO;
using System.Xml;
using UnityEngine;


public class PascalVocGenerator : MonoBehaviour
{

    Camera cameraDepth;
    Camera cameraCapture;
    GameObject temporaryStage;
    GameObject[] allObjects;

    float left;
    float top;
    float right;
    float bottom;

    private void FindBoundingBox()
    {
        // Use the depth camera
        RenderTexture activeRenderTexture = RenderTexture.active;
        RenderTexture.active = cameraDepth.targetTexture;

        cameraDepth.Render();

        Texture2D image = new Texture2D(cameraDepth.targetTexture.width, cameraDepth.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, cameraDepth.targetTexture.width, cameraDepth.targetTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = activeRenderTexture;

        // Initialize bounding box coordinates
        left = image.width;
        top = 0;
        right = 0F;
        bottom = image.height;

        // Scan the depth image to find a bounding box 
        for (int y = 0; y < image.height; y++)
        {
            for (int x = 0; x < image.width; x++)
            {
                float px = image.GetPixel(x, y).r;
                if (px > 0)
                {
                    if (x < left) left = x;
                    if (x > right) right = x;
                    if (y > top) top = y;
                    if (y < bottom) bottom = y;
                }
            }
        }
        Debug.Log($"[BoundingBox] left: {left}, top: {top}, right: {right}, bottom: {bottom}");

        Destroy(image);

        // Draw the bounding box on the panel
        GameObject bndBox = GameObject.FindWithTag("BoundingBox");
        RectTransform rectTransform = bndBox.GetComponent<RectTransform>();
        rectTransform.position = new Vector3(left, bottom, 0);
        rectTransform.sizeDelta = new Vector2(right - left, top - bottom);
    }

    private void CaptureImage(string timestamp)
    {
        // Use the depth camera
        RenderTexture activeRenderTexture = RenderTexture.active;
        RenderTexture.active = cameraCapture.targetTexture;

        cameraCapture.Render();

        Texture2D image = new Texture2D(cameraCapture.targetTexture.width, cameraCapture.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, cameraCapture.targetTexture.width, cameraCapture.targetTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = activeRenderTexture;

        // Convert the image into JPEG format
        byte[] bytes = image.EncodeToJPG();

        Destroy(image);

        File.WriteAllBytes($"{Application.dataPath}/Capture/{timestamp}.jpg", bytes);
    }

    private void GeneratePascalVOC(string timestamp)
    {
        string pascalVoxXmlFilename = $"{Application.dataPath}/Capture/{timestamp}.xml";

        XmlDocument xml = new XmlDocument();
        


    }

    // Start is called before the first frame update
    void Start()
    {
        allObjects = FindObjectsOfType<GameObject>();
        cameraDepth = GameObject.FindWithTag("CameraDepth").GetComponent<Camera>();
        cameraCapture = GameObject.FindWithTag("CameraCapture").GetComponent<Camera>();
        temporaryStage = GameObject.FindWithTag("TemporaryStage");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.B))
        {
            temporaryStage.SetActive(true);

            foreach (GameObject obj in allObjects)
            {
                if (obj.transform.parent == null && obj.tag == "GreenScreen") obj.SetActive(false);
            }

            FindBoundingBox();

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

            GeneratePascalVOC(timestamp);

            foreach (GameObject obj in allObjects)
            {
                if (obj.transform.parent == null && obj.tag == "GreenScreen") obj.SetActive(true);
            }

            temporaryStage.SetActive(false);

            CaptureImage(timestamp);
        }

    }
}
