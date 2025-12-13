using LUP.ES;
using UnityEngine;

namespace LUP.ES
{
    public enum WeaponState
    {
        READY,
        ATTACKING,
        RELOADING,
    }


    public abstract class Weapon : MonoBehaviour
    {
        [HideInInspector]
        public EventBroker eventBroker;
        [HideInInspector]
        public WeaponState state;
        [HideInInspector]
        public WeaponItem weaponItem;
        public Transform leftHandGrip;
        public abstract bool Attack();

        private void Start()
        {
            eventBroker = FindAnyObjectByType<EventBroker>();
        }
    }

   
    
}

