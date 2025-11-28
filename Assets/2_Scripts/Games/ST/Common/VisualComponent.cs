using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace LUP.ST
{
    public class VisualComponent : MonoBehaviour
    {
        [Header("3D Model 설정")]
        [SerializeField] private GameObject modelPrefab;
        [SerializeField] private RuntimeAnimatorController animatorController;
        [SerializeField] private Vector3 modelOffset = Vector3.zero;
        [SerializeField] private float modelScale = 1f;

        private GameObject modelInstance;
        private StatComponent statComponent;
        private Animator animator;

        // 이동 추적
        private Vector3 lastPosition;
        private bool isMoving = false;

        void Awake()
        {
            if (Application.isPlaying)
            {
                statComponent = GetComponent<StatComponent>();
                if (statComponent != null)
                {
                    statComponent.OnHealthChanged += OnHealthChanged;
                    statComponent.OnDeath += OnDeath;
                }
                lastPosition = transform.position;
            }

            InitializeVisuals();
        }

        // OnValidate 수정 - 모델 재생성 대신 업데이트만
        void OnValidate()
        {
            if (!Application.isPlaying && modelPrefab != null)
            {
                // 지연 실행으로 DestroyImmediate 오류 방지
                UnityEditor.EditorApplication.delayCall += () =>
                {
                    if (this != null)
                    {
                        InitializeVisuals();
                    }
                };
            }
        }

        void Update()
        {
            UpdateMovementAnimation();
        }

        private void InitializeVisuals()
        {
            // 이미 Model이 있으면 재사용
            modelInstance = transform.Find("Model")?.gameObject;

            if (modelInstance == null && modelPrefab != null)
            {
                // 없을 때만 생성
                modelInstance = Instantiate(modelPrefab, transform);
                modelInstance.name = "Model";
            }

            if (modelInstance != null)
            {
                // Transform 업데이트
                UpdateModelTransform();

                // Animator 설정
                animator = modelInstance.GetComponent<Animator>();
                if (animator == null)
                {
                    animator = modelInstance.GetComponentInChildren<Animator>();
                }

                if (animator != null && animatorController != null)
                {
                    animator.runtimeAnimatorController = animatorController;
                }
            }
        }

        // Transform만 업데이트하는 별도 메서드
        private void UpdateModelTransform()
        {
            if (modelInstance != null)
            {
                modelInstance.transform.localPosition = modelOffset;
                modelInstance.transform.localScale = Vector3.one * modelScale;
            }
        }

        // Model 정리용 public 메서드
        public void ClearModel()
        {
            // 모든 Model 찾아서 제거
            foreach (Transform child in transform)
            {
                if (child.name == "Model")
                {
                    if (Application.isPlaying)
                        Destroy(child.gameObject);
                    else
                        DestroyImmediate(child.gameObject);
                }
            }
            modelInstance = null;
        }

        // Model 재생성용 public 메서드
        public void RecreateModel()
        {
            ClearModel();
            InitializeVisuals();
        }
        private void UpdateMovementAnimation()
        {
            if (animator == null) return;

            Vector3 currentPosition = transform.position;
            float moveDistance = Vector3.Distance(currentPosition, lastPosition);

            bool currentlyMoving = moveDistance > 0.01f;

            if (currentlyMoving != isMoving)
            {
                isMoving = currentlyMoving;
                animator.SetBool("IsMoving", isMoving);

                // 3D는 보통 MoveSpeed float로 블렌드
                if (isMoving)
                {
                    float speed = moveDistance / Time.deltaTime;
                    animator.SetFloat("MoveSpeed", speed / statComponent.MoveSpeed);
                }
                else
                {
                    animator.SetFloat("MoveSpeed", 0);
                }
            }

            // 3D 회전 처리 (이동 방향을 바라보기)
            if (currentlyMoving)
            {
                Vector3 direction = (currentPosition - lastPosition).normalized;
                direction.y = 0; // Y축 회전만

                if (direction != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    modelInstance.transform.rotation = Quaternion.Slerp(
                        modelInstance.transform.rotation,
                        targetRotation,
                        Time.deltaTime * 10f
                    );
                }
            }

            lastPosition = currentPosition;
        }

        // Public 메서드들
        public void PlayAttackAnimation()
        {
            animator?.SetTrigger("Attack");
        }

        public void PlayHitAnimation()
        {
            animator?.SetTrigger("Hit");
        }

        public void PlayReloadAnimation()
        {
            animator?.SetTrigger("Reload");
        }

        public void SetMoving(bool moving)  // ← 여기 있어요!
        {
            if (animator != null)
            {
                animator.SetBool("IsMoving", moving);
            }
        }

        // 이벤트 핸들러
        private void OnHealthChanged(float current, float max)
        {
            float healthRatio = current / max;
            if (animator != null)
            {
                animator.SetFloat("HealthRatio", healthRatio);
            }
        }

        private void OnDeath()
        {
            if (animator != null)
            {
                animator.SetBool("IsDead", true);
            }
        }

        void OnDestroy()
        {
            if (statComponent != null)
            {
                statComponent.OnHealthChanged -= OnHealthChanged;
                statComponent.OnDeath -= OnDeath;
            }
        }
    }
}