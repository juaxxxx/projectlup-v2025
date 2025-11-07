using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace RL
{
    public class EnemyBlackBoard : MonoBehaviour ,IBlackBoardAble
    {
        public PlayerMove Move { get; private set; }
        public PlayerArrowShooter Shooter { get; private set; }
        public Archer Health { get; private set; }


        public bool Alive
        {
            get => Health != null & Health.Adata.currentData.Hp > 0;
            set { }  //인터페이스 요구만 충족 .
        }
        public bool isLocallyControlled { get; set; } = false;

        public float HP = 0.0f;
        public float MaxHP = 100.0f;
        public float Speed
        {
            get => Move != null ? Move.speed : 5;
            set { } 
        }

        public bool CanAtk
        {
            get => Alive && !OnHitted && !OnAtk && !Move.isMoving && Shooter != null;
            set { }
        }
        public float AtkRange { get; set; } = 100f;
        public float AtkCollTime
        {
            get => Shooter != null ? Shooter.fireDelay : 0f;
            set { }
        }
        public bool OnAtk { get; set; } = true;

        public float HittedAccumTime { get; set; } = 0.0f;
        public bool OnHitted { get; set; } = true;
        public float RampageTime { get; set; } = 5.0f;
        public bool OnRampage { get; set; } = true;

        public GameObject Target;
        public bool HasTarget { get; set; } = true;

        public float TargetDistance{ get; set; }
        

        private void Awake()
        {
            Move = GetComponent<PlayerMove>();
            Shooter = GetComponent<PlayerArrowShooter>();
            Health = GetComponent<Archer>();
        }
    }
}

