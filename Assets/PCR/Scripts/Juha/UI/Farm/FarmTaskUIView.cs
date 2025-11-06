using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FarmTaskUIView : MonoBehaviour, IFarmTaskUIView
{
    // Button
    [SerializeField]
    private Button taskBtn;
    [SerializeField]
    private Button turboBtn;
    [SerializeField]
    private Button workerBtn;
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


    private void Awake()
    {
        taskBtn?.onClick.AddListener(() => OnClickTask?.Invoke());
        turboBtn?.onClick.AddListener(() => OnClickTurbo?.Invoke());
        workerBtn?.onClick.AddListener(() => OnClickWorker?.Invoke());
        upgradeBtn?.onClick.AddListener(() => OnClickUpgrade?.Invoke());
        backBtn?.onClick.AddListener(() => OnClickBack?.Invoke());
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    // 늘어날 때마다 갱신
    public void UpdateUIStats(FarmUIData data)
    {
        buildingNameText.SetText(data.buildingName);
        productionTimeText.SetText("{0}", data.productionTime);
        powerText.SetText("{0}", data.power);
    }
}
