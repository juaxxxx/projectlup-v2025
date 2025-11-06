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

        view.OnClickReject += HandleCancelConstruct;
    }

    private void HandleCancelConstruct()
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

    public void BindActionReject(Action action)
    {
        view.OnClickReject += action;
    }

}
