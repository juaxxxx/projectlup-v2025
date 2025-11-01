using UnityEditor;
using UnityEngine;

public class FormationSystem : MonoBehaviour
{
    [SerializeField]
    DataCenter dataCenter;

    [SerializeField]
    private TeamDataTable teamDataTable;

    public GameObject[] slots = new GameObject[5];

    public UserData.Team selectedTeam { get; private set; }

    [SerializeField]
    private Transform characterListContent;

    public int selectedTeamIndex = 0;
    public int selectedCount = 0;

    public delegate void CharacterPlacedHandler(int slotIndex, CharacterData characterBase);
    public delegate void CharacterReleasedHandler(int slotIndex);

    public CharacterPlacedHandler placedHandler;
    public CharacterReleasedHandler releaseHandler;

    void Start()
    {
        //for(int i = 0; i < slots.Length; ++i)
        //{
        //    CreateSlot(i);
        //}
    }

    private void CreateSlot(int index)
    {

    }

    public void PlaceTeam(int teamIndex)
    {
        selectedTeamIndex = teamIndex;
        ResetCharacterList();

        for (int i = 0; i < slots.Length; ++i)
        {
            LineupSlot slot = slots[i].GetComponent<LineupSlot>();
            slot.DeselectCharacter();
        }
        selectedCount = 0;
        if (teamDataTable.teams[selectedTeamIndex] == null) return;

        selectedTeam = teamDataTable.teams[selectedTeamIndex];

        for (int i = 0; i < slots.Length; ++i)
        {
            if (selectedTeam.characters[i].characterID == 0) return;
            LineupSlot slot = slots[i].GetComponent<LineupSlot>();

            //slot.SetSelectedCharacter(selectedTeam.characters[i], false);
            //++selectedCount;
            CharacterIcon[] icons = characterListContent.GetComponentsInChildren<CharacterIcon>();
            foreach (var icon in icons)
            {
                if (icon.characterInfo.characterID == selectedTeam.characters[i].characterID)
                {
                    icon.OnButtonClicked();
                    //icon.selectedButton.ButtonClicked();
                    break;
                }
            }
        }
    }

    public void PlaceCharacter(OwnedCharacterInfo info, SelectedButton button)
    {
        if (selectedCount >= 5) return;

        for (int i = 0; i < slots.Length; ++i)
        {
            LineupSlot slot = slots[i].GetComponent<LineupSlot>();
            if (!slot.isPlaced)
            {
                slot.SetSelectedCharacter(info, false);
                selectedTeam.characters[i] = info;
                ++selectedCount;
                button.ButtonClicked();
                return;
            }
        }
    }

    // �̹� ���õ� ĳ���͸� ������ ��
    public void ReleaseCharacter(int characterID, SelectedButton button)
    {
        if (selectedCount <= 0) return;

        for (int i = 0; i < slots.Length; ++i)
        {
            LineupSlot slot = slots[i].GetComponent<LineupSlot>();
            // ĳ���Ͱ� ������� ���� ������ �ѱ�
            if (slot.character == null || slot.character.characterData == null) continue;
            if (slot.character.characterData.ID == characterID)
            {
                slot.DeselectCharacter();
                selectedTeam.characters[i] = null;

                --selectedCount;
                button.ButtonClicked();
                return;
            }
        }
    }

    private void ResetCharacterList()
    {
        CharacterIcon[] icons = characterListContent.GetComponentsInChildren<CharacterIcon>();
        foreach (var icon in icons)
        {
            if (icon.selectedButton.isSelected)
            {
                icon.selectedButton.ButtonClicked();
            }
        }
    }

    public void SaveTeam()
    {
        for (int i = 0; i < slots.Length; ++i)
        {
            LineupSlot slot = slots[i].GetComponent<LineupSlot>();
            if (teamDataTable.teams[selectedTeamIndex] == null) teamDataTable.teams[selectedTeamIndex] = new UserData.Team();
            teamDataTable.teams[selectedTeamIndex] = selectedTeam;
        }
#if UNITY_EDITOR
        EditorUtility.SetDirty(teamDataTable);
        AssetDatabase.SaveAssets();
#endif
    }
}
