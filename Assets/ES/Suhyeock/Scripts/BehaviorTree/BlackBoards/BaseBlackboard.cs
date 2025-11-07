using UnityEngine;

namespace ES
{
    public class BaseBlackboard : MonoBehaviour
    {
        [HideInInspector]
        public HealthComponent healthComponent;

        public float speed = 5.0f;

        public void Awake()
        {
            healthComponent = GetComponent<HealthComponent>();
        }
    }
}
