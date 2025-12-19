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
        public Animator Animator => animator;

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

        private void InitializeVisuals()
        {
            modelInstance = transform.Find("Model")?.gameObject;

            if (modelInstance == null && modelPrefab != null)
            {
                modelInstance = Instantiate(modelPrefab, transform);
                modelInstance.name = "Model";
            }

            if (modelInstance != null)
            {
                // 로컬 포지션으로 설정! (월드 포지션 X)
                modelInstance.transform.localPosition = modelOffset;  // (0,0,0) 이어야 함
                modelInstance.transform.localRotation = Quaternion.identity;
                modelInstance.transform.localScale = Vector3.one * modelScale;

            }

            animator = modelInstance.GetComponent<Animator>();
            if (animator == null)
            {
                animator = modelInstance.GetComponentInChildren<Animator>();
            }

            if (animator != null && animatorController != null)
            {
                animator.applyRootMotion = false;

                if (animatorController != null)
                {
                    animator.runtimeAnimatorController = animatorController;
                }
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

        public void SetMoving(bool moving) 
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
            PlayDeathAnimation();
        }

        public void PlayDeathAnimation()
        {
            if (animator != null)
            {
                animator.SetBool("IsDead", true);
            }
        }


        public void ResetDeathAnimation()
        {
            if (animator != null)
            {
                animator.SetBool("IsDead", false);
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