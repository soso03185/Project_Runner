using System;
using System.Xml;
using TMPro;
using Unity.VisualScripting;
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

    public int m_Exp = 100;
    
    HitEffectController hitEffectController;

    private void Awake()
    {
        hitEffectController = GetComponent<HitEffectController>();
        Init();
    }

    public override void Init()
    {
        m_Hp = m_MaxHp;
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
}
