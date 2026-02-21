using DG.Tweening;
using OpenCvSharp.Flann;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.DSG
{
    public class BattleSystem : MonoBehaviour
    {
        public GameObject[] friendlySlots = new GameObject[5];
        public GameObject[] enemySlots = new GameObject[5];

        [SerializeField]
        private RectTransform characterSequenceList;
        [SerializeField]
        private GameObject iconPrefab;
        [SerializeField]
        private GameObject battleCanvas;
        [SerializeField]
        private GameObject readyCanvas;
        [SerializeField]
        private GameObject characterUICanvas;
        [SerializeField]
        private GameObject EmptyMessage;

        [SerializeField]
        private TextMeshProUGUI roundText;
        [SerializeField]
        private TextMeshProUGUI playerCP;
        [SerializeField]
        private TextMeshProUGUI enemyCP;
        [SerializeField]
        private TextMeshProUGUI waveText;

        [SerializeField] private bool isAutoRound = false;

        private Coroutine autoRoutine;
        private bool isBattleEnded = false;

        private bool waitingNextRound = false;

        private bool isRoundRunning = false;

        private bool isBattleStarting = false;

        [Header("Auto Button UI")]
        [SerializeField] private Button autoButton;
        private Color autoOffColor = Color.white;
        private Color autoOnColor = Color.red;

        private List<Character> battleSequence = new List<Character>();
        private List<GameObject> sequenceImage = new List<GameObject>();

        private ChainedTargetSelector targetSelector;

        private int currentTurnIndex = 0;
        private int currentRound = 1;
        private float currentGameSpeed = 1f;
        private bool isBattleStart = false;
        private int currentWave = 0;

        public float iconSize = 1000f;
        private Dictionary<string, (Color Color, float Score)> deadScores = new();
        private List<(string Name, int CharId, Sprite Icon, float Score, GameObject Prefab, bool IsEnemy)> deadCharacterData = new();

        //public event Action<Character> onStartAttack;
        public event Action<Character> onStartSkill;

        private List<ObjectFader> fadedList = new List<ObjectFader>();
        private EnemyStageData stageData;

        void Awake()
        {
            StageInitializeInvoker.OnDSGStagePostInitialize += Initialize;
        }

        private void OnDestroy()
        {
            StageInitializeInvoker.OnDSGStagePostInitialize -= Initialize;
        }

        private void Initialize(DeckStrategyStage stage)
        {
            stageData = stage.GetEnemyStage();
            SetEnemyWave(currentWave);

            for (int i = 0; i < friendlySlots.Length; i++)
            {
                LineupSlot friendlySlot = friendlySlots[i].GetComponent<LineupSlot>();
            }
            FormationSystem formationSystem = GetComponent<FormationSystem>();
            if(formationSystem)
            {
                formationSystem.OnPowerUpdated += UpdatePlayerCP;
            }
            UpdatePlayerCP();
            UpdateEnemyCP();

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

                if (stageData.enemyTeamData[currentWave].characters[i] == null) continue;
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

                GameObject icon = Instantiate(iconPrefab, characterSequenceList);
                CharacterSequenceIcon sequenceIcon = icon.GetComponent<CharacterSequenceIcon>();
                sequenceIcon.SetIconData(character.characterData.type, character.characterInfo.characterLevel, character.isEnemy);

                var portrait = icon.transform.Find("Portrait")?.GetComponent<Image>();
                if (portrait != null)
                {
                    int characterId = character.IconCacheKey;
                    int modelId = character.characterModelData.ID;

                    var prtRt = portrait.rectTransform;
                    prtRt.localScale = Vector3.one * 5.0f;

                    Sprite sprite = null;

                    CharacterIconCache.TryGet(characterId, modelId, out sprite);
                    if (sprite == null) CharacterIconCache.TryGetByCharacterId(characterId, out sprite);

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
                character.StatusEffectComp.ClearRemoveList(); //@TODO Event Action으로 빼자

                if (!character.BattleComp.isAlive)
                {
                    character.BattleComp.OnDie -= OnDieIndexCharacter;
                    battleSequence.Remove(character);
                    //Destroy(character.gameObject);
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
                StartCoroutine(ViewWarningMessage());
                return;
            }

            StartCoroutine(BattleStart());
        }
        private IEnumerator ViewWarningMessage()
        {
            EmptyMessage.gameObject.SetActive(true);
            yield return new WaitForSeconds(2);
            EmptyMessage.gameObject.SetActive(false);
        }

        public IEnumerator BattleStart()
        {

            UpdateAutoButtonUI();

            if (isBattleStart || isBattleStarting)
                yield break;

            isBattleStarting = true;
            isBattleEnded = false;
            waitingNextRound = false;
            isRoundRunning = false;

            readyCanvas.SetActive(false);
            characterUICanvas.SetActive(false);
            
            //카메라 배틀 인트로
            Camera camera = Camera.main;
            BattleCameraDirector Director = camera.GetComponent<BattleCameraDirector>();
            yield return Director.PlayBattleIntroSequence().WaitForCompletion();
            yield return null;

            ChangeBattleUI(friendlySlots);
            ChangeBattleUI(enemySlots);

            battleCanvas.SetActive(true);
            characterUICanvas.SetActive(true);

            SortBattleSequence();

            currentTurnIndex = 0;
            currentRound = 1;
            UpdateUI();



            isBattleStart = true;
            isBattleStarting = false;

            waitingNextRound = true;

            if (isAutoRound)
            {
                NextRound();
            }
        }

        public void NextTurn()
        {
            if (!isBattleStart || battleSequence.Count == 0)
                return;

            if (currentTurnIndex >= battleSequence.Count)
            {
                waitingNextRound = true;

                InitSequence();
                return;
            }

            Character currentChar = battleSequence[currentTurnIndex];
            if (currentChar == null || currentChar.BattleComp == null || !currentChar.BattleComp.isAlive)
            {
                currentTurnIndex++;
                return;
            }
            if (currentChar.BattleComp.isSkillOn)
            {
                StartCoroutine(FocusSkillCaster(currentChar));
            }
            else
            {
                List<LineupSlot> targetslot = targetSelector.SelectEnemyTargets(currentChar, 1);
                CollectFadedTargets(currentChar.GetComponentInParent<LineupSlot>(), targetslot);
                TurnOnFader();
                currentChar.BattleComp.Attack(targetslot);
                StartCoroutine(WaitForAttackEnd(currentChar));
                //onStartAttack?.Invoke(currentChar);
            }

            return;
        }
        public void BackupScore(Character c)
        {
            if (c == null || c.characterData == null || c.ScoreComp == null)
                return;

            string name = c.characterData.characterName;
            Color color = Color.white;
            float score = c.ScoreComp.CalculateMVPScore();

            if (!deadScores.ContainsKey(name))
            {
                deadScores[name] = (color, score);
            }
        }

        public void EndBattle(string resultText)
        {
            DeckStrategyStage stage = LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
            if (stage == null) return;
            var mvp = stage.mvpData;
            if (mvp == null) return;
            mvp.battleResult = resultText;

            mvp.char1Score = mvp.char2Score = mvp.char3Score = mvp.char4Score = mvp.char5Score = 0f;
            mvp.char1Name = mvp.char2Name = mvp.char3Name = mvp.char4Name = mvp.char5Name = "";
            mvp.char1CharacterId = mvp.char2CharacterId = mvp.char3CharacterId = mvp.char4CharacterId = mvp.char5CharacterId = 0;
            mvp.char1ModelId = mvp.char2ModelId = mvp.char3ModelId = mvp.char4ModelId = mvp.char5ModelId = 0;
            mvp.char1Prefab = mvp.char2Prefab = mvp.char3Prefab = mvp.char4Prefab = mvp.char5Prefab = null;
            mvp.char1Icon = mvp.char2Icon = mvp.char3Icon = mvp.char4Icon = mvp.char5Icon = null;

            var friendlyChars = new List<(string Name, int CharId, int ModelId, Sprite Icon, float Score, GameObject Prefab)>();

            if (friendlySlots != null)
            {
                foreach (var slotObj in friendlySlots)
                {
                    if (slotObj == null) continue;

                    var slot = slotObj.GetComponent<LineupSlot>();
                    if (slot == null || slot.character == null) continue;

                    var ch = slot.character;
                    if (ch.characterData == null || ch.ScoreComp == null) continue;

                    if (ch.isEnemy) continue;

                    string name = ch.characterData.characterName;

                    int charId = ch.IconCacheKey;
                    int modelId = (ch.characterModelData != null) ? ch.characterModelData.ID : 0;

                    float score = ch.ScoreComp.CalculateMVPScore();
                    GameObject prefab = (ch.characterModelData != null) ? ch.characterModelData.prefab : null;

                    Sprite icon = ch.GetBattleIcon();

                    if (icon == null)
                    {
                        if (modelId != 0) CharacterIconCache.TryGet(charId, modelId, out icon);
                        if (icon == null) CharacterIconCache.TryGetByCharacterId(charId, out icon);
                        if (icon == null && modelId != 0) CharacterIconCache.TryGetByModelId(modelId, out icon);
                    }

                    if (icon == null)
                    {
                        Debug.LogWarning($"[EndBattle-ICON-NULL] name={name} charId={charId} modelId={modelId}");
                    }

                    friendlyChars.Add((name, charId, modelId, icon, score, prefab));
                }
            }

            foreach (var d in deadCharacterData)
            {
                if (d.IsEnemy) continue;

                if (friendlyChars.Exists(x => x.CharId == d.CharId))
                    continue;

                int modelId = 0;
                Sprite icon = d.Icon;
                if (icon == null)
                    CharacterIconCache.TryGetByCharacterId(d.CharId, out icon);

                friendlyChars.Add((d.Name, d.CharId, modelId, icon, d.Score, d.Prefab));
            }

            Debug.Log("==== [EndBattle] FriendlySlots ====");
            foreach (var slotObj in friendlySlots)
            {
                var slot = slotObj?.GetComponent<LineupSlot>();
                var ch = slot?.character;
                if (ch == null || ch.characterData == null) continue;
                Debug.Log($"[FRIEND] {ch.characterData.characterName} isEnemy={ch.isEnemy} charId={ch.IconCacheKey}");
            }

            Debug.Log("==== [EndBattle] deadCharacterData ====");
            foreach (var d in deadCharacterData)
            {
                Debug.Log($"[DEAD] {d.Name} charId={d.CharId} icon={(d.Icon ? d.Icon.name : "NULL")}");
            }

            var ranked = friendlyChars.OrderByDescending(x => x.Score).ToList();
            if (ranked.Count == 0) return;

            for (int i = 0; i < Mathf.Min(5, ranked.Count); i++)
            {
                var entry = ranked[i];

                ApplyMVP(
                    mvp,
                    i + 1,
                    entry.Name,
                    entry.CharId,
                    entry.ModelId,
                    entry.Score,
                    entry.Icon,
                    entry.Prefab);

                Debug.Log($"[EndBattle-SAVE] mvpInstanceID={mvp.GetInstanceID()} result={mvp.battleResult}");
                Debug.Log($"[EndBattle-SAVE] #1 name={mvp.char1Name} score={mvp.char1Score} icon={(mvp.char1Icon ? mvp.char1Icon.name : "NULL")} modelId={mvp.char1ModelId}");
                Debug.Log($"[EndBattle-SAVE] #2 name={mvp.char2Name} score={mvp.char2Score} icon={(mvp.char2Icon ? mvp.char2Icon.name : "NULL")} modelId={mvp.char2ModelId}");
            }

            Time.timeScale = 1f;

            isBattleEnded = true;
            waitingNextRound = false;
            isRoundRunning = false;
        }
        public void BackupDeadCharacter(Character ch)
        {
            if (ch == null || ch.characterData == null || ch.ScoreComp == null) return;

            if (ch.isEnemy) return;

            string name = ch.characterData.characterName;
            int charId = ch.IconCacheKey;
            float score = ch.ScoreComp.CalculateMVPScore();
            GameObject prefab = ch.characterModelData != null ? ch.characterModelData.prefab : null;

            Sprite icon = ch.GetBattleIcon();
            int modelId = (ch.characterModelData != null) ? ch.characterModelData.ID : 0;
            if (icon == null)
            {
                if (modelId != 0) CharacterIconCache.TryGet(charId, modelId, out icon);
                if (icon == null) CharacterIconCache.TryGetByCharacterId(charId, out icon);
                if (icon == null && modelId != 0) CharacterIconCache.TryGetByModelId(modelId, out icon);
            }

            if (deadCharacterData.Exists(x => x.CharId == charId)) return;
            deadCharacterData.Add((name, charId, icon, score, prefab, ch.isEnemy));
        }
        private void ApplyMVP(TeamMVPData data, int index, string name, int charId, 
            int modelId, float score, Sprite icon, GameObject prefab = null)
        {
            switch (index)
            {
                case 1:
                    data.char1Name = name;
                    data.char1CharacterId = charId;
                    data.char1ModelId = modelId;
                    data.char1Score = score;
                    data.char1Prefab = prefab;
                    data.char1Icon = icon;
                    break;

                case 2:
                    data.char2Name = name;
                    data.char2CharacterId = charId;
                    data.char2ModelId = modelId;
                    data.char2Score = score;
                    data.char2Prefab = prefab;
                    data.char2Icon = icon;
                    break;

                case 3:
                    data.char3Name = name;
                    data.char3CharacterId = charId;
                    data.char3ModelId = modelId;
                    data.char3Score = score;
                    data.char3Prefab = prefab;
                    data.char3Icon = icon;
                    break;

                case 4:
                    data.char4Name = name;
                    data.char4CharacterId = charId;
                    data.char4ModelId = modelId;
                    data.char4Score = score;
                    data.char4Prefab = prefab;
                    data.char4Icon = icon;
                    break;

                case 5:
                    data.char5Name = name;
                    data.char5CharacterId = charId;
                    data.char5ModelId = modelId;
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
        }

        private IEnumerator WaitForAttackEnd(Character currentChar)
        {
            yield return new WaitWhile(() => currentChar.BattleComp.isAttacking);
            ClearFadedList();
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
            if (isBattleEnded) return;
            if (isRoundRunning) return;

            StartCoroutine(CoNextRound());
        }

        private IEnumerator CoNextRound()
        {
            isRoundRunning = true;
            waitingNextRound = false;

            currentTurnIndex = 0;

            while (currentTurnIndex < battleSequence.Count)
            {
                if (currentTurnIndex >= battleSequence.Count) break;

                var currentChar = battleSequence[currentTurnIndex];
                if (currentChar == null || !currentChar.BattleComp.isAlive)
                {
                    currentTurnIndex++;
                    continue;
                }

                // 턴 실행
                NextTurn();

                yield return new WaitWhile(() => currentChar.BattleComp.isAttacking);

                if (isBattleEnded)
                {
                    isRoundRunning = false;
                    yield break;
                }

                yield return new WaitForSeconds(1);
            }

            InitSequence();

            isRoundRunning = false;

            waitingNextRound = true;

            if (isAutoRound && isBattleStart && !isBattleEnded)
            {
                yield return new WaitForSeconds(1);
                NextRound();
            }
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

            DeckStrategyStage stage = LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
            EnemyStageData stageData = stage.GetEnemyStage();
            if (stageData == null) return;

            foreach(Team enemyTeam in stageData.enemyTeamData)
            {
                for (int i = 0; i < enemyTeam.characters.Length; ++i)
                {
                    if (enemyTeam.characters[i] == null) continue;
                    CharacterData data = stage.FindCharacterData(enemyTeam.characters[i].characterID, enemyTeam.characters[i].characterLevel);
                    cp += data.maxHp + data.attack + data.defense + data.speed;
                }
            }
            enemyCP.text = cp.ToString();
        }

        private void CheckBattleEnd()
        {
            bool allFriendDead = true;
            bool allEnemyDead = true;

            foreach (var slotObj in enemySlots)
            {
                if (slotObj == null) continue;

                var slot = slotObj.GetComponent<LineupSlot>();
                if (slot == null) continue;

                var ch = slot.character;
                var bc = ch != null ? ch.BattleComp : null;

                if (bc != null && bc.isAlive)
                {
                    allEnemyDead = false;
                    break;
                }
            }

            foreach (var slotObj in friendlySlots)
            {
                var slot = slotObj.GetComponent<LineupSlot>();
                if (slot.character.BattleComp.isAlive)
                {
                    allFriendDead = false;
                    break;
                }
            }

            if (allEnemyDead)
            {
                if (currentWave + 1 < stageData.enemyTeamData.Count)
                {
                    ++currentWave;
                    SetEnemyWave(currentWave);
                    ChangeBattleUI(enemySlots);
                    SortBattleSequence();
                    currentTurnIndex = battleSequence.Count;
                }
                else
                {
                    EndBattle("Victory");
                    var stage = GetComponent<DeckStrategyStage>();
                    if (stage != null) stage.BattleEnd();
                    Time.timeScale = 1f;
                }
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
            CollectFadedTargets(currentSlot, targetList);
            TurnOnFader();
            currentChar.BattleComp.Skill(targetList);
            StartCoroutine(WaitForAttackEnd(currentChar));
        }

        public void OnClickPauseButton()
        {
            float Curr = Time.timeScale;

            if (Curr == 0f)
            {
                Time.timeScale = currentGameSpeed;
            }
            else
            {
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

        void CollectFadedTargets(LineupSlot attacker, List<LineupSlot> targets)
        {
            fadedList.Clear();

            foreach (GameObject slot in friendlySlots)
            {
                if (!slot.GetComponentInChildren<Character>()) continue;

                LineupSlot curSlot = slot.GetComponent<LineupSlot>();
                if (curSlot == attacker) continue;

                bool isContained = false;
                foreach (LineupSlot target in targets)
                {
                    if (curSlot == target)
                    {
                        isContained = true;
                        break;
                    }
                }
                if (isContained) continue;

                ObjectFader[] faders = curSlot.GetComponentsInChildren<ObjectFader>();
                fadedList.AddRange(faders);
            }

            foreach (GameObject slot in enemySlots)
            {
                if (!slot.GetComponentInChildren<Character>()) continue;

                LineupSlot curSlot = slot.GetComponent<LineupSlot>();
                if (curSlot == attacker) continue;

                bool isContained = false;
                foreach (LineupSlot target in targets)
                {
                    if (curSlot == target)
                    {
                        isContained = true;
                        break;
                    }
                }
                if (isContained) continue;

                ObjectFader[] faders = curSlot.GetComponentsInChildren<ObjectFader>();
                fadedList.AddRange(faders);
            }
        }

        void TurnOnFader()
        {
            foreach(ObjectFader fader in fadedList)
            {
                fader.doFade = true;
            }
        }

        void ClearFadedList()
        {
            foreach (ObjectFader fader in fadedList)
            {
                fader.doFade = false;
            }

            fadedList.Clear();
        }

        public void OnClickAutoButton()
        {
            isAutoRound = !isAutoRound;

            UpdateAutoButtonUI();

            if (isAutoRound)
            {
                if (!isBattleStart && !isBattleStarting && !isBattleEnded)
                {
                    StartCoroutine(BattleStart());
                    return;
                }
                if (isBattleStart && !isBattleEnded && waitingNextRound && !isRoundRunning)
                {
                    NextRound();
                }
            }
        }
        private void UpdateAutoButtonUI()
        {
            if (autoButton == null)
                return;

            Image img = autoButton.GetComponent<Image>();
            if (img == null)
                return;

            img.color = isAutoRound ? autoOnColor : autoOffColor;
        }

        private void SetEnemyWave(int index)
        {
            for (int i = 0; i < enemySlots.Length; i++)
            {
                LineupSlot enemySlot = enemySlots[i].GetComponent<LineupSlot>();
                enemySlot.DeselectCharacter();
                if (stageData.enemyTeamData[index].characters[i] == null) continue;

                enemySlot.SetSelectedCharacter(stageData.enemyTeamData[index].characters[i], true);
            }

            StringBuilder wave = new StringBuilder();
            wave.Append(currentWave+1);
            wave.Append(" / ");
            wave.Append(stageData.enemyTeamData.Count);
            waveText.SetText(wave);
        }

        private void ChangeBattleUI(GameObject[] slots)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                var slot = slots[i].GetComponent<LineupSlot>();
                if (slot.character != null && slot.isPlaced)
                {
                    slot.ActivateBattleUI();
                    slot.character.BattleComp.PlusGuage(50);
                }
            }
        }
    }
}
