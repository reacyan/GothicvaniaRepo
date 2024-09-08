using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    protected float cooldown;
    [SerializeField] protected float cooldownTimer;

    protected Player player;

    protected virtual void Start()
    {
        player = PlayerManager.instance.player;
    }

    protected virtual void Update()
    {
        cooldown -= Time.deltaTime;
    }


    public virtual bool CanUseSkill()
    {
        if (cooldown < 0)
        {
            UseSkill();
            cooldown = cooldownTimer;
            return true;
        }

        return false;
    }

    public virtual void UseSkill()
    {

    }
}
