using Manager;
using UnityEngine;
using Framework;

namespace Manager
{
    public class DebugStage : BaseStage
    {
        //public Define.StageKind TargetStage = Define.StageKind.Main;

        private Framework.Inventory PlayerInventory;

        protected override void Awake()
        {
            base.Awake();
            StageKind = Define.StageKind.Debug;
        }

        void Start()
        {
            //StageManager.Instance.LoadStage(TargetStage);
        }
        protected override void LoadResources()
        {

        }

        protected override void GetDatas()
        {

        }

        protected override void SaveDatas()
        {
            
        }

        protected override void SetupInventory()
        {

            // 인벤토리 생성
            PlayerInventory = gameObject.AddComponent<Framework.Inventory>();

            // InventoryManager에 등록 (자동으로 저장된 데이터 로드)
            InventoryManager.Instance.RegisterInventoryAndLoad(PlayerInventory, Define.StageKind.Debug);

            // 테스트 실행
            TestInventorySystem();
        }

        /// <summary>
        /// 인벤토리 시스템 테스트
        /// </summary>
        private void TestInventorySystem()
        {
            Debug.Log("========== 인벤토리 시스템 테스트 시작 ==========");

            // 1. 재화 테스트
            TestCurrency();

            // 2. 공용 재화 테스트
            TestGlobalCurrency();

            // 3. 아이템 테스트 (아이템 데이터가 있는 경우)
            TestItems();

            // 4. 장비 테스트 (아이템 데이터가 있는 경우)
            TestEquipment();

            Debug.Log("========== 인벤토리 시스템 테스트 완료 ==========");
        }

        private void TestCurrency()
        {
            Debug.Log("----- 재화 테스트 시작 -----");

            // 재화 추가
            PlayerInventory.AddCurrency("Gold", 1000);
            PlayerInventory.AddCurrency("Silver", 500);
            PlayerInventory.AddCurrency("TestCurrency", 100);

            // 재화 조회
            Debug.Log($"Gold: {PlayerInventory.GetCurrency("Gold")}");
            Debug.Log($"Silver: {PlayerInventory.GetCurrency("Silver")}");
            Debug.Log($"TestCurrency: {PlayerInventory.GetCurrency("TestCurrency")}");

            // 재화 사용
            if (PlayerInventory.RemoveCurrency("Gold", 100))
            {
                Debug.Log("Gold 100 사용 성공!");
            }

            // 재화 확인
            if (PlayerInventory.HasCurrency("Silver", 500))
            {
                Debug.Log("Silver 500 이상 보유 확인!");
            }

            Debug.Log("----- 재화 테스트 완료 -----");
        }

        private void TestGlobalCurrency()
        {
            Debug.Log("----- 공용 재화 테스트 시작 -----");

            // 공용 재화 추가
            InventoryManager.Instance.AddGlobalCurrency("Diamond", 50);
            InventoryManager.Instance.AddGlobalCurrency("AccountExp", 200);

            // 공용 재화 조회
            Debug.Log($"Diamond: {InventoryManager.Instance.GetGlobalCurrency("Diamond")}");
            Debug.Log($"AccountExp: {InventoryManager.Instance.GetGlobalCurrency("AccountExp")}");

            // 공용 재화 사용
            if (InventoryManager.Instance.RemoveGlobalCurrency("Diamond", 10))
            {
                Debug.Log("Diamond 10 사용 성공!");
            }

            Debug.Log("----- 공용 재화 테스트 완료 -----");
        }

        private void TestItems()
        {
            Debug.Log("----- 아이템 테스트 시작 -----");

            // 아이템 데이터베이스에 아이템이 있는지 확인
            var allItems = InventoryManager.Instance.GetAllItems();

            if (allItems.Count > 0)
            {
                // 첫 번째 아이템으로 테스트
                var testItem = allItems[0];
                Debug.Log($"테스트 아이템: {testItem.ItemName} (ID: {testItem.ItemID})");

                // 아이템 추가
                PlayerInventory.AddItem(testItem, 5);
                Debug.Log($"{testItem.ItemName} 5개 추가");

                // 아이템 개수 확인
                int count = PlayerInventory.GetItemCount(testItem.ItemID);
                Debug.Log($"{testItem.ItemName} 보유 개수: {count}");

                // 아이템 제거
                PlayerInventory.RemoveItem(testItem.ItemID, 2);
                Debug.Log($"{testItem.ItemName} 2개 제거");

                // 최종 개수 확인
                count = PlayerInventory.GetItemCount(testItem.ItemID);
                Debug.Log($"{testItem.ItemName} 최종 개수: {count}");
            }
            else
            {
                Debug.LogWarning("⚠️ 아이템 데이터베이스가 비어있습니다. Resources/Items 폴더에 아이템 데이터를 추가하세요.");
            }

            Debug.Log("----- 아이템 테스트 완료 -----");
        }

        private void TestEquipment()
        {
            Debug.Log("----- 장비 테스트 시작 -----");

            var allItems = InventoryManager.Instance.GetAllItems();

            if (allItems.Count > 0)
            {
                var testEquipment = allItems[0];
                Debug.Log($"테스트 장비: {testEquipment.ItemName} (ID: {testEquipment.ItemID})");

                // 장비 장착
                if (PlayerInventory.EquipItem("Weapon", testEquipment))
                {
                    Debug.Log($"{testEquipment.ItemName}을(를) Weapon 슬롯에 장착!");
                }

                // 장착된 장비 확인
                var equipped = PlayerInventory.GetEquippedItem("Weapon");
                if (equipped != null)
                {
                    Debug.Log($"Weapon 슬롯: {equipped.ItemName}");
                }

                // 장비 해제
                var unequipped = PlayerInventory.UnequipItem("Weapon");
                if (unequipped != null)
                {
                    Debug.Log($"{unequipped.ToString()}을(를) 해제!");
                }
            }
            else
            {
                Debug.LogWarning("⚠️ 아이템 데이터베이스가 비어있습니다.");
            }

            Debug.Log("----- 장비 테스트 완료 -----");
        }
    }
}

