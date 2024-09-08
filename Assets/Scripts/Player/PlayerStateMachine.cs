using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    //������ǰ״̬
    public playerstate currentState { get; private set; }
    
    //��ʼ�����״̬
    public void Initialize(playerstate _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    //�ı����״̬
    public void ChangeState(playerstate _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }
}
