using LUP.DSG;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class FilterButtonBase : MonoBehaviour
{
    [SerializeField]
    protected Button filterButton;
    [SerializeField]
    protected TextMeshProUGUI filterText;
    protected bool isSelected;

    public abstract void Init(CharacterFilterPanel panel, System.Enum enumVal);
}
