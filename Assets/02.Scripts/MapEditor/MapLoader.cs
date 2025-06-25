using UnityEngine;
using System.IO;
using System.Collections.Generic;
using static Define;

public class MapLoader
{
    string jsonFilePath = "Assets/mapData.json"; // 경로 수정 가능
    public List<MapData> m_StageDataList = new List<MapData>();

    public void Init()
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
        m_StageDataList.Add(mapData);
    }
}
