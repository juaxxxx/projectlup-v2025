using Roguelike.Define;
using Roguelike.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

namespace LUP.RL
{
    public class InGameCenter : MonoBehaviour
    {
        public static InGameCenter Instance;
        [Header("레벨데이터")]
        public LevelDataTable levelTable;

        [SerializeField]
        private GameObject inGamePopupPanels;

        public GameObject gameResultPanel;
        public GameObject gamePausePanel;

        [HideInInspector]
        public PlatformAdapter platformAdapter;

        [SerializeField]
        private ChapterData chapterData;

        [SerializeField]
        private RLCharacterData characterData;

        //private ItemData[] spawnableItemDatas;
        private Dictionary<ItemData, int> gainItem = new Dictionary<ItemData, int>(new ItemDataComparer());

        private StageController stageController;
        private FollowCamera followCamera;

        public bool gameClear = false;
        [SerializeField] private BuffSelectionUI buffUI;
        private PlayerBuff playerBuff;

        public Action<GameObject> OnPlayerCharacterSpawned;

        private CircleButton pauseBtn;
        public Button Confirm;

        private GameObject player;
        private PlayerMove playerMove;

        public GameObject Player => player;
        public PlayerMove PlayerMove => playerMove;

        private Archer controlledPlayer = null;

        public List<WeaponData> WeaponList;
        private Dictionary<int, WeaponData> _weaponDict;


        //private RoguelikeStage stage;

        [HideInInspector]
        public ItemSpawner itemSpawner;

        private void Awake()
        {
            Instance = this;
        }
      
        private void Start()
        {
            //stage = FindFirstObjectByType<RoguelikeStage>();

            //if (stage == null)
            //    Debug.Log("Fail To Find Stage Object");

            itemSpawner = FindFirstObjectByType<ItemSpawner>();
            if(itemSpawner)
            {
                itemSpawner.OnItemGained += OnGainSpawnableItem;
            }

        }

        public void InitializeCenter()
        {
            platformAdapter = new PlatformAdapter();

            if (platformAdapter != null)
            {
                platformAdapter.LinkToPlatform();

                LoadInGameData();

            }

            {
                stageController = GameObject.FindFirstObjectByType<StageController>();
                if (stageController == null)
                {
                    UnityEngine.Debug.LogError("Fail To Find StageController!");
                }

                else
                {
                    stageController.onStageClear.AddListener(GameClear);
                    stageController.onMoveToNextRoom.AddListener(OnMoveToNextRoom);
                }
            }

            {
                followCamera = GameObject.FindFirstObjectByType<FollowCamera>();
                if (followCamera == null)
                {
                    UnityEngine.Debug.LogError("Fail To Find FollowCam");
                }
            }

            InitInGameUIElement();
            InitWeaponDataMap();

            Confirm.onClick.AddListener(UploadGameResult);

            SpawnPlayer();
        }

        // Update is called once per frame
        void Update()
        {

        }

        void LoadInGameData()
        {
            LoadSelectionData();
        }
        public void RegisterPlayer(GameObject newplayer)
        {
            player = newplayer;
            if (player == null)
            {
                Debug.LogError("RegisterPlayer failed: player is null");
                return;
            }

            playerMove = player.GetComponent<PlayerMove>();
            if (PlayerMove == null)
            {
                Debug.LogError("RegisterPlayer failed: PlayerMove not found");
            }

            Debug.Log($"Player registered to InGameCenter: {player.name}");
        }
        void LoadSelectionData()
        {
            if (platformAdapter.LoadSelectionData())
            {
                chapterData = platformAdapter.selectedChapter;
                characterData = platformAdapter.selectedCharacter;
            }

            else
            {
                UnityEngine.Debug.LogError("Fail To Load Selection Data", this.gameObject);
            }
        }

        void InitInGameUIElement()
        {
            {
                if (gameResultPanel != null)
                {
                    gameResultPanel.SetActive(false);
                }

            }

            {
                ButtonRule[] circleButtons = inGamePopupPanels.GetComponentsInChildren<ButtonRule>();

                for (int i = 0; i < circleButtons.Length; i++)
                {
                    ButtonRole buttonRole = circleButtons[i].buttonRole;

                    switch (buttonRole)
                    {
                        case ButtonRole.PauseGameBtn:
                            pauseBtn = circleButtons[i].gameObject.GetComponent<CircleButton>();
                            pauseBtn.button.onClick.AddListener(OnPauseBtnClicked);
                            break;
                    }
                }
            }

        }

