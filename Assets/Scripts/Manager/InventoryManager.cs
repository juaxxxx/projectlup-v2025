using System;
using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace Manager
{
    public class InventoryManager : Singleton<InventoryManager>
    {
        [Header("아이템 데이터베이스")]
        [SerializeField] private string itemDatabasePath = "Items"; // Resources 폴더 경로

        // 게임별 설정
        private Framework.IGameInventoryConfig gameConfig;

        // 현재 활성 인벤토리
        private MonoBehaviour currentInventory;

        // 아이템 데이터베이스 (ID → IInventoryItem)
        private Dictionary<int, IInventoryItemable> itemDatabase;

        // ⭐ 공용 재화 (모든 게임에서 공유)
        private Dictionary<string, long> globalCurrencies;

        // 전역 이벤트
        public event Action<IInventoryItemable, int> OnAnyItemAdded;
        public event Action<IInventoryItemable, int> OnAnyItemRemoved;
        public event Action OnAnyInventoryUpdated;

        // 공용 재화 이벤트
        public event Action<string, long> OnGlobalCurrencyChanged;

        private bool isDatabaseLoaded = false;
        private bool isConfigured = false;

        public override void Awake()
        {
            base.Awake();
            InitializeItemDatabase();
            InitializeGlobalCurrencies();
        }

        /// 게임 시작 시 설정 주입 (필수)
        public void Initialize(Framework.IGameInventoryConfig config)
        {
            if (config == null)
            {
                Debug.LogError("[InventoryManager] Config cannot be null!");
                return;
            }

            gameConfig = config;
            isConfigured = true;

            Debug.Log($"[InventoryManager] Initialized with {config.GetType().Name}");
        }

        /// 공용 재화 초기화 (모든 게임에서 공유되는 재화)
        private void InitializeGlobalCurrencies()
        {
            if (globalCurrencies == null)
            {
                globalCurrencies = new Dictionary<string, long>
                {
                    { "Diamond", 0 },     
                    { "AccountExp", 0 }    
                };

                Debug.Log("[InventoryManager] Global currencies initialized");
            }
        }

        /// 현재 게임 설정 조회
        public Framework.IGameInventoryConfig GetConfig() => gameConfig;

        /// 설정이 완료되었는지 확인
        public bool IsConfigured() => isConfigured;

        public void RegisterInventory(MonoBehaviour inventory)
        {
            currentInventory = inventory;
            Debug.Log($"[InventoryManager] Registered: {inventory.GetType().Name}");

            if (isConfigured && inventory is Framework.Inventory inv)
                SetupInventoryFromConfig(inv);
        }

        private void SetupInventoryFromConfig(Framework.Inventory inventory)
        {
            if (gameConfig == null) return;

            foreach (string currency in gameConfig.GetCurrencyTypes())
                inventory.SetCurrency(currency, 0);

            Debug.Log($"[InventoryManager] Configured inventory with {gameConfig.GetCurrencyTypes().Count} currencies and {gameConfig.GetEquipmentSlots().Count} equipment slots");
        }

        /// 이 재화가 현재 게임에서 유효한지 검증
        public bool IsCurrencyValid(string currencyName)
        {
            if (!isConfigured)
            {
                Debug.LogWarning("[InventoryManager] Not configured. Cannot validate currency.");
                return false;
            }

            bool isValid = gameConfig.GetCurrencyTypes().Contains(currencyName);
            if (!isValid)
            {
                Debug.LogWarning($"[InventoryManager] Currency '{currencyName}' is not defined in game config. Valid currencies: {string.Join(", ", gameConfig.GetCurrencyTypes())}");
            }

            return isValid;
        }

        /// 이 장비 슬롯이 현재 게임에서 유효한지 검증
        public bool IsEquipSlotValid(string slotName)
        {
            if (!isConfigured)
            {
                Debug.LogWarning("[InventoryManager] Not configured. Cannot validate equipment slot.");
                return false;
            }

            bool isValid = gameConfig.GetEquipmentSlots().Contains(slotName);
            if (!isValid)
            {
                Debug.LogWarning($"[InventoryManager] Equipment slot '{slotName}' is not defined in game config. Valid slots: {string.Join(", ", gameConfig.GetEquipmentSlots())}");
            }

            return isValid;
        }

        public T GetCurrentInventory<T>() where T : MonoBehaviour
        {
            if (currentInventory == null)
            {
                Debug.LogWarning("[InventoryManager] No inventory registered");
                return null;
            }

            if (currentInventory is T inventory)
            {
                return inventory;
            }

            Debug.LogError($"[InventoryManager] Current inventory is not of type {typeof(T).Name}");
            return null;
        }

        public bool HasInventory()
        {
            return currentInventory != null;
        }

        private void InitializeItemDatabase()
        {
            if (isDatabaseLoaded)
                return;

            itemDatabase = new Dictionary<int, IInventoryItemable>();

            // 이건 리소스매니저 또는 어드레서블과 연결해봐야할듯
            Framework.BaseItemData[] items = Resources.LoadAll<Framework.BaseItemData>(itemDatabasePath);

            foreach (var item in items)
            {
                if (itemDatabase.ContainsKey(item.ItemID))
                {
                    Debug.LogWarning($"[InventoryManager] Duplicate ItemID: {item.ItemID} ({item.ItemName})");
                    continue;
                }

                itemDatabase[item.ItemID] = item;
            }

            isDatabaseLoaded = true;
            Debug.Log($"[InventoryManager] Loaded {itemDatabase.Count} items from database");
        }

        public IInventoryItemable GetItemData(int itemID)
        {
            if (!isDatabaseLoaded)
            {
                InitializeItemDatabase();
            }

            if (itemDatabase.TryGetValue(itemID, out var item))
            {
                return item;
            }

            Debug.LogError($"[InventoryManager] Item not found: ID {itemID}");
            return null;
        }

        public T GetItemData<T>(int itemID) where T : class, IInventoryItemable
        {
            IInventoryItemable item = GetItemData(itemID);
            return item as T;
        }

        public bool ItemExists(int itemID)
        {
            if (!isDatabaseLoaded)
                InitializeItemDatabase();

            return itemDatabase.ContainsKey(itemID);
        }

        public List<IInventoryItemable> GetItemsByType(Define.ItemType itemType)
        {
            if (!isDatabaseLoaded)
                InitializeItemDatabase();

            List<IInventoryItemable> result = new List<IInventoryItemable>();

            foreach (var item in itemDatabase.Values)
            {
                if (item.ItemType == itemType)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        public List<IInventoryItemable> GetAllItems()
        {
            if (!isDatabaseLoaded)
                InitializeItemDatabase();

            return new List<IInventoryItemable>(itemDatabase.Values);
        }

        public void SaveInventory(string saveKey = "CurrentInventory")
        {
            if (currentInventory == null)
            {
                Debug.LogWarning("[InventoryManager] No inventory to save");
                return;
            }

            var saveMethod = currentInventory.GetType().GetMethod("ToSaveData");
            if (saveMethod == null)
            {
                Debug.LogError($"[InventoryManager] {currentInventory.GetType().Name} does not implement ToSaveData()");
                return;
            }

            InventoryRuntimeData data = saveMethod.Invoke(currentInventory, null) as InventoryRuntimeData;
            if (data != null)
            {
                JsonDataHelper.SaveData(data, $"{saveKey}.json");
                Debug.Log($"[InventoryManager] Inventory saved: {saveKey}");
            }
        }

        public void LoadInventory(string saveKey = "CurrentInventory")
        {
            if (currentInventory == null)
            {
                Debug.LogWarning("[InventoryManager] No inventory to load into");
                return;
            }

            string filename = $"{saveKey}.json";

            if (!JsonDataHelper.FileExists(filename))
            {
                Debug.LogWarning($"[InventoryManager] Save file not found: {filename}");
                return;
            }

            InventoryRuntimeData data = JsonDataHelper.LoadData<InventoryRuntimeData>(filename);

            if (data != null)
            {
                var loadMethod = currentInventory.GetType().GetMethod("FromSaveData");
                if (loadMethod != null)
                {
                    loadMethod.Invoke(currentInventory, new object[] { data });
                    Debug.Log($"[InventoryManager] Inventory loaded: {saveKey}");
                }
            }
        }

        // 이건 스테이지 매니저에서 호출할듯?
        public void OnStageExit()
        {
            if (currentInventory == null)
            {
                return;
            }

            var persistProperty = currentInventory.GetType().GetProperty("ShouldPersist");
            if (persistProperty != null)
            {
                bool shouldPersist = (bool)persistProperty.GetValue(currentInventory);
                if (shouldPersist)
                {
                    SaveInventory($"Inventory_{StageManager.Instance.GetCurrentStage()?.GetType().Name}");
                }
            }
        }

        public void DeleteInventorySave(string saveKey)
        {
            string filename = $"{saveKey}.json";
            if (JsonDataHelper.FileExists(filename))
            {
                JsonDataHelper.DeleteData(filename);
                Debug.Log($"[InventoryManager] Deleted save: {saveKey}");
            }
        }

        /// 게임 전환 (인벤토리 저장 후 새 게임 설정 적용)
        public void SwitchGame(string fromGameId, string toGameId, Framework.IGameInventoryConfig newConfig)
        {
            if (newConfig == null)
            {
                Debug.LogError("[InventoryManager] Cannot switch game: newConfig is null");
                return;
            }

            // 1. 현재 게임의 인벤토리 저장
            if (currentInventory != null && !string.IsNullOrEmpty(fromGameId))
                SaveInventory($"Inventory_{fromGameId}");

            // 2. 현재 인벤토리 초기화
            currentInventory = null;

            // 3. 새 게임 설정 적용
            Initialize(newConfig);

            // 4. 새 게임의 인벤토리 로드 (존재하는 경우)
            if (!string.IsNullOrEmpty(toGameId))
            {
                string saveKey = $"Inventory_{toGameId}";
                if (JsonDataHelper.FileExists($"{saveKey}.json"))
                {
                    Debug.Log($"[InventoryManager] Found existing save for game: {toGameId}");
                }
                else
                {
                    Debug.Log($"[InventoryManager] No existing save for game: {toGameId}. Will start fresh.");
                }
            }

            Debug.Log($"[InventoryManager] Game switched: {fromGameId} → {toGameId}");
            Debug.Log($"[InventoryManager] Global currencies preserved: Diamond={GetGlobalCurrency("Diamond")}, AccountExp={GetGlobalCurrency("AccountExp")}");
        }

        /// 게임 ID로 설정 조회 (확장 가능하도록 설계)
        public Framework.IGameInventoryConfig GetConfigForGame(string gameId)
        {
            switch (gameId?.ToUpper())
            {
                case "RL":
                case "ROGUELIKE":
                    return new Framework.RoguelikeInventoryConfig();

                case "ST":
                case "SHOOTING":
                    return new Framework.ShootingInventoryConfig();

                case "ES":
                case "EXTRACTIONSHOOTER":
                    return new Framework.ExtractionShooterInventoryConfig();

                case "PCR":
                case "PRODUCTION":
                    return new Framework.ProductionInventoryConfig();

                default:
                    Debug.LogWarning($"[InventoryManager] Unknown game ID: {gameId}. Returning null.");
                    return null;
            }
        }

        /// 게임 전환 (게임 ID만으로 편리하게 호출)
        public void SwitchGame(string fromGameId, string toGameId)
        {
            var newConfig = GetConfigForGame(toGameId);
            if (newConfig != null)
            {
                SwitchGame(fromGameId, toGameId, newConfig);
            }
        }

        /// 현재 게임 ID 저장 (외부에서 관리 가능하도록)
        private string currentGameId;

        public string GetCurrentGameId() => currentGameId;

        public void SetCurrentGameId(string gameId)
        {
            currentGameId = gameId;
            Debug.Log($"[InventoryManager] Current game ID set to: {gameId}");
        }

        /// 인벤토리 등록 시 자동으로 저장된 데이터 로드 시도
        public void RegisterInventoryAndLoad(MonoBehaviour inventory, string gameId)
        {
            RegisterInventory(inventory);
            SetCurrentGameId(gameId);

            string saveKey = $"Inventory_{gameId}";
            if (JsonDataHelper.FileExists($"{saveKey}.json"))
            {
                LoadInventory(saveKey);
            }
        }

        public void BroadcastItemAdded(IInventoryItemable item, int amount)
        {
            OnAnyItemAdded?.Invoke(item, amount);
            OnAnyInventoryUpdated?.Invoke();
        }

        public void BroadcastItemRemoved(IInventoryItemable item, int amount)
        {
            OnAnyItemRemoved?.Invoke(item, amount);
            OnAnyInventoryUpdated?.Invoke();
        }

        /// 공용 재화 추가
        public bool AddGlobalCurrency(string currencyName, long amount)
        {
            if (string.IsNullOrEmpty(currencyName) || amount <= 0)
            {
                Debug.LogWarning("[InventoryManager] Invalid currency name or amount");
                return false;
            }

            if (!globalCurrencies.ContainsKey(currencyName))
            {
                Debug.LogWarning($"[InventoryManager] Global currency '{currencyName}' not defined");
                return false;
            }

            globalCurrencies[currencyName] += amount;
            OnGlobalCurrencyChanged?.Invoke(currencyName, globalCurrencies[currencyName]);

            Debug.Log($"[InventoryManager] Added {amount} {currencyName}. Total: {globalCurrencies[currencyName]}");
            return true;
        }

        /// 공용 재화 제거
        public bool RemoveGlobalCurrency(string currencyName, long amount)
        {
            if (string.IsNullOrEmpty(currencyName) || amount <= 0)
            {
                Debug.LogWarning("[InventoryManager] Invalid currency name or amount");
                return false;
            }

            if (!globalCurrencies.ContainsKey(currencyName))
            {
                Debug.LogWarning($"[InventoryManager] Global currency '{currencyName}' not defined");
                return false;
            }

            if (globalCurrencies[currencyName] < amount)
            {
                Debug.LogWarning($"[InventoryManager] Not enough {currencyName}. Have: {globalCurrencies[currencyName]}, Need: {amount}");
                return false;
            }

            globalCurrencies[currencyName] -= amount;
            OnGlobalCurrencyChanged?.Invoke(currencyName, globalCurrencies[currencyName]);

            Debug.Log($"[InventoryManager] Removed {amount} {currencyName}. Remaining: {globalCurrencies[currencyName]}");
            return true;
        }

        /// 공용 재화 조회
        public long GetGlobalCurrency(string currencyName)
        {
            if (globalCurrencies != null && globalCurrencies.ContainsKey(currencyName))
            {
                return globalCurrencies[currencyName];
            }
            return 0;
        }

        /// 공용 재화 보유 확인
        public bool HasGlobalCurrency(string currencyName, long amount)
        {
            return GetGlobalCurrency(currencyName) >= amount;
        }

        /// 공용 재화 설정
        public void SetGlobalCurrency(string currencyName, long amount)
        {
            if (!globalCurrencies.ContainsKey(currencyName))
            {
                Debug.LogWarning($"[InventoryManager] Global currency '{currencyName}' not defined");
                return;
            }

            globalCurrencies[currencyName] = Math.Max(0L, amount);
            OnGlobalCurrencyChanged?.Invoke(currencyName, globalCurrencies[currencyName]);

            Debug.Log($"[InventoryManager] Set {currencyName} to {globalCurrencies[currencyName]}");
        }

        /// 모든 공용 재화 조회
        public Dictionary<string, long> GetAllGlobalCurrencies()
        {
            return new Dictionary<string, long>(globalCurrencies);
        }
    }
}

