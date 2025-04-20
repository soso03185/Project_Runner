using System;
using UnityEngine;

/// <summary>
/// 게임을 시작했을때 맵의 Tile과 Item들을 생성해주는 Generator
/// </summary>
/// 
public class MapGenerator : MonoBehaviour
{
    public Vector2 m_TileMatrix = Vector2.zero; // 전체 타일의 행렬
    public float m_TileOffset = 0.1f;           // 타일 간의 간격
    string N_TileName = "NormalTile";           // 일반 타일

    private void Start()
    {
        SpawnTileAtPosition();
    }

    void SpawnTileAtPosition()
    {
        GameObject tilePrefab = Resources.Load<GameObject>($"Prefabs/{N_TileName}");

        if (tilePrefab == null)
        {
            Debug.LogError($"Prefabs 폴더에 '{N_TileName}' 프리팹이 존재하지 않습니다.");
            return;
        }

        GameObject emptyObj = new GameObject("EmptyObject");

        float tileScaleX = tilePrefab.transform.localScale.x + m_TileOffset;
        int centerOffset = (int)m_TileMatrix.x / 2;

        for (int i = 0; i < m_TileMatrix.x; i++)
        {
            for (int j = 0; j < m_TileMatrix.y; j++)
            {                
                Vector3 spawnPosition = new Vector3((tileScaleX * i) - (tileScaleX * centerOffset), 0f, tileScaleX * j);
                Instantiate(tilePrefab, spawnPosition, Quaternion.identity, emptyObj.transform);
            }
        }
    }
}
