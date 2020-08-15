// [Source] https://wiki.unity3d.com/index.php/Object2Terrain

using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class MultipleTerrains : EditorWindow
{

    private string id = "unknown";
    private Vector2 scroll;
    private string text = "";

    static HeightMapSwapper swapper;
    
    [MenuItem("Terrain/Swapper", false, 2000)]
    static void OpenWindow()
    {
        EditorWindow.GetWindow<MultipleTerrains>(true);
    }

    void OnGUI()
    {
        Terrain terrain = Selection.activeGameObject.GetComponent<Terrain>();
        swapper = new HeightMapSwapper(terrain);

        id = EditorGUILayout.TextField("TerrainId", id);

        scroll = EditorGUILayout.BeginScrollView(scroll);
        List<string> list = swapper.listOfTerrainData();
        string text = string.Join("\n", list);
        text = EditorGUILayout.TextArea(text, GUILayout.Height(position.height - 30));
        EditorGUILayout.EndScrollView();

        if (GUILayout.Button("Save"))
        {
            swapper.SaveTerrainData(id + ".ser");
        }
        if (GUILayout.Button("Load"))
        {
            swapper.LoadTerrainData(id + ".ser");
        }

    }
}