using LUP.ST;
using System;
using System.Drawing.Text;
using UnityEngine;

[Serializable]
public class ShootingRuntimeData : BaseRuntimeData
{
    [SerializeField] private STCharacterData[] _team = new STCharacterData[5];
    public STCharacterData[] Team => _team;

    public void SetTeam(STCharacterData[] team5)
    {
        if (team5 == null || team5.Length != 5)
            return;

        if (_team == null || _team.Length != 5)
            _team = new STCharacterData[5];

        // 항상 "전체 복사"
        for (int i = 0; i < 5; i++)
        {
            _team[i] = team5[i];
        }
    }

    public override void ResetData()
    {
        for (int i = 0; i < _team.Length; i++)
            _team[i] = null;
    }
}
