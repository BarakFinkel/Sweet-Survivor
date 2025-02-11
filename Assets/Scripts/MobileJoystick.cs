using UnityEngine;

public class MobileJoystick : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private RectTransform joystickOutline;
    [SerializeField] private RectTransform joystickKnob;

    [Header("Settings")]
    [SerializeField] private float moveFactor;
    private Vector3 clickPosition;
    private Vector3 movement;
    private bool canControl;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HideJoystick();
    }

    // Update is called once per frame
    void Update()
    {
        if (canControl)
        {
            ControlJoystick();
        }
    }

    public void ClickJoystickZoneCallback()
    {
        clickPosition = Input.mousePosition;
        joystickOutline.position = clickPosition;

        ShowJoystick();
    }

    private void ShowJoystick()
    {
        joystickOutline.gameObject.SetActive(true);
        canControl = true;
    }

    private void HideJoystick()
    {
        joystickOutline.gameObject.SetActive(false);
        movement = Vector3.zero;
        canControl = false;
    }

    private void ControlJoystick()
    {
        // We calculate the direction vector of the click / drag
        Vector3 currentPosition = Input.mousePosition;
        Vector3 direction = currentPosition - clickPosition;
     
        // We multiply by a chosen moveFactor for tweaking, and devide by the screen size to make the changes appear responsive.
        float movementMagnitude = direction.magnitude * moveFactor / Screen.width; 

        // We clamp the magnitude between itself and half of the joystick's outline rect.
        movementMagnitude = Mathf.Min(movementMagnitude, joystickOutline.rect.width / 2);

        // We calculate the position in which the knob should be.
        movement = direction.normalized * movementMagnitude;
        Vector3 targetPosition = clickPosition + movement;

        // Finally, we set the knob's position accordingly
        joystickKnob.position = targetPosition;

        if (Input.GetMouseButtonUp(0))
        {
            HideJoystick();
        }
    }

    public Vector3 GetMoveVector()
    {
        return movement;
    }
}
