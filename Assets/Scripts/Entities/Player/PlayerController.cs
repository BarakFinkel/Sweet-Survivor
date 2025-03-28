using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour, IPlayerStatsDependency
{
    [SerializeField] private MobileJoystick joyStick;
    
    [SerializeField] private float baseMoveSpeed;
    private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.linearVelocity = joyStick.GetMoveVector() * moveSpeed * Time.deltaTime;
    }

    public void UpdateStats(PlayerStatsManager playerStatsManager)
    {
        float moveSpeedFactorAddend = playerStatsManager.GetStatValue(Stat.MoveSpeed) / 100;
        moveSpeed = baseMoveSpeed * (1 + moveSpeedFactorAddend);
    }
}
