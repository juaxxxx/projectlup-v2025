using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Character : MonoBehaviour
{
    public struct StatusEffect
    {
        public IStatusEffect statusEffect;
        public string Name; //이름 같으면
        public int Stack; // 스택 수 +
        public int RemainsTurn; // 이전부여된 라운드 수 < 이번에부여된 라운드 수 일 경우 이번에부여된 라운드 수로 덮어씌운다.
    }

    private StatusEffectComponent statusEffectComp;
    private BattleComponent battleComp;
    private ScoreComponent scoreComp;

    public StatusEffectComponent StatusEffectComp => statusEffectComp;
    public BattleComponent BattleComp => battleComp;
    public ScoreComponent ScoreComp => scoreComp;

    //public OwnedCharacterInfo characterInfo;

    public CharacterData characterData { get; private set; }
    public CharacterModelData characterModelData { get; private set; }

    public float maxSkillGauge { get; private set; }

    public bool isEnemy = false;
    public float combatPower { get; private set; }

    [SerializeField]
    private GameObject characterUIPrefab;

    private CharacterInfoUI characterInfoUI;
    private CharacterBattleUI chracterBattleUI;

    private void Awake()
    {
        //healthComp = GetComponent<HealthComponent>();
        statusEffectComp = GetComponent<StatusEffectComponent>();
        battleComp = GetComponent<BattleComponent>();
        scoreComp = GetComponent<ScoreComponent>();

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

    public void EndTurn()
    {
        statusEffectComp.TurnAll();
    }

    public void UpdateCombatPower()
    {
        if(characterData == null)
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
        battleComp.SetHp(data.maxHp);

        characterData = data;
        characterModelData = modelData;
        GetComponent<MeshRenderer>().material.color = modelData.material.color;
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
