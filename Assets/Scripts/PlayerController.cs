using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private MobileJoystick joyStick;
    [SerializeField] private float moveSpeed;
    private Rigidbody2D rb;

    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        rb.linearVelocity = joyStick.GetMoveVector() * moveSpeed * Time.deltaTime;
    }
}
