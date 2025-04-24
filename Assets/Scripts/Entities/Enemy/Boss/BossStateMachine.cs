using UnityEngine;

public class BossStateMachine
{
    public BossState CurrentState { get; private set; }

    public void Initiallize(BossState _startState)
    {
        CurrentState = _startState;
        CurrentState.Enter();
    }

    public void ChangeState(BossState _newState)
    {
        CurrentState.Exit();
        CurrentState = _newState;
        CurrentState.Enter();
    }
}
