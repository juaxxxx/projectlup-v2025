using UnityEngine;
using System.Collections;

namespace LUP.ES
{
    public class Bullet : MonoBehaviour
    {
        [Header("VFX")]
        [SerializeField] private ParticleSystem bulletBodyVFX;
        [SerializeField] private GameObject trailPrefab; // 여기에 트레일 프리팹을 넣으세요
        [SerializeField] private float trailFadeTime = 0.5f; // 꼬리가 사라지는 시간

        [SerializeField]
        private string targetTag;
        private float maxDistance;
        private Vector3 spawnPostion;
        private float speed = 0f;
        private float damage = 0f;
        private BulletObjectPool ownerPool;

        private Collider bulletCollider;
        private bool isDeactivating = false;
        private TrailRenderer[] trailInstances;

        private void Awake()
        {
            bulletCollider = GetComponent<Collider>();

            // 2. 트레일 프리팹을 총알의 자식으로 생성 (딱 한 번만 실행됨)
            if (trailPrefab != null)
            {
                GameObject tObj = Instantiate(trailPrefab, transform);
                tObj.transform.localPosition = Vector3.zero; // 위치 정렬
                trailInstances = tObj.GetComponentsInChildren<TrailRenderer>();
            }
        }

        void Update()
        {
            //if (isDeactivating) return;
            transform.position += transform.forward * speed * Time.deltaTime;
            if (Vector3.Distance(spawnPostion, transform.position) > maxDistance)
            {
                Deactivate();
            }
        }

        public void Init(BulletObjectPool ownerPool, Vector3 position, Quaternion rotation, float maxDistance, float damage, float speed)
        {
            this.ownerPool = ownerPool;
            transform.position = position;
            transform.rotation =  rotation;
            spawnPostion = position;
            this.maxDistance = maxDistance;
            this.damage = damage;
            this.speed = speed;

            isDeactivating = false;
            bulletCollider.enabled = true;
            if (bulletBodyVFX != null)
            {
                bulletBodyVFX.gameObject.SetActive(true);
            }

            if(trailInstances != null)
            {
                foreach (var trail in trailInstances)
                {
                    if (trail != null)
                    {
                        trail.Clear(); // 잔상 지우기
                        trail.emitting = true; // 다시 그리기 시작
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (isDeactivating) return;

            if (other.gameObject.CompareTag("Player"))
                return;
            if (other.gameObject.CompareTag(targetTag))
            {
                if (other.TryGetComponent(out HealthComponent healthComponent))
                {
                    healthComponent.TakeDamage(damage);
                    
                    //ownerPool.Return(gameObject);
                }
            }
            Debug.Log(other.gameObject.name);
            Deactivate();
        }

        private void Deactivate()
        {
            if (isDeactivating) return;
            StartCoroutine(DeactivateRoutine());
        }

        private IEnumerator DeactivateRoutine()
        {
            isDeactivating = true;
            bulletCollider.enabled = false; // 충돌 끄기

            if (bulletBodyVFX != null)
            {
                bulletBodyVFX.gameObject.SetActive(false);
            }

            if (trailInstances != null)
            {
                foreach (var trail in trailInstances)
                {
                    if (trail != null)
                    {
                        trail.emitting = false; // 꼬리 끊기
                    }
                }
            }

            // 3. 꼬리가 사라질 때까지 대기
            yield return new WaitForSeconds(trailFadeTime);

            // 4. 반납
            ownerPool.Return(gameObject);
        }
    }
}
