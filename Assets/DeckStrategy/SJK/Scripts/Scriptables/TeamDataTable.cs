using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Team Data", menuName = "Scriptable/Team Data", order = int.MaxValue)]
public class TeamDataTable : ScriptableObject
{
    public const int MaxTeams = 8;
    public List<UserData.Team> teams = new List<UserData.Team>();
}
