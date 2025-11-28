using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DeckStrategyRuntimeData : BaseRuntimeData
{
    [SerializeField]
    private int playerId;

    [SerializeField]
    private List<OwnedCharacterInfo> ownedCharacterList = new List<OwnedCharacterInfo>();

    [SerializeField]
    private List<UserData.Team> teams = new List<UserData.Team>();

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

    public List<UserData.Team> Teams
    {
        get => teams;
        set => SetValue(ref teams, value);
    }

    public override void ResetData() 
    {
        playerId = 0;
        ownedCharacterList.Clear();
        teams.Clear();
    }
}
