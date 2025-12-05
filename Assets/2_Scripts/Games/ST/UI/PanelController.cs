using UnityEngine;
namespace LUP.ST
{

    public class PanelController : MonoBehaviour
    {
        public GameObject characterSelectPanel;
        public GameObject OptionPanel;
        public void OpenCharacterSelect()
        {
            if (characterSelectPanel != null)
                characterSelectPanel.SetActive(true);
        }

        public void CloseCharacterSelect()
        {
            if (characterSelectPanel != null)
                characterSelectPanel.SetActive(false);
        }

        public void OpenOptionPanel()
        {
            if(OptionPanel != null)
                OptionPanel.SetActive(true);
        }

        public void CloseOptionPanel()
        {
            if(OptionPanel != null)
                OptionPanel.SetActive(false);
        }
    }

}