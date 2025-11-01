using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextImageBtn : MonoBehaviour , IPanelContentAble
{
    [HideInInspector]
    public TextMeshProUGUI btnText;
    [HideInInspector]
    public Image btnBackGroundImage;
    [HideInInspector]
    public Image btnIcon;
    [HideInInspector]
    public Button button;

    public Color btnActiveColor;
    public Color btnDeActiveColor;
    public bool Init()
    {
        btnBackGroundImage = gameObject.GetComponent<Image>();
        btnIcon = gameObject.GetComponentInChildren<Image>();
        btnText = gameObject.GetComponentInChildren<TextMeshProUGUI>();

        button = gameObject.GetComponent<Button>();

        if((btnBackGroundImage == null &&  
            btnText == null&& 
            button == null))
        {
            return false;
        }
        btnBackGroundImage.color = btnDeActiveColor;

        return true;
    }

    public void SetActive(bool activate)
    {
        if(activate)
        {
            btnBackGroundImage.color = btnActiveColor;
        }

        else
        {
            btnBackGroundImage.color = btnDeActiveColor;
        }
    }

    void Update()
    {
        
    }
}
