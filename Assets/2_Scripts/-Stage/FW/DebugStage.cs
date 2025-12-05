using UnityEngine;

namespace LUP
{
    public class DebugStage : BaseStage
    {
        //public Define.StageKind TargetStage = Define.StageKind.Main;

        [SerializeField] private Inventory testInventory;
        [SerializeField] private string saveFileName = "debug_inventory";
        IItemable gold;

        protected override void Awake()
        {
            base.Awake();
            StageKind = Define.StageKind.Debug;

            if (testInventory == null)

                testInventory = new Inventory();
        }

        void Start()
        {
            //testInventory.LoadInventory(saveFileName);
            //testInventory.AddItem(gold, 50);
        }

        void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    IItemable gold = ItemManager.Instance.GetItem("gold");
            //    testInventory.AddItem(gold, 50);

            //    testInventory.SaveInventory(saveFileName);
            //    Debug.Log($"[DebugStage] 인벤토리 세이브 완료: {saveFileName}");
            //}

            //// L키로 인벤토리 로드 (추가 기능)
            //if (Input.GetKeyDown(KeyCode.L))
            //{
            //    if (testInventory != null)
            //    {
            //        testInventory.LoadInventory(saveFileName);
            //        Debug.Log($"[DebugStage] 인벤토리 로드 완료: {saveFileName}");
            //    }
            //    else
            //    {
            //        Debug.LogWarning("[DebugStage] 로드할 인벤토리가 없습니다!");
            //    }
            //}
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


    }
}

