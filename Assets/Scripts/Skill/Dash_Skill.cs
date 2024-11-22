using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{

    // [Header("Dash")]
    // [SerializeField] private SkillTree_UI cloneOnDashUnlockedButton;

    // [Header("Dash")]
    // [SerializeField] private SkillTree_UI cloneOnArrivalDashUnlockedButton;

    public override void UseSkill()
    {
        base.UseSkill();

    }

    protected override void Start()
    {
        base.Start();

        baseSkillUnlockButton[0].GetComponent<Button>().onClick.AddListener(UnlockBaseSkill);

        // cloneOnDashUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        // cloneOnArrivalDashUnlockedButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOArrivalDash);
    }



    // private void UnlockCloneOnDash()
    // {
    //     if (cloneOnDashUnlockedButton.unlocked)
    //     {
    //         cloneOnDashUnlocked = true;
    //     }
    // }

    // private void UnlockCloneOArrivalDash()
    // {
    //     if (cloneOnArrivalDashUnlockedButton.unlocked)
    //     {
    //         cloneOnArrivalDashUnlocked = true;
    //     }
    // }

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
