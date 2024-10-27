using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill : Skill
{
    [SerializeField] private GameObject BlackholePrefab;

    [SerializeField] private float maxSize;//黑洞大小
    [SerializeField] private float growSpeed;//生成速度
    [SerializeField] private float shrinkSpeed;//缩小速度
    [SerializeField] private float blackholeDuration;//持续时间
    public float blackholeCooldown;
    public float blackholeCooldownTimer;

    [Space]
    [SerializeField] private float CloneAttackCooldown;
    [SerializeField] private int amountOfAttack;

    Blackhole_Skill_Controller currentBlackhole;

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackhole = Instantiate(BlackholePrefab, player.transform.position, Quaternion.identity);

        currentBlackhole = newBlackhole.GetComponent<Blackhole_Skill_Controller>();

        currentBlackhole.SetupBlackhole(maxSize, growSpeed, shrinkSpeed, CloneAttackCooldown, amountOfAttack, blackholeDuration);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();

        if (!currentBlackhole)
        {
            blackholeCooldownTimer -= Time.deltaTime;
        }
    }

    public bool SkillCompleted()//技能结束
    {
        if (currentBlackhole.playerCanExitState)//有报错，黑洞在判断之前被销毁
        {
            currentBlackhole = null;
            return true;
        }

        if (currentBlackhole == null)
        {
            return false;
        }

        return false;
    }

    public float GetBlackholeRadius()
    {
        return maxSize / 2;
    }
}
