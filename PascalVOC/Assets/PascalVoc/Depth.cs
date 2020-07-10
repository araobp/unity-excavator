using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depth : MonoBehaviour
{
    public Material mat;

    public void OnRenderImage(RenderTexture source, RenderTexture dest)
    {
        Graphics.Blit(source, dest, mat);
    }

}
