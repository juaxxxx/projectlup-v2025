using System;
using UnityEngine;

namespace LUP.PCR
{
    public class PCRUICenter : MonoBehaviour
    {
        [Header("View")]
        [SerializeField]
        private MainUIView mainView;
        [SerializeField]
        private SelectConstructUIView selectConstructView;
        [SerializeField]
        private FarmTaskUIView farmTaskView;
        [SerializeField]
        private ConstructionDecisionView constructionDecisionView;
        [SerializeField]
        private InventoryUIView inventoryView;


        private TaskController taskController;

        private MainUIPresenter mainPresenter;
        private SelectConstructUIPresenter selectConstructPresenter;
        private FarmTaskUIPresenter farmTaskPresenter;
        private ConstructionDecisionPresenter constructionDecisionPresenter;
        private InventoryUIPresenter inventoryPresenter;

        private ActiveUIType uiType;
        private BuildingBase currBuilding;

        private void Start()
        {
            uiType = ActiveUIType.None;
            currBuilding = null;
        }

        private void Update()
        {
            switch(uiType)
            {
                case ActiveUIType.Main:
                    mainPresenter.UpdateResourceText();
                    break;
                case ActiveUIType.ProductableBuilding:
                    if (currBuilding != null)
                    {
                        ProductableBuilding building = currBuilding as ProductableBuilding;

                        if (building)
                        {
                            farmTaskPresenter.UpdateUI(building);
                        }
                    }
                    break;
                case ActiveUIType.SelectConstrcut:

                    break;
                case ActiveUIType.ConstructionDecision:

                    break;
                case ActiveUIType.Inventory:
                    inventoryPresenter.UpdateInventory();
                    break;

            }
        }

        public void InitUI(TaskController controller, PCRResourceCenter resourceCenter)
        {
            taskController = controller;

            if (!taskController)
            {
                Debug.Log("Task Controller is null in UICenter");
                return;
            }

            mainPresenter = new MainUIPresenter();
            selectConstructPresenter = new SelectConstructUIPresenter();
            farmTaskPresenter = new FarmTaskUIPresenter();
            constructionDecisionPresenter = new ConstructionDecisionPresenter();
            inventoryPresenter = new InventoryUIPresenter();

            mainPresenter.InitPresenter(mainView, new MainUIModel(), selectConstructPresenter, resourceCenter);
            selectConstructPresenter.InitPresenter(selectConstructView, new SelectConstructUIModel(), mainPresenter, constructionDecisionPresenter);
            farmTaskPresenter.InitPresenter(farmTaskView, new FarmTaskUIModel(), mainPresenter);
            constructionDecisionPresenter.InitPresenter(constructionDecisionView, new ConstructionDecisionModel(), mainPresenter);
            inventoryPresenter.InitPresenter(inventoryView, new InventoryUIModel(), mainPresenter, resourceCenter);

            uiType = ActiveUIType.Main;
            mainPresenter.Show();
            selectConstructPresenter.Hide();
            farmTaskPresenter.Hide();
            constructionDecisionPresenter.Hide();
            inventoryPresenter.Hide();

            // Bind
            mainPresenter.BindActionDig(taskController.DigWallTask);
            mainPresenter.BindActionConstruct(taskController.SetIdleActiveTrue);
            mainPresenter.BindActionInventory(OpenInventoryTask);

            farmTaskPresenter.BindActionBack(ReturnToMainScreen);
            inventoryPresenter.BindActionBack(ReturnToMainScreen);

            selectConstructPresenter.BindActionBuildingType(taskController.SetCurrSelectedBuildingType);
            selectConstructPresenter.BindActionBack(taskController.IdleTask);
            selectConstructPresenter.BindActionSelectedBuilding(taskController.BuildingTask);

            constructionDecisionPresenter.BindActionReject(taskController.IdleTask);
            constructionDecisionPresenter.BindActionAccept(taskController.IdleTask);
            constructionDecisionPresenter.BindActionAccept(taskController.CreateBuilding);


            Debug.Log("UICenter Init");
        }

        // 건설 건물 선택 시 공통 이벤트
        //uiCenter.mainMenuUI.OnContructBuildingTypeButtonClick += BuildingTask;
        // 클릭 건물 버튼 
        //uiCenter.mainMenuUI.OnBuildingTypeChanged += SetBuildingType;

        public void ReturnToMainScreen()
        {
            // 메인화면을 제외한 나머지 Hide
            uiType = ActiveUIType.Main;

            mainPresenter.Show();
            selectConstructPresenter.Hide();
        }

        public void OpenProductableTask(ProductableBuilding building)
        {
            //BuildingWheatFarm wheatFarm = building as BuildingWheatFarm;
            //if (wheatFarm)
            //{
            //    farmTaskPresenter.UpdateUI(building);
            //    farmTaskPresenter.Show();
            //    mainPresenter.Hide();
            //}
            currBuilding = building;

            farmTaskPresenter.UpdateUI(building);
            farmTaskPresenter.Show();
            mainPresenter.Hide();

            uiType = ActiveUIType.ProductableBuilding;
        }

        public void OpenRestaurantTask(BuildingRestaurant building)
        {
            currBuilding = building;

            // 추가 구현.            
        }

        public void OpenInventoryTask()
        {
            inventoryPresenter.UpdateInventory();
            inventoryPresenter.Show();
            mainPresenter.Hide();

            uiType = ActiveUIType.Inventory;
        }
    }

}
