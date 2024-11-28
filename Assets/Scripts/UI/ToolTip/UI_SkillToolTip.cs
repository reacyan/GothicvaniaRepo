using TMPro;
using UnityEngine;

public class UI_SkillToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillName;
    [SerializeField] private TextMeshProUGUI skillCost;

    public void ShowSkillToolTip(string _skillDescription, string _skillName,int _skillCost)
    {
        skillDescription.text = _skillDescription;
        skillName.text = _skillName;
        skillCost.text ="Cost: " + _skillCost.ToString();

        AdjustPosition();

        gameObject.SetActive(true);
    }

    public void HideSkillToolTip()
    {
        gameObject.SetActive(false);
    }
}
