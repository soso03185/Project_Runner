using System;
using System.Collections;
using UnityEngine;
using static Define;

public class EnemyController : LaneObject, IDamageable
{
    public int m_MaxHp { get; private set; } = 100;
    private float m_hp;

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

    [Tooltip("경험치")]
    public int m_Exp = 100;

    [Tooltip("공격력")] 
    public float m_Atk = 10;

    [Tooltip("공격 범위")]
    public float m_AtkRange = 10f;

    [Tooltip("공격 쿨타임")]
    public float m_AtkDelay = 3f;

    [Tooltip("공격 판정 유지 시간")]
    public float m_AtkHoldingTime = 0.7f;
    
    bool m_IsCanAtk = true;

    public AttackColliderController atkColController; // 공격 판정 범위 관리
    HitEffectController hitEffectController;  // 모든 HitEffect 관리

    private void Awake()
    {
        hitEffectController = GetComponent<HitEffectController>();
        Init();
    }

    public override void Init()
    {
        m_Hp = m_MaxHp;
    }

    public void Update()
    {
        // ToDo : attack anim play
        // 
        if (m_IsCanAtk) StartCoroutine(CoAttack());
    }

    public void TakeDamage(float damage, Vector3 attackerPos)
    {
        m_Hp -= damage;
        hitEffectController.PlayDamageFont(damage, attackerPos);
    }

    public void GetPlayerExp(int exp)
    {
        int PlayerLvUpExp = 100;

        UIDataStats.Exp.Value += exp;

        if (UIDataStats.Exp.Value >= PlayerLvUpExp)
        {
            UIDataStats.Exp.Value -= PlayerLvUpExp;
            UIDataStats.Level.Value += 1;
        }
    }

    public override void Die()
    {
        DEBUG_LOG($"Die: '{gameObject.name}'");
        GetPlayerExp(m_Exp);
        ResourceManager.Instance.Destroy(gameObject);
    }

    IEnumerator CoAttack()
    {
        DEBUG_LOG("Attack !");
        m_IsCanAtk = false;
        atkColController.StartAttack();
        
        yield return new WaitForSeconds(m_AtkHoldingTime);
        atkColController.EndAttack();

        yield return new WaitForSeconds(m_AtkDelay);
        m_IsCanAtk = true;

        yield break;
    }
}
