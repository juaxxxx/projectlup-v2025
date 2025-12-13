using UnityEngine;

namespace LUP.RL
{
    public class PlayerBlackBoard : MonoBehaviour
    {
        public Archer Health { get;  set; }
        public PlayerMove Move { get;  set; }
        public ShooterComp Shooter { get;  set; }
        public Transform currentRoom;
        [SerializeField]
        private int attackRanage;
        public void Initialize(GameObject player)
        {
            Move = player.GetComponent<PlayerMove>();
            Shooter = player.GetComponent<ShooterComp>();
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
        public Enemy FindClosestEnemy()
        {
            if (currentRoom == null)
            {
                return null;
            }

            Enemy[] enemies = currentRoom.GetComponentsInChildren<Enemy>(false);
            if (enemies.Length == 0)
            {
                return null;
            }
            Enemy closest = null;
            float minDist = attackRanage;

            foreach (var e in enemies)
            {

                if (e == null) continue;
              
                float dist = Vector3.Distance(Health.transform.position, e.TargetPoint.position);
                if (dist < minDist)
                {
                    minDist = dist;
                    closest = e;
                }
            }

            return closest;
        }
        public void SetCurrentRoom(Transform room)
        {
            currentRoom = room;
        }
        public bool IsMoving => Health.Adata.currentData.speed > 0;
        public bool OnHit = false;


        public void SetHitted()
        {
            OnHit = true;
        }
    }
}

