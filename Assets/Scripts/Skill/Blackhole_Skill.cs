using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill : Skill
{
    [SerializeField] private GameObject BlackholePrefab;

    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private float blackholeDuration;
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

    public bool SkillCompleted()
    {

        if (!currentBlackhole)
        {
            return false;
        }

        if (currentBlackhole.playerCanExitState)
        {
            currentBlackhole = null;
            return true;
        }

        return false;
    }
}
