using UnityEngine;

namespace RL
{
    public abstract class BlackBoar : MonoBehaviour
    {
        public bool Alive = true;

        public float HP = 0;
        public float MaxHP = 100;

        public bool OnAtk = false;
        public bool InAtkState = false;

        public bool OnHitted = false;
        public bool InHittedState = false;
        public bool OnRampage = false;

        public bool HasTarget = false;

        public bool isLocallyControlled = false;

        public float Speed = 5;
        public float AtkRange = 10;
        public float AtkCollTime = 0;
        public float HittedAccumTime = 0;
        public float RampageTime = 3;
        public float TargetDistance = 0;

        public GameObject Target = null;
    }
}

