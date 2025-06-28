using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static Define;

/// <summary>
/// ������ ���������� ���� Tile�� Item���� �������ִ� Generator
/// </summary>
/// 
public class MapGenerator : Singleton<MapGenerator>
{    
    private MapLoader m_MapLoader;
    private List<MapData> m_stageData;

    public override void Awake()
    {
        base.Awake();
        m_MapLoader = new MapLoader();
        m_MapLoader.Init(); // Data Load
    }

    public void Init()
    {
        m_stageData = m_MapLoader.m_StageDataList;
        StageCreate(1);
    }

    void StageCreate(int stage)
    {
        if (stage > m_stageData.Count)
            DEBUG_ERROR("over Stage Index");
        
        stage--;

        Transform tileParent = new GameObject().transform;
        tileParent.name = $"Tiles.Parent";

        foreach (var tile in m_stageData[stage].tiles) // "Tile"
        {
            GameObject go = ResourceManager.Instance.InstantiatePrefab(tile.PrefabName);
            go.transform.position = tile.position;            
            go.transform.parent = tileParent;
        }

        Transform objParent = new GameObject().transform;
        objParent.name = $"Object.Parent";

        foreach (var obj in m_stageData[stage].objects) // "Object"
        {
            Transform tr = ResourceManager.Instance.InstantiatePrefab(obj.PrefabName).transform;
            LaneObject lane = tr.GetComponent<LaneObject>();

            lane.Init();
            lane.m_CurrentLane = obj.laneNum;
            tr.position = obj.position;
            tr.parent = objParent;
        }
    }
}
