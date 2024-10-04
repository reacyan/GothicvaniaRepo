using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerBlackholeState : playerstate
{

    private float flyTime = .4f;
    public bool skillUsed = false;

    private float defaultGravity;

    public PlayerBlackholeState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = flyTime;
        player.skill.blackhole.blackholeCooldownTimer = player.skill.blackhole.blackholeCooldown;
        defaultGravity = player.rb.gravityScale;
        skillUsed = false;
        rb.gravityScale = 0;
    }

    public override void Exit()
    {
        base.Exit();

        player.rb.gravityScale = defaultGravity;

        player.MakeTransprent(false);
        //We exit in blackhole skills controller when all of the attacks are over
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer > 0)
        {
            rb.velocity = new Vector2(0, 15);//腾空
        }
        else
        {
            rb.velocity = new Vector2(0, 0);

            if (!skillUsed)
            {
                if (player.skill.blackhole.CanUseSkill())
                {
                    skillUsed = true;
                }
            }
        }

        if (player.skill.blackhole.SkillCompleted())
        {
            stateMachine.ChangeState(player.airState);
        }

    }
}
