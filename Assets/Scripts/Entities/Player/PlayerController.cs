using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private MobileJoystick joyStick;

    [SerializeField] private float baseMoveSpeed;
    private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;

    private void Awake()
    {
        PlayerStatsManager.onStatsChanged += UpdateStats;
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = joyStick.GetMoveVector() * moveSpeed * Time.deltaTime;
    }

    private void OnDisable()
    {
        PlayerStatsManager.onStatsChanged -= UpdateStats;
    }

    public void UpdateStats()
    {
        float moveSpeedFactorAddend = PlayerStatsManager.instance.GetStatValue(Stat.MoveSpeed) / 100;
        moveSpeed = baseMoveSpeed * (1 + moveSpeedFactorAddend);
    }
}
