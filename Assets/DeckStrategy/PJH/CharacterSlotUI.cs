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
                Debug.LogWarning($"{gameObject.name}: �����Ͱ� ����ֽ��ϴ�!");
                return;
            }

            // �̹��� ����
            if (image != null)
                image.material = modelData.material;

            // �̸� ����
            if (nameText != null)
                nameText.text = data.characterName;

            // ���ݷ�(�⿩��) ����
            if (contributionText != null)
                contributionText.text = $"���ݷ�: {data.attack}";
        }
    }
}