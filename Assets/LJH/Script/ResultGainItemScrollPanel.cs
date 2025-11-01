using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultGainItemScrollPanel : BaseScrollAblePanel
{

    new protected void Start()
    {
        InGameCenter gameCenter =  GameObject.FindFirstObjectByType<InGameCenter>();

        if(gameCenter.gameClear)
        {
            gameObject.SetActive(true);
        }
    }

    protected void OnEnable()
    {
        if(scrollRect == null)
        {
            scrollRect = GetComponentInChildren<ScrollRect>(true);


            if (scrollRect == null)
            {
                UnityEngine.Debug.LogError("Can't Load ScrollAblePanel !!", this.gameObject);
            }
        }
        
    }

    protected override void GenerateContent()
    {
        base.EraseContents();

        for (int i = 0; i < displayedData.Length; i++)
        {
            int index = i;

            GameObject Itembox = Instantiate(displayedPrefab, contentParent);
            DisplayableImageBox displayAbleItemBox = Itembox.GetComponent<DisplayableImageBox>();

            if (displayAbleItemBox == null)
            {
                UnityEngine.Debug.LogError("Cast ImageBox Fail!", this.gameObject);
                continue;
            }

            displayAbleItemBox.SetDisplayableImage(displayedData[index].GetDisplayableImage());

            Transform textTransform = Itembox.transform.Find("numText");

            string itemAmount = displayedData[index].GetExtraInfo().ToString();

            textTransform.GetComponent<TextMeshProUGUI>().SetText(itemAmount);
        }

        setconstraintCount();
        StartCoroutine(SetActivate());
    }

    IEnumerator SetActivate()
    {
        yield return null;

        gameObject.SetActive(true);
    }
}
