using OpenCvSharp.Aruco;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RoguelikeRuntimeData : BaseRuntimeData
{
    [SerializeField] private int _id = 0;
    [SerializeField] private string _name = "RoguelikePlayer";

    [SerializeField] private int _lastSelectedCharacter = 0;
    [SerializeField] private int _lastPlayedChapter = 0;
    [SerializeField] private Dictionary<int, int> _maxClearRoom;

    [SerializeField] private ChapterData _selectedChapter;
    [SerializeField] private RLCharacterData _selectedCharacter;

    //[SerializeField] private CharacterEquipsID _F001Equips;
    //[SerializeField] private CharacterEquipsID _F002Equips;
    //[SerializeField] private CharacterEquipsID _F003Equips;
    //[SerializeField] private CharacterEquipsID _M001Equips;
    //[SerializeField] private CharacterEquipsID _M002Equips;

    public int id
    {
        get => _id;
        set => SetValue(ref _id, value);
    }

    public string name
    {
        get => _name;
        set => SetValue(ref _name, value);
    }

    public override void ResetData()
    {
        _id = 0;
        _name = "RoguelikePlayer";

        _lastSelectedCharacter = -1;
        _lastPlayedChapter = -1;

        _maxClearRoom.Clear();
    }

    public int lastSelectedCharacter
    {
        get => _lastSelectedCharacter;
        set => SetValue(ref _lastSelectedCharacter, value);
    }

    public int lastPlayedChapter
    {
        get => _lastPlayedChapter;
        set => SetValue(ref _lastPlayedChapter, value);
    }

    public Dictionary<int, int> maxClearRoom
    {
        get => _maxClearRoom;
        set => SetValue(ref _maxClearRoom, value);
    }

    public ChapterData selectedChapter
    {
        get => _selectedChapter;
        set => SetValue(ref _selectedChapter, value);
    }

    public RLCharacterData selectedCharacter
    {
        get => _selectedCharacter;
        set => SetValue(ref _selectedCharacter, value);
    }

    //public CharacterEquipsID F001Equips
    //{
    //    get => _F001Equips;
    //    set => SetValue(ref _F001Equips, value);
    //}

    //public CharacterEquipsID F002Equips
    //{
    //    get => _F002Equips;
    //    set => SetValue(ref _F002Equips, value);
    //}

    //public CharacterEquipsID F003Equips
    //{
    //    get => _F003Equips;
    //    set => SetValue(ref _F003Equips, value);
    //}

    //public CharacterEquipsID M001Equips
    //{
    //    get => _M001Equips;
    //    set => SetValue(ref _M001Equips, value);
    //}

    //public CharacterEquipsID M002Equips
    //{
    //    get => _M002Equips;
    //    set => SetValue(ref _M002Equips, value);
    //}
}
