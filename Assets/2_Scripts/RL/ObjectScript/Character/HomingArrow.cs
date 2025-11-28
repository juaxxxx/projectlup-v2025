
using UnityEngine;
namespace LUP.RL
{
    public class HomingArrow : MonoBehaviour
    {
        public Transform target;
        public float speed;
        public float LifeTime = 10f;
        private int damage;
        [SerializeField]
        public Archer archer;
        private void Start()
        {

            if (!archer) return;
            Debug.Log($"{archer.Adata.currentData.Attack}");
            Destroy(gameObject, LifeTime);
        }
        public void Initialize(Archer Player, Transform target, float speed, int damage)
        {
            this.target = target;
            this.speed = speed;
            this.damage = damage;
            this.archer = Player;
        }

        private void OnTriggerEnter(Collider other)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                Debug.Log("충돌");
                Destroy(gameObject);
                enemy.TakeDamage(archer.Adata.currentData.Attack);
            }
        }
        void Update()
        {

            if (target == null)
            {
                transform.position += transform.forward * speed * Time.deltaTime;
                return;
            }

            // 적을 향한 방향 벡터 계산
            Vector3 dir = (target.position - transform.position).normalized;

            // 회전은 안 하고, 방향 벡터로 직접 이동
            transform.position += dir * speed * Time.deltaTime;
        }


    }
}
