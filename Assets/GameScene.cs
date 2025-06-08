using UnityEngine;

public class GameScene : MonoBehaviour
{
    Transform mPlayerTransform;

    void Start()
    {
        mPlayerTransform = ResourceManager.Instance.InstantiatePrefab("Player").transform;

        CameraController mainCam = Camera.main.transform.GetComponent<CameraController>();
        mainCam.m_Target = mPlayerTransform;
        mainCam.Init();
    }

}
