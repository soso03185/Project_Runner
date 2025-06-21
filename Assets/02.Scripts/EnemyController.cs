using System;
using System.Xml;
using TMPro;
using UnityEngine;
using static Define;

public class EnemyController : LaneObject, IDamageable
{
    public int m_MaxHp { get; private set; } = 100;
    [SerializeField] private float m_hp;
    [SerializeField] private Transform m_uiPos;

    public float m_Hp
    {
        get => m_hp;
        set
        {
            m_hp = Mathf.Clamp(value, 0, m_MaxHp);
            m_OnHPChanged?.Invoke(m_hp);
            if (m_hp <= 0)
                Die();
        }
    }
    public event Action<float> m_OnHPChanged;

    public override void Init()
    {
        m_Hp = m_MaxHp;
    }

    public void TakeDamage(float damage, Vector3 attackPos)
    {
        m_Hp -= damage;
        DEBUG_LOG($"Damage: {damage}, HP: {m_Hp}");

        // Damage Text UI 생성
        GameObject dmgText = ResourceManager.Instance.InstantiatePrefab("UI/T_Damage");
        dmgText.GetComponent<TextMeshPro>().text = damage.ToString();

        // 해당 방향으로 offset을 적용한 위치
        Vector3 fromPlayerDir = (attackPos - transform.position).normalized;
        Vector3 offsetPos = m_uiPos.position + fromPlayerDir * 0.5f;
        dmgText.transform.position = offsetPos;
    }
    
    public override void Die()
    {
        DEBUG_LOG($"Die: '{gameObject.name}'");
        ResourceManager.Instance.Destroy(gameObject);
    }
}
