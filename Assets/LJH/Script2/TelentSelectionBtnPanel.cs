using UnityEngine;

public class TelentSelectionBtnPanel : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    TextImageBtn[] textImageBtns;
    private void Awake()
    {
        textImageBtns = GetComponentsInChildren<TextImageBtn>();
    }
    void Start()
    {
        for(int i = 0; i < textImageBtns.Length; i++)
        {
            int index = i;
            textImageBtns[i].Init();
            textImageBtns[i].button.onClick.AddListener(() => OnBtnClicked(index));
        }
    }

    void OnBtnClicked(int index)
    {
        for (int i = 0; i < textImageBtns.Length; i++)
        {
            textImageBtns[i].SetActive(false);
        }

        textImageBtns[index].SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
