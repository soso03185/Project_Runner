// MapEditorWindow.cs
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class MapEditorWindow : EditorWindow
{
    private GameObject selectedPrefab;
    private List<GameObject> placedObjects = new();
    private Vector2 scrollPos;

    [MenuItem("Tools/Map Editor2")]
    public static void ShowWindow()
    {
        GetWindow<MapEditorWindow>("Map Editor");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Map Editor Tool", EditorStyles.boldLabel);
        selectedPrefab = (GameObject)EditorGUILayout.ObjectField("Select Prefab", selectedPrefab, typeof(GameObject), false);

        if (GUILayout.Button("Place Prefab") && selectedPrefab != null)
        {
            PlacePrefab();
        }

        if (GUILayout.Button("Save Map"))
        {
            SaveMap();
        }

        if (GUILayout.Button("Clear Scene Objects"))
        {
            ClearPlacedObjects();
        }

        EditorGUILayout.Space();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        EditorGUILayout.LabelField("Placed Objects:", EditorStyles.boldLabel);

        foreach (var obj in placedObjects)
        {
            if (obj != null)
                EditorGUILayout.LabelField(obj.name);
        }

        EditorGUILayout.EndScrollView();
    }

    private void PlacePrefab()
    {
        GameObject obj = (GameObject)PrefabUtility.InstantiatePrefab(selectedPrefab);
        obj.transform.position = Vector3.zero;
        Undo.RegisterCreatedObjectUndo(obj, "Place Prefab");
        placedObjects.Add(obj);
    }

    private void ClearPlacedObjects()
    {
        foreach (var obj in placedObjects)
        {
            if (obj != null)
                DestroyImmediate(obj);
        }
        placedObjects.Clear();
    }

    private void SaveMap()
    {
        MapData data = new();

        foreach (var obj in placedObjects)
        {
            if (obj == null) continue;

            MapObjectInfo info = new()
            {
                prefabName = obj.name.Replace("(Clone)", "").Trim(),
                position = obj.transform.position,
                rotation = obj.transform.rotation.eulerAngles
            };
            data.objects.Add(info);
        }

        string json = JsonUtility.ToJson(data, true);
        string path = Application.dataPath + "/Resources/MapData.json";
        File.WriteAllText(path, json);
        AssetDatabase.Refresh();
        Debug.Log($"Map saved to {path}");
    }
}

// MapData.cs
[System.Serializable]
public class MapData
{
    public List<MapObjectInfo> objects = new();
}

[System.Serializable]
public class MapObjectInfo
{
    public string prefabName;
    public Vector3 position;
    public Vector3 rotation;
}
