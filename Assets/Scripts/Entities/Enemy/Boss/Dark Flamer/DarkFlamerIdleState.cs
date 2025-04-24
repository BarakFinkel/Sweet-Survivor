using UnityEngine;

public class DarkFlamerIdleState : BossState
{
    private DarkFlamer darkFlamer;

    public DarkFlamerIdleState(Enemy _bossBase, BossStateMachine _stateMachine, DarkFlamer _darkFlamer) : base(_bossBase, _stateMachine)
    {
        darkFlamer = _darkFlamer;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = Random.Range(1.0f, darkFlamer.MaxIdleDuration);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer == 0)
            darkFlamer.StateMachine.ChangeState(darkFlamer.MoveState);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
