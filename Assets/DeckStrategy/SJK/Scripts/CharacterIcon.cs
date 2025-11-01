using System;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Utils.Enums;

public class CharacterIcon : MonoBehaviour
{
    [SerializeField]
    private Image portrait;
    [SerializeField]
    private Image attributeIcon;
    [SerializeField]
    private TextMeshProUGUI level;

    public SelectedButton selectedButton;

    public OwnedCharacterInfo characterInfo;

    private event Action<OwnedCharacterInfo, SelectedButton> OnSelected;
    private event Action<int, SelectedButton> OnDeselected;

    private void Awake()
    {
        FormationSystem formationSystem = FindAnyObjectByType<FormationSystem>();
        OnSelected = formationSystem.PlaceCharacter;
        OnDeselected = formationSystem.ReleaseCharacter;
    }

    private void Start()
    {
        selectedButton.button.onClick.AddListener(OnButtonClicked);
    }

    public void SetData(CharacterData statusData, CharacterModelData modelData, int characterLevel)
    {
        level.text = "Lv." + characterLevel.ToString();

        portrait.color = modelData.material.color;

        if(statusData.type == EAttributeType.ROCK)
        {
            attributeIcon.color = Color.red;
        }
        else if(statusData.type == EAttributeType.SCISSORS)
        {
            attributeIcon.color = Color.green;
        }
        else
        {
            attributeIcon.color = Color.blue;
        }

        characterInfo.characterID = statusData.ID;
        characterInfo.characterModelID = modelData.ID;
        characterInfo.characterLevel = characterLevel;
    }

    public void OnButtonClicked()
    {
        if (selectedButton.isSelected)
        {
            OnDeselected?.Invoke(characterInfo.characterID, selectedButton);
        }
        else
        {
            OnSelected?.Invoke(characterInfo, selectedButton);
        }
    }
}
