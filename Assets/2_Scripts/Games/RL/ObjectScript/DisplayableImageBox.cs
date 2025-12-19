using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class DisplayableImageBox : MonoBehaviour, IDisplayable
{
    private string imageName = null;
    private Sprite sprite;
    private TMP_Text numText;
    public int extraInfo = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        numText = gameObject.GetComponentInChildren<TMP_Text>();

        if (numText == null)
            Debug.LogWarning("Cant find Item Image's Text");
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string GetDisplayableName()
    {
        return imageName;
    }
    public Sprite GetDisplayableImage()
    {
        return sprite;
    }
    public void SetDisplayableImage(Sprite Inputimage)
    {
        sprite = Inputimage;

        Image img = gameObject.GetComponentInChildren<Image>();

        if (img != null)
            img.sprite = sprite;
    }

    public void SetDisplayableImageBackGroundColor(Color color)
    {
        Image img = gameObject.GetComponent<Image>();
        if (img != null)
            img.color = color;
    }

    public void SetText(string Text)
    {
        numText.text = Text;
    }

    public int GetExtraInfo() { return extraInfo; }
    public void SetExtraInfo(int Extrainfo) { extraInfo = Extrainfo; }


}
