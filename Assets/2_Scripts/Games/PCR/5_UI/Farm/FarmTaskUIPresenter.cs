//using System;
//using UnityEngine;

//namespace LUP.PCR
//{
//    public class FarmTaskUIPresenter
//    {
//        private IFarmTaskUIView view;
//        private FarmTaskUIModel model;
//        private ProductableBuilding currBuilding;


//        private void HanldeWorkRequestToggle()
//        {
//            if(currBuilding == null)
//            {
//                return;
//            }
//            currBuilding.ToggleWorkRequest();

//            UpdateUI(currBuilding);
//        }

//        private void HandleUpgrade()
//        {
//            if (currBuilding == null)
//            {
//                return;
//            }

//            currBuilding.Upgrade();

//            UpdateUI(currBuilding);
//        }


//        public void BindActionBack(Action action)
//        {
//            view.OnClickBack += action;
//        }


//        public void Show()
//        {
//            view.Show();
//        }

//        public void Hide()
//        {
//            view.Hide();
//            currBuilding = null;
//        }

//        public void UpdateUI(ProductableBuilding building)
//        {
//            currBuilding = building;
//            if (currBuilding)
//            {
//                model.UpdateData(currBuilding);

//                UpdateUpgradeData(currBuilding);
//            }

//            UpdateUIData(model.uiData);
//        }
//        public void UpdateUIData(FarmUIData data)
//        {
//            view.UpdateUIStats(data);
//        }
//        private void UpdateUpgradeData(ProductableBuilding building)
//        {
//            ProductionStage stage = StageManager.Instance.GetCurrentStage() as ProductionStage;
//            if (stage == null) return;

//            int buildingType = (int)building.GetBuildingInfo().buildingType;
//            int currentLevel = building.GetBuildingInfo().level;
//            int nextLevel = currentLevel + 1;

//            // 다음 레벨 데이터 가져오기
//            PCRConstructionStaticData nextConstData = stage.GetCurrentConstructionData(buildingType, nextLevel);
//            PCRProductionStaticData nextProdData = stage.GetCurrentProductionData(buildingType, nextLevel);
//            PCRProductionStaticData currProdData = stage.GetCurrentProductionData(buildingType, currentLevel);

//            // 데이터가 없으면 만렙
//            if (nextConstData == null || nextProdData == null)
//            {
//                model.uiData.isMaxLevel = true;
//            }
//            else
//            {
//                model.uiData.isMaxLevel = false;

//                // 비용 설정
//                model.uiData.costType1 = nextConstData.resourceType1;
//                model.uiData.costAmount1 = nextConstData.amount1;
//                model.uiData.costType2 = nextConstData.resourceType2;
//                model.uiData.costAmount2 = nextConstData.amount2;

//                // 효과 설정 (건물 종류에 따라 보여줄 스탯 분기)
//                // WheatFarm(농장)이라면 '저장 용량'을 보여준다고 가정
//                if (building is BuildingWheatFarm)
//                {
//                    model.uiData.effectName = "저장 용량";
//                    model.uiData.currentStatValue = currProdData.StorageCapacity;
//                    model.uiData.nextStatAddedValue = nextProdData.StorageCapacity - currProdData.StorageCapacity;
//                }
//                else if (building is BuildingPowerStation) // 예: 발전소는 전력량
//                {
//                    model.uiData.effectName = "전력 생산";
//                    model.uiData.currentStatValue = (int)currProdData.productionPerHour; // 예시
//                    model.uiData.nextStatAddedValue = (int)(nextProdData.productionPerHour - currProdData.productionPerHour);
//                }
//                // ... 다른 건물 추가 가능
//            }
//        }

//        public void SelectTab(FarmUIBtnType tabType)
//        {
//            view.ChangeTab(tabType);
//        }


//    }

//}
