using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using UnityEngine.WSA;
using static Define;

/// <summary>
/// ������ ���������� ���� Tile�� Item���� �������ִ� Generator
/// </summary>
/// 
public class MapGenerator : Singleton<MapGenerator>
{    
    public Vector2 m_TileMatrix = Vector2.zero; // ��ü Ÿ���� ���
    public float m_TileOffset = 0.1f;           // Ÿ�� ���� ����

    private MapLoader m_MapLoader;
    private List<MapData> m_stageData;

    public override void Awake()
    {
        base.Awake();
        m_MapLoader = new MapLoader();
        m_MapLoader.Init();
    }

    private void Start()
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
            GameObject go = ResourceManager.Instance.InstantiatePrefab(obj.PrefabName);
            go.transform.position = obj.position;
            go.transform.parent = objParent;
        }
    }
}
