using System;
using UnityEngine;

public class ConstructionDecisionPresenter
{
    private IConstructionDecisionView view;
    private ConstructionDecisionModel model;
    private MainUIPresenter mainPresenter;

    public void InitPresenter(IConstructionDecisionView view, ConstructionDecisionModel model, MainUIPresenter mainPresenter)
    {
        this.view = view;
        this.model = model;
        this.mainPresenter = mainPresenter;

        view.OnClickAccept += HandleStartConstruction;
        view.OnClickReject += HandleCancelConstruction;
    }

    private void HandleCancelConstruction()
    {
        view.Hide();
        mainPresenter.Show();
    }

    private void HandleStartConstruction()
    {
        view.Hide();
        mainPresenter.Show();
    }

    public void Show()
    {
        view.Show();
    }

    public void Hide()
    {
        view.Hide();
    }

    public void BindActionAccept(Action action)
    {
        view.OnClickAccept += action;
    }

    public void BindActionReject(Action action)
    {
        view.OnClickReject += action;
    }

}
