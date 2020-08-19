using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;

public class TerrainSwapper : EditorWindow
{

    private string terrainName = "Unknown";

    static HeightMapSwapper swapper;
    
    [MenuItem("Terrain/Swapper", false, 100)]
    static void OpenWindow()
    {
        GetWindow<TerrainSwapper>(true);
    }

    void OnGUI()
    {
        try
        {
            Terrain terrain = Selection.activeGameObject.GetComponent<Terrain>();
            swapper = new HeightMapSwapper(terrain);

            terrainName = EditorGUILayout.TextField("Terrain Name", terrainName);

            List<string> list = swapper.listOfTerrainData();

            if (GUILayout.Button("Save"))
            {
                swapper.SaveTerrainData(terrainName + ".ser");
            }

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Terrain Heightmaps");

            foreach (string filename in list)
            {
                if (GUILayout.Button(filename))
                {
                    swapper.LoadTerrainData(filename + ".ser");
                }
            }
        } catch (Exception e)
        {
            EditorGUILayout.LabelField("Please select your terrain!");
        }

    }
}