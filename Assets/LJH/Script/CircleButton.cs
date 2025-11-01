using UnityEngine;
using UnityEngine.UI;

public class CircleButton : MonoBehaviour
{
    [HideInInspector]
    public Button button;
    [HideInInspector]
    public GameObject maskObject;
    [HideInInspector]
    public Image buttonImage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        if(button == null && maskObject == null && buttonImage == null)
        {
            button = this.GetComponent<Button>();
            maskObject = this.transform.Find("MaskObject").gameObject;
            //buttonImage = maskObject.transform.gameObject.GetComponentInChildren<Image>();
            buttonImage = maskObject.transform.Find("BtnImage").GetComponent<Image>();
        }
        
    }

    public void ManualAwkae()
    {
        button = this.GetComponent<Button>();
        maskObject = this.transform.Find("MaskObject").gameObject;
        //buttonImage = maskObject.transform.gameObject.GetComponentInChildren<Image>();
        buttonImage = maskObject.transform.Find("BtnImage").GetComponent<Image>();
    }
    void Start()
    {
        

    }
}