        void InitWeaponDataMap()
        {
            _weaponDict = new Dictionary<int, WeaponData>();

            foreach (var weapon in WeaponList)
            {
                _weaponDict[weapon.ItemID] = weapon;
            }
        }

        void SpawnPlayer()
        {
            Vector3 pos = new Vector3(0, 0.7f, 0);
            Quaternion rot = Quaternion.identity;
                                                                                                                                                               
            GameObject character = Instantiate(characterData.CharacterPrefab, pos, rot);
            playerBuff = character.GetComponent<PlayerBuff>();
            FindFirstObjectByType<ExpCenter>().BindPlayer(character);
            if (playerBuff && buffUI)
            {
                playerBuff.OnRequestBuffUI += buffUI.Bind;
            }

            (WeaponData, GameObject) ArmedResult = BeArmed(character);
            WeaponData equipedWeaponData = ArmedResult.Item1;
            GameObject weaponInstance = ArmedResult.Item2;


            //WeaponHand weaponHand = character.GetComponent<WeaponHand>();
            FireSystem fireSystem = character.GetComponent<FireSystem>();
            MeleeSystem meleeSystem = character.GetComponent<MeleeSystem>();
            ShooterComp shooterComp = character.GetComponent<ShooterComp>();

            if(shooterComp)
            {
                shooterComp.weapon = equipedWeaponData;
            }

            if(equipedWeaponData != null)
            {
                if (equipedWeaponData.weaponType == RWeaponType.Throw)
                {
                    SetCustumProjectile(character, equipedWeaponData);
                    //character.GetComponent<FireSystem>().bulletData.bulletPrefab = characterData.GetWeaponProjecTile();
                }
                else if (equipedWeaponData.weaponType == RWeaponType.TwoHandSword)
                {
                    Collider hitcol = weaponInstance.GetComponent<Collider>();
                    if (hitcol && meleeSystem)
                    {
                        meleeSystem.hitcolider = hitcol;
                    }
                }

                else if (equipedWeaponData.weaponType == RWeaponType.Magic)
                {
                    SetCustumProjectile(character, equipedWeaponData);
                    //character.GetComponent<FireSystem>().bulletData.bulletPrefab = characterData.GetWeaponProjecTile();
                }
            }

            OnPlayerCharacterSpawned?.Invoke(character);

            itemSpawner.SetPlayerPos(character.transform);

            FollowCamera followCamera = FindFirstObjectByType<FollowCamera>();
            if(followCamera)
            {
                followCamera.FindTarget();
            }

            controlledPlayer = character.gameObject.GetComponent<Archer>();
          
            if (controlledPlayer == null)
                UnityEngine.Debug.LogError("Fail to Find Player!!");
        }

        void OnMoveToNextRoom()
        {
            followCamera.FindTarget();
        }

        void OnPauseBtnClicked()
        {
            Time.timeScale = 0f;

            ShowPausePanel();
        }

        void ShowPausePanel()
        {
            OwningBuffListScrollPanel buffScrollPanel = inGamePopupPanels.GetComponentInChildren<OwningBuffListScrollPanel>(true);
            buffScrollPanel.SetScrollPanelType(ScrollRect.MovementType.Elastic, LayoutDirection.Grid, TextAnchor.UpperLeft);

            IDisplayable[] buffs = controlledPlayer.PlayerBuff.GetActiveBufflist().ToArray();

            gamePausePanel.SetActive(true);
            buffScrollPanel.OpenPanel(buffs, DisplayableDataType.ItemData);
        }

        public void OnEnemyDie(Transform diePosition)
        {
            //AddItem(spawnableItemDatas[0]);
        }

        public void AddItem(ItemData pickedItem, int gainedAmount)
        {
            if (!gainItem.ContainsKey(pickedItem))
            {
                gainItem[pickedItem] = gainedAmount;
            }

            else
            {
                gainItem[pickedItem] += gainedAmount;
            }
        }

        public void RoomClear()
        {
            itemSpawner.OnRoomCleared();
        }

