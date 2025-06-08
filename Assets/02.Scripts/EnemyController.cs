using UnityEngine;

public class EnemyController : MonoBehaviour
{
    BoxCollider mBoxCollider;

    private void Awake()
    {
        mBoxCollider = GetComponent<BoxCollider>();
    }

}
