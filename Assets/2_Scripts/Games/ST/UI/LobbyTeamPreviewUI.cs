using UnityEngine;
using UnityEngine.UI;

namespace LUP.ST
{
    public class LobbyTeamPreviewUI : MonoBehaviour
    {
        [SerializeField] private Image[] teamSlotImages; // 5개

        public void SetTeam(STCharacterData[] team5)
        {
            if (teamSlotImages == null || teamSlotImages.Length < 5) return;
            if (team5 == null || team5.Length < 5) return;

            for (int i = 0; i < 5; i++)
            {
                teamSlotImages[i].sprite = team5[i] != null ? team5[i].thumbnail : null;
            }
        }

        void OnEnable()
        {
            var team = LobbyTeamCache.GetCopy();
            if (team != null)
                SetTeam(team);
            else
                SetTeam(new STCharacterData[5]); // 비우기(선택)
        }
    }
}
