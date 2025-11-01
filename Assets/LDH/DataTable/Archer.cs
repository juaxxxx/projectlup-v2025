using Roguelike.Define;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Archer : MonoBehaviour
{
    [Header("캐릭터 데이터")]
    [SerializeField] public RLCharacterData CData;
    [SerializeField] public ArcherData Adata;
    [SerializeField] public LevelDataTable levelTable;
    //public BaseStats stats;

    [Header("버프 관련")]
    public List<BuffData> allBuffs;          // 인스펙터에서 등록
    public GameObject buffSelectionUI;       // 전체 패널 (Canvas 안)
    public Transform optionParent;           // 버튼들을 넣을 부모 (예: Horizontal Layout)
    public GameObject optionButtonPrefab;    // 버튼 프리팹 (Text만 있어도 OK)

    public event System.Action OnExpChanged;
    public event System.Action OnArcherDataReady;
    List<BuffData> randomBuffs = new List<BuffData>();
    List<BuffData> GetBuffList = new List<BuffData>();
    private bool isSelecting = false;
    void Awake()
    {
        if (CData == null)
        {
            return;
        }
        Adata = new ArcherData(CData, 100, 10);
        OnArcherDataReady?.Invoke();
        Debug.Log($"현재 체력 :  {Adata.currentData.MaxHp}");

    }
    //구독 

    //버프 뽑기
    void ShowBuffSelection()
    {
        buffSelectionUI.SetActive(true);

        //기존 버튼 제거
        foreach (Transform child in optionParent)
            Destroy(child.gameObject);


        randomBuffs.Clear(); // 리스트 비우기
        //버프3개뽑기
        while (randomBuffs.Count < 3)
        {
            BuffData candidate = allBuffs[UnityEngine.Random.Range(0, allBuffs.Count)];
            if (!randomBuffs.Contains(candidate))
            {
                randomBuffs.Add(candidate);
            }
        }

        foreach (BuffData buff in randomBuffs)
        {
            //프리팹 복제
            GameObject btnObj = Instantiate(optionButtonPrefab, optionParent);
            //인덱스 번호 매기기

            //  복사본으로  생성
            BuffData copy = ScriptableObject.CreateInstance<BuffData>();
            copy.buffName = buff.buffName;
            copy.SetDisplayableImage(buff.GetDisplayableImage());

            OptionButtonUI btnUI = btnObj.GetComponent<OptionButtonUI>();
            btnUI.SetData(copy, this);


        }
        Time.timeScale = 0;
    }
    public void ApplyBuff(BuffData buff)
    {
        if (buff == null) Debug.Log("null");
        switch (buff.type)
        {
            case BuffType.AddMaxHp:
                Adata.currentData.Attack += 5;
                break;

            case BuffType.AddAtkHigh:
                Adata.currentData.Hp += 30;
                break;

            case BuffType.AddSpeed:
                Adata.currentData.speed += 1;
                break;
        }
        Debug.Log($"버프 적용됨: {buff.name} | HP: {Adata.currentData.Hp}, 공격력: {Adata.currentData.Attack}, 속도: {Adata.currentData.speed}");
        GetBuffList.Add(buff);

        buffSelectionUI.SetActive(false);
        Time.timeScale = 1f;
        isSelecting = false;
    }
    public List<BuffData> GetActiveBufflist()
    {
        return GetBuffList;
    }
    private void OnEnable()
    {
        Enemy.OnEnemyDied += GainExp;

    }
    private void GainExp(int exp)
    {
        var data = levelTable.GetLevelData(Adata.level);
        if (data == null) Debug.Log("데이터테이블없음");
        Adata.xp += exp;
        if (Adata.xp >= data.RequiredExp)
            LevelUp();
        Debug.Log($"플레이어가 {exp} 경험치 획득! 현재 총 {Adata.xp}");
        OnExpChanged?.Invoke();
    }
    private void LevelUp()
    {
        Adata.level++;
        Adata.xp = 0;
        var levelData = levelTable.GetLevelData(Adata.level);
        if (levelData != null)
        {
            Adata.currentData.Attack = levelData.AttackBouns;
            Adata.currentData.Hp = levelData.HpBouns;
            Adata.currentData.MaxHp = levelData.HpBouns;
        }
        ShowBuffSelection();
        Debug.Log($"레벨{Adata.level}, 체력 :  {Adata.currentData.Hp} ,  공격력  {Adata.currentData.Attack}");
    }
}
