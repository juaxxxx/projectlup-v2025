using DSG.Utils.Enums;
using System;
using TMPro;
using UnityEngine;
using static DSG.Result_CharacterDisplay;
using static UnityEngine.UI.GridLayoutGroup;

namespace DSG
{
    public class BattleComponent : MonoBehaviour
    {
        private Character owner;

        public float currHp;
        public float maxSkillGauge { get; private set; }
        public float currGauge = 0;

        public bool isAttacking = false;

        private LineupSlot targetSlot;
        private Vector3 originPosition;
        private Vector3 targetPosition;

        [SerializeField]
        private GameObject bulletPrefab;

        private GameObject bullet;
        private float bulletSpeed = 0.8f;

        private bool impactApplied = false;

        private Vector3 knockbackTarget;
        private float knockbackDistance = 0.4f;
        private float knockbackDuration = 0.2f;
        private float knockbackTimer = 0f;
        private bool isKnockback = false;

        public bool isAlive { get; private set; } = true;

        public event Action<float> OnDamaged;
        public event Action<int> OnDie;
        public event Action<float> OnChangeGauge;

        public event Action<ERangeType> OnAttackStarted;
        public event Action<bool> OnReachedTargetPos;
        public event Action OnMeleeAttack;
        public event Action OnEndMelee;

        [SerializeField]
        private GameObject damageLogPrefab;

        void Start()
        {
            owner = GetComponent<Character>();
            originPosition = owner.gameObject.transform.position;
            SetStatus(owner.characterData);
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

                    if (!isAlive)
                    {
                        owner.ClearCharacterInfo();
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            if (isAttacking)
            {
                if (owner.characterData.rangeType == ERangeType.Melee)
                {
                    transform.position = Vector3.MoveTowards(gameObject.transform.position, targetPosition, 0.5f);
                    if (transform.position == targetSlot.AttackedPosition.position)
                    {
                        if (!impactApplied)
                        {
                            // ���� ��ġ�� �̵� �Ϸ��
                            OnReachedTargetPos?.Invoke(true);
                            // ���� �ִϸ��̼� ���� 
                            OnMeleeAttack?.Invoke();
                            // ���� �ִϸ��̼� �����
                            OnEndMelee?.Invoke();
                            ApplyDamageOnce();
                            if (currGauge == maxSkillGauge)
                            {
                                currGauge = 0;
                            }
                            impactApplied = true;
                        }
                        targetPosition = originPosition;
                    }
                    else if (transform.position == originPosition)
                    {
                        isAttacking = false;
                        impactApplied = false;
                        OnReachedTargetPos?.Invoke(false);
                    }
                }
                else
                {
                    if (bullet == null) return;

                    Vector3 dir = targetPosition - bullet.transform.position;
                    float distanceToTarget = dir.magnitude;
                    float moveDistance = bulletSpeed;

                    if (distanceToTarget <= moveDistance)
                    {
                        bullet.transform.position = targetPosition;

                        if (!impactApplied)
                        {
                            ApplyDamageOnce();
                            impactApplied = true;
                        }

                        isAttacking = false;
                        impactApplied = false;
                        Destroy(bullet);
                        bullet = null;
                        return;
                    }
                    bullet.transform.position += dir.normalized * moveDistance;
                }
            }
        }

        public void SetStatus(CharacterData data)
        {
            maxHp = data.maxHp;
            attack = data.attack;
            defense = data.defense;
            speed = data.speed;
            currHp = maxHp;
            maxSkillGauge = 100;
        }

        public void Attack(LineupSlot target)
        {
            if (isAttacking) return;

            if (target == null)
                return;

            targetSlot = target;
            targetPosition = targetSlot.AttackedPosition.position;
            if (owner.characterData.rangeType == ERangeType.Range)
            {
                bullet = Instantiate(bulletPrefab, originPosition, Quaternion.identity);
            }

            OnAttackStarted?.Invoke(owner.characterData.rangeType);

            isAttacking = true;
        }

        private void ApplyDamageOnce()
        {
            if (targetSlot == null)
                return;

            var targetChar = targetSlot.character;

            if (targetSlot == null || targetSlot.character == null || targetSlot.character.BattleComp == null)
                return;

            float damage = attack;
            targetChar.BattleComp.TakeDamage(damage);
            owner.ScoreComp.UpdateDamageDealt(damage);

            PlusGuage(50);
        }

        public virtual void TakeDamage(float amount)
        {
            if (!isAlive)
                return;

            currHp -= amount;

            OnDamaged?.Invoke(currHp);
            owner.ScoreComp.UpdateDamageTaken(amount);
            TriggerKnockback();

            if (damageLogPrefab != null)
            {
                Vector3 headPos = transform.position + Vector3.up * 1.8f;
                Quaternion rot = Quaternion.LookRotation(Camera.main.transform.forward);
                GameObject log = Instantiate(damageLogPrefab, headPos, rot);
                log.GetComponent<DamageLog>()?.Setup(amount);
            }

            Camera mainCam = Camera.main;
            if (mainCam != null)
            {
                var shaker = mainCam.GetComponent<CameraShake>();
                if (shaker == null)
                    shaker = mainCam.gameObject.AddComponent<CameraShake>();

                shaker.StartCoroutine(shaker.Shake(0.2f, 0.05f));
            }

            if (currHp <= 0)
            {
                currHp = 0;
                Die();
            }
        }
        public virtual void Die()
        {
            isAlive = false;
            if (owner != null && owner.characterData != null && owner.characterModelData != null && owner.ScoreComp != null)
            {
                float score = owner.ScoreComp.CalculateMVPScore();
                Color color = owner.characterModelData.material.GetColor("_BaseColor");
                BattleSystem.Instance?.BackupDeadCharacter(owner.characterData.characterName, color, score);
            }
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
            currGauge += amount;
            OnChangeGauge?.Invoke(currGauge);
        }
    }
}