using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dodge_Skill : Skill
{
    protected override void Start()
    {
        base.Start();

        baseSkillUnlockButton[0].GetComponent<Button>().onClick.AddListener(UnlockBaseSkill);
    }

    public void CreateMirageOnDodge()
    {
        SkillManager.instance.clone.CreateClone(player.transform, new Vector3(2 * player.facingDir, 0));
    }

    public void DodgeCooldown()
    {
        if (player.stats.onSkillBeUse != null)
        {
            player.stats.onSkillBeUse(skillSprite, skillType, cooldown);
        }
    }
}
