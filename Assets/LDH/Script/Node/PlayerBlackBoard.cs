using UnityEngine;

namespace RL
{
    public class PlayerBlackBoard  
    {
        public Archer Health { get;  set; }
        public PlayerMove Move { get;  set; }
        public PlayerArrowShooter Shooter { get;  set; }
        public bool isAlive
        {
            get
            {
                if (Health == null)
                {
               
                    Debug.Log("null helath");
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

