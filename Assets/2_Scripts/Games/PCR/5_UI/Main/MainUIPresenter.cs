using System;
using UnityEngine;

namespace LUP.PCR
{
    public class MainUIPresenter
    {
        private IMainUIView view;
        private MainUIModel model;
        private SelectConstructUIPresenter selectConstructPresenter;
        private FarmTaskUIPresenter farmTaskPresenter;
        //private InventoryUIPresenter inventoryPresenter;

        public void InitPresenter(IMainUIView view, MainUIModel model, SelectConstructUIPresenter presenter, PCRResourceCenter resourceCenter)
        {
            this.view = view;
            this.model = model;
            this.selectConstructPresenter = presenter;
            //this.inventoryPresenter = inventoryPresenter;

            // 초기화 작업
            view.OnClickDig += HandleDigClick;
            view.OnClickConstruct += HandleConstructClick;
            model.InitModel(resourceCenter);

            BuildingBase.OnGlobalUIRequest += HandleOpenBuildingUI;
        }

        public void HandleDigClick()
        {
            Hide();
        }

        public void HandleConstructClick()
        {
            Hide();
            selectConstructPresenter.Show();
        }

        public void Show()
        {
            view.Show();
        }

        public void Hide()
        {
            view.Hide();
        }

        public void BindActionDig(Action action)
        {
            view.OnClickDig += action;
        }

        public void BindActionConstruct(Action action)
        {
            view.OnClickConstruct += action;
        }

        public void BindActionInventory(Action action)
        {
            view.OnClickInventory += action;
        }

        public void Release()
        {
            BuildingBase.OnGlobalUIRequest -= HandleOpenBuildingUI;
        }
        private void HandleOpenBuildingUI(ProductableBuilding building, FarmUIBtnType initTab)
        {
            if (farmTaskPresenter == null)
            {
                return;
            }

            farmTaskPresenter.Show();
            farmTaskPresenter.UpdateUI(building);
            farmTaskPresenter.SelectTab(initTab);
        }

        public void UpdateResourceText()
        {
            view.UpdateResourceText(model.GetResourceCenter());
        }
    }
}
