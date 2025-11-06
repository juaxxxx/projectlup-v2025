using UnityEngine;
using System;
using UnityEngine.UI;

public class MainUIView : MonoBehaviour, IMainUIView
{
    // Button
    [SerializeField]
    private Button digBtn;
    [SerializeField]
    private Button constructBtn;

    // Event
    public event Action OnClickDig;
    public event Action OnClickConstruct;

    private void Awake()
    {
        digBtn?.onClick.AddListener(() => OnClickDig?.Invoke());
        constructBtn?.onClick.AddListener(() => OnClickConstruct?.Invoke());
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
