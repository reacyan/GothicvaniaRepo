using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerIdleState : PlayerGroundedState  
{
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        //检测是否撞墙
        if (xInput != 0 && !player.IsWallDetected() && !player.isBusy)
        {
            stateMachine.ChangeState(player.moveState);
        }
        //检测撞墙时输入方向是否与墙面相反
        else if (player.IsWallDetected())
        {
            if (xInput == -player.facingDir && !player.isBusy) 
            {
                stateMachine.ChangeState(player.moveState);
            }
        }
    }
}
