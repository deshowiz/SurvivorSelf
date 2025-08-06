using UnityEngine;

public interface IDamageable
{
    public void Damage(float incomingDamage, Vector2 incomingKnockback);
    public void Die();
}
