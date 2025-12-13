using UnityEngine;

namespace LUP.RL
{
    public interface IAttackHandler
    {
        void Attack(Transform target, int damage);
    }
}
