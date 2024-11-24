using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{
    public override void UseSkill()
    {
        base.UseSkill();

    }

    protected override void Start()
    {
        base.Start();

        baseSkillUnlockButton[0].GetComponent<Button>().onClick.AddListener(UnlockBaseSkill);

    }

    public void CloneOnDash()
    {
        if (baseSkillUnlockButton[1].unlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
        }
    }

    public void CloneOnOver()
    {
        if (baseSkillUnlockButton[2].unlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
        }
    }
}
