using UnityEngine;

public class DarkFlamerFlameBarrageState : BossState
{
    private DarkFlamer darkFlamer;
    private float intervalTimer = 0;
    private bool isDiagonalBarrage = true;
    private Vector2[] diagonalDirections = { new Vector2(1,1).normalized, new Vector2(-1,1).normalized, new Vector2(-1,-1).normalized, new Vector2(1,-1).normalized };
    private Vector2[] straightDirections = { Vector2.up, Vector2.right, Vector2.down, Vector2.left };

    public DarkFlamerFlameBarrageState(Enemy _bossBase, BossStateMachine _stateMachine, DarkFlamer _darkFlamer) : base(_bossBase, _stateMachine)
    {
        darkFlamer = _darkFlamer;
    }

    public override void Enter()
    {
        base.Enter();
        
        stateTimer = darkFlamer.FlameBarrageDuration;
    }

    public override void Update()
    {
        base.Update();

        intervalTimer = Mathf.Max(intervalTimer - Time.deltaTime, 0);

        if (intervalTimer == 0)
            HandleShooting();

        if (stateTimer == 0)
            darkFlamer.StateMachine.ChangeState(darkFlamer.MoveState);
    }

    public override void Exit()
    {
        base.Exit();

        isDiagonalBarrage = true;
        intervalTimer = 0;
    }

    # region Barrage Logic

    private void HandleShooting()
    {
        if (isDiagonalBarrage)
            DiagonalBarrage();
        else
            StraightBarrage();

        darkFlamer.PlayAttackSound();

        intervalTimer = darkFlamer.FlameBarrageInterval;
        isDiagonalBarrage = !isDiagonalBarrage;
    }

    private void DiagonalBarrage()
    {
        ShootBarrage(diagonalDirections);
    }

    private void StraightBarrage()
    {
        ShootBarrage(straightDirections);
    }

    private void ShootBarrage(Vector2[] barrageDirections)
    {
        for (int i = 0; i < barrageDirections.Length; i++)
        {
            darkFlamer.ProjectilesManager.UseProjectile
            (
                darkFlamer.transform.position,
                barrageDirections[i],
                darkFlamer.ProjectileVelocity,
                darkFlamer.ProjectileRange,
                1,
                darkFlamer.damage,
                false
            );            
        }
    }

    # endregion
}
