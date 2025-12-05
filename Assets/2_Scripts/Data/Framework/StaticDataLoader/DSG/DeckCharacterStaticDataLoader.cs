using UnityEngine;

[CreateAssetMenu(fileName = "DeckCharacterStaticData", menuName = "Scriptable Objects/DeckCharacterStaticData")]
public class DeckCharacterStaticDataLoader : BaseStaticDataLoader<DeckCharacterStaticData>
{
    protected override string CSV_URL =>
        "https://docs.google.com/spreadsheets/d/1tvPilM12p7L2pu0WucoM4P9QVry-oFme41mZH6xRJB4/export?format=csv&gid=1245166024#gid=1245166024";
}
