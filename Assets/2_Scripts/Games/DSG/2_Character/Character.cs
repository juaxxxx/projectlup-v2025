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
        public CharacterModelData characterModelData { get; private set; }
        public OwnedCharacterInfo characterInfo { get; private set; }

        public bool isEnemy = false;
        public int battleIndex = -1;

        [SerializeField]
        private GameObject characterUIPrefab;

        public EffectPool actionEffectPool;

        private CharacterHeadupUI characterUI;

        [SerializeField]
        private Transform uiTransform;

        [SerializeField]
        private Vector3 uiOffset = new Vector3(0, 0, 0);

        public int IconCacheKey { get; private set; }
        public Sprite BattleIcon { get; private set; }

        private void Awake()
        {
            statusEffectComp = GetComponent<StatusEffectComponent>();
            battleComp = GetComponent<BattleComponent>();
            scoreComp = GetComponent<ScoreComponent>();
            animationComp = GetComponent<AnimationComponent>();
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

            if (characterUI != null)
                Destroy(characterUI.gameObject);
        }
        public void ManualInitializeAfterSpawn()
        {
            if (battleComp == null) battleComp = GetComponent<BattleComponent>();
            if (animationComp == null) animationComp = GetComponent<AnimationComponent>();
            if (statusEffectComp == null) statusEffectComp = GetComponent<StatusEffectComponent>();
            if (scoreComp == null) scoreComp = GetComponent<ScoreComponent>();

            battleComp.OnAttackStarted += animationComp.StartAttackAnimation;
            battleComp.OnDamaged += animationComp.PlayHittedAnimation;
            battleComp.OnDie += animationComp.PlayDiedAnimation;
            battleComp.OnStartDash += animationComp.StartDashAnimation;

            animationComp.OnHitAttack += battleComp.ApplyDamageOnce;
            animationComp.OnShootRangeAttack += battleComp.TrySpawnProjectileForRangedAttack;
            animationComp.OnAttackStart += battleComp.AttackStart;

            BattleComp.OnDie += statusEffectComp.HandleOwnerDie;

            Canvas uiCanvas = GameObject.Find("Canvas_CharacterUI").GetComponent<Canvas>();
            if (uiCanvas == null || characterUIPrefab == null) return;

            GameObject ui = Instantiate(characterUIPrefab, uiCanvas.transform);
            characterUI = ui.GetComponent<CharacterHeadupUI>();
            if (characterUI == null) return;

            characterUI.SetTarget(uiCanvas, transform, uiOffset);
            characterUI.gameObject.SetActive(true);
        }

        public void EndTurn()
        {
            statusEffectComp.TurnAll();
        }

        public void SetCharacterData(OwnedCharacterInfo info)
        {
            DeckStrategyStage stage = LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
            if (stage == null || info == null) return;

            CharacterData data = stage.FindCharacterData(info.characterID, info.characterLevel);
            CharacterModelData modelData = stage.FindCharacterModel(info.characterModelID);

            IconCacheKey = info.characterID;

            if (data == null || modelData == null) return;

            characterInfo = info;
            battleComp.SetHp(data.maxHp);
            BattleComp.SetMaxGauge(100);

            characterData = data;
            characterModelData = modelData;
            gameObject.SetActive(true);
            if (characterUI == null) return;

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
            if (characterUI == null) return;

            characterUI.InitBattleUI(this);
            characterUI.ActiveBattleUI();
        }

        public void DestroyUI()
        {
            if (characterUI == null) return;

            Destroy(characterUI);
            characterUI = null;
        }

        public void SetBattleIcon(Sprite sprite)
        {
            BattleIcon = sprite;
        }

        public Sprite GetBattleIcon() => BattleIcon;
    }
}