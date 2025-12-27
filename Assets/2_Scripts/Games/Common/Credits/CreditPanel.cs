using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace LUP
{
    public class CreditPanel : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameObject panel;
        [SerializeField] private ScrollRect scrollRect;
        [SerializeField] private Transform contentParent;

        [Header("Scroll Settings")]
        [SerializeField] private float scrollSpeed = 0.15f;
        [SerializeField] private bool autoScroll = true;

        private bool isScrolling = false;
        private CreditData creditData;

        private void Awake()
        {
            creditData = new CreditData();
        }

        private void Start()
        {
            if (panel != null)
            {
                panel.SetActive(false);
            }
        }

        private void Update()
        {
            if (isScrolling && autoScroll)
            {
                scrollRect.verticalNormalizedPosition -= scrollSpeed * Time.deltaTime;

                if (scrollRect.verticalNormalizedPosition <= 0f)
                {
                    scrollRect.verticalNormalizedPosition = 0f;
                    isScrolling = false;
                }
            }

            if (panel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            {
                Hide();
            }
        }

        public void Show()
        {
            if (panel == null)
            {
                Debug.LogError("CreditPanel: Panel reference is null!");
                return;
            }

            panel.SetActive(true);
            GenerateCredits();

            if (scrollRect != null)
            {
                scrollRect.verticalNormalizedPosition = 1f;
                isScrolling = true;
            }
        }

        public void Hide()
        {
            if (panel != null)
            {
                panel.SetActive(false);
                isScrolling = false;
            }
        }

        private void GenerateCredits()
        {
            if (contentParent == null)
            {
                Debug.LogError("CreditPanel: ContentParent is null!");
                return;
            }

            ClearContent();

            AddTitle(creditData.projectTitle);
            AddSpacer();

            foreach (var team in creditData.GetTeams())
            {
                AddSectionHeader(team.gameName);

                foreach (var member in team.members)
                {
                    AddCreditEntry(member.role, member.name);
                }

                AddSpacer();
            }

            AddSectionHeader("Framework & Common");
            foreach (var member in creditData.GetCommonTeam())
            {
                AddCreditEntry(member.role, member.name);
            }

            AddSpacer();
            AddSpacer();

            AddTitle("Thank you for playing!");
        }

        private void ClearContent()
        {
            foreach (Transform child in contentParent)
            {
                Destroy(child.gameObject);
            }
        }


        private void AddTitle(string text)
        {
            CreateDefaultText(text, 48, FontStyles.Bold, TextAlignmentOptions.Center);
        }

        private void AddSectionHeader(string text)
        {
            CreateDefaultText(text, 36, FontStyles.Bold, TextAlignmentOptions.Center);
        }

        private void AddCreditEntry(string role, string name)
        {
            string combinedText = $"{role}: {name}";
            CreateDefaultText(combinedText, 24, FontStyles.Normal, TextAlignmentOptions.Center);
        }

        private void AddSpacer()
        {
            GameObject spacer = new GameObject("Spacer");
            spacer.transform.SetParent(contentParent);
            RectTransform rt = spacer.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(0, 40);
        }

        private void CreateDefaultText(string text, int fontSize, FontStyles fontStyle, TextAlignmentOptions alignment)
        {
            GameObject textObj = new GameObject("Text");
            textObj.transform.SetParent(contentParent, false);

            RectTransform rt = textObj.AddComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0.5f);
            rt.anchorMax = new Vector2(0.5f, 0.5f);
            rt.pivot = new Vector2(0.5f, 0.5f);
            rt.sizeDelta = new Vector2(800, fontSize + 20);
            rt.anchoredPosition = Vector2.zero;

            TextMeshProUGUI tmp = textObj.AddComponent<TextMeshProUGUI>();
            tmp.text = text;
            tmp.fontSize = fontSize;
            tmp.fontStyle = fontStyle;
            tmp.alignment = alignment;
            tmp.color = Color.white;
        }

        public void ToggleAutoScroll()
        {
            autoScroll = !autoScroll;
            if (autoScroll && panel.activeSelf)
            {
                isScrolling = true;
            }
        }

        public void ResetScroll()
        {
            if (scrollRect != null)
            {
                scrollRect.verticalNormalizedPosition = 1f;
                isScrolling = true;
            }
        }
    }
}
