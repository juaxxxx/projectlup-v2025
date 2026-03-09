using DG.Tweening;
using LUP.DSG.Utils;
using LUP.DSG.Utils.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LUP.DSG
{
    public class BattleComponent : MonoBehaviour
    {
        private Character owner;

        public float currHp;
        public float maxSkillGauge { get; private set; }
        public float currGauge = 0f;

        public bool isAttacking = false;
        public bool isUsingSkill = false;

        private List<LineupSlot> targetSlots;
        private Vector3 originPosition;
        private Vector3 targetPosition;

        [SerializeField]
        private GameObject bulletPrefab;

        private GameObject bullet;
        private EffectParticlePair bulletEffect;
        private float bulletSpeed = 10.0f;
        [SerializeField]
        private float moveSpeed = 6.0f;

        private bool impactApplied = false;
        private Vector3 knockbackTarget;
        private Vector3 knockbackOriginPosition;

        private float knockbackDistance = 0.4f;
        private float knockbackDuration = 0.2f;
        private float knockbackTimer = 0f;
        private bool isKnockback = false;

        private Vector3 projectileTargetPosition;

        public bool isAlive { get; private set; } = true;
        public bool isSkillOn { get; private set; } = false;

        public SkillInfoData skillInfo;

        public event Action<float> OnDamaged;
        public event Action<int> OnDie;
        public event Action<float> OnChangeGauge;

        public event Action<EWeaponType> OnAttackStarted;
        public event Action OnStartDash;

        [SerializeField]
        private GameObject damageLogPrefab;

        [SerializeField]
        private EWeaponType weaponType;

        private BattleCameraDirector battleCameraDirector;

        private Camera mainCamera;
        private HitVignetteEffect hitVignetteEffect;

        private void Awake()
        {
            owner = GetComponent<Character>();
            originPosition = transform.position;
            mainCamera = Camera.main;
            if (mainCamera != null)
                battleCameraDirector = mainCamera.GetComponent<BattleCameraDirector>();
        }

        // Update is called once per frame
        void Update()
        {
            if (!isKnockback) return;

            knockbackTimer += Time.deltaTime;

            float t = knockbackDuration > 0f ? (knockbackTimer / knockbackDuration) : 1f;

            transform.position = Vector3.Lerp(knockbackTarget, knockbackOriginPosition, t);

            if (knockbackTimer >= knockbackDuration)
            {
                isKnockback = false;
                transform.position = knockbackOriginPosition;
            }
        }

        private void FixedUpdate()
        {
            if (owner == null || owner.AnimationComp == null) return;

            if (owner.AnimationComp.currentState == EAnimStateType.StartDash_Fwd)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
                {
                    if (!impactApplied)
                    {
                        owner.AnimationComp.OnEndFwdDashEvent();

                        targetPosition = originPosition;
                        impactApplied = true;
                    }
                }
                else if (isUsingSkill && Vector3.Distance(transform.position, skillInfo.AttackPosition) < 0.01f)
                {
                    if (!impactApplied)
                    {
                        owner.AnimationComp.OnEndFwdDashEvent();

                        targetPosition = originPosition;
                        impactApplied = true;
                    }
                }
            }
            else if (owner.AnimationComp.currentState == EAnimStateType.StartDash_Bwd)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                if (Vector3.Distance(transform.position, originPosition) < 0.01f)
                {
                    owner.AnimationComp.OnEndBwdDashEvent();
                    impactApplied = false;
                    isUsingSkill = false;
                    isAttacking = false;
                }
            }
            else if (bullet != null)
            {
                UpdateProjectile();
            }
        }

        private void UpdateProjectile()
        {
            Vector3 dir = projectileTargetPosition - bullet.transform.position;
            float distanceToTarget = dir.magnitude;
            float moveDistance = bulletSpeed * Time.deltaTime;

            if (distanceToTarget <= moveDistance)
            {
                bullet.transform.position = projectileTargetPosition;

                if (!impactApplied)
                {
                    ApplyDamageOnce();
                    impactApplied = true;
                    battleCameraDirector.BackToOriginPos(0.5f);
                }

                StartCoroutine(WaitForRangeAttackEnd());
                impactApplied = false;

                if (owner != null && owner.actionEffectPool != null)
                    owner.actionEffectPool.StopLoopVFX(bulletEffect.particlePrefab, bulletEffect.name);

                Destroy(bullet);
                bullet = null;
                return;
            }
            else
            {
                bullet.transform.position += dir.normalized * moveDistance;
            }
        }

        private IEnumerator WaitForRangeAttackEnd()
        {
            yield return new WaitForSecondsRealtime(1f);
            isAttacking = false;
        }

        public void SetHp(float hp) => currHp = hp;
        public void SetMaxGauge(float gauge) => maxSkillGauge = gauge;

        public void Attack(List<LineupSlot> targets)
        {
            if (owner == null || owner.AnimationComp == null) return;
            if (owner.AnimationComp.currentState != EAnimStateType.Idle) return;
            if (targets == null || targets.Count == 0) return;

            targetSlots = targets;

            LineupSlot first = targetSlots[0];
            if (first == null || first.attackedPosition == null) return;

            targetPosition = first.attackedPosition.position; //@TODO 만약 targets가 여러명이면 다 가서 한명씩 때릴지 기획에따라 코드 바꿔야함
            
            HandleAttackStart();
            isAttacking = true;
        }
        public void ApplyDamageOnce()
        {
            if (!isAlive)
                return;

            if (owner == null || owner.characterData == null || owner.ScoreComp == null || 
                owner.AnimationComp == null || owner.actionEffectPool == null)
                return;

            if (isUsingSkill)
            {
                ApplySkillDamage();
                return;
            }

            if (targetSlots == null || targetSlots.Count == 0)
                return;

            for (int i = 0; i < targetSlots.Count; i++)
            {
                LineupSlot slot = targetSlots[i];
                Character targetChar = slot != null ? slot.character : null;

                if (targetChar == null || targetChar.characterData == null || targetChar.BattleComp == null)
                    continue;

                DamageContext ctx = new DamageContext
                {
                    attack = owner.characterData.attack,
                    enemyDefence = targetChar.characterData.defense,
                    Type = owner.characterData.type,
                    enemyType = targetChar.characterData.type
                };

                bool isWeak;
                float damage = DamageCalculator.Calculator(ctx, out isWeak);

                ActionEffect hiteffect = owner.actionEffectPool.GetAttackEffectByGetHITEffect(owner.AnimationComp.attackEffect);
                targetChar.BattleComp.TakeDamage(damage, hiteffect, isWeak);
                owner.ScoreComp.UpdateDamageDealt(damage);
            }

            ShakeCamera();

            targetSlots.Clear();
            PlusGuage(50);
        }
        public void ApplySkillDamage()
        {
            if (targetSlots == null || targetSlots.Count <= 0)
                return;

            if (owner == null || owner.characterData == null || owner.ScoreComp == null)
                return;

            for (int i = 0; i < targetSlots.Count; i++)
            {
                LineupSlot slot = targetSlots[i];
                Character targetChar = slot != null ? slot.character : null;

                if (targetChar == null || targetChar.characterData == null || targetChar.BattleComp == null)
                    continue;

                if (skillInfo.bIsDamaged)
                {
                    DamageContext ctx = new DamageContext
                    {
                        attack = owner.characterData.attack,
                        enemyDefence = targetSlots[i].character.characterData.defense,
                        Type = owner.characterData.type,
                        enemyType = targetSlots[i].character.characterData.type
                    };

                    bool isWeak;
                    float damage = DamageCalculator.Calculator(ctx, out isWeak) + skillInfo.damage;
                    targetChar.BattleComp.TakeDamage(damage, skillInfo.gethitEffect, isWeak);
                    owner.ScoreComp.UpdateDamageDealt(damage);
                }

                if (skillInfo.bIsStatusEffect)
                {
                    StatusEffect Status = owner.StatusEffectComp.CreateStatusEffect(skillInfo.effectType, skillInfo.operationType, skillInfo.stack, skillInfo.turn);
                    targetChar.StatusEffectComp.AddEffect(Status);
                }
            }

            ShakeCamera();
            InitGuage();
        }
        public virtual void TakeDamage(float amount, ActionEffect getHitEffect = ActionEffect.None, bool isWeak = false)
        {
            if (!isAlive)
                return;

            currHp -= amount;

            if (owner != null && owner.AnimationComp != null)
                owner.AnimationComp.hitEffect = getHitEffect;

            OnDamaged?.Invoke(currHp);

            owner?.ScoreComp?.UpdateDamageTaken(amount);
            TriggerKnockback();

            if (damageLogPrefab != null && mainCamera != null)
            {
                Vector3 headPos = transform.position + Vector3.up * 0.8f;
                Quaternion rot = Quaternion.LookRotation(mainCamera.transform.forward);
                GameObject log = Instantiate(damageLogPrefab, headPos, rot);
                log.GetComponent<DamageLog>()?.Setup(amount, isWeak);
            }

            if (hitVignetteEffect == null)
                hitVignetteEffect = FindFirstObjectByType<HitVignetteEffect>();

            hitVignetteEffect?.PlayDamageEffect();

            if (currHp <= 0)
            {
                currHp = 0;
                Die();
            }
        }

        public void Skill(List<LineupSlot> targetList)
        {
            targetSlots = targetList;
            targetPosition = skillInfo.AttackPosition;

            HandleAttackStart();

            if (owner != null && owner.AnimationComp != null)
                owner.AnimationComp.attackEffect = skillInfo.attackEffect;

            isUsingSkill = true;
            isAttacking = true;
        }

        private void HandleAttackStart()
        {
            switch (weaponType)
            {
                case EWeaponType.Melee_OneHanded:
                case EWeaponType.Melee_TwoHanded:
                    OnStartDash?.Invoke();

                    if(battleCameraDirector == null)
                    {
                        if (mainCamera == null) mainCamera = Camera.main;
                        battleCameraDirector = mainCamera?.GetComponent<BattleCameraDirector>();
                    }

                    battleCameraDirector?.FocusOnTarget(targetPosition);
                    break;
                case EWeaponType.Magic:
                case EWeaponType.Gun_Rifle:
                case EWeaponType.Throw:
                    AttackStart();
                    break;
            }
        }
        public void TrySpawnProjectileForRangedAttack()
        {
            if (weaponType != EWeaponType.Magic && weaponType != EWeaponType.Gun_Rifle && weaponType != EWeaponType.Throw)
                return;

            if (bulletPrefab == null)
                return;

            Vector3 spawnPos = originPosition;
            spawnPos.y += 1.2f;

            bullet = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
            ActionEffect effect = ActionEffect.None;
            switch (weaponType)
            {
                case EWeaponType.Magic:
                    effect = ActionEffect.MagicBullet;
                    break;
                case EWeaponType.Throw:
                    effect = ActionEffect.ThrowBullet;
                    break;
            }

            if(effect != ActionEffect.None && owner != null && owner.actionEffectPool != null)
            {
                Transform vfxRoot = bullet.transform;
                bulletEffect = owner.actionEffectPool.PlayVFXAttached(effect, vfxRoot, Vector3.zero, Quaternion.identity, true);
            }

            if (targetSlots == null || targetSlots.Count == 0 || targetSlots[0] == null || targetSlots[0].attackedPosition == null)
                return;

            projectileTargetPosition = targetSlots[0].attackedPosition.position;
            projectileTargetPosition.y += 1.2f;

            if (battleCameraDirector == null)
            {
                if (mainCamera == null) mainCamera = Camera.main;
                battleCameraDirector = mainCamera?.GetComponent<BattleCameraDirector>();
            }

            battleCameraDirector?.FocusOnTarget(projectileTargetPosition);
        }

        public virtual void Die()
        {
            isAlive = false;
            if (owner != null)
            {
                DeckStrategyStage currentStage = LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
                BattleSystem battleSystem = currentStage != null ? currentStage.GetBattleSystem() : null;
                battleSystem?.BackupDeadCharacter(owner);

                //DeckStrategyStage stage = LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
                //if (stage != null)
                //{
                //    BattleSystem battleSystem = stage.GetBattleSystem();
                //    if(battleSystem != null)
                //    {
                //        battleSystem.BackupDeadCharacter(owner);
                //    }
                //}
            }

            owner?.ClearCharacterInfo();
            OnDie?.Invoke(owner != null ? owner.battleIndex : -1);
        }

        private void TriggerKnockback()
        {
            if (isKnockback) return;

            knockbackOriginPosition = transform.position;

            float dirX = owner != null && owner.isEnemy ? 1f : -1f;
            knockbackTarget = knockbackOriginPosition + new Vector3(dirX * knockbackDistance, 0f, 0f);

            knockbackTimer = 0f;
            isKnockback = true;
        }

        private void OnDisable()
        {
            OnDamaged = null;
            OnDie = null;
        }

        public void PlusGuage(float amount)
        {
            float prevGauge = currGauge;
            bool wasSkillOn = isSkillOn;

            currGauge = Mathf.Min(maxSkillGauge, currGauge + amount);
            isSkillOn = currGauge >= maxSkillGauge;

            if (!wasSkillOn && isSkillOn)
                SoundManager.Instance.PlaySFX("Skill_Disarm");

            if (!Mathf.Approximately(prevGauge, currGauge))
                OnChangeGauge?.Invoke(currGauge);
        }

        private void InitGuage()
        {
            currGauge = 0;
            isSkillOn = false;
            isUsingSkill = false;

            targetSlots?.Clear();
            OnChangeGauge?.Invoke(currGauge);
        }

        public void AttackStart()
        {
            OnAttackStarted?.Invoke(weaponType);
        }

        //public IEnumerator FocusSkillCaster()
        //{
        //    Transform cameraOrigin = Camera.main.transform;

        //    yield return battleCameraDirector.FocusOnSkillCaster(transform, cameraOrigin).WaitForCompletion();

        //    battleCameraDirector.FocusOnTarget(targetPosition);
        //}

        private void ShakeCamera()
        {
            if (mainCamera == null) mainCamera = Camera.main;
            if (mainCamera == null) return;

            if (battleCameraDirector == null)
                battleCameraDirector = mainCamera.GetComponent<BattleCameraDirector>();

                battleCameraDirector?.StartCoroutine(battleCameraDirector.Shake(0.2f, 0.2f));
        }
    }
}