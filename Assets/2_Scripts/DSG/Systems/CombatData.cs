using UnityEngine;

[CreateAssetMenu(fileName = "Combat Data", menuName = "Scriptable Object/Combat Data", order = int.MaxValue)]
public class CombatData : ScriptableObject
{
    [SerializeField]
    private string characterName;
    public string CharacterName { get { return characterName; } }
    [SerializeField]
    private int contribution;
    public int Contribution { get { return contribution; } }
    [SerializeField]
    private Sprite icon;
    public Sprite Icon { get { return icon; } }
}
