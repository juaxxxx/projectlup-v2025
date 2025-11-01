using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPriveiwPanel : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject characterPreviewObject;
    public CharacterStatBox characterStatBox;

    private Image previewCharacterImage;
    private TextMeshProUGUI previewCharacaterNameText;

    private void Awake()
    {
        previewCharacterImage = characterPreviewObject.GetComponent<Image>();
        previewCharacaterNameText = characterPreviewObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetCharacterPreview(RLCharacterData characterData)
    {
        previewCharacterImage.sprite = characterData.GetDisplayableImage();
        previewCharacaterNameText.SetText(characterData.GetDisplayableName());

        characterStatBox.UpdateStatBox(characterData);
    }
}
