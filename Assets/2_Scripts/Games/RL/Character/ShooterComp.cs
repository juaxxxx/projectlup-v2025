using UnityEngine;

namespace LUP.RL
{
    public class ShooterComp : MonoBehaviour
    {
        [HideInInspector]
        public FireSystem fireSystem;

        private void Start()
        {
            fireSystem = GetComponent<FireSystem>();

            if( fireSystem == null )
            {
                Debug.LogError("Fail To Find fireSystem");
            }
        }

        public void TryShoot(Transform target, int damage)
        {
            if (fireSystem)
            {
                fireSystem.TryFire(target, damage);
            }
            
        }
    }
}