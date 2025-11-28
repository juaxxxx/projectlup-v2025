using UnityEngine;
using System.Collections.Generic;

public class DSTeamRuntimeData : BaseRuntimeData
{
    public const int MaxTeams = 8;

    [SerializeField]
    private List<UserData.Team> teams = new List<UserData.Team>();

    public List<UserData.Team> Teams
    {
        get => teams;
        set => SetValue(ref teams, value);
    }

    public override void ResetData()
    {

    }
}
