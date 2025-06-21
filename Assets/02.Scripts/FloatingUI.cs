using Unity.VisualScripting;
using UnityEngine;

public class FloatingUI : MonoBehaviour
{
    void Start()
    {
        Invoke("Destroy", 0.45f); 
    }

    public void Destroy()
    {
        ResourceManager.Instance.Destroy(gameObject);
    }
}
