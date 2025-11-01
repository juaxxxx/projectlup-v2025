using System;
using UnityEngine;
using UnityEngine.UI;

public class CharacterInfoController : MonoBehaviour
{
    [Header("로비에서 보여질 캐릭터들")]
    public GameObject[] characters;

    [Header("로비 관련")]
    public CharacterSelector characterSelector;

    [Header("UI 패널")]
    public GameObject characterSelectPanel;
    public GameObject[] characterInfoPanels;
    public GameObject lobbyPanel;

    private int currentIndex = -1;


    void Start()
    {
        currentIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);
    }

    public void OnSelectButtonClicked(int index)
    {

        if (index < 0 || index >= characters.Length) return;

        currentIndex = index;

        PlayerPrefs.SetInt("SelectedCharacterIndex", currentIndex);

        for (int i = 0; i < characters.Length; i++)
            characters[i].SetActive(i == currentIndex);

        characterInfoPanels[currentIndex].SetActive(false);
        characterSelectPanel.SetActive(false);
        lobbyPanel.SetActive(true);
    }
}