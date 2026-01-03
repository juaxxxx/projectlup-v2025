
using System.Data;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
namespace LUP.RL
{
    public class ExpBar : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private Image fillImage;
        [SerializeField] private Slider expSlider;
        [SerializeField] private TextMeshProUGUI levelText;
        private Coroutine fillCoroutine;
        [Header("Data Reference")]
        [SerializeField] public Archer archer;
        [SerializeField] private LevelDataTable levelTable;
        void Start()
        {
            FindFirstObjectByType<InGameCenter>().OnPlayerCharacterSpawned += OnPlayerCharacterSpanwed;
        }

        void OnPlayerCharacterSpanwed(GameObject playerObj)
        {
            archer = playerObj.GetComponent<Archer>();
            archer.OnExpChanged += UpdateUi;
            UpdateUi();
        }

        public void UpdateUi()
        {
            //현재 레벨 경험치
            int level = archer.RuntimeData.level;
            int Exp = archer.RuntimeData.xp;

            //다음레벨 존재할 경우  필요 경험치 갖고오기 .
            int requirExp = levelTable.levelList[level - 1].RequiredExp;
            //비율   계산  
            float ratio = (float)Exp / requirExp;
            if (fillCoroutine != null)
                StopCoroutine(fillCoroutine);

            float currentFill = fillImage.fillAmount;
            if (ratio < currentFill)
            {
                fillImage.fillAmount = 0f;
            }

            fillCoroutine = StartCoroutine(SmoothFill(ratio));
            levelText.text = $"Lv. {level}";
          
        }
        private IEnumerator SmoothFill(float target)
        {
            float current = fillImage.fillAmount;
            float speed = 1f; // 속도 조절 (클수록 빠름)

            while (Mathf.Abs(current - target) > 0.001f)
            {
                current = Mathf.Lerp(current, target, Time.deltaTime * speed);
                fillImage.fillAmount = current;
                yield return null;
            }

            fillImage.fillAmount = target;
        }
        // Update is called once per frame

    }
}