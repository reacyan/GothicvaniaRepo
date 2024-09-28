using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Player : Entity
{
    [Header("Attack details")]
    public Vector2[] attackmovement;
    public float counterAttackDuration = .2f;
    public bool isBusy { get; private set; }
    [Header("Move info")]
    public float moveSpeed;
    public float jumpForce;
    public float swordReturnImpact;


    [Header("Dash Info")]
    public float dashSpeed;
    public float dashDuration;
    public float dashDir { get; private set; }


    public SkillManager skill { get; private set; }
    public GameObject sword { get; private set; }

    //?????
    #region States
    public PlayerStateMachine stateMachine { get; private set; }

    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerPrimaryAttackState PrimaryAttack { get; private set; }
    public PlayerCounterAttackState CounterAttack { get; private set; }

    public PlayerAimSwordState aimSword { get; private set; }
    public PlayerCatchSwordState catchSword { get; private set; }
    public PlayerBlackholeState blackhole { get; private set; }
    #endregion


    protected override void Awake()
    {
        base.Awake();

        //?????
        stateMachine = new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");

        PrimaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        CounterAttack = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");

        aimSword = new PlayerAimSwordState(this, stateMachine, "AimSword");
        catchSword = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
        blackhole = new PlayerBlackholeState(this, stateMachine, "Jump");
    }

    protected override void Start()
    {
        base.Start();


        skill = SkillManager.instance;
        stateMachine.Initialize(idleState);
    }


    protected override void Update()
    {
        base.Update();

        stateMachine.currentState.Update();
        CheckForDashInput();
        CheckForBlackHoleInput();

        if (Input.GetKeyDown(KeyCode.T)&&skill)
        {
            skill.crystal.CanUseSkill();
        }
    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void CatchTheSword()
    {
        stateMachine.ChangeState(catchSword);
        Destroy(sword);
    }


    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;
        yield return new WaitForSeconds(_seconds);
        isBusy = false;
    }


    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();

    public void CheckForDashInput()
    {
        if (IsWallDetected())
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill() && rb.gravityScale == 3.5f)
        {
            dashDir = Input.GetAxisRaw("Horizontal");

            if (dashDir == 0)
            {
                dashDir = facingDir;
            }
            stateMachine.ChangeState(dashState);
        }
    }


    public void CheckForBlackHoleInput()
    {
        if (Input.GetKeyDown(KeyCode.R) && skill.blackhole.blackholeCooldownTimer < 0)
        {
            stateMachine.ChangeState(blackhole);
        }
    }
}
