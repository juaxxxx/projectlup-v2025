using UnityEngine;

namespace RL
{
    public interface IBlackBoardAble
    {
        public bool Alive { get; set; }

        public bool CanAtk { get; set; }
        public bool OnAtk { get; set; }

        public bool OnHitted { get; set; }
        public bool OnRampage { get; set; }

        public bool HasTarget { get; set; }

        public bool isLocallyControlled { get; set; }

        public float Speed { get; set; }
        public float AtkRange { get; set; }
        public float AtkCollTime { get; set; }
        public float HittedAccumTime { get; set; }
        public float RampageTime { get; set; }
        public float TargetDistance { get; set; }
    }
}

