using UnityEngine;
namespace ST
{

    public class CharacterInfo : MonoBehaviour
    {
        public string characterName = "";
        public bool manualMode = false;
        //public int hp = 10;
        public int currentAmmo = 0;
        public int maxAmmo = 10;
        public bool playerInputExists;
        public bool enemyInRange;

        private StatComponent stats;

        void Awake()
        {
            currentAmmo = maxAmmo;
            stats = GetComponent<StatComponent>();
            if (stats == null)
            {
                Debug.LogError($"{gameObject.name}: StatComponent가 없습니다!");
            }
        }

        public bool IsManualMode() => manualMode;
        //public bool IsHpZero() => hp <= 0;
        public bool IsHpZero() => stats != null && stats.IsDead;
        public bool IsPlayerInputExists() => playerInputExists;
        public bool IsCurrentAmmoFull() => currentAmmo >= maxAmmo;
        public bool HasAmmo() => currentAmmo > 0;
        public bool IsEnemyInRange() => enemyInRange;

        public StatComponent Stats => stats;
    }


}