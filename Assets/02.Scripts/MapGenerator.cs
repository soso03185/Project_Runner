using System;
using UnityEngine;

/// <summary>
/// ������ ���������� ���� Tile�� Item���� �������ִ� Generator
/// </summary>
/// 
public class MapGenerator : MonoBehaviour
{
    public Vector2 m_TileMatrix = Vector2.zero; // ��ü Ÿ���� ���
    public float m_TileOffset = 0.1f;           // Ÿ�� ���� ����
    string N_TileName = "NormalTile";           // �Ϲ� Ÿ��

    private void Start()
    {
        SpawnTileAtPosition();
    }

    void SpawnTileAtPosition()
    {
        GameObject tilePrefab = Resources.Load<GameObject>($"Prefabs/{N_TileName}");

        if (tilePrefab == null)
        {
            Debug.LogError($"Prefabs ������ '{N_TileName}' �������� �������� �ʽ��ϴ�.");
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
