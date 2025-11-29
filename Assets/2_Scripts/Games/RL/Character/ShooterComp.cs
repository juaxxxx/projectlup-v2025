using UnityEngine;

namespace LUP.RL
{
    public class ShooterComp : MonoBehaviour
    {
        public FireSystem fireSystem;
        public void TryShoot(Transform target, int damage)
        {
            fireSystem.TryFire(target, damage);
        }
    }
}