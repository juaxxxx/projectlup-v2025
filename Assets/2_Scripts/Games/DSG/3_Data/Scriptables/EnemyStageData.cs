using System.Collections.Generic;
using UnityEngine;

namespace LUP.DSG
{
    [CreateAssetMenu(fileName = "Enemy Stage Data", menuName = "DSG/EnemyStageData", order = int.MaxValue)]
    public class EnemyStageData : ScriptableObject
    {
        public List<Team> enemyTeamData = new List<Team>();
    }
}