        void GameClear()
        {
            Debug.Log("클리어 버튼");
            gameResultPanel.SetActive(true);

            ShowGameResult();


            gameClear = true;

        }
        void OnGainSpawnableItem(int itemID, int gainedAmount)
        {
            IItemable item = ItemManager.Instance.GetItem(itemID);
            ItemData gaindedItem = ScriptableObject.CreateInstance<ItemData>();

            gaindedItem.SetItemName(item.ItemName);
            gaindedItem.SetDisplayableImage(item.Icon);

            AddItem(gaindedItem, gainedAmount);

            //string itemName = item.ItemName;
            //for(int i = 0; i < spawnableItemDatas.Length; i++)
            //{
            //    if (spawnableItemDatas[i].GetDisplayableName() == itemName)
            //    {
            //        AddItem(spawnableItemDatas[i]);
            //    }
            //}


        }

        void ShowGameResult()
        {
            //Time.timeScale = 0;

            ResultGainItemScrollPanel resultIteScrollPanel = inGamePopupPanels.GetComponentInChildren<ResultGainItemScrollPanel>(true);
            resultIteScrollPanel.SetScrollPanelType(ScrollRect.MovementType.Elastic, LayoutDirection.Grid);

            IDisplayable[] gainedItems = MakeGainItemArray();

            resultIteScrollPanel.OpenPanel(gainedItems, DisplayableDataType.ItemData);
        }

        void UploadGameResult()
        {
            platformAdapter.ApplyGameResult(gainItem, chapterData, characterData, gameClear);
        }

        IDisplayable[] MakeGainItemArray()
        {
            int count = gainItem.Count;
            IDisplayable[] gainedItem = new IDisplayable[count];

            int index = 0;
            foreach (var pair in gainItem)
            {
                var itemData = pair.Key;
                var value = pair.Value;

                gainedItem[index] = itemData;
                gainedItem[index].SetExtraInfo(value);

                index++;
            }

            return gainedItem;
        }

        (WeaponData, GameObject) BeArmed(GameObject playingCharacter)
        {
            Animator animator = playingCharacter.GetComponent<Animator>();
            if (animator)
            {
                int weaponItemId = characterData.EquipItems.Weapon;

                if (weaponItemId == 0 || _weaponDict.ContainsKey(weaponItemId) == false)
                {
                    SetCharacterCombatDefault();
                    return (null, null);
                }

                else
                {
                    WeaponData weaponData = _weaponDict[weaponItemId];

                    Transform rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);
                    Transform leftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);

                    GameObject weaponInstance = Instantiate(weaponData.weaponPrefab);
                    weaponInstance.transform.SetParent(rightHand);
                    weaponInstance.transform.localPosition = Vector3.zero;
                    weaponInstance.transform.localRotation = Quaternion.identity;

                    weaponInstance.transform.localPosition = weaponData.IdleweaponRightHandGrapPos;
                    weaponInstance.transform.localRotation = Quaternion.Euler(weaponData.IdleweaponRotate);

                    if (weaponData.overrideController)
                        animator.runtimeAnimatorController = weaponData.overrideController;

                    if(weaponData.handType == WeaponHandType.TowHand)
                    {
                        Transform leftHandGrip = weaponInstance.transform.Find("LeftHandGrapPos");
                        if (leftHandGrip)
                            playingCharacter.GetComponent<PlayerBehaviorTree>().leftHandIKTransform = leftHandGrip;
                    }

                    return (_weaponDict[weaponItemId], weaponInstance);
                }
            }

            return (null, null);
        }

        void SetCharacterCombatDefault()
        {

        }

        void SetCustumProjectile(GameObject character, WeaponData equipedWeaponData)
        {
            BulletData custumBulletData = ScriptableObject.CreateInstance<BulletData>();
            custumBulletData.bulletPrefab = equipedWeaponData.weaponProjecTile;
            custumBulletData.Speed = equipedWeaponData.projecTileSpeed;

            character.GetComponent<FireSystem>().bulletData = custumBulletData;

            //미리 Warmup
            Instantiate(equipedWeaponData.weaponProjecTile, new Vector3(100.0f, 100.0f, 100.0f), Quaternion.identity);
        }
    }
}

