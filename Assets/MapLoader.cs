using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class MapLoader : MonoBehaviour
{
    public string jsonFilePath = "Assets/map.json"; // 경로 수정 가능

    void Start()
    {
        LoadMap(jsonFilePath);
    }

    public void LoadMap(string filePath)
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError($"Map file not found: {filePath}");
            return;
        }

        string json = File.ReadAllText(filePath);
        MapData mapData = JsonUtility.FromJson<MapData>(json);

        foreach (var tile in mapData.tiles)
        {
            Debug.Log($"[Tile] Name: {tile.PrefabName}, Pos: ({tile.position.x}, {tile.position.y}, {tile.position.z})");
        }

        foreach (var obj in mapData.objects)
        {
            Debug.Log($"[Object] Name: {obj.PrefabName}, Type: {obj.objectType}, Pos: ({obj.position.x}, {obj.position.y}, {obj.position.z})");
        }
    }

    private static string ExtractPrefabName(string path)
    {
        if (string.IsNullOrEmpty(path)) return string.Empty;

        int lastSlash = path.LastIndexOf('/');
        int dot = path.LastIndexOf(".prefab");

        if (lastSlash >= 0 && dot > lastSlash)
            return path.Substring(lastSlash + 1, dot - lastSlash - 1);

        return path;
    }



    [System.Serializable]
    public class MapData
    {
        public int gridSizeX;
        public int gridSizeZ;
        public List<TileData> tiles;
        public List<ObjectData> objects;
    }

    [System.Serializable]
    public class TileData
    {
        public string prefabPath;
        public Vector3 position;

        public string PrefabName => ExtractPrefabName(prefabPath);
    }

    [System.Serializable]
    public class ObjectData
    {
        public string prefabPath;
        public Vector3 position;
        public string objectType;

        public string PrefabName => ExtractPrefabName(prefabPath);
    }
     
}
