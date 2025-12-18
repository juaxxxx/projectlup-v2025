using UnityEngine;

namespace LUP.ES
{
    public class BaseBlackboard : MonoBehaviour
    {
        [HideInInspector]
        public HealthComponent healthComponent;

        public float speed = 5.0f;
        protected virtual void Awake()
        {
            healthComponent = GetComponent<HealthComponent>();
        }
    }
}
