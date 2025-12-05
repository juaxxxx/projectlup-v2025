using UnityEngine;

[CreateAssetMenu(fileName = "DeckCharacterAnimationStaticData", menuName = "Scriptable Objects/DeckCharacterAnimationStaticData")]
public class DeckCharacterAnimationStaticDataLoader : BaseStaticDataLoader<DeckCharacterAnimationStaticData>
{
    protected override string CSV_URL =>
        "https://docs.google.com/spreadsheets/d/1tvPilM12p7L2pu0WucoM4P9QVry-oFme41mZH6xRJB4/export?format=csv&gid=160148361#gid=160148361";
}