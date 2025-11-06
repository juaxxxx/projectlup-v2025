using System;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionDecisionView : MonoBehaviour, IConstructionDecisionView
{
    // Button
    [SerializeField]
    private Button acceptBtn;
    [SerializeField]
    private Button rejectBtn;

    // Event
    public event Action OnClickAccept;
    public event Action OnClickReject;
    
    private void Awake()
    {
        acceptBtn?.onClick.AddListener(() => OnClickAccept?.Invoke());
        rejectBtn?.onClick.AddListener(() => OnClickReject?.Invoke());
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
