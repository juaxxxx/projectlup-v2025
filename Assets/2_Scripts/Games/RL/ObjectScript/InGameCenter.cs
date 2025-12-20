using Roguelike.Define;
using Roguelike.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
        private Dictionary<ItemData, int> gainItem = new Dictionary<ItemData, int>();

        private StageController stageController;
        private FollowCamera followCamera;

        public bool gameClear = false;
        [SerializeField] private BuffSelectionUI buffUI;
        private PlayerBuff playerBuff;

        public Action<GameObject> OnPlayerCharacterSpawned;

        //------Temp Test Button
        //public Button AddItem1Btn;
        //public Button AddItem2Btn;
        //public Button AddItem3Btn;
        //public Button AddTestItemBtn;

        //public Button ClearGameBtn;
        //public Button DebugBtn;

        //private bool debugMode = false;

        //public GameObject DebugPanel;
        //------------------------

        private CircleButton pauseBtn;
        public Button Confirm;

        private GameObject player;
        private PlayerMove playerMove;

        public GameObject Player => player;
        public PlayerMove PlayerMove => playerMove;

        private Archer controlledPlayer = null;


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

        void SpawnPlayer()
        {
            Vector3 pos = new Vector3(0, 0.7f, 0);
            Quaternion rot = Quaternion.identity;

            GameObject character = Instantiate(characterData.CharacterPrefab, pos, rot);
            playerBuff = character.GetComponent<PlayerBuff>();
            if (playerBuff && buffUI)
            {
                playerBuff.OnRequestBuffUI += buffUI.Bind;
            }
            WeaponHand weaponHand = character.GetComponent<WeaponHand>();
            FireSystem fireSystem = character.GetComponent<FireSystem>();
            MeleeSystem meleeSystem = character.GetComponent<MeleeSystem>();
            if (weaponHand)
            {
                Transform weaponHandPos = weaponHand.weaponHandPos;
                GameObject weaponPrefab = characterData.WeaponPrefab;

                if (weaponHandPos && weaponPrefab)
                {
                    GameObject weapon = Instantiate(weaponPrefab, weaponHandPos);

                    weapon.transform.localPosition = weaponHand.weaponPos;
                    weapon.transform.localRotation = Quaternion.Euler(weaponHand.rotate);

                    if(weaponHand.weaponType == RWeaponType.Throw)
                    {
                        SetCustumProjectile(character);
                        character.GetComponent<FireSystem>().bulletData.bulletPrefab = characterData.GetWeaponProjecTile();
                    }
                    else if(weaponHand.weaponType == RWeaponType.TwoHandSword)
                    {
                        Collider hitcol = weapon.GetComponent<Collider>();
                        if(hitcol && meleeSystem)
                        {
                            meleeSystem.hitcolider = hitcol;
                        }
                    }

                }

                else if(weaponHand.weaponType == RWeaponType.Magic)
                {
                    SetCustumProjectile(character);
                    character.GetComponent<FireSystem>().bulletData.bulletPrefab = characterData.GetWeaponProjecTile();
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

        public void AddItem(ItemData pickedItem)
        {
            if (!gainItem.ContainsKey(pickedItem))
            {
                gainItem[pickedItem] = 1;
            }

            else
            {
                gainItem[pickedItem]++;
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
        void OnGainSpawnableItem(int itemID)
        {
            IItemable item = ItemManager.Instance.GetItem(itemID);
            ItemData gaindedItem = ScriptableObject.CreateInstance<ItemData>();

            gaindedItem.SetItemName(item.ItemName);
            gaindedItem.SetDisplayableImage(item.Icon);

            AddItem(gaindedItem);

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

        void SetCustumProjectile(GameObject character)
        {
            BulletData custumBulletData = ScriptableObject.CreateInstance<BulletData>();
            custumBulletData.bulletPrefab = characterData.GetWeaponProjecTile();
            custumBulletData.Speed = characterData.projecTileSpeed;

            character.GetComponent<FireSystem>().bulletData = custumBulletData;
        }
    }
}

