using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Define;

public class PoolManager : MonoBehaviour
{
    class Pool
    {
        Stack<Poolable> PoolStack = new Stack<Poolable>();
        public GameObject Original;
        public Transform Root;

        public void Init(GameObject original, int count = 5)
        {
            Original = original;
            Root = new GameObject().transform;
            Root.name = $"{original.name}_Root";

            for (int i = 0; i < count; i++)
                Push(Create());
        }

        public void Push(Poolable poolable)
        {
            if (poolable == null)
                return;

            poolable.transform.parent = Root;
            poolable.gameObject.SetActive(false);
            poolable.IsUsing = false;

            PoolStack.Push(poolable);
        }

        public Poolable Create(Transform parent = null)
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;
            return go.GetOrAddComponent<Poolable>();
        }

        public Poolable Pop(Transform parent)
        {
            Poolable poolable;

            if (PoolStack.Count > 0)
                poolable = PoolStack.Pop();
            else
                poolable = Create();

            poolable.gameObject.SetActive(true);
            poolable.transform.parent = parent;
            poolable.IsUsing = true;

            return poolable;
        }
    }

    Dictionary<string, Pool> m_PoolDictionary = new Dictionary<string, Pool>();
    Transform m_Root;

    public void Init()
    {
        if (m_Root == null)
        {
            m_Root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(m_Root);
        }
    }

    public void CreatePool(GameObject original, Transform parent = null)
    {
        Pool pool = new Pool();
        pool.Init(original);
        pool.Root.parent = m_Root;

        m_PoolDictionary.Add(original.name, pool);
    }

    public Poolable Pop(GameObject original, Transform parent = null)
    {
        if (m_PoolDictionary.ContainsKey(original.name) == false)
            CreatePool(original, parent);

        return m_PoolDictionary[original.name].Pop(parent);
    }

    public void Push(Poolable poolable)
    {
        string name = poolable.gameObject.name;

        if (m_PoolDictionary.ContainsKey(name) == false)
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }
        m_PoolDictionary[name].Push(poolable);
    }

    public GameObject GetOriginal(string name)
    {
        if (m_PoolDictionary.ContainsKey(name) == false)
            return null;
        return m_PoolDictionary[name].Original;
    }
}
