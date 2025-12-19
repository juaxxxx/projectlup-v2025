using DG.Tweening;
using LUP.Define;
using LUP.DSG.Utils.Enums;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

namespace LUP.DSG
{
    public class BattleSystem : MonoBehaviour
    {
        //private UserData.Team friendlyTeam = new UserData.Team();

        [SerializeField]
        private Team enemyTeam;

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
        private GameObject characterUICanvas;

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

        private ChainedTargetSelector targetSelector;
        private PickRandomTarget randomTargetSelector;

        private int currentTurnIndex = 0;
        private int currentRound = 1;
        private float currentGameSpeed = 1f;
        private bool isBattleStart = false;

        [SerializeField]
        private DataCenter dataCenter;

        public float iconSize = 1000f;

        public static BattleSystem Instance { get; private set; }
        private Dictionary<string, (Color Color, float Score)> deadScores = new();
        private List<(string Name, Color Color, float Score, GameObject Prefab)> deadCharacterData = new();

        public event Action<Character> onStartAttack;
        public event Action<Character> onStartSkill;

        void Awake()
        {
            StageInitializeInvoker.OnDSGStagePostInitialize += Initialize;
            StageInitializeInvoker.OnDSGStagePostInitialize += PostInitialize;

            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            StageInitializeInvoker.OnDSGStagePostInitialize -= Initialize;
            StageInitializeInvoker.OnDSGStagePostInitialize -= PostInitialize;
        }

        private void PostInitialize(DeckStrategyStage stage)
        {

        }
        private void Initialize(DeckStrategyStage stage)
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

            randomTargetSelector = new PickRandomTarget(this);

            targetSelector = new ChainedTargetSelector(new PickWeakTarget(this),
                new PickHighestHpTarget(this),
                randomTargetSelector);
        }

        private void Update()
        {
            if (!isBattleStart || battleSequence.Count == 0)
                return;

            if (currentTurnIndex < battleSequence.Count)
            {
                var currentChar = battleSequence[currentTurnIndex];
                if (currentChar != null && currentChar.BattleComp.isAttacking)
                    return;
            }
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

            Resort();
        }

        public void Resort()
        {
            foreach (var icon in sequenceImage)
                Destroy(icon);
            sequenceImage.Clear();

            battleSequence.Sort((x, y) => y.characterData.speed.CompareTo(x.characterData.speed));

            for (int i = 0; i < battleSequence.Count; i++)
            {
                Character character = battleSequence[i];
                character.battleIndex = i;

                var icon = Instantiate(iconPrefab, characterSequenceList);
                var bg = icon.transform.Find("Background")?.GetComponent<Image>();
                if (bg != null)
                {
                    var c = character.isEnemy ? Color.red : Color.blue;
                    c.a = 0.6f;
                    bg.color = c;
                }
                var portrait = icon.transform.Find("Portrait")?.GetComponent<Image>();
                if (portrait != null)
                {
                    int characterId = character.IconCacheKey;
                    int modelId = character.characterModelData.ID;

                    var prtRt = portrait.rectTransform;
                    prtRt.localScale = Vector3.one * 5.0f;

                    Sprite sprite = null;

                    if (!CharacterIconCache.TryGetByCharacterId(characterId, out sprite))
                    {
                        CharacterIconCache.TryGetByModelId(modelId, out sprite);
                    }

                    if (sprite != null)
                    {
                        portrait.sprite = sprite;
                        portrait.color = Color.white;
                        portrait.preserveAspect = true;
                        portrait.type = Image.Type.Simple;
                        portrait.material = null;
                    }
                    else
                    {
                        portrait.sprite = null;
                        portrait.color = Color.gray;
                    }
                }

                character.BattleComp.OnDie += OnDieIndexCharacter;
                sequenceImage.Add(icon);
            }
        }

