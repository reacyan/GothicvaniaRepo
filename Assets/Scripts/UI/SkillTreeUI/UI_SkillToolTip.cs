using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillDescription;
    [SerializeField] private TextMeshProUGUI skillName;


    public void ShowSkillToolTip(string _skillDescription, string _skillName)
    {
        skillDescription.text = _skillDescription;
        skillName.text = _skillName;

        gameObject.SetActive(true);
    }

    public void HideSkillToolTip()
    {
        gameObject.SetActive(false);
    }
}
