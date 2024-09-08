using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform player;

    private Enemy_Skeleton enemy;

    private int moveDir;

    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Skeleton _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy=_enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
        enemy.moveSpeed = enemy.defaultMoveSpeed;
    }
    public override void Update()
    {
        base.Update();

        if (player.position.x > enemy.transform.position.x)
        {
            moveDir = 1;
        }
        else if (player.position.x < enemy.transform.position.x)
        {
            moveDir = -1;
        }

        if (enemy.IsPlayerDetected())//¼ì²âÍæ¼Ò¾àÀë
        {
            stateTimer = enemy.battleTime;

            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)//¼ì²âÍæ¼ÒÓë¹¥»÷¾àÀë
            {
                enemy.moveSpeed = 0;//Õ¾¶¨¹¥»÷
                if (CanAttack())
                {
                    stateMachine.ChangeState(enemy.attackState);
                }
            }
        }
        else
        {

            if (stateTimer < 0 || Vector2.Distance(player.transform.position, enemy.transform.position) > 10) 
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }

        enemy.SetVelocity(enemy.moveSpeed * moveDir, rb.velocity.y);
    }   

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack()
    {
        if (Time.time >= enemy.lastTimeAttacked + enemy.attackCooldown)
        {
            enemy.lastTimeAttacked = Time.time;
            return true;
        }
        return false;
    }
}
