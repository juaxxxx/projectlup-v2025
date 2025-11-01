using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TeamMVPData", menuName = "Team MVP Data", order = int.MaxValue)]
public class TeamMVPData : ScriptableObject
{
    public float char1Score, char2Score, char3Score, char4Score, char5Score;
    public string char1Name, char2Name, char3Name, char4Name, char5Name;
    public Color char1Color, char2Color, char3Color, char4Color, char5Color;
}