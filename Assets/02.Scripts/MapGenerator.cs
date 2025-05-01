using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

/// <summary>
/// ������ ���������� ���� Tile�� Item���� �������ִ� Generator
/// </summary>
/// 
public class MapGenerator : MonoBehaviour
{
    public Vector2 m_TileMatrix = Vector2.zero; // ��ü Ÿ���� ���
    public float m_TileOffset = 0.1f;           // Ÿ�� ���� ����

    string N_TileName_L = "NormalTile_L";           // �Ϲ� Ÿ�� Left
    string N_TileName_M = "NormalTile_M";           // �Ϲ� Ÿ�� Mid
    string N_TileName_R = "NormalTile_R";           // �Ϲ� Ÿ�� Right
    string N_GoldCoin = "GoldCoin";           // ��� ��ȭ

    private void Start()
    {
        SpawnTileAtPosition(N_TileName_L, 0);
        SpawnTileAtPosition(N_TileName_M, 1);
        SpawnTileAtPosition(N_TileName_R, 2);
        SpawnItemAtPosition();
    }

    void SpawnTileAtPosition(string tileName, int index)
    {
        int centerOffset = (int)m_TileMatrix.x / 2; // Player Center Line Start

        Transform parent = new GameObject().transform;
        parent.name = $"{tileName}.Parent";

        for (int i = 0; i < m_TileMatrix.x; i++)
        {
            for (int j = 0; j < m_TileMatrix.y; j++)
            {
                GameObject tile = ResourceManager.Instance.InstantiatePrefab(tileName);
                float tileScaleX = tile.transform.localScale.x + m_TileOffset; // ���� 0.01 ���� ����. (����)
                
                // index : 0-L, 1-M, 2-R
                Vector3 spawnPosition = new Vector3((tileScaleX * index) - (tileScaleX * centerOffset), 0f, tileScaleX * j);                
                tile.transform.position = spawnPosition;
                tile.transform.parent = parent;
            }
        }
    }

    void SpawnItemAtPosition()
    {
        GameObject prefab = Resources.Load<GameObject>($"Prefabs/{N_GoldCoin}");
        if (prefab == null)
        {
            Debug.LogError($"Prefabs ������ '{prefab}' �������� �������� �ʽ��ϴ�.");
            return;
        }

        float[] positionX = new float[] { 0.2f, 1, -0.6f }; // ��, ��, �� // ������ 0.8f�� Ÿ���� ũ����. ���߿� �����ϱ� ���ϰ� �ٲٱ�

        GameObject parent = new GameObject();
        parent.name = $"{N_GoldCoin}.Parent";

        for (int i = 0; i < 100; i++)
        {
            Vector3 spawnPosition = new Vector3(positionX[Random.Range(0, 3)], 0.5f, 2.4f * i);
            Instantiate(prefab, spawnPosition, Quaternion.identity, parent.transform);
        }
    } 
}
