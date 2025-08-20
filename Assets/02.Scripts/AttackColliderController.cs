using UnityEngine;


/// <summary>
/// 몬스터가 공격했을때, 공격 판정 범위 컨트롤러
/// </summary>
/// 
public class AttackColliderController : MonoBehaviour
{
    [Tooltip("판정 사거리 (Z축)")]
    public float m_AttackRange = 2f;

    [Tooltip("몬스터 스크립트에서 받아올 공격력")]
    public float m_Atk = 1;

    float m_AttackHeight = 1f;
    float m_AttackWidth = 0.7f;
    bool m_IsAttacked = false; // 플레이어에게 피격처리를 했다면.

    BoxCollider m_AtkCol;

    private void Awake()
    {
        m_AtkCol = GetComponent<BoxCollider>();        
    }

    private void Start()
    {
        m_AtkCol.enabled = false; // 시작 시 비활성화
    }

    public void StartAttack()
    {
        // 위치 조정 (앞으로 사거리만큼 절반 위치)
        m_AtkCol.center = new Vector3(0, m_AttackHeight / 2f, -m_AttackRange / 2f);
        m_AtkCol.size = new Vector3(m_AttackWidth, m_AttackHeight, m_AttackRange); // 크기 설정
        m_AtkCol.enabled = true; // 활성화
    }

    public void EndAttack()
    {
        m_AtkCol.enabled = false;
        m_IsAttacked = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_AtkCol.enabled && other.CompareTag("Player") && m_IsAttacked == false)
        {
            m_IsAttacked = true;
            Debug.Log("플레이어 피격");

            // 데미지 처리
            other.GetComponent<PlayerController>().TakeDamage(m_Atk, transform.position);
        }
    }
}
