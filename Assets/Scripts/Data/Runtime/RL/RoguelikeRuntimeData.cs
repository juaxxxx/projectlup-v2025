using System;
using UnityEngine;

[Serializable]
public class RoguelikeRuntimeData : BaseRuntimeData
{
    [SerializeField] private int _id = 0;
    [SerializeField] private string _name = "RoguelikePlayer";

    [SerializeField] private int _lastSelectedCharacter = 0;
    [SerializeField] private int _lastPlayedChapter = 0;


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
}
