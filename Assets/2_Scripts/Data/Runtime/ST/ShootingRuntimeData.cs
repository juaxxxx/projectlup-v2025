using LUP.ST;
using System;
using System.Drawing.Text;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShootingRuntimeData : BaseRuntimeData
{
    [SerializeField]
    private int playerId;

    [SerializeField] 
    private List<LUP.ST.OwnedCharacterInfo> ownedCharacterList = new List<LUP.ST.OwnedCharacterInfo>();
    
    [SerializeField] 
    private List<int> teamSlots = new List<int> { 0, 0, 0, 0, 0 };

    [NonSerialized]
    private STCharacterData[] _teamCache;

    public int PlayerId
    {
        get => playerId;
        set => SetValue(ref playerId, value);
    }

    public List<LUP.ST.OwnedCharacterInfo> OwnedCharacterList
    {
        get => ownedCharacterList;
        set => SetValue(ref ownedCharacterList, value);
    }

    public List<int> TeamSlots
    {
        get => teamSlots;
        set => teamSlots = value;
    }

    public STCharacterData[] Team
    {
        get => _teamCache;
        set => _teamCache = value;
    }

    public override void ResetData()
    {
        playerId = 0;
        ownedCharacterList.Clear();
        teamSlots = new List<int> { 0, 0, 0, 0, 0 };  
    }


    public OwnedCharacterInfo GetCharacterInfo(int characterId)
    {
        return ownedCharacterList.Find(c => c.characterId == characterId);
    }

    public int GetCharacterLevel(int characterId)
    {
        var info = GetCharacterInfo(characterId);
        return info != null ? info.level : 1;
    }

}
