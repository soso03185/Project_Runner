using TMPro;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Player의 전체적인 관리를 해주는 곳
/// </summary>
/// 
public class PlayerController : MonoBehaviour
{
    public float m_ForwardSpeed = 5f;          // 전진 속도
    public float m_LaneDistance = 2.5f;        // 레인 간 거리
    public float m_LaneSwitchSpeed = 10f;      // 좌우 전환 속도
    private int m_CurrentLane = 1;             // 0 = 왼쪽, 1 = 중간, 2 = 오른쪽
    private Vector3 m_TargetPos;

    Rigidbody m_Rigidbody;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_TargetPos = transform.position;
    }

    void Update()
    {
        // 좌우 입력 처리
        if (Input.GetKeyDown(KeyCode.LeftArrow) && m_CurrentLane > 0)
        {
            m_CurrentLane--;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && m_CurrentLane < 2)
        {
            m_CurrentLane++;
        }

        // 목표 위치 계산 (X 위치만 갱신)
        float targetX = (m_CurrentLane - 1) * m_LaneDistance;
        m_TargetPos = new Vector3(targetX, transform.position.y, transform.position.z);
    }

    private void FixedUpdate()
    {
        // 좌우 위치 보간
        float newX = Mathf.Lerp(transform.position.x, m_TargetPos.x, m_LaneSwitchSpeed * Time.fixedDeltaTime);

        // 전진 거리 계산
        float newZ = transform.position.z + m_ForwardSpeed * Time.fixedDeltaTime;

        Vector3 newPosition = new Vector3(newX, transform.position.y, newZ);
        m_Rigidbody.MovePosition(newPosition);
    }
}
