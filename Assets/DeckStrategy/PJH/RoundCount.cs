using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoundCount : MonoBehaviour
{
    [SerializeField] private TMP_Text roundText;
    [SerializeField] private Button nextRoundButton;

    private int currentRound = 1;

    void Start()
    {
        UpdateRoundText();

        nextRoundButton.onClick.AddListener(OnNextRoundClicked);
    }

    private void OnNextRoundClicked()
    {
        currentRound++;
        UpdateRoundText();
    }

    private void UpdateRoundText()
    {
        roundText.text = $"Round {currentRound}";
    }
}