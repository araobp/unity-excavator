using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class HeightMapSwapper
{

    Terrain terrain;
    int heightmapWidth;
    int heightmapHeight;

    public HeightMapSwapper(Terrain terrain)
    {
        this.terrain = terrain;
        heightmapWidth = terrain.terrainData.heightmapWidth;
        heightmapHeight = terrain.terrainData.heightmapHeight;
    }

    private TerrainData getHeightMap()
    {
        return terrain.terrainData;
    }

    public void SaveTerrainData(string id)
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream($"./SerializedHeightMaps/{id}", FileMode.Create, FileAccess.Write);
        float[,] heights = terrain.terrainData.GetHeights(0, 0, heightmapWidth, heightmapHeight);
        formatter.Serialize(stream, heights);
        stream.Close();
    }

    public void LoadTerrainData(string id)
    {
        IFormatter formatter = new BinaryFormatter();
        Stream stream = new FileStream($"./SerializedHeightMaps/{id}", FileMode.Open, FileAccess.Read);
        float[,] heights = (float[,])formatter.Deserialize(stream);
        stream.Close();
        terrain.terrainData.SetHeights(0, 0, heights);
    }

    public List<string> listOfTerrainData()
    {
        DirectoryInfo d = new DirectoryInfo($"./SerializedHeightMaps");
        FileInfo[] Files = d.GetFiles("*.ser");
        List<string> list = new List<string>();
        foreach (FileInfo file in Files)
        {
            string filename = file.Name;
            filename = filename.Remove(filename.Length - 4);
            list.Add(filename);
        }
        return list;
    }
}
