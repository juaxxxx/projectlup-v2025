using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class RouletteScript : MonoBehaviour
{
 
    private PlatformAdapter adapter;
    [Header("UI References")]
    [SerializeField] private Button spinButton;
    [SerializeField] private GameObject roulette;
    [SerializeField] private GameObject roulettePanel;
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
       
        spinButton.onClick.AddListener(OnButtonClick);
        adapter = new PlatformAdapter();
        adapter.LinkToPlatform();
        resultPanel.SetActive(false);
        //·ê·¿¿¡  ¹öÇÁ¸®½ºÆ®¿¬°á
       buffList.AddRange(adapter.gainableBuffDatas);
    }
    void OnButtonClick()
    {
        if (!isSpinning && !isResultReady)
        {
            Debug.Log("·ê·¿ ½ÃÀÛ!");
            Time.timeScale = 1;
            isSpinning = true;

            //¹öÆ° ºñÈ°¼ºÈ­
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
        if(other.CompareTag("Player"))
        {
            roulette.gameObject.SetActive(true);
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
            roulettePanel.transform.Rotate(0, 0, currentSpeed);
            currentSpeed *= 0.98f;

            if (currentSpeed < 1f)
            {
                currentSpeed = 0;
                isSpinning = false;
                isResultReady = true;
                spinButton.interactable = true;
                Debug.Log("È¸Àü ³¡");
                SelectRandomBuff();
            }
        }

    }
    void ShowResult()
    {
        int randIndex = Random.Range(0, buffList.Count);
        BuffData selectedBuff = buffList[randIndex];
        resultImage.sprite = selectedBuff.GetDisplayableImage();

        Debug.Log($"´çÃ·: {selectedBuff.buffName}");
        resultPanel.SetActive(true);
        roulettePanel.SetActive(false);
        Time.timeScale = 1;
        StartCoroutine(CloseResultAfterDelay(2f));
    }
    IEnumerator CloseResultAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        roulette.SetActive(false);
        Destroy(gameObject);
    }
}
