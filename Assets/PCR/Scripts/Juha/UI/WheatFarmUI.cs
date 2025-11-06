using UnityEngine;
using System;

public class WheatFarmUI : MonoBehaviour
{
    // 추후 농장UI 통합해서 쓸수도 있다.

    public event Action OnActiveWheatFarmUI;


    private void OnDisable()
    {
        
    }

    private void OnEnable()
    {
        

    }

    void Start()
    {
        CloseUI();
    }

    void Update()
    {
        
    }

    public void OpenUI()
    {
        gameObject.SetActive(true);
    }

    public void CloseUI()
    {
        gameObject.SetActive(false);
    }
}
