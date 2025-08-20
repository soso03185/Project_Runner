using UnityEngine;

/// <summary>
/// 데미지를 받는 인터페이스
/// </summary>
/// 
public interface IDamageable
{
    void TakeDamage(float damage, Vector3 attackPos); // attackPos은 맞은 방향
}
