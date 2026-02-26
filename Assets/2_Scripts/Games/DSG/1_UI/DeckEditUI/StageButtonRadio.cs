using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageButtonRadio : MonoBehaviour
{
    [SerializeField]
    private List<Button> buttons;
    [SerializeField]
    private Color selectedColor;
    [SerializeField]
    public Color normalColor;

    void Start()
    {
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => OnButtonClick(btn));
        }
    }

    void OnButtonClick(Button clickedButton)
    {
        foreach (Button btn in buttons)
        {
            btn.GetComponent<Image>().color = normalColor;
        }

        clickedButton.GetComponent<Image>().color = selectedColor;
    }
}
