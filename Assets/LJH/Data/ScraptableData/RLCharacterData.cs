using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

using Roguelike.Define;

[System.Serializable]
public struct BaseStats
{
    public int Hp;
    public int Attack;
    public int speed;
    public int MaxHp;

}
[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class RLCharacterData : ScriptableObject, IDisplayable
{
    [SerializeField] private string characterName;
    [SerializeField] private Sprite characterPreviewImage;
    [SerializeField] private GameObject prefab;
    [SerializeField] public BaseStats stats;

    public GameObject CharacterPrefab => prefab;
    public Sprite CharacterPreview => characterPreviewImage;
    public string Name => characterName;

    private int canSeletable = 1;

    public CharacterType characterType = CharacterType.None;

    public string GetDisplayableName() { return characterName; }
    public Sprite GetDisplayableImage() { return characterPreviewImage; }

    public void SetDisplayableImage(Sprite image) { characterPreviewImage = image; }

    public int GetExtraInfo() { return canSeletable; }

    public void SetExtraInfo(int extraInfo) { canSeletable = extraInfo; }

}
