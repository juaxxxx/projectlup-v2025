using UnityEngine;

namespace ES
{
    [CreateAssetMenu(fileName = "GunData", menuName = "Scriptable Objects/GunData")]
    public class GunData : ScriptableObject
    {
        public float bulletSpeed = 20f;
        public float damage = 20f;
        public float distance = 10f;

        public int startAmmoRemain = 100; // 처음에 주어질 전체 탄약
        public int magCapacity = 30; // 탄창 용량

        public float timeBetFire = 0.12f; // 총알 발사 간격
        public float reloadTime = 1.5f; // 재장전 소요 시간
    }
}
