using UnityEngine;
using UnityEngine.UI;

using TMPro;

public class DisplayableImageBox : MonoBehaviour, IDisplayable
{
    private string imageName = null;
    private Sprite image;
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
        return image;
    }
    public void SetDisplayableImage(Sprite Inputimage)
    {
        image = Inputimage;

        Image img = gameObject.GetComponent<Image>();

        if (img != null)
            img.sprite = image;
    }

    public int GetExtraInfo() { return extraInfo; }
    public void SetExtraInfo(int Extrainfo) { extraInfo = Extrainfo; }


}
