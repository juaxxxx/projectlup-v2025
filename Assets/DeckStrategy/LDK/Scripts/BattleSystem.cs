using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils.Enums;


public class BattleSystem : MonoBehaviour
{
    //private UserData.Team friendlyTeam = new UserData.Team();

    [SerializeField]
    private UserData.Team enemyTeam;

    public GameObject[] friendlySlots = new GameObject[5];
    public GameObject[] enemySlots = new GameObject[5];

    [SerializeField]
    private RectTransform characterSequenceList;
    [SerializeField]
    private GameObject iconPrefab;
    [SerializeField]
    private GameObject battleCanvas;
    [SerializeField]
    private GameObject formationCanvas;

    [SerializeField]
    private TextMeshProUGUI roundText;
    [SerializeField]
    private TextMeshProUGUI turnText;

    [SerializeField]
    private TextMeshProUGUI playerCP;
    [SerializeField]
    private TextMeshProUGUI enemyCP;

    private List<Character> battleSequence = new List<Character>();
    private List<GameObject> sequenceImage = new List<GameObject>();

    private IAttackTargetSelector targetSelector;

    private int currentTurnIndex = 0;
    private int currentRound = 1;

    private bool isBattleStart = false;

    private void Start()
    {
        for (int i = 0; i < enemySlots.Length; i++)
        {
            LineupSlot enemySlot = enemySlots[i].GetComponent<LineupSlot>();
            enemySlot.OnCPUpdated += UpdateEnemyCP;
            if (enemyTeam.characters[i] == null) continue;

            enemySlot.SetSelectedCharacter(enemyTeam.characters[i], true);
        }

        for (int i = 0; i < friendlySlots.Length; i++)
        {
            LineupSlot friendlySlot = friendlySlots[i].GetComponent<LineupSlot>();
            friendlySlot.OnCPUpdated += UpdatePlayerCP;
        }

        targetSelector = new ChainedTargetSelector(new PickWeakTarget(this),
            new PickHighestHpTarget(this),
            new PickRandomTarget(this));
    }

    private void SortBattleSequence()
    {
        battleSequence.Clear();
        sequenceImage.ForEach(Destroy);
        sequenceImage.Clear();

        for (int i = 0; i < friendlySlots.Length; i++)
        {
            LineupSlot friendlySlot = friendlySlots[i].GetComponent<LineupSlot>();
            if (friendlySlot.character != null && friendlySlot.isPlaced)
                battleSequence.Add(friendlySlot.character);
        }

        for (int i = 0; i < enemySlots.Length; i++)
        {
            LineupSlot enemySlot = enemySlots[i].GetComponent<LineupSlot>();
            if (enemyTeam.characters[i] == null) continue;
            battleSequence.Add(enemySlot.character);
        }

        battleSequence.Sort((a, b) => b.characterData.speed.CompareTo(a.characterData.speed));

        foreach (var character in battleSequence)
        {
            var icon = Instantiate(iconPrefab, characterSequenceList);
            var portrait = icon.transform.Find("Portrait")?.GetComponent<Image>();
            if (portrait != null)
            {
                Color color = character.characterModelData.material.GetColor("_BaseColor");
                portrait.color = color;
            }
            sequenceImage.Add(icon);
        }
    }

    public void Resort()
    {
        foreach (var icon in sequenceImage)
            Destroy(icon);
        sequenceImage.Clear();

        battleSequence.Sort((x, y) => y.characterData.speed.CompareTo(x.characterData.speed));

        foreach (var character in battleSequence)
        {
            var icon = Instantiate(iconPrefab, characterSequenceList);
            var portrait = icon.transform.Find("Portrait")?.GetComponent<Image>();
            if (portrait != null)
            {
                var color = character.characterModelData.material.GetColor("_BaseColor");
                portrait.color = color;
            }
            sequenceImage.Add(icon);
        }
    }

    public void InitSequence()
    {
        currentRound++;

        for (int i = 0; i < battleSequence.Count;)
        {
            if (!battleSequence[i].BattleComp.isAlive)
            {
                Character character = battleSequence[i];
                battleSequence.Remove(character);
                Destroy(character.gameObject);
            }
            else
            {
                i++;
            }
        }

        Resort();

        currentTurnIndex = 0;

        UpdateUI();
    }

    public void BattleStart()
    {
        isBattleStart = true;

        for (int i = 0; i < friendlySlots.Length; i++)
        {
            var slot = friendlySlots[i].GetComponent<LineupSlot>();
            if (slot.character != null && slot.isPlaced)
            {
                slot.ActivateBattleUI();
            }
        }

        for (int i = 0; i < enemySlots.Length; i++)
        {
            var slot = enemySlots[i].GetComponent<LineupSlot>();
            if (slot.character != null)
            {
                slot.ActivateBattleUI();
            }
        }

        formationCanvas.SetActive(false);
        battleCanvas.SetActive(true);

        SortBattleSequence();

        currentTurnIndex = 0;
        currentRound = 1;
        UpdateUI();
    }

    public void NextTurn()
    {
        if (!isBattleStart || battleSequence.Count == 0)
            return;
        
        if (currentTurnIndex >= battleSequence.Count)
        {
            InitSequence();
            return;
        }

        Character currentChar = battleSequence[currentTurnIndex];
        if (currentChar == null || currentChar.BattleComp == null || !currentChar.BattleComp.isAlive)
        {
            if (currentTurnIndex < sequenceImage.Count && sequenceImage[currentTurnIndex] != null)
            {
                sequenceImage[currentTurnIndex].SetActive(false);
            }
            currentTurnIndex++;
            return;
        }
        if (turnText != null)
            turnText.text = $"{currentChar.characterData.characterName} Turn";

        LineupSlot targetslot = targetSelector.SelectTarget(currentChar);
        currentChar.BattleComp.Attack(targetslot);

        StartCoroutine(WaitForAttackEnd(currentChar));
        return;
    }

