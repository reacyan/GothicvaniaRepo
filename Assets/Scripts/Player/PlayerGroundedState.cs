using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerGroundedState : playerstate
{
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword())
        {
            stateMachine.ChangeState(player.aimSword);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            stateMachine.ChangeState(player.CounterAttack);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            stateMachine.ChangeState(player.PrimaryAttack);
        }

        if (!player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.airState);
        }

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.jumpState);
        }
    }

    private bool HasNoSword()
    {
        if (player.sword == null)
        {
            return true;
        }

        player.sword.GetComponent<Sword_Skill_Controller>().ReturnSword();
        return false;
    }
}
