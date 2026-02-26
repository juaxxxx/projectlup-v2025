using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.DSG
{
    public class MVPDisplay : MonoBehaviour
    {
        //private DataCenter dataCenter;
        private float average = 0;

        private void Awake()
        {
            StageInitializeInvoker.OnDSGStagePostInitialize += PostInitialize;
        }

        private void OnDestroy()
        {
            StageInitializeInvoker.OnDSGStagePostInitialize -= PostInitialize;
        }

        private void PostInitialize(DeckStrategyStage stage)
        {
            DeckStrategyStage deckStage = LUP.StageManager.Instance.GetCurrentStage() as DeckStrategyStage;
            if (deckStage == null || deckStage.mvpData == null) return;

            var mvp = deckStage.mvpData;

            TMP_Text resultText = transform.Find("Text_Result")?.GetComponent<TMP_Text>();
            if (resultText != null)
            {
                resultText.text = (mvp.battleResult == "Victory") ? "Victory" : "Defeat";
            }

            average = mvp.char1Score;

            var icon1 = ResolveIcon(mvp.char1Icon, mvp.char1CharacterId, mvp.char1ModelId);
            var icon2 = ResolveIcon(mvp.char2Icon, mvp.char2CharacterId, mvp.char2ModelId);
            var icon3 = ResolveIcon(mvp.char3Icon, mvp.char3CharacterId, mvp.char3ModelId);
            var icon4 = ResolveIcon(mvp.char4Icon, mvp.char4CharacterId, mvp.char4ModelId);
            var icon5 = ResolveIcon(mvp.char5Icon, mvp.char5CharacterId, mvp.char5ModelId);

            mvp.char1Icon = icon1;
            mvp.char2Icon = icon2;
            mvp.char3Icon = icon3;
            mvp.char4Icon = icon4;
            mvp.char5Icon = icon5;

            SetSlot("MVP1", "Image", "Text_Name", "Score", "Text_Score", mvp.char1Name, icon1, mvp.char1Score, average);
            SetSlot("MVP2", "Image", "Text_Name", "Score", "Text_Score", mvp.char2Name, icon2, mvp.char2Score, average);
            SetSlot("MVP3", "Image", "Text_Name", "Score", "Text_Score", mvp.char3Name, icon3, mvp.char3Score, average);
            SetSlot("MVP4", "Image", "Text_Name", "Score", "Text_Score", mvp.char4Name, icon4, mvp.char4Score, average);
            SetSlot("MVP5", "Image", "Text_Name", "Score", "Text_Score", mvp.char5Name, icon5, mvp.char5Score, average);

            SetMainMVP("MVP/RawImage", "Text_Name", mvp.char1Name);

            ShowMVPModelIfAvailable(mvp);
        }

        private Sprite ResolveIcon(Sprite current, int charId, int modelId)
        {
            if (current != null) return current;

            Sprite cached;

            if (modelId != 0 && CharacterIconCache.TryGet(charId, modelId, out cached) && cached != null)
                return cached;

            if (charId != 0 && CharacterIconCache.TryGetByCharacterId(charId, out cached) && cached != null)
                return cached;

            if (modelId != 0 && CharacterIconCache.TryGetByModelId(modelId, out cached) && cached != null)
                return cached;

            return null;
        }

        private void SetSlot(
            string slotName,
            string imageName,
            string textName,
            string sliderName,
            string scoreTextName,
            string charName,
            Sprite icon,
            float score,
            float maxScore)
        {
            Transform slot = transform.Find(slotName);
            if (slot == null) return;

            Image characterImage = slot.Find("ImageRoot/Image")?.GetComponent<Image>();
            TMP_Text characterName = slot.Find(textName)?.GetComponent<TMP_Text>();
            Slider characterScore = slot.Find(sliderName)?.GetComponent<Slider>();
            TMP_Text scoreText = slot.Find(scoreTextName)?.GetComponent<TMP_Text>();

            if (characterImage != null)
            {
                if (icon != null)
                {
                    characterImage.sprite = icon;
                    characterImage.color = Color.white;
                    characterImage.preserveAspect = true;
                    characterImage.type = Image.Type.Simple;
                    characterImage.material = null;
                    
                    RectTransform rt = characterImage.rectTransform;
                    rt.localScale = Vector3.one;

                    rt.sizeDelta = new Vector2(300, 300);
                }
                else
                {
                    characterImage.sprite = null;
                    characterImage.color = Color.gray;
                }
            }

            if (characterName != null)
            {
                characterName.text = string.IsNullOrEmpty(charName) ? "-" : charName;
            }

            float ratio = (maxScore > 0f) ? (score / maxScore) : 0f;
            if (characterScore != null)
            {
                characterScore.value = ratio;
            }

            if (scoreText != null)
            {
                scoreText.text = $"{score:F0}";
            }
        }

        private void SetMainMVP(string slotName, string textName, string charName)
        {
            Transform slot = transform.Find(slotName);
            if (slot == null) return;

            TMP_Text characterName = slot.Find(textName)?.GetComponent<TMP_Text>();
            if (characterName != null)
            {
                characterName.text = string.IsNullOrEmpty(charName) ? "-" : charName;
            }
        }

        private void ShowMVPModelIfAvailable(TeamMVPData mvp)
        {
            if (mvp == null) return;
            if (mvp.char1Prefab == null) return;

            MVPModelViewer viewer = FindFirstObjectByType<MVPModelViewer>();
            if (viewer == null) return;
            viewer.ShowMVPModel(mvp.char1Prefab);
        }
    }
}