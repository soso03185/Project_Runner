using TMPro;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Player�� ��ü���� ������ ���ִ� ��
/// </summary>
/// 
public class PlayerController : MonoBehaviour
{
    public float m_ForwardSpeed = 5f;          // ���� �ӵ�
    public float m_LaneDistance = 2.5f;        // ���� �� �Ÿ�
    public float m_LaneSwitchSpeed = 10f;      // �¿� ��ȯ �ӵ�
    private int m_CurrentLane = 1;             // 0 = ����, 1 = �߰�, 2 = ������
    private Vector3 m_TargetPos;

    Rigidbody m_Rigidbody;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_TargetPos = transform.position;
    }

    void Update()
    {
        // �¿� �Է� ó��
        if (Input.GetKeyDown(KeyCode.LeftArrow) && m_CurrentLane > 0)
        {
            m_CurrentLane--;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && m_CurrentLane < 2)
        {
            m_CurrentLane++;
        }

        // ��ǥ ��ġ ��� (X ��ġ�� ����)
        float targetX = (m_CurrentLane - 1) * m_LaneDistance;
        m_TargetPos = new Vector3(targetX, transform.position.y, transform.position.z);
    }

    private void FixedUpdate()
    {
        // �¿� ��ġ ����
        float newX = Mathf.Lerp(transform.position.x, m_TargetPos.x, m_LaneSwitchSpeed * Time.fixedDeltaTime);

        // ���� �Ÿ� ���
        float newZ = transform.position.z + m_ForwardSpeed * Time.fixedDeltaTime;

        Vector3 newPosition = new Vector3(newX, transform.position.y, newZ);
        m_Rigidbody.MovePosition(newPosition);
    }
}
