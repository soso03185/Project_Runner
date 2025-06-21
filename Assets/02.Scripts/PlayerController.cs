using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR;
using static Define;
using static UnityEngine.EventSystems.EventTrigger;

/// <summary>
/// Player의 전체적인 관리를 해주는 곳
/// </summary>
/// 
public class PlayerController : MonoBehaviour, IDamageable
{
    [SerializeField] private float m_FallMultiplier = 3f;
    [SerializeField] private float m_LowJumpMultiplier = 5f;

    float m_ForwardSpeed = 5f;          // 전진 속도
    float m_LaneDistance = 1.0f;        // 레인 간 거리
    float m_LaneSwitchSpeed = 10f;      // 좌우 전환 속도
    float m_JumpForce = 7f;             // 점프 힘
    int m_CurrentLane = 1;              // 0 = 왼쪽, 1 = 중간, 2 = 오른쪽
    float m_AttackDamage = 99;

    bool m_IsGrounded = true;
    bool m_IsKnockBack = false;
    Vector3 m_TargetPos;
    Rigidbody m_Rigidbody;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_TargetPos = transform.position;
    }

    void Update()
    {
        // 입력 처리
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

        // 목표 위치 계산 (X 위치만 갱신)
        float targetX = m_CurrentLane * m_LaneDistance;
        m_TargetPos = new Vector3(targetX, transform.position.y, transform.position.z);
    }

    private void FixedUpdate()
    {
        // 점프 중
        if (m_Rigidbody.linearVelocity.y < 0)
        {
            // 낙하 가속 강화
            Vector3 extraGravity = (m_FallMultiplier - 1) * Physics.gravity;
            m_Rigidbody.AddForce(extraGravity, ForceMode.Acceleration);
        }
        else if (m_Rigidbody.linearVelocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            // 점프 버튼 뗐을 때 빠르게 상승 멈춤
            Vector3 extraGravity = (m_LowJumpMultiplier - 1) * Physics.gravity;
            m_Rigidbody.AddForce(extraGravity, ForceMode.Acceleration);
        }

        // 좌우 위치 보간
        float newX = Mathf.Lerp(transform.position.x, m_TargetPos.x, m_LaneSwitchSpeed * Time.fixedDeltaTime);

        // 전진 거리 계산
        float newZ = transform.position.z + m_ForwardSpeed * Time.fixedDeltaTime;

        Vector3 newPosition = new Vector3(newX, transform.position.y, newZ);
        m_Rigidbody.MovePosition(newPosition);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            UIController.Instance.GetCoin(++DataManager.m_GameCoin);
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            LaneObject laneObj = other.gameObject.GetComponent<LaneObject>();
            if (laneObj.m_CurrentLane == m_CurrentLane)
            {
                if (m_IsKnockBack == false)
                {
                    StartCoroutine(CoKnockBack());
                    Attack(laneObj);
                }
            }
        }
    }

    public void TakeDamage(float damage, Vector3 attackPos)
    {

    }

    void Attack(LaneObject laneObj)
    {
        if(laneObj.TryGetComponent<IDamageable>(out var dmg))
        {
            dmg.TakeDamage(m_AttackDamage, transform.position);
        }
        
        GameObject fxSlash = ResourceManager.Instance.InstantiatePrefab("FX/FX_Slash_Blue");
        fxSlash.transform.position = laneObj.transform.position;
    }

    IEnumerator CoKnockBack()
    {
        m_IsKnockBack = true;
        m_Rigidbody.AddForce(Vector3.back * m_JumpForce * 1.5f, ForceMode.Impulse);
        yield return new WaitForSeconds(0.2f);

        m_IsKnockBack = false;
        yield break;
    }

}
