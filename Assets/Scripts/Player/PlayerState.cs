using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerstate
{
    //声明玩家状态机
    protected PlayerStateMachine stateMachine;
    //声明玩家状态
    protected Player player;

    protected Rigidbody2D rb;
    //状态命名
    private string animBoolName;

    protected float xInput;
    protected float yInput;

    protected float stateTimer;
    protected bool triggerCalled;

    //构造玩家状态机
    public playerstate(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.stateMachine = _stateMachine;
        this.player = _player;
        this.animBoolName = _animBoolName;
    }


    //当玩家进入状态时
    public virtual void Enter()
    {
        //设置动画布尔值
        player.anim.SetBool(animBoolName, true);   
        rb=player.rb;
        triggerCalled = false;
    }

    //当玩家处于状态时
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.anim.SetFloat("yVelocity", rb.velocity.y);
    }

    //当玩家退出状态时
    public virtual void Exit()
    {
        player.SetVelocity(0, rb.velocity.y);
        player.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }
}
