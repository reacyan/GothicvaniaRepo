using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : playerstate
{
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.skill.dash.CloneOnDash();

        player.stats.MakeInvincible(true);

        stateTimer = player.dashDuration;
    }

    public override void Exit()
    {
        base.Exit();

        player.stats.MakeInvincible(false);

        player.skill.dash.CloneOnOver();
        player.SetVelocity(0, rb.velocity.y);
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(player.dashSpeed * player.dashDir, 0);

        if (!player.IsGroundDetected() && player.IsWallDetected())
        {
            stateMachine.ChangeState(player.wallSlideState);
        }

        if (stateTimer < 0)
        {
            stateMachine.ChangeState(player.idleState);
        }

        player.fx.CreateShadow();
    }
}
