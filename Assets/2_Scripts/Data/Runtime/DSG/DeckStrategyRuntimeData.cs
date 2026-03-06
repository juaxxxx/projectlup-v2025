using LUP.DSG;
using System.Collections.Generic;
using UnityEngine;

public class DeckStrategyRuntimeData : BaseRuntimeData
{
    [SerializeField]
    private int playerId;

    [SerializeField]
    private List<OwnedCharacterInfo> ownedCharacterList = new List<OwnedCharacterInfo>();

    [SerializeField]
    private List<Team> teams = new List<Team>();

    [SerializeField]
    private int selectedTeamIndex;

    public int PlayerId
    {
        get => playerId;
        set => SetValue(ref playerId, value);
    }

    public List<OwnedCharacterInfo> OwnedCharacterList
    {
        get => ownedCharacterList;
        set => SetValue(ref ownedCharacterList, value);
    }

    public List<Team> Teams
    {
        get => teams;
        set => SetValue(ref teams, value);
    }

    public int SelectedTeamIndex
    {
        get => selectedTeamIndex;
        set => SetValue(ref selectedTeamIndex, value);
    }

    public override void ResetData()
    {
        playerId = 0;
        ownedCharacterList.Clear();
        teams.Clear();
        selectedTeamIndex = 0;
    }

    public OwnedCharacterInfo GetCharacterInfo(int characterId)
    {
        foreach (OwnedCharacterInfo data in ownedCharacterList)
        {
            if (data.characterID == characterId) return data;
        }

        return null;
    }

}