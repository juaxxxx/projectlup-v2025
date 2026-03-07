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

    //[SerializeField] private RLCharacterData[] _characterDatas;

    [SerializeField] private CharacterEquipsID[] _CharacterEquips;

    [SerializeField] private CharacterEquipsID _F001Data;
    [SerializeField] private CharacterEquipsID _F002Data;
    [SerializeField] private CharacterEquipsID _F003Data;
    [SerializeField] private CharacterEquipsID _M001Data;
    [SerializeField] private CharacterEquipsID _M002Data;

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
    public CharacterEquipsID F001Data
    {
        get => _F001Data;
        set => SetValue(ref _F001Data, value);
    }

    public CharacterEquipsID F002Data
    {
        get => _F002Data;
        set => SetValue(ref _F002Data, value);
    }

    public CharacterEquipsID F003Data
    {
        get => _F003Data;
        set => SetValue(ref _F003Data, value);
    }

    public CharacterEquipsID M001Data
    {
        get => _M001Data;
        set => SetValue(ref _M001Data, value);
    }

    public CharacterEquipsID M002Data
    {
        get => _M002Data;
        set => SetValue(ref _M002Data, value);
    }

    public CharacterEquipsID[] characterEquips
    {
        get => _CharacterEquips;
        set => SetValue(ref _CharacterEquips, value);
    }
}
