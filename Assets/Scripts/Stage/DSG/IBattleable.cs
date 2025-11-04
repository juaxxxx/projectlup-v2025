using UnityEngine;
using static DSG.Character;

namespace DSG
{
    public interface IBattleable
    {
        void TakeDamage(float amount);
        void Die();
    }
}