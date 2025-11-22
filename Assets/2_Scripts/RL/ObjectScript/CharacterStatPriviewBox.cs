using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStatPriviewBox : MonoBehaviour
{
    public GameObject Icon;
    public Image PercentageBackGround;
    public Image PercentageBar;
    public TextMeshProUGUI StatText;
    public TextMeshProUGUI StatText2;

    public StringBuilder sb = new StringBuilder();

    public void SetLevel(int currentLevel, int maxLevel)
    {
        PercentageBar.fillAmount = (float)currentLevel / maxLevel;

        sb.Clear();
        sb.Append(currentLevel);
        sb.Append('/');
        sb.Append(maxLevel);
        StatText.SetText(sb);
    }

    public void SetAtk(int atk)
    {
        sb.Clear();
        sb.Append(atk);
        StatText2.SetText(sb);
    }

    public void SetHP(int hp)
    {
        sb.Clear();
        sb.Append(hp);
        StatText2.SetText(sb);
    }
}
