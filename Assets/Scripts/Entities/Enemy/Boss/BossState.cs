using UnityEngine;

public class BossState
{
    protected Enemy bossBase;
    protected BossStateMachine stateMachine;
    protected float stateTimer;

    public BossState(Enemy _enemyBase, BossStateMachine _stateMachine)
    {
        bossBase = _enemyBase;
        stateMachine = _stateMachine;
    }

    public virtual void Enter()
    {
        
    }

    public virtual void Update()
    {
        if (stateTimer > 0)
        {
            stateTimer = Mathf.Max(stateTimer - Time.deltaTime, 0);
        }
    }

    public virtual void Exit()
    {

    }
}
