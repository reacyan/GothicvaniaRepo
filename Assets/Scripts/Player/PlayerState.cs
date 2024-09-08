using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class playerstate
{
    //�������״̬��
    protected PlayerStateMachine stateMachine;
    //�������״̬
    protected Player player;

    protected Rigidbody2D rb;
    //״̬����
    private string animBoolName;

    protected float xInput;
    protected float yInput;

    protected float stateTimer;
    protected bool triggerCalled;

    //�������״̬��
    public playerstate(Player _player, PlayerStateMachine _stateMachine, string _animBoolName)
    {
        this.stateMachine = _stateMachine;
        this.player = _player;
        this.animBoolName = _animBoolName;
    }


    //����ҽ���״̬ʱ
    public virtual void Enter()
    {
        //���ö�������ֵ
        player.anim.SetBool(animBoolName, true);   
        rb=player.rb;
        triggerCalled = false;
    }

    //����Ҵ���״̬ʱ
    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;

        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        player.anim.SetFloat("yVelocity", rb.velocity.y);
    }

    //������˳�״̬ʱ
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
