using UnityEngine;

public class DarkFlamerBasicAttackState : BossState
{
    private DarkFlamer darkFlamer;

    public DarkFlamerBasicAttackState(Enemy _bossBase, BossStateMachine _stateMachine, DarkFlamer _darkFlamer) : base(_bossBase, _stateMachine)
    {
        darkFlamer = _darkFlamer;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = darkFlamer.AttackDuration;
    }

    public override void Update()
    {
        base.Update();
        
        TryAttack();

        if (stateTimer == 0)
            darkFlamer.StateMachine.ChangeState(darkFlamer.IdleState);
    }

    public override void Exit()
    {
        base.Exit();
    }

    private void TryAttack()
    {
        if (darkFlamer.CanAttack())
            darkFlamer.Attack();
    }
}
