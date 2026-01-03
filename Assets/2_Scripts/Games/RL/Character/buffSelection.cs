using LUP.RL;
using System.Collections;
using UnityEngine;
namespace LUP.RL
{
    public class BuffSelectionUI : MonoBehaviour
    {
        [SerializeField] private GameObject panel;
        [SerializeField] private Transform optionParent;
        [SerializeField] private GameObject optionButtonPrefab;

        private PlayerBuff currentBuff;
        private bool isFirstLevelUp = true;
        public void Bind(PlayerBuff buff)
        {
           if (isFirstLevelUp)
           {
               isFirstLevelUp = false;
                Debug.Log("return");
               return;
           }
            currentBuff = buff;
            panel.SetActive(true);
        
            Build();
        
        }

        private void Build()
        {
           
            foreach (Transform c in optionParent)
                Destroy(c.gameObject);
            foreach (var buff in currentBuff.GetRandomBuffOptions())
            {
                var obj = Instantiate(optionButtonPrefab, optionParent);
                var btn = obj.GetComponent<OptionButtonUI>();
                btn.SetData(buff, currentBuff, this);
            }
            Time.timeScale = 0f;
        }
        private IEnumerator PauseNextFrame()
        {
            yield return null;              // 다음 프레임
            Time.timeScale = 0f;
        }
        public void Close()
        {
            panel.SetActive(false);
            currentBuff = null;
            Time.timeScale = 1f;
        }
    }
}