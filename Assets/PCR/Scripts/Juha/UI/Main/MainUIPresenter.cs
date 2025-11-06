using System;
using UnityEngine;

public class MainUIPresenter
{
    private IMainUIView view;
    private MainUIModel model;
    private SelectConstructUIPresenter selectConstructPresenter;

    public void InitPresenter(IMainUIView view, MainUIModel model, SelectConstructUIPresenter presenter)
    {
        this.view = view;
        this.model = model;
        this.selectConstructPresenter = presenter;

        // 초기화 작업
        view.OnClickDig += HandleDigClick;
        view.OnClickConstruct += HandleConstructClick;
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
}
