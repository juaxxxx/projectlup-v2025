using UnityEngine;
using System;

public class MainMenuUI : MonoBehaviour
{
    public event Action OnDigButtonClick;
    public event Action OnConstructButtonClick;
    public event Action OnMainMenuCancelButtonClick;
    public event Action OnContructBuildingTypeButtonClick;
    public event Action<BuildingType> OnBuildingTypeChanged;

    // OnMainMenuCancelButtonClick 고민
    [SerializeField]
    private GameObject taskPanel;
    [SerializeField]
    private GameObject constructPanel;

    void Start()
    {
        InitMenu();

        OnDigButtonClick += CloseTaskUI;
        OnConstructButtonClick += CloseTaskUI;
        OnConstructButtonClick += OpenConstructUI;
        OnMainMenuCancelButtonClick += CloseConstructUI;
        OnMainMenuCancelButtonClick += OpenTaskUI;

        // 건설 건물 선택
        OnContructBuildingTypeButtonClick += CloseTaskUI;
        OnContructBuildingTypeButtonClick += CloseConstructUI;
    }

    public void InitMenu()
    {
        constructPanel.SetActive(false);
        taskPanel.SetActive(true);
    }

    public void DigButtonClick()
    {
        OnDigButtonClick?.Invoke();
    }

    public void ConstructButtonClick()
    {
        OnConstructButtonClick?.Invoke();
    }

    public void MainMenuCancelButtonClick()
    {
        OnMainMenuCancelButtonClick?.Invoke();
    }

    public void ContructBuildingTypeButtonClick()
    {
        OnContructBuildingTypeButtonClick?.Invoke();
    }

    public void OpenConstructUI()
    {
        constructPanel.SetActive(true);
    }
    public void OpenTaskUI()
    {
        taskPanel.SetActive(true);
    }
    public void CloseConstructUI()
    {
        constructPanel.SetActive(false);
    }
    public void CloseTaskUI()
    {
        taskPanel.SetActive(false);
    }

    public void CloseAllMainTaskUI()
    {
        constructPanel.SetActive(false);
        taskPanel.SetActive(false);
    }

    public void ClickWheatFarm()
    {
        OnBuildingTypeChanged.Invoke(BuildingType.WHEATFARM);
        OnContructBuildingTypeButtonClick?.Invoke();
    }
    public void ClickMushroomFarm()
    {
        OnBuildingTypeChanged.Invoke(BuildingType.MUSHROOMFARM);
        OnContructBuildingTypeButtonClick?.Invoke();
    }

}
