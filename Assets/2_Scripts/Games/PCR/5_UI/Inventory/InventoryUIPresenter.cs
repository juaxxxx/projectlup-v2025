using UnityEngine;
using System;

namespace LUP.PCR
{
    public class InventoryUIPresenter
    {
        private IInventoryUIView view;
        private InventoryUIModel model;
        private MainUIPresenter mainPresenter;

        public void InitPresenter(IInventoryUIView view, InventoryUIModel model, MainUIPresenter mainPresenter, PCRResourceCenter resourceCenter)
        {
            this.view = view;
            this.model = model;
            this.mainPresenter = mainPresenter;
            model.InitModel(resourceCenter);

            view.OnClickBack += HandleBackClick;
        }

        private void HandleBackClick()
        {
            Hide();
            mainPresenter.Show();
        }

        public void BindActionBack(Action action)
        {
            view.OnClickBack += action;
        }


        public void Show()
        {
            view.Show();
        }

        public void Hide()
        {
            view.Hide();
        }

        public void UpdateInventory()
        {
            view.UpdateResourceText(model.GetResourceCenter());
        }
    }
}
