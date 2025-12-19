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

        protected Renderer[] weaponRenderers;
        public abstract bool Attack();
        public abstract void SetWeaponVisible(bool isVisible);
        public abstract bool CanAttack();
        public abstract void Init(int id);
        protected virtual void Start()
        {
            eventBroker = FindAnyObjectByType<EventBroker>();
        }

       

    }

   
    
}

