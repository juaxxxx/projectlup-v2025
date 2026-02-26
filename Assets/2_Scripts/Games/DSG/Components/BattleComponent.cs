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
        private float bulletSpeed = 0.2f;
        [SerializeField]
        private float moveSpeed = 6.0f;

        private bool impactApplied = false;
        private Vector3 knockbackTarget;
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

        private BattleCameraDirector battleCameraDirector;
        [SerializeField]
        private EWeaponType weaponType;
        private void Awake()
        {
            owner = GetComponent<Character>();
            originPosition = owner.gameObject.transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            if (isKnockback)
            {
                knockbackTimer += Time.deltaTime;
                float t = knockbackTimer / knockbackDuration;

                transform.position = Vector3.Lerp(knockbackTarget, originPosition, t);

                if (knockbackTimer >= knockbackDuration)
                {
                    isKnockback = false;
                    transform.position = originPosition;
                }
            }
        }

        private void FixedUpdate()
        {
            if (owner.AnimationComp.currentState == EAnimStateType.StartDash_Fwd)
            {
                Debug.Log(targetPosition);
                Debug.Log(impactApplied);

                transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, (moveSpeed * Time.deltaTime));
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
                Debug.Log(targetPosition);
                Debug.Log(impactApplied);
                transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, (moveSpeed * Time.deltaTime));
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
                Vector3 dir = projectileTargetPosition - bullet.transform.position;
                float distanceToTarget = dir.magnitude;
                float moveDistance = bulletSpeed;

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
                    owner.ActioneffectPool.StopLoopVFX(bulletEffect.particlePrefab, bulletEffect.name);
                    Destroy(bullet);
                    bullet = null;
                    return;
                }
                else
                {
                    bullet.transform.position += dir.normalized * moveDistance;
                }
            }
        }

        private IEnumerator WaitForRangeAttackEnd()
        {
            yield return new WaitForSecondsRealtime(1f);

            isAttacking = false;
        }

        public void SetHp(float hp)
        {
            currHp = hp;
        }

        public void SetMaxGauge(float gauge)
        {
            maxSkillGauge = gauge;
        }

        public void Attack(List<LineupSlot> targets)
        {
            //if (isAttacking) return;
            if (owner.AnimationComp.currentState != EAnimStateType.Idle) return;

            if (targets == null)
                return;

            targetSlots = targets;

            if (targetSlots.Count < 1)
                return;

            targetPosition = targetSlots[0].AttackedPosition.position; //@TODO 만약 targets가 여러명이면 다 가서 한명씩 때릴지 기획에따라 코드 바꿔야함
            HandleAttackStart();

            isAttacking = true;
        }
        public void ApplyDamageOnce()
        {
            if (isUsingSkill)
            {
                ApplySkillDamage();
                return;
            }

            if (targetSlots == null)
                return;

            for (int i = 0; i < targetSlots.Count; i++)
            {
                var targetChar = targetSlots[i].character;

                if (targetSlots == null || targetSlots[0].character == null || targetSlots[0].character.BattleComp == null)
                    return;

                DamageContext ctx = new DamageContext
                {
                    attack = owner.characterData.attack,
                    enemyDefence = targetChar.characterData.defense,
                    Type = owner.characterData.type,
                    enemyType = targetChar.characterData.type
                };

                bool isWeak;
                float damage = DamageCalculator.Calculator(ctx, out isWeak);

                ActionEffect hiteffect = owner.ActioneffectPool.GetAttackEffectByGetHITEffect(owner.AnimationComp.attackEffect);
                targetChar.BattleComp.TakeDamage(damage, hiteffect, isWeak);
                owner.ScoreComp.UpdateDamageDealt(damage);
            }

            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                var shaker = mainCam.GetComponent<BattleCameraDirector>();
                if (shaker == null)
                    shaker = mainCam.gameObject.AddComponent<BattleCameraDirector>();

                shaker.StartCoroutine(shaker.Shake(0.2f, 0.2f));
            }

            targetSlots.Clear();
            PlusGuage(50);
        }
        public void ApplySkillDamage()
        {
            if (targetSlots == null || targetSlots.Count <= 0)
                return;

            for (int i = 0; i < targetSlots.Count; i++)
            {
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
                    targetSlots[i].character.BattleComp.TakeDamage(damage, skillInfo.gethitEffect, isWeak);
                    owner.ScoreComp.UpdateDamageDealt(damage);
                }

                if (skillInfo.bIsStatusEffect)
                {
                    StatusEffect Status = owner.StatusEffectComp.CreateStatusEffect(skillInfo.effectType, skillInfo.operationType, skillInfo.stack, skillInfo.turn);
                    targetSlots[i].character.StatusEffectComp.AddEffect(Status);
                }
            }

            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                var shaker = mainCam.GetComponent<BattleCameraDirector>();
                if (shaker == null)
                    shaker = mainCam.gameObject.AddComponent<BattleCameraDirector>();

                shaker.StartCoroutine(shaker.Shake(0.2f, 0.2f));
            }

            InitGuage();
        }

        //public virtual void TakeDamage(float amount)
        //{
        //    TakeDamage(amount, ActionEffect.None,false);
        //}
        //public virtual void TakeDamage(float amount, ActionEffect getHitEffect)
        //{
        //    TakeDamage(amount, getHitEffect, false);
        //}
        public virtual void TakeDamage(float amount, ActionEffect getHitEffect = ActionEffect.None, bool isWeak = false)
        {
            if (!isAlive)
                return;

            currHp -= amount;

            owner.AnimationComp.hitEffect = getHitEffect;
            OnDamaged?.Invoke(currHp);

            owner.ScoreComp.UpdateDamageTaken(amount);
            TriggerKnockback();

            if (damageLogPrefab != null)
            {
                Vector3 headPos = transform.position + Vector3.up * 0.8f;
                Quaternion rot = Quaternion.LookRotation(Camera.main.transform.forward);
                GameObject log = Instantiate(damageLogPrefab, headPos, rot);
                log.GetComponent<DamageLog>()?.Setup(amount, isWeak);
            }

            FindFirstObjectByType<HitVignetteEffect>()?.PlayDamageEffect();

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
                    if (!battleCameraDirector)
                    {
                        Camera camera = Camera.main;
                        battleCameraDirector = camera.GetComponent<BattleCameraDirector>();
                    }

                    battleCameraDirector.FocusOnTarget(targetPosition);
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

            if(effect != ActionEffect.None)
            {
                Transform vfxRoot = bullet.transform;
                bulletEffect = owner.ActioneffectPool.PlayVFXAttached(effect, vfxRoot, new Vector3(0, 0, 0), Quaternion.identity, true);
            }

            projectileTargetPosition = targetSlots[0].AttackedPosition.position;
            projectileTargetPosition.y += 1.2f;

            if (!battleCameraDirector)
            {
                Camera camera = Camera.main;
                battleCameraDirector = camera.GetComponent<BattleCameraDirector>();
            }

            battleCameraDirector.FocusOnTarget(projectileTargetPosition);
        }

        public virtual void Die()
        {
            isAlive = false;
            if (owner != null)
            {
                float score = owner.ScoreComp.CalculateMVPScore();
                int charid = owner.IconCacheKey;

                GameObject prefab = (owner.characterModelData != null) ? owner.characterModelData.prefab : null;

                DeckStrategyStage stage = LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
                if (stage != null)
                {
                    BattleSystem battleSystem = stage.GetBattleSystem();
                    if(battleSystem != null)
                    {
                        battleSystem.BackupDeadCharacter(owner);
                    }
                }
            }

            owner.ClearCharacterInfo();
            OnDie?.Invoke(owner.battleIndex);
        }

        private void TriggerKnockback()
        {
            if (isKnockback) return;

            originPosition = transform.position;

            float dirX = owner.isEnemy ? 1f : -1f;
            knockbackTarget = originPosition + new Vector3(dirX * knockbackDistance, 0f, 0f);

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
            currGauge = Mathf.Min(maxSkillGauge, currGauge + amount);

            if (currGauge >= maxSkillGauge)
            {
                isSkillOn = true;
                SoundManager.Instance.PlaySFX("Skill_Disarm");
            }

            OnChangeGauge?.Invoke(currGauge);
        }

        private void InitGuage()
        {
            maxSkillGauge = 100;
            currGauge = 0;
            isSkillOn = false;
            isUsingSkill = false;
            targetSlots.Clear();
            OnChangeGauge?.Invoke(currGauge);
        }

        public void AttackStart()
        {
            OnAttackStarted?.Invoke(weaponType);
        }

        public IEnumerator FocusSkillCaster()
        {
            Transform cameraOrigin = Camera.main.transform;

            yield return battleCameraDirector.FocusOnSkillCaster(transform, cameraOrigin).WaitForCompletion();

            battleCameraDirector.FocusOnTarget(targetPosition);
        }
    }
}