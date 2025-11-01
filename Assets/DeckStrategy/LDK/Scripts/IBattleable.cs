using UnityEngine;
using static Character;
public interface IBattleable
{
    void TakeDamage(float amount);
    void Die();
}
