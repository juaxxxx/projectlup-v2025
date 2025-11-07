
using Roguelike.Define;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace RL
{
    public class OwningBuffListScrollPanel : BaseScrollAblePanel
    {

        private Button BackToGameBtn = null;
        private Button BackToLobbyBtn = null;

        //TODO 차후 플레이어 타입 구별 필요! 계속 아쳐로 받아올거야??
        //private Archer controlledPlayer = null;
        //private StageController stageController = null;

        private void Awake()
        {
            ButtonRule[] uiButtons = GetComponentsInChildren<ButtonRule>();

            for (int i = 0; i < uiButtons.Length; i++)
            {
                ButtonRole buttonRole = uiButtons[i].buttonRole;

                switch (buttonRole)
                {
                    case ButtonRole.None:
                        UnityEngine.Debug.LogError("ButtonRole Is Not Assine");
                        break;

                    case ButtonRole.BackToLobbyBtn:
                        BackToLobbyBtn = uiButtons[i].GetComponent<Button>();
                        BackToLobbyBtn.onClick.AddListener(BackToLobby);
                        break;

                    case ButtonRole.BackToGameBtn:
                        BackToGameBtn = uiButtons[i].GetComponent<Button>();
                        BackToGameBtn.onClick.AddListener(BackToGame);
                        break;
                }
            }
        }

        private void OnEnable()
        {
        }

        private void OnDisable()
        {

        }

        protected override void GenerateContent()
        {
            base.EraseContents();

            for (int i = 0; i < displayedData.Length; i++)
            {
                int index = i;

                GameObject Itembox = Instantiate(displayedPrefab, contentParent);
                DisplayableImageBox displayAbleItemBox = Itembox.GetComponent<DisplayableImageBox>();

                if (displayAbleItemBox == null)
                {
                    UnityEngine.Debug.LogError("Cast ImageBox Fail!", this.gameObject);
                    continue;
                }

                displayAbleItemBox.SetDisplayableImage(displayedData[index].GetDisplayableImage());

                Transform textTransform = Itembox.transform.Find("numText");

                string itemAmount = displayedData[index].GetExtraInfo().ToString();

                textTransform.GetComponent<TextMeshProUGUI>().SetText(itemAmount);
            }

            setconstraintCount();

        }

        new private void Start()
        {
            base.Start();

            transform.parent.gameObject.SetActive(false);

        }

        private void BackToGame()
        {
            Time.timeScale = 1f;

            GameObject gamePausePanel = transform.parent.gameObject;

            if (gamePausePanel == null)
                UnityEngine.Debug.LogError("Fail To Back Game");

            gamePausePanel.SetActive(false);
        }

        private void BackToLobby()
        {
            Time.timeScale = 1f;

            PlatformAdapter platformAdapter = new PlatformAdapter();

            if (platformAdapter == null)
            {
                UnityEngine.Debug.LogError("Fail To Create platformAdapter");
            }

            platformAdapter.LinkToPlatform();
            platformAdapter.LoadLobbyScene();
        }

    }
}


