using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MVPDisplay : MonoBehaviour
{
    private DataCenter dataCenter;

    void Start()
    {
        dataCenter = FindFirstObjectByType<DataCenter>();
        if (dataCenter == null || dataCenter.mvpData == null)
            return;

        var mvp = dataCenter.mvpData;

        SetSlot("MVP1", "Image", "Text_Name", "Score", "Text_Score", mvp.char1Name, mvp.char1Color, mvp.char1Score, mvp.char1Score);
        SetSlot("MVP2", "Image", "Text_Name", "Score", "Text_Score", mvp.char2Name, mvp.char2Color, mvp.char2Score, mvp.char1Score);
        SetSlot("MVP3", "Image", "Text_Name", "Score", "Text_Score", mvp.char3Name, mvp.char3Color, mvp.char3Score, mvp.char1Score);
        SetSlot("MVP4", "Image", "Text_Name", "Score", "Text_Score", mvp.char4Name, mvp.char4Color, mvp.char4Score, mvp.char1Score);
        SetSlot("MVP5", "Image", "Text_Name", "Score", "Text_Score", mvp.char5Name, mvp.char5Color, mvp.char5Score, mvp.char1Score);

        SetMainMVP("MVP", "Image", "Text_Name", mvp.char1Name, mvp.char1Color);
    }

    private void SetSlot(string slotName, string imageName, string textName, string sliderName, string scoreTextName, string charName
        , Color color, float score, float maxScore)
    {
        Transform slot = transform.Find(slotName);
        if (slot == null)
        {
            return;
        }

        Image charcterImage = slot.Find(imageName)?.GetComponent<Image>();
        TMP_Text characterName = slot.Find(textName)?.GetComponent<TMP_Text>();
        Slider characterScore = slot.Find(sliderName)?.GetComponent<Slider>();
        TMP_Text scoreText = slot.Find(scoreTextName)?.GetComponent<TMP_Text>();

        if (charcterImage != null)
        {
            charcterImage.color = color;
        }
        if (characterName != null)
        {
            characterName.text = string.IsNullOrEmpty(charName) ? "-" : charName;
        }
        float ratio = (maxScore > 0) ? (score / maxScore) : 0;
        if (characterScore != null)
        {
            characterScore.value = ratio;
        }

        if (scoreText != null)
        {
            scoreText.text = $"{score:F0}";
        }
            
    }

    private void SetMainMVP(string slotName, string imageName, string textName, string charName, Color color)
    {
        Transform slot = transform.Find(slotName);
        if (slot == null)
        {
            return;
        }

        Image charcterImage = slot.Find(imageName)?.GetComponent<Image>();
        TMP_Text characterName = slot.Find(textName)?.GetComponent<TMP_Text>();

        if (charcterImage != null)
        {
            charcterImage.color = color;
        }
        if (characterName != null)
        {
            characterName.text = string.IsNullOrEmpty(charName) ? "-" : charName;
        }
    }
}

