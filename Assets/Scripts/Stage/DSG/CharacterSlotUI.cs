using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace DSG
{
    public class CharacterSlotUI : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text nameText;
        [SerializeField] private TMP_Text contributionText;

        public void SetData(CharacterData data, CharacterModelData modelData)
        {
            if (data == null || modelData == null)
            {
                Debug.LogWarning($"{gameObject.name}: 데이터가 비어있습니다!");
                return;
            }

            // 이미지 적용
            if (image != null)
                image.material = modelData.material;

            // 이름 적용
            if (nameText != null)
                nameText.text = data.characterName;

            // 공격력(기여도) 적용
            if (contributionText != null)
                contributionText.text = $"공격력: {data.attack}";
        }
    }
}