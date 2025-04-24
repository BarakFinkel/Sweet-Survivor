using UnityEngine;

public class DarkFlamerSpawnMinionsState : BossState
{
    private DarkFlamer darkFlamer;
    Vector2[] directions = { Vector2.up, Vector2.right, Vector2.down, Vector2.left };

    public DarkFlamerSpawnMinionsState(Enemy _bossBase, BossStateMachine _stateMachine, DarkFlamer _darkFlamer) : base(_bossBase, _stateMachine)
    {
        darkFlamer = _darkFlamer;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = darkFlamer.MinionsSpawnDuration;
        ShootMinions();
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

    private void ShootMinions()
    {
        for (int i = 0; i < directions.Length; i++)
        {
            darkFlamer.MinionProjectilesManager.UseProjectile
            (
                darkFlamer.transform.position,
                directions[i],
                darkFlamer.MinionProjectileVelocity,
                darkFlamer.ProjectileRange,
                1,
                darkFlamer.damage,
                false
            );
        }

        darkFlamer.PlayAttackSound();
    }
}
