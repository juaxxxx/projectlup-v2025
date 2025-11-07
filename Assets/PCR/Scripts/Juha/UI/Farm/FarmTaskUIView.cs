using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum FarmUIBtnType
{
    Product,
    Fertilizer,
    Work,
    Upgrade
}

public class FarmTaskUIView : MonoBehaviour, IFarmTaskUIView
{
    // Button
    [SerializeField]
    private Button productionBtn;
    [SerializeField]
    private Button fertilizerBtn;
    [SerializeField]
    private Button workBtn;
    [SerializeField]
    private Button upgradeBtn;
    [SerializeField]
    private Button backBtn;

    //Text
    [SerializeField]
    TextMeshProUGUI buildingNameText;
    [SerializeField]
    TextMeshProUGUI productionTimeText;
    [SerializeField]
    TextMeshProUGUI powerText;
    
    // Event
    public event Action OnClickTask;
    public event Action OnClickTurbo;
    public event Action OnClickWorker;
    public event Action OnClickUpgrade;
    public event Action OnClickBack;
    public event Action<FarmUIBtnType> OnChangeTask;

    private void Awake()
    {
        productionBtn?.onClick.AddListener(() => OnClickTask?.Invoke());
        fertilizerBtn?.onClick.AddListener(() => OnClickTurbo?.Invoke());
        workBtn?.onClick.AddListener(() => OnClickWorker?.Invoke());
        upgradeBtn?.onClick.AddListener(() => OnClickUpgrade?.Invoke());
        backBtn?.onClick.AddListener(() => OnClickBack?.Invoke());

        productionBtn?.onClick.AddListener(() => OnChangeTask?.Invoke(FarmUIBtnType.Product));
        fertilizerBtn?.onClick.AddListener(() => OnChangeTask?.Invoke(FarmUIBtnType.Fertilizer));
        workBtn?.onClick.AddListener(() => OnChangeTask?.Invoke(FarmUIBtnType.Work));
        upgradeBtn?.onClick.AddListener(() => OnChangeTask?.Invoke(FarmUIBtnType.Upgrade));

        OnChangeTask += ChangeOptionBtn;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    private void ChangeOptionBtn(FarmUIBtnType type)
    {
        productionBtn.image.color = new Color(1f, 1f, 1f, 0f);
        fertilizerBtn.image.color = new Color(1f, 1f, 1f, 0f);
        workBtn.image.color = new Color(1f, 1f, 1f, 0f);
        upgradeBtn.image.color = new Color(1f, 1f, 1f, 0f);

        switch (type)
        {
            case FarmUIBtnType.Product:
                productionBtn.image.color = new Color(1f, 1f, 1f, 1f);
                break;
            case FarmUIBtnType.Fertilizer:
                fertilizerBtn.image.color = new Color(1f, 1f, 1f, 1f);

                break;
            case FarmUIBtnType.Work:
                workBtn.image.color = new Color(1f, 1f, 1f, 1f);

                break;
            case FarmUIBtnType.Upgrade:
                upgradeBtn.image.color = new Color(1f, 1f, 1f, 1f);

                break;
        }
    }

    // 늘어날 때마다 갱신
    public void UpdateUIStats(FarmUIData data)
    {
        buildingNameText.SetText(data.buildingName);
        productionTimeText.SetText("{0}", data.productionTime);
        powerText.SetText("{0}", data.power);
    }
}
