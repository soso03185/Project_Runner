using System;
using TMPro;
using UnityEngine;
using UnityEngine.XR;
using static Define;

/// <summary>
/// Player�� ��ü���� ������ ���ִ� ��
/// </summary>
/// 
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_FallMultiplier = 3f;
    [SerializeField] private float m_LowJumpMultiplier = 5f;

    public float m_ForwardSpeed = 5f;          // ���� �ӵ�
    public float m_LaneDistance = 2.5f;        // ���� �� �Ÿ�
    public float m_LaneSwitchSpeed = 10f;      // �¿� ��ȯ �ӵ�
    public float m_JumpForce = 7f;  // ���� ��

    private int m_CurrentLane = 1;             // 0 = ����, 1 = �߰�, 2 = ������
    private bool m_IsGrounded = true;
    private Vector3 m_TargetPos;
    Rigidbody m_Rigidbody;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_TargetPos = transform.position;
    }

    void Update()
    {
        // �Է� ó��
        if (Input.GetKeyDown(KeyCode.LeftArrow) && m_CurrentLane > 0)
        {
            m_CurrentLane--;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && m_CurrentLane < 2)
        {
            m_CurrentLane++;
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            m_Rigidbody.AddForce(Vector3.up * m_JumpForce, ForceMode.Impulse);
        }

        // ��ǥ ��ġ ��� (X ��ġ�� ����)
        float targetX = (m_CurrentLane) * m_LaneDistance;
        m_TargetPos = new Vector3(targetX, transform.position.y, transform.position.z);
    }

    private void FixedUpdate()
    {
        // ���� ��
        if (m_Rigidbody.linearVelocity.y < 0)
        {
            // ���� ���� ��ȭ
            Vector3 extraGravity = (m_FallMultiplier - 1) * Physics.gravity;
            m_Rigidbody.AddForce(extraGravity, ForceMode.Acceleration);
        }
        else if (m_Rigidbody.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            // ���� ��ư ���� �� ������ ��� ����
            Vector3 extraGravity = (m_LowJumpMultiplier - 1) * Physics.gravity;
            m_Rigidbody.AddForce(extraGravity, ForceMode.Acceleration);
        }

        // �¿� ��ġ ����
        float newX = Mathf.Lerp(transform.position.x, m_TargetPos.x, m_LaneSwitchSpeed * Time.fixedDeltaTime);

        // ���� �Ÿ� ���
        float newZ = transform.position.z + m_ForwardSpeed * Time.fixedDeltaTime;

        Vector3 newPosition = new Vector3(newX, transform.position.y, newZ);
        m_Rigidbody.MovePosition(newPosition);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Coin"))
        {
            DEBUG_LOG("TestCoin");
            Destroy(collision.gameObject);
        }
    }
}
