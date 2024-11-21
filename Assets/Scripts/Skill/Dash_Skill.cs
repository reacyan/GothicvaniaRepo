using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dash_Skill : Skill
{

    [Header("Dash")]
    public bool dashUnlocked;
    [SerializeField] private SkillTree_UI dashUnlockButton;

    [Header("Dash")]
    public bool cloneOnDashUnlocked;
    [SerializeField] private SkillTree_UI cloneOnDashUnlockedButton;

    [Header("Dash")]
    public bool cloneOnArrivalDashUnlocked;
    [SerializeField] private SkillTree_UI cloneOnArrivalDashUnlockedButton;

    public override void UseSkill()
    {
        base.UseSkill();

    }

    protected override void Start()
    {
        base.Start();
        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockDash);
        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOnDash);
        dashUnlockButton.GetComponent<Button>().onClick.AddListener(UnlockCloneOArrivalDash);
    }

    private void UnlockDash()
    {
        Debug.Log(dashUnlockButton.unlocked);
        if (dashUnlockButton.unlocked)
        {
            Debug.Log("unlocked this");

            dashUnlocked = true;
        }
    }

    private void UnlockCloneOnDash()
    {
        if (cloneOnDashUnlockedButton.unlocked)
        {
            cloneOnDashUnlocked = true;
        }
    }

    private void UnlockCloneOArrivalDash()
    {
        if (cloneOnArrivalDashUnlockedButton.unlocked)
        {
            cloneOnArrivalDashUnlocked = true;
        }
    }


    public void CloneOnDash()
    {
        if (cloneOnDashUnlockedButton.unlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
        }
    }

    public void CloneOnOver()
    {
        if (cloneOnArrivalDashUnlockedButton.unlocked)
        {
            SkillManager.instance.clone.CreateClone(player.transform, Vector3.zero);
        }
    }
}
