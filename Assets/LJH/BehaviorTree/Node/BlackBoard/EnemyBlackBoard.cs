using UnityEngine;

namespace RL
{
    public class EnemyBlackBoard
    {
        public bool Alive = true;

        public float HP = 0.0f;
        public float MaxHP = 100.0f;
        public float Speed = 5.0f;

        public bool CanAtk = true;
        public float AtkRange = 5.0f;
        public float AtkCollTime = 0;
        public bool OnAtk = false;

        public float HittedAccumTime = 0.0f;
        public bool OnHitted = false;
        public float RampageTime = 5.0f;
        public bool OnRampage = false;

        public GameObject Target;
        public bool HasTarget = false;
        public float TargetDistance = 0.0f;
    }
}

