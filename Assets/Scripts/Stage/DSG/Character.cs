using System.Runtime.CompilerServices;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

namespace DSG
{
    public class Character : MonoBehaviour
    {
        private StatusEffectComponent statusEffectComp;
        private BattleComponent battleComp;
        private ScoreComponent scoreComp;
        private AnimationComponent animationComp;

        [SerializeField]
        private SkinnedMeshRenderer bodySkin;
        [SerializeField]
        private MeshRenderer headSkin;

        public StatusEffectComponent StatusEffectComp => statusEffectComp;
        public BattleComponent BattleComp => battleComp;
        public ScoreComponent ScoreComp => scoreComp;

        public AnimationComponent AnimationComp => animationComp;

        //public OwnedCharacterInfo characterInfo;

        public CharacterData characterData { get; private set; }
        public CharacterModelData characterModelData { get; private set; }

        public bool isEnemy = false;
        public int battleIndex = -1;

        public float combatPower { get; private set; }
        [SerializeField]
        private GameObject characterUIPrefab;

        private CharacterInfoUI characterInfoUI;
        private CharacterBattleUI chracterBattleUI;

        private void Awake()
        {
            statusEffectComp = GetComponent<StatusEffectComponent>();
            battleComp = GetComponent<BattleComponent>();
            scoreComp = GetComponent<ScoreComponent>();
            animationComp = GetComponent<AnimationComponent>();

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

        private void Start()
        {
            battleComp.OnAttackStarted += animationComp.StartAttackAnimation;
            battleComp.OnReachedTargetPos += animationComp.EndDashLoop;
            battleComp.OnDamaged += animationComp.PlayHittedAnimation;
            battleComp.OnDie += animationComp.PlayDiedAnimation;
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

            CharacterData data = dataCenter.FindCharacterData(info.characterID);
            CharacterModelData modelData = dataCenter.FindCharacterModel(info.characterModelID);

            if (data == null || modelData == null) return;

            //characterInfo = info;
            battleComp.SetStatus(data);

            characterData = data;
            characterModelData = modelData;
            bodySkin.material.color = modelData.material.color;
            headSkin.material.color = modelData.material.color;
            gameObject.SetActive(true);
            if (characterInfoUI != null)
            {
                characterInfoUI.SetCharacterInfo(data.type, info.characterLevel);
                characterInfoUI.gameObject.SetActive(true);
            }

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
            chracterBattleUI.Init(this);

            chracterBattleUI.gameObject.SetActive(true);
            characterInfoUI.gameObject.SetActive(false);
        }
    }
}