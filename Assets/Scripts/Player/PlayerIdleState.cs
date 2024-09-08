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

        //����Ƿ�ײǽ
        if (xInput != 0 && !player.IsWallDetected() && !player.isBusy)
        {
            stateMachine.ChangeState(player.moveState);
        }
        //���ײǽʱ���뷽���Ƿ���ǽ���෴
        else if (player.IsWallDetected())
        {
            if (xInput == -player.facingDir && !player.isBusy) 
            {
                stateMachine.ChangeState(player.moveState);
            }
        }
    }
}
