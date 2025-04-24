using UnityEngine;

public class DarkFlamerMoveState : BossState
{
    private DarkFlamer darkFlamer;

    public DarkFlamerMoveState(Enemy _bossBase, BossStateMachine _stateMachine, DarkFlamer _darkFlamer) : base(_bossBase, _stateMachine)
    {
        darkFlamer = _darkFlamer;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = Random.Range(1.0f, darkFlamer.MaxMoveDuration);
        darkFlamer.EnableMovement();
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer == 0)
        {
            float distanceToPlayer = Vector2.Distance(Player.instance.transform.position, darkFlamer.transform.position);
            float chanceVar = Random.Range(0.0f, 1.0f);
            
            // If close enough to the player, execute one of 3 possible attacks.
            if (distanceToPlayer <= darkFlamer.CloseRange)
            {
                if ( 0.0f <= chanceVar && chanceVar < 1.0f/3.0f )
                    darkFlamer.StateMachine.ChangeState(darkFlamer.BasicAttackState);
                else if ( 1.0f/3.0f <= chanceVar && chanceVar < 2.0f/3.0f )
                    darkFlamer.StateMachine.ChangeState(darkFlamer.SpawnMinionsState);
                else
                    darkFlamer.StateMachine.ChangeState(darkFlamer.FlameBarrageState);
            }
            else // Else, execute one of another set of 3 possible attacks.
            {
                if ( 0.0f <= chanceVar && chanceVar < 1.0f/3.0f )
                    darkFlamer.StateMachine.ChangeState(darkFlamer.BasicAttackState);
                else if ( 1.0f/3.0f <= chanceVar && chanceVar < 2.0f/3.0f )
                    darkFlamer.StateMachine.ChangeState(darkFlamer.HopState);
                else
                    darkFlamer.StateMachine.ChangeState(darkFlamer.SpawnMinionsState);             
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

        darkFlamer.DisableMovement();
    }
}
