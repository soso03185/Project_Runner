using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using static Define;

/// <summary>
/// 게임을 시작했을때 맵의 Tile과 Item들을 생성해주는 Generator
/// </summary>
/// 
public class MapGenerator : Singleton<MapGenerator>
{
    private List<MapData> m_stageData;

    public override void Awake()
    {
        base.Awake();
    }

    public void Init()
    {

    }
}