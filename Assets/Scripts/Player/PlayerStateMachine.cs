using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    //声明当前状态
    public playerstate currentState { get; private set; }
    
    //初始化玩家状态
    public void Initialize(playerstate _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    //改变玩家状态
    public void ChangeState(playerstate _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
