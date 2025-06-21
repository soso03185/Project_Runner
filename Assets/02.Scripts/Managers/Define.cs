using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���������� �����ؼ� ����ϴ� �� ����
/// </summary>
/// 
public class Define
{
    public static void DEBUG_LOG(string msg)
    {
#if UNITY_EDITOR
        Debug.Log(msg);
#endif
    }
    public static void DEBUG_ERROR(string msg)
    {
#if UNITY_EDITOR
        Debug.LogError(msg);
#endif
    }

    enum DamageType
    {

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
        public int laneNum;

        public string PrefabName => ExtractPrefabName(prefabPath);
    }

    [System.Serializable]
    public class Vector3Ref
    {
        public float x;
        public float y;
        public float z;

        public Vector3Ref(Vector3 v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }

        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }

        public void Set(Vector3 v)
        {
            x = v.x;
            y = v.y;
            z = v.z;
        }

        public void CopyFrom(Vector3Ref other)
        {
            x = other.x;
            y = other.y;
            z = other.z;
        }
    }
}
