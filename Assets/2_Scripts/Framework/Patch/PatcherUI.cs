using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LUP
{
    public class PatcherUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Patcher patcher;

        [Header("UI Elements")]
        [SerializeField] private GameObject downloadPanel;     
        [SerializeField] private Image progressBar;             

        [Header("Settings")]
        [SerializeField] private bool autoHideWhenComplete = true;  // 완료 시 자동으로 패널 숨기기

        private bool isDownloading = false;
        private float lastProgress = 0f;

        void Start()
        {
            if (patcher == null)
                patcher = FindFirstObjectByType<Patcher>();

            if (patcher == null)
            {
                Debug.LogError("Patcher를 찾을 수 없습니다.");
                return;
            }

            // 처음엔 패널 숨기기
            if (downloadPanel != null)
            {
                downloadPanel.SetActive(false);
            }

            // 패치 시작 감지를 위한 코루틴 시작
            StartCoroutine(MonitorPatchProgress());
        }

        void Update()
        {
            if (patcher == null) return;

            float currentProgress = patcher.TotalProgress;

            // 진행률이 변경되면 UI 업데이트
            if (Mathf.Abs(currentProgress - lastProgress) > 0.001f)
            {
                UpdateProgressUI(currentProgress);
                lastProgress = currentProgress;

                // 다운로드 시작 감지
                if (currentProgress > 0f && currentProgress < 1f && !isDownloading)
                {
                    isDownloading = true;
                    ShowDownloadPanel();
                }

                // 다운로드 완료 감지
                if (currentProgress >= 1f && isDownloading)
                {
                    isDownloading = false;
                    OnDownloadComplete();
                }
            }
        }

        private void UpdateProgressUI(float progress)
        {
            if (progressBar != null)
            {
                progressBar.fillAmount = progress;
            }
        }

        private void ShowDownloadPanel()
        {
            if (downloadPanel != null)
            {
                downloadPanel.SetActive(true);
            }
        }

        private void OnDownloadComplete()
        {
            Debug.Log("[PatcherUI] 다운로드 완료");

            if (autoHideWhenComplete)
            {
                StartCoroutine(HideDownloadPanelAfterDelay(0.1f));
            }
        }

        private IEnumerator HideDownloadPanelAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (downloadPanel != null)
            {
                downloadPanel.SetActive(false);
            }
        }

        private IEnumerator MonitorPatchProgress()
        {
            while (true)
            {
                if (patcher != null)
                {
                    float progress = patcher.TotalProgress;

                    // 진행률이 0보다 크면 다운로드가 시작된 것
                    if (progress > 0f && !isDownloading)
                    {
                        isDownloading = true;
                        ShowDownloadPanel();
                    }
                }

                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
