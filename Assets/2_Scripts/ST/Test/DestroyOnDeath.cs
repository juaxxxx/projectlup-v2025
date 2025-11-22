using UnityEngine;

namespace LUP.ST
{
    [RequireComponent(typeof(StatComponent))]
    public class DestroyOnDeath : MonoBehaviour
    {
        private StatComponent stats;

        void Awake()
        {
            stats = GetComponent<StatComponent>();
            stats.OnDeath += HandleDeath;
        }

        private void HandleDeath()
        {
            Debug.Log($"{name} ¢º »ç¸Á! ¿ÀºêÁ§Æ® ÆÄ±«");
            Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if (stats != null)
                stats.OnDeath -= HandleDeath;
        }
    }
}
