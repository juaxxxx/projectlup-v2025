using UnityEngine;

namespace LUP.RL
{
    public class PlayerBlackBoard  
    {
        public Archer Health { get;  set; }
        public PlayerMove Move { get;  set; }
        public PlayerArrowShooter Shooter { get;  set; }
        public void Initialize(GameObject player)
        {
            Move = player.GetComponent<PlayerMove>();
            Shooter = player.GetComponent<PlayerArrowShooter>();
            Health = player.GetComponent<Archer>();
        }
        public bool isAlive
        {
            get
            {
                if (Health == null)
                {
    
                    return false; 
                }
          
                return Health.Adata.currentData.Hp > 0;
            }
        }
        public bool IsMoving => Health.Adata.currentData.speed > 0;
        public bool OnHit = false;

        public void SetHitted()
        {
            OnHit = true;
        }
    }
}

