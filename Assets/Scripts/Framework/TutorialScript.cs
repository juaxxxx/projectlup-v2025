using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour, IPointerClickHandler
{
    public int textIndex=0;
    public Text text;
    public TutorialStaticDataLoader tutorialStaticData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tutorialStaticData = (TutorialStaticDataLoader)LUP.ResourceManager.Instance.LoadStaticData(LUP.Define.StageKind.Tutorial , 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (textIndex >= tutorialStaticData.DataList.Count)
        {
            Destroy(gameObject);
        }
        else
        {
            text.text = tutorialStaticData.DataList[textIndex].description;
        }
        textIndex++;
    }
}
