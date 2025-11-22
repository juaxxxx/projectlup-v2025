using UnityEngine;
using static LUP.DSG.Character;

namespace LUP.DSG
{
    public interface IBattleable
    {
        void TakeDamage(float amount);
        void Die();
    }
}
