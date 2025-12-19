using DG.Tweening;
using DG.Tweening.Core.Easing;
using LUP.Define;
using LUP.DSG.Utils.Enums;
using Mono.Cecil;
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
        private RectTransform EmptyMessage;

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

        private int currentTurnIndex = 0;
        private int currentRound = 1;
        private float currentGameSpeed = 1f;
        private bool isBattleStart = false;

        [SerializeField]
        private DataCenter dataCenter;

        public float iconSize = 1000f;

        public static BattleSystem Instance { get; private set; }
        private Dictionary<string, (Color Color, float Score)> deadScores = new();
        private List<(string Name, int CharId, Sprite Icon, float Score, GameObject Prefab)> deadCharacterData = new();

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

            targetSelector = new ChainedTargetSelector(new PickWeakTarget(this),
                new PickHighestHpTarget(this),
                new PickRandomTarget(this));
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
                        CharacterIconCache.TryGetByModelId(characterId, out sprite);
                    }

                    if (sprite != null)
                    {
                        portrait.sprite = sprite;
                        portrait.color = Color.white;
                        portrait.preserveAspect = true;
                        portrait.type = Image.Type.Simple;
                        portrait.material = null;
                        character.SetBattleIcon(sprite);
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
            if(FriendlySlotsIsEmpty() || EnemySlotsIsEmpty())
            {
                ViewWarningMessage();
                return;
            }

            StartCoroutine(BattleStart());
        }
        private IEnumerable ViewWarningMessage()
        {
            EmptyMessage.gameObject.SetActive(true);
            yield return new WaitForSeconds(2);
            EmptyMessage.gameObject.SetActive(false);
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
                StartCoroutine(FocusSkillCaster(currentChar));
            }
            else
            {
                List<LineupSlot> targetslot = targetSelector.SelectEnemyTargets(currentChar, 1);
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

            mvp.char1Score = mvp.char2Score = mvp.char3Score = mvp.char4Score = mvp.char5Score = 0f;
            mvp.char1Name = mvp.char2Name = mvp.char3Name = mvp.char4Name = mvp.char5Name = "";
            mvp.char1CharacterId = mvp.char2CharacterId = mvp.char3CharacterId = mvp.char4CharacterId = mvp.char5CharacterId = 0;
            mvp.char1Prefab = mvp.char2Prefab = mvp.char3Prefab = mvp.char4Prefab = mvp.char5Prefab = null;

            mvp.char1Icon = mvp.char2Icon = mvp.char3Icon = mvp.char4Icon = mvp.char5Icon = null;

            var friendlyChars = new List<(string Name, int CharId, Sprite Icon, float Score, GameObject Prefab)>();

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

                    int charId = ch.IconCacheKey;

                    float score = ch.ScoreComp.CalculateMVPScore();
                    GameObject prefab = (ch.characterModelData != null) ? ch.characterModelData.prefab : null;

                    CharacterIconCache.TryGetByCharacterId(charId, out Sprite icon);

                    friendlyChars.Add((name, charId, icon, score, prefab));
                }
            }

            foreach (var d in deadCharacterData)
            {
                if (!friendlyChars.Exists(x => x.CharId == d.CharId))
                    friendlyChars.Add((d.Name, d.CharId, d.Icon, d.Score, d.Prefab));
            }

            var ranked = friendlyChars.OrderByDescending(x => x.Score).ToList();
            if (ranked.Count == 0) return;

            for (int i = 0; i < Mathf.Min(5, ranked.Count); i++)
            {
                var entry = ranked[i];

                ApplyMVP(mvp, i + 1, entry.Name, entry.CharId, entry.Score, entry.Icon, entry.Prefab);
                Debug.Log($"[EndBattle-SAVE] mvpInstanceID={mvp.GetInstanceID()} result={mvp.battleResult}");
                Debug.Log($"[EndBattle-SAVE] #1 name={mvp.char1Name} score={mvp.char1Score} icon={(mvp.char1Icon ? mvp.char1Icon.name : "NULL")}");
                Debug.Log($"[EndBattle-SAVE] #2 name={mvp.char2Name} score={mvp.char2Score} icon={(mvp.char2Icon ? mvp.char2Icon.name : "NULL")}");
            }

            Time.timeScale = 1f;
        }
        public void BackupDeadCharacter(string name, int charid, float score, GameObject prefab)
        {
            if (deadCharacterData.Exists(x => x.Name == name))
                return;

            CharacterIconCache.TryGetByCharacterId(charid, out var icon);

            deadCharacterData.Add((name, charid, icon, score, prefab));
        }
        private void ApplyMVP(
                TeamMVPData data,
                int index,
                string name,
                int charId,
                float score,
                Sprite icon,
                GameObject prefab = null)
        {
            switch (index)
            {
                case 1:
                    data.char1Name = name;
                    data.char1CharacterId = charId;
                    data.char1Score = score;
                    data.char1Prefab = prefab;
                    data.char1Icon = icon;
                    break;

                case 2:
                    data.char2Name = name;
                    data.char2CharacterId = charId;
                    data.char2Score = score;
                    data.char2Prefab = prefab;
                    data.char2Icon = icon;
                    break;

                case 3:
                    data.char3Name = name;
                    data.char3CharacterId = charId;
                    data.char3Score = score;
                    data.char3Prefab = prefab;
                    data.char3Icon = icon;
                    break;

                case 4:
                    data.char4Name = name;
                    data.char4CharacterId = charId;
                    data.char4Score = score;
                    data.char4Prefab = prefab;
                    data.char4Icon = icon;
                    break;

                case 5:
                    data.char5Name = name;
                    data.char5CharacterId = charId;
                    data.char5Score = score;
                    data.char5Prefab = prefab;
                    data.char5Icon = icon;
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
                Time.timeScale = 1f;
            }
            else if (allFriendDead)
            {
                EndBattle("Defeat");
                DeckStrategyStage stage = GetComponent<DeckStrategyStage>();
                stage.BattleEnd();
                Time.timeScale = 1f;
            }
        }
        private void OnDieIndexCharacter(int index)
        {
            sequenceImage[index].gameObject.SetActive(false);
        }

        private IEnumerator FocusSkillCaster(Character currentChar)
        {
            currentChar.BattleComp.isAttacking = true;
            onStartSkill?.Invoke(currentChar);
            Camera camera = Camera.main;
            Transform cameraOrigin = camera.transform;
            BattleCameraDirector Director = camera.GetComponent<BattleCameraDirector>();
            LineupSlot currentSlot = currentChar.GetComponentInParent<LineupSlot>();
            Transform focusTransform = currentSlot.FocusedPosition;
            yield return Director.FocusOnSkillCaster(focusTransform, cameraOrigin).WaitForCompletion();

            Director.FocusOnTarget(cameraOrigin.position);

            List<LineupSlot> targetList = targetSelector.SelectEnemyTargets(currentChar, currentChar.BattleComp.skillInfo.targetCount);
            currentChar.BattleComp.Skill(targetList);
            StartCoroutine(WaitForAttackEnd(currentChar));
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

        public bool FriendlySlotsIsEmpty()
        {
            for(int i = 0; i < friendlySlots.Length; i++)
            {
                LineupSlot slot = friendlySlots[i].GetComponent<LineupSlot>();
                if(slot.character == null)
                {
                    continue;
                }
                return false;
            }
            return true;   
        }

        public bool EnemySlotsIsEmpty()
        {
            for (int i = 0; i < enemySlots.Length; i++)
            {
                LineupSlot slot = enemySlots[i].GetComponent<LineupSlot>();
                if (slot.character == null)
                {
                    continue;
                }
                return false;
            }
            return true;
        }
    }
}
