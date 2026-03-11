using UnityEngine;

namespace LUP.ES
{
    public class InteractableButton : MonoBehaviour
    {
        [SerializeField] private GameObject Panel;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        public void OnClickEvent()
        {
            SoundManager.Instance.PlaySFX("OnButton");
            Panel.SetActive(true);
        }

        public void OnClickCloseEvent()
        {
            SoundManager.Instance.PlaySFX("ExitButton");
            Panel.SetActive(false);
        }
    }
}