using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 3.5f;
    [SerializeField] private float jumpForce = 8f;
    private float xinput;
    private InputAction inputAction;

    void Awake()
    {
        inputAction = new InputAction(
            type: InputActionType.Value,
            binding: "<Keyboard>/a"
        );
        inputAction.AddCompositeBinding("1DAxis")
            .With("Negative", "<Keyboard>/a")
            .With("Positive", "<Keyboard>/d");
        inputAction.Enable();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        HandleInput();
        Move(xinput);
    }

    private void HandleInput()
    {
        xinput = inputAction.ReadValue<float>();

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    private void Move(float by)
    {
        rb.linearVelocity = new Vector2(by * moveSpeed, rb.linearVelocity.y);
    }
}
