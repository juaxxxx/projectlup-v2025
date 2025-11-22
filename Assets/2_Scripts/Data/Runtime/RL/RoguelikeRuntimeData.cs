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
}
