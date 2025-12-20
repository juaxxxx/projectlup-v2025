using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace LUP.RL
{
    public class RouletteScript : MonoBehaviour
    {
        private PlatformAdapter adapter;
        [Header("UI References")]
        [SerializeField] private Button spinButton;
        [SerializeField] private GameObject RoulletPanel;
        [SerializeField] private GameObject RoulletImagel;
        [SerializeField] private GameObject resultPanel;
        [SerializeField] private Image resultImage;

        [Header("Buff List")]
        public List<BuffData> buffList = new();

        private bool isSpinning = false;
        private bool isResultReady = false;
        private float currentSpeed = 0;
        private BuffData selectedBuff;


        void Start()
        {
            if (spinButton == null) return;
            resultPanel.SetActive(false);
            spinButton.onClick.AddListener(OnButtonClick);
            adapter = new PlatformAdapter();
            adapter.LinkToPlatform();

        }
        void OnButtonClick()
        {
            if (!isSpinning && !isResultReady)
            {
                Debug.Log("∑Í∑ø Ω√¿€!");
                Time.timeScale = 1;
                isSpinning = true;

                //πˆ∆∞ ∫Ò»∞º∫»≠
                spinButton.interactable = false;
                currentSpeed = 300f;

            }
            else if (!isSpinning && isResultReady)
            {
                spinButton.interactable = false;

                ShowResult();
            }

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                RoulletPanel.gameObject.SetActive(true);
                Time.timeScale = 0;

            }
        }
        void SelectRandomBuff()
        {
            int rand = Random.Range(0, buffList.Count);
            selectedBuff = buffList[rand];
        }
        private void Update()
        {
            if (isSpinning)
            {
                RoulletImagel.transform.Rotate(0, 0, currentSpeed);
                currentSpeed *= 0.98f;

                if (currentSpeed < 1f)
                {
                    currentSpeed = 0;
                    isSpinning = false;
                    isResultReady = true;
                    spinButton.interactable = true;
                    SelectRandomBuff();
                }
            }

        }
        void ShowResult()
        {
            int randIndex = Random.Range(0, buffList.Count);
            BuffData selectedBuff = buffList[randIndex];
            resultImage.sprite = selectedBuff.GetDisplayableImage();

            Debug.Log($"¥Á√∑: {selectedBuff.buffName}");
            resultPanel.SetActive(true);
            RoulletImagel.SetActive(false);
            StartCoroutine(CloseResultAfterDelay(2f));
            Time.timeScale = 1;
       
        }
      
        IEnumerator CloseResultAfterDelay(float delay)
        {
            yield return new WaitForSecondsRealtime(delay);
            RoulletPanel.SetActive(false);
            Destroy(gameObject);
        }
    }
}

