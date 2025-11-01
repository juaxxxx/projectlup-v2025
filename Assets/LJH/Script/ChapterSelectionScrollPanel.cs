
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ChapterSelectionScrollPanel : BaseScrollAblePanel
{
    public GameObject chapterSelectionInfo = null;

    private TextMeshProUGUI chapterInfoText = null;

    private float contetnWidth = -1;

    private List<Vector2> selectionButtonsBound = new List<Vector2>();

    private Vector2 minMaxButtonLR;
    private float contentSizeDelta;

    new private void Start()
    {
        base.Start();

        if(chapterSelectionInfo == null)
        {
            UnityEngine.Debug.LogError("Check ChapterSElectionPanel's SelectionInfo ");
            return;
        }

        chapterInfoText = chapterSelectionInfo.GetComponentInChildren<TextMeshProUGUI>(true);

    }

    protected override void GenerateContent()
    {

        base.EraseContents();

        for (int i = 0; i < displayedData.Length; i++)
        {
            int index = i;

            GameObject button = Instantiate(displayedPrefab, contentParent);

            DisplayableButton displayableDataButton = button.GetComponent<DisplayableButton>();

            if (displayableDataButton == null)
            {
                UnityEngine.Debug.LogError("Cast CustumBtn Fail!", this.gameObject);
                continue;
            }

            displayableDataButton.SetDisplayableImage(displayedData[index].GetDisplayableImage());
            displayableDataButton.BindingFunction(() => OnItemSelected(index));

        }

        //SetHolizonScrollOffset(displayOffset);
        //TODO
        //(일반 클릭에선 정상)
        //순회 후, Chapter Offset이 정상적으로 적용되지 않는 형상 발견. 타이밍 문제라곤 하는데 임시로 한 프레임 늦게 호출되게 하자
        StartCoroutine(SetHolizonScrollOffset(displayOffset));
    }

    //private void SetHolizonScrollOffset(int offset)
    //{
    //    float buttonSize = displayedPrefab.GetComponent<RectTransform>().rect.width;

    //    if (offset < 0 || offset >= displayedData.Length) return;

    //    RefreshPanel();

    //    float spacing = contentParent.GetComponent<HorizontalLayoutGroup>().spacing;
    //    float padding = contentParent.GetComponent<HorizontalLayoutGroup>().padding.left;

    //    float viewportWidth = scrollRect.viewport.rect.width;
    //    float contentWidth = contentParent.GetComponent<RectTransform>().rect.width;

    //    float buttonCenterX = offset * (buttonSize + spacing) + buttonSize / 2 + padding;

    //    float normalizedX = (buttonCenterX - viewportWidth / 2) / (contentWidth - viewportWidth);
    //    scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(normalizedX);
    //}

    IEnumerator SetHolizonScrollOffset(int offset)
    {

        var test = GetComponentInChildren<GridLayoutGroup>();

        yield return null;

        float buttonSize = displayedPrefab.GetComponent<RectTransform>().rect.width;

        //if (offset < 0 || offset >= displayedData.Length)

        RefreshPanel();

        float spacing = contentParent.GetComponent<HorizontalLayoutGroup>().spacing;
        float padding = contentParent.GetComponent<HorizontalLayoutGroup>().padding.left;

        float viewportWidth = scrollRect.viewport.rect.width;
        float contentWidth = contentParent.GetComponent<RectTransform>().rect.width;

        float buttonCenterX = offset * (buttonSize + spacing) + buttonSize / 2 + padding;

        float normalizedX = (buttonCenterX - viewportWidth / 2) / (contentWidth - viewportWidth);
        scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(normalizedX);
    }

    private void Update()
    {
        if(contetnWidth < 0)
        {
            RectTransform rectTransform = contentParent.gameObject.GetComponent<RectTransform>();
            rectTransform.pivot = new Vector2(0.0f, 0.5f);
            contetnWidth = rectTransform.rect.width;
            contentSizeDelta = rectTransform.sizeDelta.x;
        }

        if(selectionButtonsBound.Count == 0)
        {
            CarlkButtonsBounds();
        }


        if(contetnWidth > 0 && selectionButtonsBound.Count > 0)
        {
            //float currentPos = scrollRect.horizontalNormalizedPosition * contetnWidth;
            float currentPos = scrollRect.content.anchoredPosition.x;
            currentPos = PositionInterpolation(currentPos);
            int currentIndex = BinarySearchButtonIndex(currentPos);

            if(currentIndex == -1 )
            {
                chapterSelectionInfo.SetActive(false);
            }

            else
            {
                chapterSelectionInfo.SetActive(true);
                chapterInfoText.SetText(displayedData[currentIndex].GetDisplayableName());
            }
        }
        
    }

    private void CarlkButtonsBounds()
    {
        List<RectTransform> buttonPositions = new List<RectTransform>();
        RectTransform[] Transforms = contentParent.gameObject.GetComponentsInChildren<RectTransform>();
        for (int i = 0; i < Transforms.Length; i++)
        {
            if (Transforms[i].gameObject.GetComponent<DisplayableButton>())
            {
                buttonPositions.Add(Transforms[i]);
            }
        }

        float halfButtonWidth = displayedPrefab.GetComponent<RectTransform>().rect.size.x * 0.5f;

        for (int i = 0; i < buttonPositions.Count; i++)
        {
            float buttonPos = buttonPositions[i].localPosition.x;
            Vector2 bound = new Vector2(buttonPos - halfButtonWidth, buttonPos + halfButtonWidth);
            selectionButtonsBound.Add(bound);
        }

        minMaxButtonLR = new Vector2
            (selectionButtonsBound[0].x + halfButtonWidth,
             selectionButtonsBound[selectionButtonsBound.Count - 1].x + halfButtonWidth);
    }

    int BinarySearchButtonIndex(float pos)
    {
        int left = 0;
        int right = selectionButtonsBound.Count - 1;

        while (left <= right)
        {
            int mid = (left + right) / 2;
            Vector2 bound = selectionButtonsBound[mid];

            if (pos < bound.x)
                right = mid - 1;
            else if (pos > bound.y)
                left = mid + 1;
            else
                return mid;
        }

        return -1;
    }

    float PositionInterpolation(float currentPos)
    {
        float startPos = 0f;
        float endPos = -contentSizeDelta;
        float outputMin = minMaxButtonLR.x;
        float outputMax = minMaxButtonLR.y;

        float t = Mathf.InverseLerp(startPos, endPos, currentPos);

        return Mathf.Lerp(outputMin, outputMax, t);
    }

}
