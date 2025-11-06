using System;
using UnityEngine;

public class PCRUICenter : MonoBehaviour
{
    [Header("View")]
    [SerializeField]
    private MainUIView mainView;
    [SerializeField]
    private SelectConstructUIView selectConstructView;
    [SerializeField]
    private FarmTaskUIView farmTaskView;

    private TaskController taskController;

    // 아마 Presenter를 통해 UI 기능을 호출하는 방식으로 만들건데..
    // 이게 맞는지는 하면서 파악하기
    private MainUIPresenter mainPresenter;
    private SelectConstructUIPresenter selectConstructPresenter;
    private FarmTaskUIPresenter farmTaskPresenter;

    public void InitUI(TaskController controller)
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

        mainPresenter.InitPresenter(mainView, new MainUIModel(), selectConstructPresenter);
        selectConstructPresenter.InitPresenter(selectConstructView, new SelectConstructUIModel(), mainPresenter);
        farmTaskPresenter.InitPresenter(farmTaskView, new FarmTaskUIModel(), mainPresenter);

        mainPresenter.Show();
        selectConstructPresenter.Hide();
        farmTaskPresenter.Hide();


        // Bind
        mainPresenter.BindActionDig(taskController.DigWallTask);
        mainPresenter.BindActionConstruct(taskController.SetIdleActiveTrue);


        selectConstructPresenter.BindActionBuildingType(taskController.SetBuildingType);
        selectConstructPresenter.BindActionBack(taskController.IdleTask);
        selectConstructPresenter.BindActionSelectedBuilding(taskController.BuildingTask);

    }
    
    // 건설 건물 선택 시 공통 이벤트
    //uiCenter.mainMenuUI.OnContructBuildingTypeButtonClick += BuildingTask;
    // 클릭 건물 버튼 
    //uiCenter.mainMenuUI.OnBuildingTypeChanged += SetBuildingType;

    public void ReturnToMainScreen()
    {
        // 메인화면을 제외한 나머지 Hide

        mainPresenter.Show();
        selectConstructPresenter.Hide();
    }

    public void OpenProductableTask(ProductableBuilding building)
    {
        BuildingWheatFarm wheatFarm = building as BuildingWheatFarm;
        if (wheatFarm)
        {
            farmTaskPresenter.UpdateUI(building);
            farmTaskPresenter.Show();
            mainPresenter.Hide();
        }
    }
}
