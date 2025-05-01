using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 게임을 시작했을때 맵의 Tile과 Item들을 생성해주는 Generator
/// </summary>
/// 
public class MapGenerator : MonoBehaviour
{
    public Vector2 m_TileMatrix = Vector2.zero; // 전체 타일의 행렬
    public float m_TileOffset = 0.1f;           // 타일 간의 간격

    string N_TileName_L = "NormalTile_L";           // 일반 타일 Left
    string N_TileName_M = "NormalTile_M";           // 일반 타일 Mid
    string N_TileName_R = "NormalTile_R";           // 일반 타일 Right
    string N_GoldCoin = "GoldCoin";           // 골드 재화

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
                float tileScaleX = tile.transform.localScale.x + m_TileOffset; // 현재 0.01 오차 생김. (오류)
                
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
            Debug.LogError($"Prefabs 폴더에 '{prefab}' 프리팹이 존재하지 않습니다.");
            return;
        }

        float[] positionX = new float[] { 0.2f, 1, -0.6f }; // 중, 우, 좌 // 차이인 0.8f은 타일의 크기임. 나중에 관리하기 편하게 바꾸기

        GameObject parent = new GameObject();
        parent.name = $"{N_GoldCoin}.Parent";

        for (int i = 0; i < 100; i++)
        {
            Vector3 spawnPosition = new Vector3(positionX[Random.Range(0, 3)], 0.5f, 2.4f * i);
            Instantiate(prefab, spawnPosition, Quaternion.identity, parent.transform);
        }
    } 
}
