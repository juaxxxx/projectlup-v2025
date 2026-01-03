using UnityEngine;
using TMPro;

namespace LUP
{
    public class CreditEntry : MonoBehaviour
    {
        [Header("Text References")]
        [SerializeField] private TextMeshProUGUI roleText;
        [SerializeField] private TextMeshProUGUI nameText;

        [Header("Combined Text (Optional)")]
        [SerializeField] private TextMeshProUGUI combinedText;

        [Header("Text Settings")]
        [SerializeField] private Color roleColor = new Color(0.7f, 0.7f, 0.7f);
        [SerializeField] private Color nameColor = Color.white;

        public void SetEntry(string role, string name)
        {
            if (roleText != null && nameText != null)
            {
                roleText.text = role;
                roleText.color = roleColor;

                nameText.text = name;
                nameText.color = nameColor;
            }
            else if (combinedText != null)
            {
                combinedText.text = $"<color=#{ColorUtility.ToHtmlStringRGB(roleColor)}>{role}</color>: " +
                                   $"<color=#{ColorUtility.ToHtmlStringRGB(nameColor)}>{name}</color>";
            }
            else
            {
                Debug.LogWarning($"CreditEntry: No text components assigned! Role: {role}, Name: {name}");
            }
        }

        public void SetColors(Color roleColor, Color nameColor)
        {
            this.roleColor = roleColor;
            this.nameColor = nameColor;

            if (roleText != null)
                roleText.color = roleColor;

            if (nameText != null)
                nameText.color = nameColor;
        }

        public void SetRole(string role)
        {
            if (roleText != null)
            {
                roleText.text = role;
            }
            else if (combinedText != null)
            {
                string currentText = combinedText.text;
                combinedText.text = $"<color=#{ColorUtility.ToHtmlStringRGB(roleColor)}>{role}</color>: {currentText.Split(':')[1]}";
            }
        }

        public void SetName(string name)
        {
            if (nameText != null)
            {
                nameText.text = name;
            }
            else if (combinedText != null)
            {
                string currentText = combinedText.text;
                string role = currentText.Split(':')[0];
                combinedText.text = $"{role}: <color=#{ColorUtility.ToHtmlStringRGB(nameColor)}>{name}</color>";
            }
        }
    }
}
