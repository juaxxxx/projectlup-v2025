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

    private readonly List<Image> buttonImages = new List<Image>();

    void Start()
    {
        buttonImages.Clear();
        if (buttons == null) return;

        for (int i = 0; i < buttons.Count; i++)
        {
            Button button = buttons[i];
            if (button == null) continue;

            buttonImages.Add(button.GetComponent<Image>());

            Button capture = button;
            capture.onClick.AddListener(() => OnButtonClick(capture));
        }
    }

    void OnButtonClick(Button clickedButton)
    {
        for (int i = 0; i < buttonImages.Count; i++)
        {
            if (buttonImages[i] != null)
                buttonImages[i].color = normalColor;
        }

        if (clickedButton == null) return;
        Image img = clickedButton.GetComponent<Image>();
        if (img != null) img.color = selectedColor;

    }
}
