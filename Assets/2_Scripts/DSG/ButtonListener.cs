using UnityEngine;
using UnityEngine.UI;

namespace LUP.DSG
{
    public class ButtonListener : MonoBehaviour
    {
        public Image targetImage;

        void Awake()
        {
            this.targetImage = GetComponent<Image>();
        }
        public void OnButtonClick()
        {
            if (targetImage != null)
            {
                if (targetImage.enabled == false)
                {
                    targetImage.enabled = true; // È°¼ºÈ­
                }
                else
                {
                    targetImage.enabled = false;
                }
            }
        }
    }
}