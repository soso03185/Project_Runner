using UnityEngine;

public class MapLoader : MonoBehaviour
{
    void Start()
    {
        LoadMap();
    }

    private void LoadMap()
    {
        TextAsset json = Resources.Load<TextAsset>("MapData");
        if (json == null)
        {
            Debug.LogError("MapData.json not found in Resources folder");
            return;
        }

        MapData mapData = JsonUtility.FromJson<MapData>(json.text);

        foreach (MapObjectInfo info in mapData.objects)
        {
            GameObject prefab = Resources.Load<GameObject>("Prefabs/" + info.prefabName);
            if (prefab != null)
            {
                GameObject obj = Instantiate(prefab);
                obj.transform.position = info.position;
                obj.transform.rotation = Quaternion.Euler(info.rotation);
            }
            else
            {
                Debug.LogWarning($"Prefab not found: {info.prefabName}");
            }
        }
    }
}
