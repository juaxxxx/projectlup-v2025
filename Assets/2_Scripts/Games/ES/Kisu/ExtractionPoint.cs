using UnityEngine;
using TMPro;
using UnityEngine.UI; // 1. UI 네임스페이스 추가

namespace LUP.ES
{
    public class ExtractionPoint : MonoBehaviour, IInteractable
    {
        [Header("Open FX")] // 기수 추가한 코드
        private VFXObjectPool vfxObjectPool;
        public GameObject extractionFxPrefab; // 기수 추가한 코드
        public Transform fxAnchor;  // 기수 추가한 코드

        private GameObject fxInstance;  // 기수 추가한 코드
        private ParticleSystem[] fxSystems; // 기수 추가한 코드

        [Header("설정")]
        [SerializeField] private float extractionTime = 10.0f;

        [Header("색상")]
        [SerializeField] private Color startFillColor = Color.green;
        [SerializeField] private Color successFillColor = Color.green;

        [Header("참조")]
        public EventBroker eventBroker;
        private InteractionUIController interactionUI;

        // 2. 머터리얼 대신 Image 참조 추가
        [Header("UI 요소")]
        [SerializeField] private Image progressCircle;
        [SerializeField] private TextMeshProUGUI timerText;

        public bool InterruptsOnMove => false;

        private bool isExtracting = false;
        private bool isExtracted = false;
        private float currentTime = 0.0f;

        public bool CanInteract() => !isExtracted;

        void Start()
        {
            vfxObjectPool = FindFirstObjectByType<VFXObjectPool>();
            interactionUI = GetComponent<InteractionUIController>();
            eventBroker = FindAnyObjectByType<EventBroker>();
            // 초기 상태 설정
            if (progressCircle != null)
            {
                progressCircle.fillAmount = 0f;
                progressCircle.color = startFillColor;
            }

            HideTimerTextObject();
        }

        public bool TryStartInteraction(float deltaTime)
        {
            if (isExtracted) return true;

            if (!isExtracting)
            {
                isExtracting = true;
                currentTime = extractionTime;

                PlayOpenFX();

                ShowTimerTextObject();
                HideInteractionPrompt();
            }

            currentTime -= deltaTime;

            float progress = Mathf.Clamp01(1f - (currentTime / extractionTime));

            if (progressCircle != null)
            {
                progressCircle.fillAmount = progress;
                progressCircle.color = Color.Lerp(startFillColor, successFillColor, progress);
            }

            UpdateInteractionTimerText(Mathf.Max(0, currentTime));

            if (currentTime <= 0.0f)
            {
                Interact();
                return true;
            }

            return false;
        }

        public void Interact()
        {
            isExtracted = true;
            isExtracting = false;

            if (progressCircle != null)
            {
                progressCircle.fillAmount = 1f;
                progressCircle.color = successFillColor;
            }

            StopOpenFX();

            HideTimerTextObject();
            Debug.Log("탈출 성공!");

            if (eventBroker != null)
                eventBroker.ReportGameFinish(true);
        }

        public void ResetInteraction()
        {
            isExtracting = false;
            currentTime = 0.0f;

            if (progressCircle != null)
                progressCircle.fillAmount = 0f;

            StopOpenFX();

            HideTimerTextObject();
        }

        public void ShowInteractionPrompt() { if (!isExtracted) interactionUI.ShowInteractionPrompt(); }
        public void HideInteractionPrompt() { interactionUI.HideInteractionPrompt(); }
        public void ShowInteractionTimerUI() { }
        public void HideInteractionTimerUI() { }

        private void UpdateInteractionTimerText(float remainTime)
        {
            if (timerText != null) timerText.text = remainTime.ToString("F1");
        }

        private void ShowTimerTextObject() 
        { if (timerText != null) timerText.gameObject.SetActive(true); }
        private void HideTimerTextObject() 
        { if (timerText != null) timerText.gameObject.SetActive(false); }

        void PlayOpenFX()
        {
            if (fxInstance != null || vfxObjectPool == null) return;
            fxInstance = vfxObjectPool.SpawnVFX(extractionFxPrefab, transform.position, true);
            fxInstance.transform.localScale = Vector3.one * 0.5f;
            //fxInstance = Instantiate(extractionFxPrefab, fxAnchor.position, fxAnchor.rotation);

            //fxInstance.transform.localScale *= 0.5f;

            //fxSystems = fxInstance.GetComponentsInChildren<ParticleSystem>();

            //foreach (var ps in fxSystems)
            //{
            //    var main = ps.main;
            //    main.loop = true;
            //    ps.Play();
            //}
        }
        // 기수 추가한 코드
        void StopOpenFX()
        {
            if (fxSystems == null || vfxObjectPool == null) return;
            vfxObjectPool.DespawnVFX(extractionFxPrefab, fxInstance);

            fxInstance = null;
            //foreach (var ps in fxSystems)
            //{
            //    if (ps == null) continue;
            //    var main = ps.main;
            //    main.loop = false;
            //    ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            //}

            //Destroy(fxInstance, 2.0f);
            //fxInstance = null;
            //fxSystems = null;
        }
    }
}