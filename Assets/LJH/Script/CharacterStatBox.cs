using TMPro;
using UnityEngine;

public class CharacterStatBox : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public CharacterStatPriviewBox LevelIconBox;
    public CharacterStatPriviewBox AtkIconBox;
    public CharacterStatPriviewBox HPIconBox;

    public TextMeshProUGUI SkillText;

    public void UpdateStatBox(CharacterData characterData)
    {
        LevelIconBox.SetLevel(1, 5);
        AtkIconBox.SetAtk(characterData.stats.Attack);
        HPIconBox.SetHP(characterData.stats.Hp);
    }
}


