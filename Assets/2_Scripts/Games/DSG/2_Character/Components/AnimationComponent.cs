using DG.Tweening;
using LUP.DSG.Utils.Enums;
using System;
using System.Numerics;
using TMPro;
using UnityEngine;

namespace LUP.DSG
{
    public class AnimationComponent : MonoBehaviour
    {
        private Character owner;

        [SerializeField]
        private Animator animator;

        public event Action OnHitAttack;
        public event Action OnShootRangeAttack;
        public event Action OnAttackStart;

        public EAnimStateType currentState { get; private set; }

        public ActionEffect hitEffect { private get; set; }
        public ActionEffect attackEffect;
        public UnityEngine.Vector3 effectOffset = new UnityEngine.Vector3(0, 1.5f, 0);

        private BattleCameraDirector battleCameraDirector;

        [SerializeField]
        private float attackSpeed = 1.0f;
        [SerializeField]
        private float hitSpeed = 1.0f;
        [SerializeField]
        private float backwardSpeed = 1.0f;


        void Start()
        {
            owner = GetComponent<Character>();
            currentState = EAnimStateType.Idle;

            if (!battleCameraDirector)
            {
                Camera camera = Camera.main;
                battleCameraDirector = camera.GetComponent<BattleCameraDirector>();
            }
            animator.SetFloat("AttackSpeed", attackSpeed);
            animator.SetFloat("HitSpeed", hitSpeed);
            animator.SetFloat("BackwardSpeed", backwardSpeed);
        }

        public void StartDashAnimation()
        {
            currentState = EAnimStateType.StartDash_Fwd;
            SetAnimationState(currentState);
        }

        public void StartAttackAnimation(EWeaponType weaponType)
        {
            switch (weaponType)
            {
                case EWeaponType.Melee_OneHanded:
                    attackEffect = ActionEffect.Attack_Melee_OneHanded;
                    break;
                case EWeaponType.Melee_TwoHanded:
                    attackEffect = ActionEffect.Attack_Melee_TwoHanded;
                    break;
                case EWeaponType.Gun_Rifle:
                    attackEffect = ActionEffect.Attack_Gun_Rifle;
                    break;
                case EWeaponType.Magic:
                    attackEffect = ActionEffect.Attack_Magic;
                    break;
                case EWeaponType.Throw:
                    attackEffect = ActionEffect.Attack_Throw;
                    break;
                default:
                    break;
            }
            currentState = EAnimStateType.Attack;
            SetAnimationState(currentState);
        }

        public void StartMeleeAnimation()
        {
            SetAnimationState(currentState);
        }

        public void OnEndMeleeAnimationEvent()
        {
            currentState = EAnimStateType.StartDash_Bwd;
            SetAnimationState(currentState);
            battleCameraDirector.BackToOriginPos();
        }

        public void OnEndRangeAnimationEvent()
        {
            currentState = EAnimStateType.Idle;
            SetAnimationState(currentState);
        }

        public void PlayHittedAnimation(float damage)
        {
            currentState = EAnimStateType.Hitted;
            SetAnimationState(currentState);

            PlayHitSoundEffect();
        }

        public void PlayDiedAnimation(int index)
        {
            owner.transform.DORotate(new UnityEngine.Vector3(0, -90, 0), 0.5f, RotateMode.WorldAxisAdd);
            currentState = EAnimStateType.Died;
            SetAnimationState(currentState);
        }

        private void SetAnimationState(EAnimStateType type)
        {
            animator.SetInteger("CharacterState", (int)type);
        }

        public void OnHitMeleeAttackEvent()
        {
            OnHitAttack?.Invoke();
            OnPunchEffect();
        }

        public void OnShootRangeAttackEvent()
        {
            OnShootRangeAttack?.Invoke();

            PlayAttackSoundEffect();
        }

        public void OnEndFwdDashEvent()
        {
            OnAttackStart?.Invoke();
        }

        public void OnEndBwdDashEvent()
        {
            currentState = EAnimStateType.Idle;
            SetAnimationState(currentState);
        }

        public void OnHittedEndEvent()
        {
            currentState = EAnimStateType.Idle;
            SetAnimationState(currentState);

        }

        public void OnPunchEffect()
        {
            PlayAttackSoundEffect();
        }

        private void PlayAttackSoundEffect()
        {
            if (owner == null || owner.actionEffectPool == null)
                return;

            if (attackEffect == ActionEffect.None) return;

            owner.actionEffectPool.PlayVFX(attackEffect, owner.transform.position + effectOffset, owner.transform.rotation, false);
            string sfx = owner.actionEffectPool.GetActionBySound(attackEffect);
            if (!string.IsNullOrEmpty(sfx))
                SoundManager.Instance.PlaySFX(sfx);
        }

        private void PlayHitSoundEffect()
        {
            if (owner == null || owner.actionEffectPool == null)
                return;

            if (hitEffect == ActionEffect.None)
                return;

            owner.actionEffectPool.PlayVFX(hitEffect, owner.transform.position + effectOffset, owner.transform.rotation, false);
            string sfx = owner.actionEffectPool.GetActionBySound(hitEffect);
            if (!string.IsNullOrEmpty(sfx))
                SoundManager.Instance.PlaySFX(sfx);
        }
    }
}