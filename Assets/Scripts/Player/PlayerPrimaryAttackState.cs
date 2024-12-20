using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrimaryAttackState : playerstate
{
    private int comboCounter;

    private float lastTimeAttacked;
    private float comboWindow = .2f;


    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        ItemData_Equipment equipmentEffect = Inventory.instance.GetEquipment(EquipmentType.weapon);

        //Debug.Log(equipmentEffect.itemName);
        xInput = 0;

        if (comboCounter > 2 || Time.time >= lastTimeAttacked + comboWindow)
        {
            comboCounter = 0;
        }

        else if (comboCounter == 2 && equipmentEffect != null)//当攻击连段达到第三段时
        {
            if (equipmentEffect.itemName == "IceAndFire Swrod")
            {
                equipmentEffect.Effect(player.GetComponent<CharacterStats>().SkillHitNearsTarget(player.skillCheckRadius / 5));//调用装备特效
            }
        }

        player.anim.SetInteger("ComboCounter", comboCounter);

        float attackDir = player.facingDir;

        if (xInput != 0)
        {
            attackDir = xInput;
        }

        player.SetVelocity(player.attackmovement[comboCounter].x * attackDir, player.attackmovement[comboCounter].y);

        stateTimer = .1f;
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .15f);

        comboCounter++;
        lastTimeAttacked = Time.time;
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
        if (stateTimer < 0)
        {
            player.SetZeroVelocity();
        }
    }
}
