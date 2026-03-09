using UnityEngine;

namespace LUP.DSG
{
    public class Character : MonoBehaviour
    {
        private StatusEffectComponent statusEffectComp;
        private BattleComponent battleComp;
        private ScoreComponent scoreComp;
        private AnimationComponent animationComp;
        public StatusEffectComponent StatusEffectComp => statusEffectComp;
        public BattleComponent BattleComp => battleComp;
        public ScoreComponent ScoreComp => scoreComp;
        public AnimationComponent AnimationComp => animationComp;

        public CharacterData characterData { get; private set; }
        public CharacterPrefabData characterPrefabData { get; private set; }
        public CharacterInfo characterInfo { get; private set; }

        public bool isEnemy = false;
        public int battleIndex = -1;

        public EffectPool actionEffectPool;

        private CharacterHeadupUI characterUI;

        [SerializeField]
        private Vector3 uiOffset = Vector3.zero;

        public int IconCacheKey { get; private set; }
        public Sprite BattleIcon { get; private set; }

        private void Awake()
        {
            InitComponents();
            actionEffectPool = FindAnyObjectByType<EffectPool>();
        }
        private void OnDestroy()
        {
            if (battleComp != null && animationComp != null)
            {
                battleComp.OnAttackStarted -= animationComp.StartAttackAnimation;
                battleComp.OnDamaged -= animationComp.PlayHittedAnimation;
                battleComp.OnDie -= animationComp.PlayDiedAnimation;
                battleComp.OnStartDash -= animationComp.StartDashAnimation;

                animationComp.OnHitAttack -= battleComp.ApplyDamageOnce;
                animationComp.OnShootRangeAttack -= battleComp.TrySpawnProjectileForRangedAttack;
                animationComp.OnAttackStart -= battleComp.AttackStart;

                if (statusEffectComp != null)
                    battleComp.OnDie -= statusEffectComp.HandleOwnerDie;
            }

            ReleaseCharacterUI();
        }
        public void ManualInitializeAfterSpawn()
        {
            InitComponents();

            battleComp.OnAttackStarted += animationComp.StartAttackAnimation;
            battleComp.OnDamaged += animationComp.PlayHittedAnimation;
            battleComp.OnDie += animationComp.PlayDiedAnimation;
            battleComp.OnStartDash += animationComp.StartDashAnimation;

            animationComp.OnHitAttack += battleComp.ApplyDamageOnce;
            animationComp.OnShootRangeAttack += battleComp.TrySpawnProjectileForRangedAttack;
            animationComp.OnAttackStart += battleComp.AttackStart;

            BattleComp.OnDie += statusEffectComp.HandleOwnerDie;

            EnsureCharacterUI();
        }

        public void EndTurn()
        {
            statusEffectComp?.TurnAll();
        }

        public void SetCharacterData(CharacterInfo info)
        {
            DeckStrategyStage stage = LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
            if (stage == null || info == null) return;

            CharacterData data = stage.FindCharacterData(info.characterID, info.characterLevel);
            CharacterPrefabData prefabData = stage.FindCharacterPrefabData(info.characterModelID);

            IconCacheKey = info.characterID;

            if (data == null || prefabData == null) return;

            characterInfo = info;
            characterData = data;
            characterPrefabData = prefabData;

            battleComp.SetHp(data.maxHp);
            BattleComp.SetMaxGauge(100);

            gameObject.SetActive(true);

            EnsureCharacterUI();
            if (characterUI == null) return;

            characterUI.gameObject.SetActive(true);
            characterUI.InitInfoUI(data.type, info.characterLevel);
            characterUI.ActiveInfoUI();
        }

        public void ClearCharacterInfo()
        {
            if (characterUI != null)
                characterUI.gameObject.SetActive(false);
        }

        public void ActiveBattleUI()
        {
            EnsureCharacterUI();
            if (characterUI == null) return;

            characterUI.gameObject.SetActive(true);
            characterUI.InitBattleUI(this);
            characterUI.ActiveBattleUI();
        }

        public void SetBattleIcon(Sprite sprite)
        {
            BattleIcon = sprite;
        }

        public Sprite GetBattleIcon() => BattleIcon;

        private void InitComponents()
        {
            if (statusEffectComp == null)
                statusEffectComp = GetComponent<StatusEffectComponent>();

            if (battleComp == null)
                battleComp = GetComponent<BattleComponent>();

            if (scoreComp == null)
                scoreComp = GetComponent<ScoreComponent>();

            if (animationComp == null)
                animationComp = GetComponent<AnimationComponent>();
        }

        private void EnsureCharacterUI()
        {
            if (characterUI != null) return;

            DeckStrategyStage stage = LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
            if (stage == null) return;

            CharacterUIPool uiPool = stage.GetComponent<CharacterUIPool>();
            if (uiPool == null) return;

            Transform target = transform;
            characterUI = uiPool.GetUI(target, uiOffset);
        }
        public void ReleaseCharacterUI()
        {
            if (characterUI == null) return;

            DeckStrategyStage stage = LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
            if (stage == null) return;

            CharacterUIPool uiPool = stage.GetComponent<CharacterUIPool>();

            if (uiPool != null)
                uiPool.Release(characterUI);
            else if (characterUI != null)
                Destroy(characterUI.gameObject);

            characterUI = null;
        }
    }
}