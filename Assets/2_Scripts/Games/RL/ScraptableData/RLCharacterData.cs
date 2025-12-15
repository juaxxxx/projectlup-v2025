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
    public int Exp;
}

//아이템 ID로 할당 & 조회
[System.Serializable]
public struct EquipmentData
{
    public int Weapon;
    public int Helmet;
    public int Armor;
    public int Gloves;
    public int Shoes;
    public int Accessory;
}

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class RLCharacterData : ScriptableObject, IDisplayable
{
    [SerializeField] private string characterName;
    [SerializeField] private Sprite characterPreviewImage;
    [SerializeField] private GameObject characterPrefab;
    [SerializeField] private GameObject weaponPrefab;
    [SerializeField] private GameObject weaponProjecTile = null;
    [SerializeField] public int projecTileSpeed = 0;
    [SerializeField] public BaseStats stats;
    [SerializeField] public EquipmentData EquipItems;


    public GameObject CharacterPrefab => characterPrefab;
    public GameObject WeaponPrefab => weaponPrefab;
    public Sprite CharacterPreview => characterPreviewImage;
    public string Name => characterName;

    private int canSeletable = 1;

    public CharacterType characterType = CharacterType.None;
    public string GetDisplayableName() { return characterName; }
    public Sprite GetDisplayableImage() { return characterPreviewImage; }

    public void SetDisplayableImage(Sprite image) { characterPreviewImage = image; }

    public int GetExtraInfo() { return canSeletable; }

    public void SetExtraInfo(int extraInfo) { canSeletable = extraInfo; }

    public GameObject GetWeaponProjecTile() {  return weaponProjecTile; }

}
