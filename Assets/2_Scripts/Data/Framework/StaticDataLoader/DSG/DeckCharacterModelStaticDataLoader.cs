using UnityEngine;

[CreateAssetMenu(fileName = "DeckCharacterModelStaticData", menuName = "Scriptable Objects/DeckCharacterModelStaticData")]
public class DeckCharacterModelStaticDataLoader : BaseStaticDataLoader<DeckCharacterModelStaticData>
{
    protected override string CSV_URL =>
        "https://docs.google.com/spreadsheets/d/1tvPilM12p7L2pu0WucoM4P9QVry-oFme41mZH6xRJB4/export?format=csv&gid=56059235#gid=56059235";
}