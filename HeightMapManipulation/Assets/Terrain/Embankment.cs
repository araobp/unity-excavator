using UnityEngine;

[ExecuteInEditMode]
public class Embankment
{
    Terrain terrain;

    int heightmapWidth;
    int heightmapHeight;

    Vector3 terrainSize;
    Vector3 terrainPosition;

    float ratioX;
    float ratioZ;

    int offsetX;
    int offsetY;

    public Embankment(Terrain terrain)
    {
        this.terrain = terrain;
        heightmapWidth = terrain.terrainData.heightmapResolution;
        heightmapHeight = terrain.terrainData.heightmapResolution;
        terrainSize = terrain.terrainData.size;
        terrainPosition = terrain.transform.position;
        ratioX = heightmapWidth / terrainSize.x;
        ratioZ = heightmapHeight / terrainSize.z;
        offsetX = Mathf.RoundToInt(-terrainPosition.x * ratioX);
        offsetY = Mathf.RoundToInt(-terrainPosition.z * ratioZ);

        Debug.Log($"heightmapWidth: {heightmapWidth}, heightmapHeight: {heightmapHeight}");
        Debug.Log($"ratioX: {ratioX}, ratioY: {ratioZ}");
        Debug.Log($"offsetX: {offsetX}, offsetY: {offsetY}");
        Debug.Log($"terrainSize.x: {terrainSize.x}, terrainSize.y: {terrainSize.y}, terrainSize.z: {terrainSize.z}");
    }

    public void flattenTerrain(float height = 100F)
    {
        float[,] heights = terrain.terrainData.GetHeights(0, 0, heightmapWidth, heightmapHeight);
        float embankmentHeight = height / terrainSize.y;
        for (int y = 0; y < heightmapHeight; y++)
        {
            for (int x = 0; x < heightmapWidth; x++)
            {
                heights[x, y] = embankmentHeight;
            }
        }
        terrain.terrainData.SetHeights(0, 0, heights);
    }

    public void generateEmbankment(float left, float top, float right, float bottom, float height)
    {
        int x0 = Mathf.RoundToInt(left); //+ offsetX;
        int x1 = Mathf.RoundToInt(right); //+ offsetX;
        int y0 = Mathf.RoundToInt(bottom); //+ offsetY;
        int y1 = Mathf.RoundToInt(top); //+ offsetY;
        Debug.Log($"{x0}, {x1}, {y0}, {y1}");
        float[,] heights = terrain.terrainData.GetHeights(0, 0, heightmapWidth, heightmapHeight);

        float embankmentHeight = height / terrainSize.y;
        Debug.Log($"embankmentHeight: {embankmentHeight}");

        for (int x = y0; x < y1; x++)
        {
            for (int y = x0; y < x1; y++)
            {
                Debug.Log($"x: {x}, y: {y}");
                heights[x, y] = embankmentHeight;
            }
        }
        terrain.terrainData.SetHeights(0, 0, heights);
    }

}

