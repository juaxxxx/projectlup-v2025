using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameStartToggleSwitch : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Button togleBtn;
    private bool bIsToggled = false;

    private RectTransform myrect;
    public RectTransform iconRect;

    private void Awake()
    {
        togleBtn = GetComponent<Button>();
        myrect = GetComponent<RectTransform>();

    }
    void Start()
    {
        togleBtn.onClick.AddListener(() => OnBtnClicked());
    }

    void OnBtnClicked()
    {
        bIsToggled = !bIsToggled;

        if (bIsToggled)
        {
            iconRect.anchorMin = new Vector2(0, 0);
            iconRect.anchorMax = new Vector2(1, 0.5f);

            iconRect.offsetMin = Vector2.zero;
            iconRect.offsetMax = Vector2.zero;

        }

        else
        {
            iconRect.anchorMin = new Vector2(0, 0.5f);
            iconRect.anchorMax = new Vector2(1, 1f);

            iconRect.offsetMin = Vector2.zero;
            iconRect.offsetMax = Vector2.zero;

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
