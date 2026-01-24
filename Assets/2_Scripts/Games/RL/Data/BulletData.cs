using UnityEngine;
using System.Collections.Generic;
namespace LUP.RL
{
    [CreateAssetMenu(fileName = "RLBulletData", menuName = "RLGame/BulletData")]
    public class BulletData :ScriptableObject 
    {
        public GameObject bulletPrefab;
        public GameObject effectprefab;
        public int Speed;

    }
}
