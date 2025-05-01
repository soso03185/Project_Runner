using UnityEngine;

/// <summary>
/// 메인 카메라 제어하는 스크립트
/// </summary>
/// 
public class CameraController : MonoBehaviour
{
    public Transform m_Target;
    public float m_FollowSpeed = 10f;

    private Vector3 m_Velocity = Vector3.zero;
    private float m_InitialX;
    private float m_InitialY;
    private float m_ZOffset;

    void Start()
    {
        if (m_Target == null)
        {
            Debug.LogError("CameraFollow: 타겟이 지정되지 않았습니다.");
            return;
        }

        m_InitialX = transform.position.x;
        m_InitialY = transform.position.y;
        m_ZOffset = transform.position.z - m_Target.position.z;
    }

    void FixedUpdate()
    {
        if (m_Target == null) return;

        // X, Y 고정, Z만 따라감
        Vector3 targetPos = new Vector3(m_InitialX, m_InitialY
                                    , m_Target.position.z + m_ZOffset);

        // 더 반응 빠르고 부드러운 SmoothDamp 사용
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref m_Velocity, 0.05f);
    }
}
