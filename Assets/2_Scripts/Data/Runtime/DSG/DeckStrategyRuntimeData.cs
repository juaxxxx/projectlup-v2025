using LUP.DSG;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DeckStrategyRuntimeData : BaseRuntimeData
{
    [SerializeField]
    private int playerId;
    
    [SerializeField]
    private List<LUP.DSG.OwnedCharacterInfo> ownedCharacterList = new List<LUP.DSG.OwnedCharacterInfo>();

    [SerializeField]
    private List<LUP.DSG.Team> teams = new List<LUP.DSG.Team>();

    [SerializeField]
    private int selectedTeamIndex;

    public int PlayerId
    {
        get => playerId;
        set => SetValue(ref playerId, value);
    }

    public List<LUP.DSG.OwnedCharacterInfo> OwnedCharacterList
    {
        get => ownedCharacterList;
        set => SetValue(ref ownedCharacterList, value);
    }

    public List<LUP.DSG.Team> Teams
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

    public LUP.DSG.OwnedCharacterInfo GetCharacterInfo(int characterId)
    {
        foreach(LUP.DSG.OwnedCharacterInfo data in ownedCharacterList)
        {
            if(data.characterID == characterId) return data;
        }

        return null;
    }

}
