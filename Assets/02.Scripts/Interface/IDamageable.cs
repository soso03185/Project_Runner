using UnityEngine;

public interface IDamageable
{
    void TakeDamage(float damage, Vector3 attackPos); // attackPos은 맞은 방향
}