    public void EndBattle()
    {
        DataCenter dataCenter = FindFirstObjectByType<DataCenter>();

        if (dataCenter == null || dataCenter.mvpData == null)
        {
            return;
        }

        var mvp = dataCenter.mvpData;

        mvp.char1Score = mvp.char2Score = mvp.char3Score = mvp.char4Score = mvp.char5Score = 0;

        var friendlyChars = new List<Character>();
        foreach (var slotObj in friendlySlots)
        {
            var slot = slotObj.GetComponent<LineupSlot>();
            if (slot != null && slot.character != null)
                friendlyChars.Add(slot.character);
        }

        var ranked = friendlyChars
            .Where(fchar => fchar != null && fchar.ScoreComp != null)
            .Select(fchar => new
            {
                Char = fchar,
                Score = fchar.ScoreComp.CalculateMVPScore()
            })
            .OrderByDescending(x => x.Score)
            .ToList();

        if (ranked.Count == 0)
        {
            return;
        }

        if (ranked.Count > 0) ApplyMVP(mvp, 1, ranked[0].Char, ranked[0].Score);
        if (ranked.Count > 1) ApplyMVP(mvp, 2, ranked[1].Char, ranked[1].Score);
        if (ranked.Count > 2) ApplyMVP(mvp, 3, ranked[2].Char, ranked[2].Score);
        if (ranked.Count > 3) ApplyMVP(mvp, 4, ranked[3].Char, ranked[3].Score);
        if (ranked.Count > 4) ApplyMVP(mvp, 5, ranked[4].Char, ranked[4].Score);

        DontDestroyOnLoad(dataCenter.gameObject);

        SceneManager.LoadScene("DeckBattleResultScene");
    }

    private void ApplyMVP(TeamMVPData data, int index, Character character, float score)
    {
        if (character == null) return;

        var color = character.characterModelData.material.GetColor("_BaseColor");
        var name = character.characterData.characterName;

        switch (index)
        {
            case 1: data.char1Name = name; data.char1Color = color; data.char1Score = score; break;
            case 2: data.char2Name = name; data.char2Color = color; data.char2Score = score; break;
            case 3: data.char3Name = name; data.char3Color = color; data.char3Score = score; break;
            case 4: data.char4Name = name; data.char4Color = color; data.char4Score = score; break;
            case 5: data.char5Name = name; data.char5Color = color; data.char5Score = score; break;
        }
    }


    private void UpdateUI()
    {
        if (roundText != null)
            roundText.text = $"Round {currentRound}";

        if (turnText != null)
        {
            if (currentTurnIndex < battleSequence.Count)
            {
                var currentChar = battleSequence[currentTurnIndex];
                if (currentChar != null && currentChar.characterData != null)
                    turnText.text = $"{currentChar.characterData.characterName} Turn";
                else
                    turnText.text = $"---";
            }
            else
            {
                turnText.text = $"";
            }
        }
    }

    private IEnumerator WaitForAttackEnd(Character currentChar)
    {
        yield return new WaitWhile(() => currentChar.BattleComp.isAttacking);

        if (currentTurnIndex < sequenceImage.Count && sequenceImage[currentTurnIndex] != null)
        {
            sequenceImage[currentTurnIndex].SetActive(false);
        }

        Debug.Log("WaitForAttackEnd");
        currentTurnIndex++;
        UpdateUI();

    }
    public void NextRound()
    {
        if (!isBattleStart || battleSequence.Count == 0)
            return;

        StartCoroutine(CoNextRound());
    }

    private IEnumerator CoNextRound()
    {
        Debug.Log($" Round {currentRound} !");

        Resort();
        currentTurnIndex = 0;

        while (currentTurnIndex < battleSequence.Count)
        {
            NextTurn();
            var currentChar = battleSequence[currentTurnIndex];

            if (currentChar == null || !currentChar.BattleComp.isAlive) continue;

            if (currentChar.characterData.rangeType == ERangeType.Melee)
            {
                yield return new WaitForSeconds(1.5f);
            }
            else
            {
                yield return new WaitForSeconds(1.0f);
            }
        }

        InitSequence();
        Debug.Log($" Round {currentRound - 1} ����");
    }

    void UpdatePlayerCP()
    {
        float cp = 0;
        for (int i = 0; i < friendlySlots.Length; i++)
        {
            LineupSlot slot = friendlySlots[i].GetComponent<LineupSlot>();
            if (slot.character == null || slot.character.characterData == null) continue;
            cp += slot.character.characterData.maxHp +
                slot.character.characterData.attack +
                slot.character.characterData.defense +
                slot.character.characterData.speed;
        }

        playerCP.text = cp.ToString();
    }

    void UpdateEnemyCP()
    {
        float cp = 0;
        for (int i = 0; i < enemySlots.Length; i++)
        {
            LineupSlot slot = enemySlots[i].GetComponent<LineupSlot>();
            if (slot.character == null || slot.character.characterData == null) continue;
            cp += slot.character.characterData.maxHp +
                slot.character.characterData.attack +
                slot.character.characterData.defense +
                slot.character.characterData.speed;
        }

        enemyCP.text = cp.ToString();
    }
}