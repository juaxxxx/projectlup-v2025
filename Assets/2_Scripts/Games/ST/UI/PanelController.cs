using UnityEngine;
namespace LUP.ST
{

    public class PanelController : MonoBehaviour
    {
        public GameObject characterSelectPanel;
        public void OpenCharacterSelect()
        {
            characterSelectPanel.SetActive(true);
        }

        public void CloseCharacterSelect()
        {
            characterSelectPanel.SetActive(false);
        }
    }

}