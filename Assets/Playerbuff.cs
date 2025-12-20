using LUP.RL;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
public class PlayerBuff : MonoBehaviour
{
    [Header("버프 관련")]
    public List<BuffData> allBuffs;          // 인스펙터에서 등록
    private GameObject buffSelectionUI;       // 전체 패널 (Canvas 안)
    private Transform optionParent;           // 버튼들을 넣을 부모 (예: Horizontal Layout)
    private GameObject optionButtonPrefab;    // 버튼 프리팹 (Text만 있어도 OK)
    List<BuffData> randomBuffs = new List<BuffData>();
    List<BuffData> GetBuffList = new List<BuffData>();

    public event System.Action<PlayerBuff> OnRequestBuffUI;

    private List<BuffData> activeBuffs = new();
    private List<BuffData> randomOptions = new();


    public Archer archer;
    public void init(Archer target)
    {
        archer = target;
    }

    public void ShowBuffSelection()
    {
        OnRequestBuffUI?.Invoke(this);

    }
    public IReadOnlyList<BuffData> GetRandomBuffOptions(int count = 3)
    {
        randomOptions.Clear();

        while (randomOptions.Count < count)
        {
            var candidate = allBuffs[UnityEngine.Random.Range(0, allBuffs.Count)];
            if (!randomOptions.Contains(candidate))
                randomOptions.Add(candidate);
        }

        return randomOptions;
    }

    //    //기존 버튼 제거
    //    foreach (Transform child in optionParent)
    //    {
    //        Destroy(child.gameObject);
    //    }

    //    if (randomBuffs == null)
    //    {
    //        Debug.Log("버프 데이터 비어있음");
    //    }
    //    randomBuffs.Clear(); // 리스트 비우기
    //    //버프3개뽑기
    //    while (randomBuffs.Count < 3)
    //    {
    //        BuffData candidate = allBuffs[UnityEngine.Random.Range(0, allBuffs.Count)];
    //        if (!randomBuffs.Contains(candidate))
    //        {
    //            randomBuffs.Add(candidate);
    //        }
    //    }
    //    foreach (BuffData buff in randomBuffs)
    //    {
    //        GameObject btnObj = Instantiate(optionButtonPrefab, optionParent);
    //        OptionButtonUI btnUI = btnObj.GetComponent<OptionButtonUI>();
    //        btnUI.SetData(buff, this);
    //    }

    //    Time.timeScale = 0;
    //}

    public void ApplyBuff(BuffData buff)
    {
        activeBuffs.Add(buff);
        //if (buff == null) return;
        //archer.ApplyBuff(buff);
        //GetBuffList.Add(buff);

        //buffSelectionUI.SetActive(false);
        //Time.timeScale = 1f;
    }
    public List<BuffData> GetActiveBufflist()
    {
        return activeBuffs;
    }
}
