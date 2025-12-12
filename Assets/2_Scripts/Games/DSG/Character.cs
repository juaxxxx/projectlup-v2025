using LUP.DSG.Utils.Enums;
using System.Buffers;
using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

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

        public float combatPower { get; private set; }
        [SerializeField]
        private GameObject characterUIPrefab;

        public EffectPool ActioneffectPool;

        private CharacterInfoUI characterInfoUI;
        private CharacterBattleUI chracterBattleUI;

        public int IconCacheKey { get; private set; }
        public EWeaponType weaponType;

        private void Awake()
        {
            StageInitializeInvoker.OnDSGStagePostInitialize += PostInitialize;
            StageInitializeInvoker.OnDSGStageInitialize += Initialize;

            statusEffectComp = GetComponent<StatusEffectComponent>();
            battleComp = GetComponent<BattleComponent>();
            scoreComp = GetComponent<ScoreComponent>();
            animationComp = GetComponent<AnimationComponent>();
            ActioneffectPool = FindAnyObjectByType<EffectPool>();
        }
        private void OnDestroy()
        {
            StageInitializeInvoker.OnDSGStagePostInitialize -= PostInitialize;
            StageInitializeInvoker.OnDSGStageInitialize -= Initialize;

            if (battleComp != null && animationComp != null)
            {
                battleComp.OnAttackStarted -= animationComp.StartAttackAnimation;
                battleComp.OnDamaged -= animationComp.PlayHittedAnimation;
                battleComp.OnDie -= animationComp.PlayDiedAnimation;
                battleComp.OnStartDash -= animationComp.StartDashAnimation;

                animationComp.OnHitAttack -= battleComp.ApplyDamageOnce;
                animationComp.OnShootRangeAttack -= battleComp.TrySpawnProjectileForRangedAttack;
                animationComp.OnAttackStart -= battleComp.AttackStart;
            }
        }

        private void Initialize(DeckStrategyStage stage)
        {
        }
        public void ManualInitializeAfterSpawn()
        {
            if (battleComp == null)
                battleComp = GetComponent<BattleComponent>();
            if (animationComp == null)
                animationComp = GetComponent<AnimationComponent>();
            if (statusEffectComp == null)
                statusEffectComp = GetComponent<StatusEffectComponent>();
            if (scoreComp == null)
                scoreComp = GetComponent<ScoreComponent>();

            battleComp.OnAttackStarted += animationComp.StartAttackAnimation;
            battleComp.OnDamaged += animationComp.PlayHittedAnimation;
            battleComp.OnDie += animationComp.PlayDiedAnimation;
            battleComp.OnStartDash += animationComp.StartDashAnimation;

            animationComp.OnHitAttack += battleComp.ApplyDamageOnce;
            animationComp.OnShootRangeAttack += battleComp.TrySpawnProjectileForRangedAttack;
            animationComp.OnAttackStart += battleComp.AttackStart;

            Canvas[] canvases = FindObjectsByType<Canvas>(FindObjectsSortMode.None);
            foreach (var canvas in canvases)
            {
                if (canvas.CompareTag("CharacterUI"))
                {
                    GameObject ui = Instantiate(characterUIPrefab, canvas.transform);
                    characterInfoUI = ui.GetComponentInChildren<CharacterInfoUI>();
                    characterInfoUI.SetTarget(transform);
                    characterInfoUI.gameObject.SetActive(false);

                    chracterBattleUI = ui.GetComponentInChildren<CharacterBattleUI>();
                    chracterBattleUI.Init(this);
                    chracterBattleUI.SetTarget(transform);
                    chracterBattleUI.gameObject.SetActive(false);

                    break;
                }
            }
        }

        private void PostInitialize(DeckStrategyStage stage)
        {

        }

        public void EndTurn()
        {
            statusEffectComp.TurnAll();
        }

        public void UpdateCombatPower()
        {
            if (characterData == null)
            {
                combatPower = 0;
            }
            else
            {
                combatPower = characterData.maxHp + characterData.attack + characterData.defense + characterData.speed;
            }
        }

        public void SetCharacterData(OwnedCharacterInfo info)
        {
            DataCenter dataCenter = FindAnyObjectByType<DataCenter>();
            if (dataCenter == null) return;

            DeckStrategyStage stage = LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
            if (stage == null) return;

            CharacterData data = stage.FindCharacterData(info.characterID, info.characterLevel);
            CharacterModelData modelData = stage.FindCharacterModel(info.characterModelID);
            //CharacterModelData modelData = dataCenter.FindCharacterModel(info.characterModelID);

            IconCacheKey = info.characterID;

            if (data == null || modelData == null) return;

            characterInfo = info;
            battleComp.SetHp(data.maxHp);
            BattleComp.SetMaxGauge(100);

            characterData = data;
            characterModelData = modelData;
            gameObject.SetActive(true);
            if (characterInfoUI == null) return;

            characterInfoUI.SetCharacterInfo(data.type, info.characterLevel);
            characterInfoUI.gameObject.SetActive(true);

            UpdateCombatPower();
        }

        public void ClearCharacterInfo()
        {
            characterData = null;
            characterModelData = null;
            gameObject.SetActive(false);
            if (characterInfoUI != null)
            {
                characterInfoUI.gameObject.SetActive(false);
            }
            if (chracterBattleUI != null)
            {
                chracterBattleUI.gameObject.SetActive(false);
            }

            UpdateCombatPower();
        }

        public void ActiveBattleUI()
        {
            if (chracterBattleUI == null) return;

            chracterBattleUI.Init(this);

            chracterBattleUI.gameObject.SetActive(true);
            characterInfoUI.gameObject.SetActive(false);
        }

        public void DestroyUI()
        {
            characterInfoUI.gameObject.SetActive(false);
            chracterBattleUI.gameObject.SetActive(false);
            Destroy(characterInfoUI);
            Destroy(chracterBattleUI);
        }
    }
}