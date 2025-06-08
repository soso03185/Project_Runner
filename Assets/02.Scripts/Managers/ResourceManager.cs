using UnityEngine;
using static Define;

/// <summary>
/// 모든 Resource 관리 (Object Pool 포함)
/// 씬에 스크립트가 붙어있어야 함. (싱글톤)
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


         // Object Pool이 아닌 경우
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
