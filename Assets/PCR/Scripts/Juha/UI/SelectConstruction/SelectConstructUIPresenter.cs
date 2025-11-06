using System;
using UnityEngine;

public class SelectConstructUIPresenter
{
    private ISelectConstructUIView view;
    private SelectConstructUIModel model;
    private MainUIPresenter mainPresenter;
    private ConstructionDecisionPresenter constructionDecisionPresenter;

    public void InitPresenter(ISelectConstructUIView view, SelectConstructUIModel model, MainUIPresenter mainPresenter
        , ConstructionDecisionPresenter constructionDecisionPresenter)
    {
        this.view = view;
        this.model = model;
        this.mainPresenter = mainPresenter;
        this.constructionDecisionPresenter = constructionDecisionPresenter;

        view.OnClickBack += HandleBackClick;
        view.OnClickSelectedBuilding += HandleSelectedBuildingClick;

    }
    private void HandleBackClick()
    {
        Hide();
        mainPresenter.Show();
    }

    private void HandleSelectedBuildingClick()
    {
        Hide();
        constructionDecisionPresenter.Show();
    }

    public void Show()
    {
        view.Show();
    }

    public void Hide()
    {
        view.Hide();
    }

    public void BindActionBuildingType(Action<BuildingType> action)
    {
        view.OnBuildingTypeChanged += action;
    }

    public void BindActionBack(Action action)
    {
        view.OnClickBack += action;
    }

    public void BindActionSelectedBuilding(Action action)
    {
        view.OnClickSelectedBuilding += action;
    }
}
