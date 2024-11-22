using UnityEngine;
using UnityEngine.UI;


public class Parry_Skill : Skill
{
    [SerializeField] private float restoreHealthPerentage;
    public override void UseSkill()
    {
        base.UseSkill();

        if (baseSkillUnlockButton[1].unlocked)
        {
            int restoreAmount = Mathf.RoundToInt(player.stats.GetMaxHealthValue() * restoreHealthPerentage);
            player.stats.IncreaseHealth(restoreAmount);
        }
    }

    protected override void Start()
    {
        base.Start();

        baseSkillUnlockButton[0].GetComponent<Button>().onClick.AddListener(UnlockBaseSkill);
    }

    public void MakeMirageOnParry(Transform _respawnTransform)
    {
        if (baseSkillUnlockButton[2].unlocked)
        {
            SkillManager.instance.clone.CreateCloneOnWithParry(_respawnTransform);
        }
    }
}
