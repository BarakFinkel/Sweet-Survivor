using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private MobileJoystick joyStick;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody2D rb;

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.linearVelocity = joyStick.GetMoveVector() * moveSpeed * Time.deltaTime;
    }
}
