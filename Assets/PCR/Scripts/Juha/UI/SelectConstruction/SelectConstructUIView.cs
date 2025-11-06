using System;
using UnityEngine;
using UnityEngine.UI;

public class SelectConstructUIView : MonoBehaviour, ISelectConstructUIView
{
    // 지금은 건물 몇개만 테스트
    [SerializeField]
    private Button wheatFarmBtn;
    [SerializeField]
    private Button mushroomFarmBtn;


    // Back Button
    [SerializeField]
    private Button backBtn;

    public event Action OnClickSelectedBuilding;
    public event Action<BuildingType> OnBuildingTypeChanged;
    public event Action OnClickBack;

    private void Awake()
    {
        backBtn?.onClick.AddListener(() => OnClickBack?.Invoke());
        wheatFarmBtn?.onClick.AddListener(() => OnClickSelectedBuilding?.Invoke());
        mushroomFarmBtn?.onClick.AddListener(() => OnClickSelectedBuilding?.Invoke());


        wheatFarmBtn?.onClick.AddListener(() => OnBuildingTypeChanged?.Invoke(BuildingType.WHEATFARM));
        mushroomFarmBtn?.onClick.AddListener(() => OnBuildingTypeChanged?.Invoke(BuildingType.MUSHROOMFARM));
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
