using UnityEngine;

/// <summary>
/// ���� ī�޶� �����ϴ� ��ũ��Ʈ
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
            Debug.LogError("CameraFollow: Ÿ���� �������� �ʾҽ��ϴ�.");
            return;
        }

        m_InitialX = transform.position.x;
        m_InitialY = transform.position.y;
        m_ZOffset = transform.position.z - m_Target.position.z;
    }

    void FixedUpdate()
    {
        if (m_Target == null) return;

        // X, Y ����, Z�� ����
        Vector3 targetPos = new Vector3(m_InitialX, m_InitialY
                                    , m_Target.position.z + m_ZOffset);

        // �� ���� ������ �ε巯�� SmoothDamp ���
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref m_Velocity, 0.05f);
    }
}
