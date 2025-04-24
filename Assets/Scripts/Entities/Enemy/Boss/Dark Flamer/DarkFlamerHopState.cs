using System;
using UnityEngine;

public class DarkFlamerHopState : BossState
{
    private DarkFlamer darkFlamer;
    private Player player;
    private Vector2 targetPosition;
    private bool targetPositionAssigned = false;
    private bool exclamationMarkShown = false;

    public DarkFlamerHopState(Enemy _bossBase, BossStateMachine _stateMachine, DarkFlamer _darkFlamer) : base(_bossBase, _stateMachine)
    {
        darkFlamer = _darkFlamer;
        player = Player.instance;
    }

    public override void Enter()
    {
        base.Enter();

        TryAssignPlayerAndTarget();
    }

    public override void Update()
    {
        base.Update();

        if (!targetPositionAssigned)
        {
            TryAssignPlayerAndTarget();
            return;
        }

        if (stateTimer > 0)
        {
            TryTurnOnExclamationMark();
        }
        else
        {
            TryTurnOffExclamationMark();

            darkFlamer.transform.position = Vector2.MoveTowards(darkFlamer.transform.position, targetPosition, darkFlamer.HopSpeed * Time.deltaTime);

            if (Vector2.Distance(darkFlamer.transform.position, targetPosition) < 0.01f)
                darkFlamer.StateMachine.ChangeState(darkFlamer.SpawnMinionsState);
        }
    }

    public override void Exit()
    {
        base.Exit();

        targetPositionAssigned = false;
    }

    private void TryAssignPlayerAndTarget()
    {
        if (player == null)
            player = Player.instance;

        if (player != null)
        {
            targetPosition = GetPlayerPosition();
            targetPositionAssigned = true;
            stateTimer = darkFlamer.HopWarningDuration;
        }
    }

    private Vector2 GetPlayerPosition()
    {
        Vector2 targetPosition = player.transform.position;

        float xBound = MapBoundsHolder.instance.GetXBound();
        float yBound = MapBoundsHolder.instance.GetYBound();

        targetPosition.x = Mathf.Clamp(targetPosition.x, -xBound, xBound);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -yBound, yBound);

        return targetPosition;
    }

    private void TryTurnOnExclamationMark()
    {
        if (!exclamationMarkShown)
        {
            darkFlamer.exclamationMark.enabled = true;
            exclamationMarkShown = true;
        }
    }

    private void TryTurnOffExclamationMark()
    {
        if (exclamationMarkShown)
        {
            darkFlamer.exclamationMark.enabled = false;
            exclamationMarkShown = false;
        }
    }
}
