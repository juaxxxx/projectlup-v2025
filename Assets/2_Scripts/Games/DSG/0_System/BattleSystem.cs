using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using Unity.VisualScripting;
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
        private readonly List<GameObject> sequenceImagePool = new List<GameObject>();

        private ChainedTargetSelector targetSelector;

        private int currentTurnIndex = 0;
        private int currentRound = 1;
        private float currentGameSpeed = 1f;
        private bool isBattleStart = false;
        private int currentWave = 0;

        public float iconSize = 1000f;
        private Dictionary<string, (Color Color, float Score)> deadScores = new();
        private List<(string Name, int CharId, Sprite Icon, float Score, GameObject Prefab, bool IsEnemy)> deadCharacterData = new();

        public event Action<Character> onStartSkill;

        private List<ObjectFader> fadedList = new List<ObjectFader>();
        private readonly HashSet<LineupSlot> targetSlotSet = new();
        private readonly List<ObjectFader> faderBuffer = new();

        private EnemyStageData stageData;
        private DeckStrategyStage cachedStage;

        private LineupSlot[] cachedFriendlySlots;
        private LineupSlot[] cachedEnemySlots;

        private CharacterFactory characterFactory;

        void Awake()
        {
            StageInitializeInvoker.OnDSGStagePostInitialize += Initialize;
        }

        private void OnDestroy()
        {
            StageInitializeInvoker.OnDSGStagePostInitialize -= Initialize;

            for (int i = 0; i < sequenceImagePool.Count; i++)
            {
                if (sequenceImagePool[i] != null)
                    Destroy(sequenceImagePool[i]);
            }
            sequenceImagePool.Clear();
        }

        private void Initialize(DeckStrategyStage stage)
        {
            cachedStage = stage;
            if (cachedStage == null) return;

            characterFactory = new CharacterFactory(stage);

            stageData = stage.GetEnemyStage();

            cachedFriendlySlots = CacheLineupSlots(friendlySlots);
            cachedEnemySlots = CacheLineupSlots(enemySlots);

            currentWave = 0;
            SetEnemyWave(currentWave);


            UpdatePlayerCP();
            UpdateEnemyCP();

            targetSelector = new ChainedTargetSelector(
                new PickWeakTarget(this),
                new PickHighestHpTarget(this),
                new PickRandomTarget(this)
            );
        }

        private void SortBattleSequence()
        {
            BuildBattleSequence();
            RefreshSequenceIcons();
        }

        private void BuildBattleSequence()
        {
            battleSequence.Clear();

            // Friendly
            for (int i = 0; i < cachedFriendlySlots.Length; i++)
            {
                var slot = cachedFriendlySlots[i];
                if (slot == null) continue;
                if (slot.character != null && slot.isPlaced)
                    battleSequence.Add(slot.character);
            }

            // Enemy
            if (stageData == null || stageData.enemyTeamData == null) return;
            if (currentWave < 0 || currentWave >= stageData.enemyTeamData.Count) return;
            Team waveTeam = stageData.enemyTeamData[currentWave];
            if (waveTeam == null || waveTeam.characters == null) return;

            for (int i = 0; i < cachedEnemySlots.Length; i++)
            {
                if (waveTeam.characters[i] == null) continue;

                var slot = cachedEnemySlots[i];
                if (slot == null) continue;
                if (slot.character != null)
                    battleSequence.Add(slot.character);
            }

        }

        private void RefreshSequenceIcons()
        {
            // 기존 아이콘들은 비활성(재사용)
            for (int i = 0; i < sequenceImagePool.Count; i++)
            {
                if (sequenceImagePool[i] != null)
                    sequenceImagePool[i].SetActive(false);
            }

            battleSequence.Sort((x, y) => y.characterData.speed.CompareTo(x.characterData.speed));

            EnsureSequenceIconPool(battleSequence.Count);

            for (int i = 0; i < battleSequence.Count; i++)
            {
                Character character = battleSequence[i];
                if (character == null) continue;

                character.battleIndex = i;

                GameObject icon = sequenceImagePool[i];
                if (icon == null) continue;

                icon.SetActive(true);
                icon.transform.SetSiblingIndex(i);

                CharacterSequenceIcon sequenceIcon = icon.GetComponent<CharacterSequenceIcon>();
                if (sequenceIcon != null)
                {
                    sequenceIcon.SetIconData(character.characterData.type, character.characterInfo.characterLevel, character.isEnemy);
                }

                Image portrait = icon.transform.Find("Portrait")?.GetComponent<Image>();
                if (portrait != null)
                {
                    int characterId = character.IconCacheKey;
                    int modelId = character.characterPrefabData != null ? character.characterPrefabData.ID : 0;

                    RectTransform rect = portrait.rectTransform;
                    rect.localScale = Vector3.one * 5.0f;

                    Sprite sprite = null;

                    if (modelId != 0)
                        CharacterIconCache.TryGet(characterId, modelId, out sprite);
                    if (sprite == null)
                        CharacterIconCache.TryGetByCharacterId(characterId, out sprite);

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

                // 중복 구독 방지
                if (character.BattleComp != null)
                {
                    character.BattleComp.OnDie -= OnDieIndexCharacter;
                    character.BattleComp.OnDie += OnDieIndexCharacter;
                }
            }
        }

        private void EnsureSequenceIconPool(int required)
        {
            while (sequenceImagePool.Count < required)
            {
                var icon = Instantiate(iconPrefab, characterSequenceList);
                sequenceImagePool.Add(icon);
            }
        }

        public void InitSequence()
        {
            currentRound++;

            for (int i = battleSequence.Count - 1; i >= 0; i--)
            {
                var character = battleSequence[i];
                if (character == null)
                {
                    battleSequence.RemoveAt(i);
                    continue;
                }

                // 턴/라운드 단위 상태이상 처리
                character.StatusEffectComp.TurnAll();
                character.StatusEffectComp.ClearRemoveList();

                if (!character.BattleComp.isAlive)
                {
                    if (character.BattleComp != null)
                        character.BattleComp.OnDie -= OnDieIndexCharacter;

                    battleSequence.RemoveAt(i);
                }
            }

            RefreshSequenceIcons();

            currentTurnIndex = 0;
            UpdateUI();
        }

        public void OnClickBattleStartButton()
        {
            if (!HasAnyCharacter(cachedFriendlySlots) || !HasAnyCharacter(cachedEnemySlots))
            {
                StartCoroutine(ViewWarningMessage());
                return;
            }

            StartCoroutine(BattleStart());
        }
        private IEnumerator ViewWarningMessage()
        {
            if (EmptyMessage == null) yield break;

            EmptyMessage.SetActive(true);
            yield return new WaitForSeconds(2);
            EmptyMessage.SetActive(false);
        }

        public IEnumerator BattleStart()
        {
            UpdateAutoButtonUI();

            if (isBattleStart || isBattleStarting) yield break;

            isBattleStarting = true;
            isBattleEnded = false;
            waitingNextRound = false;
            isRoundRunning = false;

            if (readyCanvas != null) readyCanvas.SetActive(false);
            if (characterUICanvas != null) characterUICanvas.SetActive(false);
            
            //카메라 배틀 인트로
            BattleCameraDirector director = Camera.main?.GetComponent<BattleCameraDirector>();
            if(director != null)
                yield return director.PlayBattleIntroSequence().WaitForCompletion();

            ChangeBattleUI(cachedFriendlySlots);
            ChangeBattleUI(cachedEnemySlots);

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
            }
        }

        public void EndBattle(string resultText)
        {
            var currentStage = LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
            if (currentStage == null) return;

            var mvp = currentStage.mvpData;
            if (mvp == null) return;

            mvp.battleResult = resultText;

            mvp.char1Score = mvp.char2Score = mvp.char3Score = mvp.char4Score = mvp.char5Score = 0f;
            mvp.char1Name = mvp.char2Name = mvp.char3Name = mvp.char4Name = mvp.char5Name = string.Empty;
            mvp.char1CharacterId = mvp.char2CharacterId = mvp.char3CharacterId = mvp.char4CharacterId = mvp.char5CharacterId = 0;
            mvp.char1ModelId = mvp.char2ModelId = mvp.char3ModelId = mvp.char4ModelId = mvp.char5ModelId = 0;
            mvp.char1Prefab = mvp.char2Prefab = mvp.char3Prefab = mvp.char4Prefab = mvp.char5Prefab = null;
            mvp.char1Icon = mvp.char2Icon = mvp.char3Icon = mvp.char4Icon = mvp.char5Icon = null;

            var friendlyChars = new List<(string Name, int CharId, int ModelId, Sprite Icon, float Score, GameObject Prefab)>();
            var friendlyIdSet = new HashSet<int>();

            if (cachedFriendlySlots != null)
            {
                for (int i = 0; i < cachedFriendlySlots.Length; i++)
                {
                    if (cachedFriendlySlots[i] == null || cachedFriendlySlots[i].character == null)
                        continue;

                    Character character = cachedFriendlySlots[i].character;
                    if (character.isEnemy) continue;
                    if (character.characterData == null || character.ScoreComp == null) continue;

                    string name = character.characterData.characterName;
                    int charId = character.IconCacheKey;
                    int modelId = (character.characterPrefabData != null) ? character.characterPrefabData.ID : 0;

                    float score = character.ScoreComp.CalculateMVPScore();
                    GameObject prefab = (character.characterPrefabData != null) ? character.characterPrefabData.prefab : null;

                    Sprite icon = ResolveBattleIcon(character, charId, modelId);

                    if (friendlyIdSet.Add(charId))
                        friendlyChars.Add((name, charId, modelId, icon, score, prefab));
                }
            }

            for (int i = 0; i < deadCharacterData.Count; i++)
            {
                var deadChar = deadCharacterData[i];
                if (deadChar.IsEnemy) continue;
                if (!friendlyIdSet.Add(deadChar.CharId)) continue;

                Sprite icon = deadChar.Icon;
                if (icon == null)
                    CharacterIconCache.TryGetByCharacterId(deadChar.CharId, out icon);

                friendlyChars.Add((deadChar.Name, deadChar.CharId, 0, icon, deadChar.Score, deadChar.Prefab));
            }

            var ranked = friendlyChars.OrderByDescending(x => x.Score).ToList();
            if (ranked.Count == 0) return;

            for (int i = 0; i < Mathf.Min(5, ranked.Count); i++)
            {
                var entry = ranked[i];
                ApplyMVP(mvp, i + 1, entry.Name, entry.CharId, entry.ModelId, entry.Score, entry.Icon, entry.Prefab);
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
            GameObject prefab = ch.characterPrefabData != null ? ch.characterPrefabData.prefab : null;

            Sprite icon = ch.GetBattleIcon();
            int modelId = (ch.characterPrefabData != null) ? ch.characterPrefabData.ID : 0;
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
            yield return new WaitWhile(() => currentChar != null && currentChar.BattleComp != null && currentChar.BattleComp.isAttacking);

            ClearFadedList();

            if (currentTurnIndex >= 0 && currentTurnIndex < sequenceImagePool.Count && sequenceImagePool[currentTurnIndex] != null)
            {
                sequenceImagePool[currentTurnIndex].SetActive(false);
            }

            currentTurnIndex++;
            UpdateUI();

            CheckBattleEnd();
        }

        public void NextRound()
        {
            if (!isBattleStart || battleSequence.Count == 0) return;
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

                Character currentChar = battleSequence[currentTurnIndex];
                if (currentChar == null || currentChar.BattleComp == null || !currentChar.BattleComp.isAlive)
                {
                    currentTurnIndex++;
                    continue;
                }

                // 턴 실행
                NextTurn();

                yield return new WaitWhile(() => currentChar.BattleComp != null && currentChar.BattleComp.isAttacking);

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

        public void UpdatePlayerCP()
        {
            if (playerCP == null) return;

            float cp = 0;
            for (int i = 0; i < cachedFriendlySlots.Length; i++)
            {
                LineupSlot slot = cachedFriendlySlots[i].GetComponent<LineupSlot>();
                if (slot == null || slot.character == null || slot.character.characterData == null) continue;

                cp += slot.character.characterData.maxHp +
                    slot.character.characterData.attack +
                    slot.character.characterData.defense +
                    slot.character.characterData.speed;
            }

            playerCP.text = cp.ToString();
        }

        void UpdateEnemyCP()
        {
            if (enemyCP == null) return;

            float cp = 0;

            DeckStrategyStage currentStage = LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
            if (currentStage == null) return;

            EnemyStageData enemyStage = currentStage.GetEnemyStage();
            if (enemyStage == null || enemyStage.enemyTeamData == null) return;

            foreach (Team enemyTeam in stageData.enemyTeamData)
            {
                if (enemyTeam == null || enemyTeam.characters == null) continue;

                for (int i = 0; i < enemyTeam.characters.Length; ++i)
                {
                    if (enemyTeam.characters[i] == null) continue;
                    CharacterData data = currentStage.FindCharacterData(enemyTeam.characters[i].characterID, enemyTeam.characters[i].characterLevel);
                    if (data == null) continue;

                    cp += data.maxHp + data.attack + data.defense + data.speed;
                }
            }
            enemyCP.text = cp.ToString();
        }

        private void CheckBattleEnd()
        {
            bool allFriendDead = true;
            bool allEnemyDead = true;

            for (int i = 0; i < cachedEnemySlots.Length; i++)
            {
                LineupSlot slot = cachedEnemySlots[i];
                Character character = slot != null ? slot.character : null;
                BattleComponent battleComp = character != null ? character.BattleComp : null;

                if (battleComp != null && battleComp.isAlive)
                {
                    allEnemyDead = false;
                    break;
                }
            }

            for (int i = 0; i < cachedFriendlySlots.Length; i++)
            {
                LineupSlot slot = cachedFriendlySlots[i];
                Character character = slot != null ? slot.character : null;
                BattleComponent battleComp = character != null ? character.BattleComp : null;

                if (battleComp != null && battleComp.isAlive)
                {
                    allFriendDead = false;
                    break;
                }
            }

            if (allEnemyDead)
            {
                if (stageData != null && stageData.enemyTeamData != null && (currentWave + 1) < stageData.enemyTeamData.Count)
                {
                    ++currentWave;
                    SetEnemyWave(currentWave);
                    ChangeBattleUI(cachedEnemySlots);
                    SortBattleSequence();
                    currentTurnIndex = battleSequence.Count;
                }
                else
                {
                    EndBattle("Victory");
                    DeckStrategyStage stage = GetComponent<DeckStrategyStage>();
                    if (stage != null) stage.BattleEnd();
                    Time.timeScale = 1f;
                }
            }
            else if (allFriendDead)
            {
                EndBattle("Defeat");
                DeckStrategyStage stage = GetComponent<DeckStrategyStage>();
                if (stage != null) stage.BattleEnd();
                Time.timeScale = 1f;
            }
        }
        private void OnDieIndexCharacter(int index)
        {
            if (index < 0 || index >= sequenceImagePool.Count) return;
            if (sequenceImagePool[index] != null)
                sequenceImagePool[index].SetActive(false);
        }

        private IEnumerator FocusSkillCaster(Character currentChar)
        {
            if (currentChar == null || currentChar.BattleComp == null)
                yield break;

            currentChar.BattleComp.isAttacking = true;
            onStartSkill?.Invoke(currentChar);

            Camera camera = Camera.main;
            if(camera != null)
            {
                BattleCameraDirector director = camera.GetComponent<BattleCameraDirector>();
                LineupSlot currentSlot = currentChar.GetComponentInParent<LineupSlot>();
                if (director != null && currentSlot != null)
                {
                    Transform focusTransform = currentSlot.focusedPosition;
                    Transform cameraOrigin = camera.transform;

                    yield return director.FocusOnSkillCaster(focusTransform, cameraOrigin).WaitForCompletion();
                    director.FocusOnTarget(cameraOrigin.position);
                }
            }

            List<LineupSlot> targetList = targetSelector.SelectEnemyTargets(currentChar, currentChar.BattleComp.skillInfo.targetCount);
            CollectFadedTargets(currentChar.GetComponentInParent<LineupSlot>(), targetList);
            TurnOnFader();

            currentChar.BattleComp.Skill(targetList);
            StartCoroutine(WaitForAttackEnd(currentChar));
        }

        public void OnClickPauseButton()
        {
            float curr = Time.timeScale;
            Time.timeScale = (curr == 0f) ? currentGameSpeed : 0f;
        }

        public void OnClickSpeedButton()
        {
            float Curr = Time.timeScale; //@TODO 구조개선 timescale XX
            if (Curr == 0f) return;

            string text = null;
            float time = 1f;

            if (currentGameSpeed == 1f)
            {
                text = "2X";
                time = 2f;
            }
            else if (currentGameSpeed == 2f)
            {
                text = "4X";
                time = 4f;
            }
            else
            {
                text = "1X";
                time = 1f;
            }

            Time.timeScale = time;
            currentGameSpeed = Time.timeScale;
        }

        void CollectFadedTargets(LineupSlot attacker, List<LineupSlot> targets)
        {
            fadedList.Clear();
            targetSlotSet.Clear();

            if (targets != null)
            {
                for (int i = 0; i < targets.Count; i++)
                {
                    if (targets[i] != null) targetSlotSet.Add(targets[i]);
                }
            }

            CollectFadersFromSlots(cachedFriendlySlots, attacker);
            CollectFadersFromSlots(cachedEnemySlots, attacker);
        }

        private void CollectFadersFromSlots(LineupSlot[] slots, LineupSlot attacker)
        {
            if (slots == null) return;

            for (int i = 0; i < slots.Length; i++)
            {
                LineupSlot curSlot = slots[i];
                if (curSlot == null) continue;
                if (curSlot == attacker) continue;

                if (curSlot.character == null) continue;
                if (targetSlotSet.Contains(curSlot)) continue;

                faderBuffer.Clear();
                curSlot.GetComponentsInChildren(includeInactive: true, faderBuffer);
                fadedList.AddRange(faderBuffer);
            }
        }

        void TurnOnFader()
        {
            foreach(ObjectFader fader in fadedList)
            {
                if (fader != null)
                    fader.doFade = true;
            }
        }

        void ClearFadedList()
        {
            foreach (ObjectFader fader in fadedList)
            {
                if (fader != null)
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
            if (stageData == null || stageData.enemyTeamData == null) return;
            if (index < 0 || index >= stageData.enemyTeamData.Count) return;

            Team team = stageData.enemyTeamData[index];
            if (team == null || team.characters == null) return;

            for (int i = 0; i < cachedEnemySlots.Length; i++)
            {
                LineupSlot enemySlot = cachedEnemySlots[i];
                if (enemySlot == null) continue;

                enemySlot.ClearCharacter();

                if (team.characters[i] == null || team.characters[i].characterID <= 0) continue;
                Character newEnemy = characterFactory.GetCharacter(team.characters[i], enemySlot.transform, true);
                if (newEnemy == null) continue;
                enemySlot.SetCharacter(newEnemy);
            }

            if (waveText != null)
                waveText.SetText($"{index + 1} / {stageData.enemyTeamData.Count}");
        }

        private void ChangeBattleUI(LineupSlot[] slots)
        {
            if (slots == null) return;

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i].character != null && slots[i].isPlaced)
                {
                    slots[i].ActivateBattleUI();
                    slots[i].character.BattleComp.PlusGuage(50);
                }
            }
        }

        private LineupSlot[] CacheLineupSlots(GameObject[] slotObjects)
        {
            if (slotObjects == null) return Array.Empty<LineupSlot>();

            LineupSlot[] result = new LineupSlot[slotObjects.Length];
            for (int i = 0; i < slotObjects.Length; i++)
            {
                GameObject go = slotObjects[i];
                result[i] = (go != null) ? go.GetComponent<LineupSlot>() : null;
            }
            return result;
        }

        private bool HasAnyCharacter(LineupSlot[] slots)
        {
            if (slots == null) return false;

            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i] != null && slots[i].character != null)
                    return true;
            }
            return false;
        }

        private Sprite ResolveBattleIcon(Character character, int charId, int modelId)
        {
            if (character == null) return null;

            Sprite icon = character.GetBattleIcon();

            if (icon == null)
            {
                if (modelId != 0)
                    CharacterIconCache.TryGet(charId, modelId, out icon);

                if (icon == null)
                    CharacterIconCache.TryGetByCharacterId(charId, out icon);

                if (icon == null && modelId != 0)
                    CharacterIconCache.TryGetByModelId(modelId, out icon);
            }

            return icon;
        }

    }
}
