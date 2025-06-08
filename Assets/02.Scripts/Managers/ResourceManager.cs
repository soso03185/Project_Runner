using UnityEngine;
using static Define;

/// <summary>
/// ��� Resource ���� (Object Pool ����)
/// ���� ��ũ��Ʈ�� �پ��־�� ��. (�̱���)
/// </summary>
/// 
public class ResourceManager : Singleton<ResourceManager>
{    
    PoolManager poolManager;

    public override void Awake()
    {
        mIsDontDestroy = true;
        base.Awake();

        poolManager = new PoolManager();
        poolManager.Init();
    }

    public T Load<T>(string path) where T : Object
    {
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf('/');

            if (index > 0)
                name = name.Substring(index + 1);

            GameObject go = poolManager.GetOriginal(name);
            if (go != null)
                return go as T;
        }

        return Resources.Load<T>(path);
    }

    public GameObject InstantiatePrefab(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");

        if (original == null)
        {
            DEBUG_ERROR($"Failed to load prefab : {path}");
            return null;
        }

        // Object Pool
        if (original.GetComponent<Poolable>() != null)
            return poolManager.Pop(original,parent).gameObject;        


         // Object Pool�� �ƴ� ���
         GameObject go = Object.Instantiate(original, parent);
         go.name = original.name;
         return go;
    }

    public void Destroy(GameObject go)
    {
        var poolable = go.GetComponent<Poolable>();

        if (poolable == null) 
            Object.Destroy(go);
        else
            poolManager.Push(poolable);
    }
}