        public void InitSequence()
        {
            currentRound++;

            for (int i = 0; i < battleSequence.Count;)
            {
                if (battleSequence[i] == null)
                {
                    battleSequence.RemoveAt(i);
                    continue;
                }

                Character character = battleSequence[i];
                character.StatusEffectComp.TurnAll();
                character.StatusEffectComp.ClearRemoveList();

                if (!character.BattleComp.isAlive)
                {
                    character.BattleComp.OnDie -= OnDieIndexCharacter;
                    battleSequence.Remove(character);
                    Destroy(character.gameObject);
                    continue;
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

        public void OnClickBattleStartButton()
        {
            if (friendlySlots.Length <= 0)
            {
                return; //@TODO friendlyslot 비어있음 이미지 띄우기
            }

            StartCoroutine(BattleStart());
        }

        public IEnumerator BattleStart()
        {
            isBattleStart = true;
            formationCanvas.SetActive(false);
            characterUICanvas.SetActive(false);

            //카메라 배틀 인트로
            Camera camera = Camera.main;
            BattleCameraDirector Director = camera.GetComponent<BattleCameraDirector>();
            yield return Director.PlayBattleIntroSequence().WaitForCompletion();
            yield return null;

            for (int i = 0; i < friendlySlots.Length; i++)
            {
                var slot = friendlySlots[i].GetComponent<LineupSlot>();
                if (slot.character != null && slot.isPlaced)
                {
                    slot.ActivateBattleUI();
                    slot.character.BattleComp.PlusGuage(50);
                }
            }

            for (int i = 0; i < enemySlots.Length; i++)
            {
                var slot = enemySlots[i].GetComponent<LineupSlot>(); //andi
                if (slot.character != null)
                {
                    slot.ActivateBattleUI();
                    slot.character.BattleComp.PlusGuage(50);
                }
            }

            battleCanvas.SetActive(true);
            characterUICanvas.SetActive(true);

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
                currentTurnIndex++;
                return;
            }
            if (turnText != null)
                turnText.text = $"{currentChar.characterData.characterName} Turn";

            if (currentChar.BattleComp.isSkillOn)
            {
                List<LineupSlot> targetList = randomTargetSelector.SelectEnemyTargets(currentChar, currentChar.BattleComp.skillInfo.targetCount);
                currentChar.BattleComp.Skill(targetList);
                StartCoroutine(WaitForAttackEnd(currentChar));
                onStartSkill?.Invoke(currentChar);
            }
            else
            {
                List<LineupSlot> targetslot = targetSelector.SelectEnemyTargets(currentChar, 2);
                currentChar.BattleComp.Attack(targetslot);
                StartCoroutine(WaitForAttackEnd(currentChar));
                onStartAttack?.Invoke(currentChar);
            }

            return;
        }
        public void BackupScore(Character c)
        {
            if (c == null || c.characterData == null || c.ScoreComp == null)
                return;

            string name = c.characterData.characterName;
            Color color = c.characterModelData != null
                ? c.characterModelData.material.GetColor("_BaseColor")
                : Color.white;
            float score = c.ScoreComp.CalculateMVPScore();

            if (!deadScores.ContainsKey(name))
            {
                deadScores[name] = (color, score);
            }
        }

        public void EndBattle(string resultText)
        {
            var dataCenter = FindFirstObjectByType<DataCenter>();
            if (dataCenter == null)
            {
                Debug.LogError("[EndBattle] DataCenter를 찾을 수 없습니다!");
                return;
            }
            if (dataCenter.mvpData == null)
            {
                Debug.LogError("[EndBattle] DataCenter.mvpData가 비어 있습니다! (TeamMVPData ScriptableObject 연결 필요)");
                return;
            }

            var mvp = dataCenter.mvpData;
            mvp.battleResult = resultText;

            // 2) 초기화
            mvp.char1Score = mvp.char2Score = mvp.char3Score = mvp.char4Score = mvp.char5Score = 0f;
            mvp.char1Name = mvp.char2Name = mvp.char3Name = mvp.char4Name = mvp.char5Name = "";
            mvp.char1Color = mvp.char2Color = mvp.char3Color = mvp.char4Color = mvp.char5Color = Color.white;

            mvp.char1Prefab = mvp.char2Prefab = mvp.char3Prefab = mvp.char4Prefab = mvp.char5Prefab = null;


            var friendlyChars = new List<(string Name, Color Color, float Score, GameObject Prefab)>();

            if (friendlySlots != null)
            {
                foreach (var slotObj in friendlySlots)
                {
                    if (slotObj == null) continue;
                    var slot = slotObj.GetComponent<LineupSlot>();
                    if (slot == null || slot.character == null) continue;

                    var ch = slot.character;
                    if (ch.characterData == null || ch.ScoreComp == null) continue;

                    string name = ch.characterData.characterName;

                    Color color = Color.white;
                    if (ch.characterModelData != null && ch.characterModelData.material != null)
                    {
                        try { color = ch.characterModelData.material.GetColor("_BaseColor"); }
                        catch { color = ch.characterModelData.material.color; }
                    }

                    float score = ch.ScoreComp.CalculateMVPScore();
                    GameObject prefab = (ch.characterModelData != null) ? ch.characterModelData.prefab : null;

                    friendlyChars.Add((name, color, score, prefab));
                }
            }


            foreach (var d in deadCharacterData)
            {
                if (!friendlyChars.Exists(x => x.Name == d.Name))
                    friendlyChars.Add((d.Name, d.Color, d.Score, d.Prefab));
            }

            var ranked = friendlyChars.OrderByDescending(x => x.Score).ToList();

            if (ranked.Count == 0)
            {
                return;
            }

            for (int i = 0; i < Mathf.Min(5, ranked.Count); i++)
            {
                var character = ranked[i];

                ApplyMVP(mvp, i + 1, character.Name, character.Color, character.Score, character.Prefab);
            }
        }
        public void BackupDeadCharacter(string name, Color color, float score, GameObject prefab)
        {
            if (deadCharacterData.Exists(x => x.Name == name))
                return;

            deadCharacterData.Add((name, color, score, prefab));
        }
        private void ApplyMVP(TeamMVPData data, int index, string name, Color color, float score, GameObject prefab = null)
        {
            switch (index)
            {
                case 1:
                    data.char1Name = name;
                    data.char1Color = color;
                    data.char1Score = score;
                    data.char1Prefab = prefab;
                    break;
                case 2:
                    data.char2Name = name;
                    data.char2Color = color;
                    data.char2Score = score;
                    data.char2Prefab = prefab;
                    break;
                case 3:
                    data.char3Name = name;
                    data.char3Color = color;
                    data.char3Score = score;
                    data.char3Prefab = prefab;
                    break;
                case 4:
                    data.char4Name = name;
                    data.char4Color = color;
                    data.char4Score = score;
                    data.char4Prefab = prefab;
                    break;
                case 5:
                    data.char5Name = name;
                    data.char5Color = color;
                    data.char5Score = score;
                    data.char5Prefab = prefab;
                    break;
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

            currentTurnIndex++;
            UpdateUI();

            CheckBattleEnd();
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

            currentTurnIndex = 0;

            while (currentTurnIndex < battleSequence.Count)
            {
                int num = currentTurnIndex;
                NextTurn();

                if (num != currentTurnIndex) continue;

                var currentChar = battleSequence[currentTurnIndex];
                if (currentChar == null || !currentChar.BattleComp.isAlive) continue;

                yield return new WaitWhile(() => currentChar.BattleComp.isAttacking);
                yield return new WaitForSeconds(1);
            }

            InitSequence();
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

        private void CheckBattleEnd()
        {
            bool allFriendDead = true;
            bool allEnemyDead = true;

            foreach (var slotObj in friendlySlots)
            {
                var slot = slotObj.GetComponent<LineupSlot>();
                if (slot.character != null && slot.character.BattleComp != null && slot.character.BattleComp.isAlive)
                {
                    allFriendDead = false;
                    break;
                }
            }

            foreach (var slotObj in enemySlots)
            {
                var slot = slotObj.GetComponent<LineupSlot>();
                if (slot.character != null && slot.character.BattleComp != null && slot.character.BattleComp.isAlive)
                {
                    allEnemyDead = false;
                    break;
                }
            }

            if (allEnemyDead)
            {
                EndBattle("Victory");
                DeckStrategyStage stage = GetComponent<DeckStrategyStage>();
                stage.BattleEnd();
            }
            else if (allFriendDead)
            {
                EndBattle("Defeat");
                DeckStrategyStage stage = GetComponent<DeckStrategyStage>();
                stage.BattleEnd();
            }
        }
        private void OnDieIndexCharacter(int index)
        {
            sequenceImage[index].gameObject.SetActive(false);
        }
        public void OnClickPauseButton()
        {
            float Curr = Time.timeScale;
            TextMeshProUGUI pauseText = battleCanvas.transform.Find("RightTop/PauseButton/PauseText").GetComponent<TextMeshProUGUI>();

            if (Curr == 0f)
            {
                pauseText.SetText("Pause");
                Time.timeScale = currentGameSpeed;
            }
            else
            {
                pauseText.SetText("Resume");
                Time.timeScale = 0f;
            }
        }
        public void OnClickSpeedButton()
        {
            float Curr = Time.timeScale; //@TODO 구조개선 timescale XX
            if (Curr == 0f)
            {
                return;
            }

            TextMeshProUGUI speedText = battleCanvas.transform.Find("RightTop/SpeedButton/SpeedText").GetComponent<TextMeshProUGUI>();

            if (currentGameSpeed == 1f)
            {
                speedText.SetText("2X");
                Time.timeScale = 2f;
                currentGameSpeed = Time.timeScale;
            }
            else if (currentGameSpeed == 2f)
            {
                speedText.SetText("4X");
                Time.timeScale = 4f;
                currentGameSpeed = Time.timeScale;
            }
            else
            {
                speedText.SetText("1X");
                Time.timeScale = 1f;
                currentGameSpeed = Time.timeScale;
            }
        }
    }
}
