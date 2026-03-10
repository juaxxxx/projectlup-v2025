using UnityEngine;
using System.Collections;

namespace LUP.ES
{
    public class Bullet : MonoBehaviour
    {
        [Header("VFX")]
        [SerializeField] private ParticleSystem bulletBodyVFX;
        [SerializeField] private GameObject trailPrefab;
        [SerializeField] private float trailFadeTime = 0.5f; // 꼬리가 사라지는 시간
        [SerializeField] private GameObject hitPrefab;

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
        private VFXObjectPool vfxObjectPool;

        private void Awake()
        {
            bulletCollider = GetComponent<Collider>();

            if (trailPrefab != null)
            {
                GameObject tObj = Instantiate(trailPrefab, transform);
                tObj.transform.localPosition = Vector3.zero;
                trailInstances = tObj.GetComponentsInChildren<TrailRenderer>();
            }

            vfxObjectPool = FindFirstObjectByType<VFXObjectPool>();
        }

        void Update()
        {
            if (isDeactivating) return;
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

            if (other.gameObject.CompareTag(targetTag))
            {
                if (other.TryGetComponent(out HealthComponent healthComponent))
                {
                    healthComponent.TakeDamage(damage);
                    Deactivate();
                    //ownerPool.Return(gameObject);
                }
            }

        }

        private void Deactivate()
        {
            if (isDeactivating) return;
            StartCoroutine(DeactivateRoutine());
            if (vfxObjectPool != null)
            {
                vfxObjectPool.SpawnVFX(hitPrefab, transform.position);
            }
            SoundManager.Instance.PlaySFX("GunHit",gameObject);
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
