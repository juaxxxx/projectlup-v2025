using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

using Roguelike.Define;
using LUP.RL;

[System.Serializable]
public struct BaseStats
{
    public int Hp;
    public int Attack;
    public int speed;
    public int MaxHp;
    public int Exp;
}

[System.Serializable]
public struct CharacterEquipsID
{
    public int Weapon;
    public int Armor;
    public int Ring1;
    public int Ring2;
    public int Pet1;
    public int Pet2;
    public int Bracelet;
    public int Necklace;


    public void ExtractEquipsID(int[] idArray)
    {
        if (idArray == null || idArray.Length < 8)
        {
            return;
        }

        idArray[0] = Weapon;
        idArray[1] = Armor;
        idArray[2] = Ring1;
        idArray[3] = Ring2;
        idArray[4] = Pet1;
        idArray[5] = Pet2;
        idArray[6] = Bracelet;
        idArray[7] = Necklace;
    }
}

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class RLCharacterData : ScriptableObject, IDisplayable
{
    [SerializeField] private string characterName;
    [SerializeField] private Sprite characterPreviewImage;
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private GameObject weaponProjecTile = null;
    [SerializeField] public RWeaponType weaponType;
    [SerializeField] public int projecTileSpeed = 0;
    [SerializeField] public BaseStats stats;
    [SerializeField] public CharacterEquipsID EquipItems;


    public GameObject CharacterPrefab => characterPrefab;
    public GameObject WeaponPrefab => weaponPrefab;
    public Sprite CharacterPreview => characterPreviewImage;
    public string Name => characterName;

    private int canSeletable = 1;

    public CharacterAtkRangeType characterAtkRangeType = CharacterAtkRangeType.None;
    public string GetDisplayableName() { return characterName; }
    public Sprite GetDisplayableImage() { return characterPreviewImage; }

    public void SetDisplayableImage(Sprite image) { characterPreviewImage = image; }

    public int GetExtraInfo() { return canSeletable; }

    public void SetExtraInfo(int extraInfo) { canSeletable = extraInfo; }

    public GameObject GetWeaponProjecTile() {  return weaponProjecTile; }

}
