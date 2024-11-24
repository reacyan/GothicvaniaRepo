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
        DashCooldown();

        if (baseSkillUnlockButton[1].unlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
        }
    }

    private void DashCooldown()
    {
        if (player.stats.onSkillBeUse != null)
        {
            player.stats.onSkillBeUse(skillSprite, skillType, cooldown);
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
