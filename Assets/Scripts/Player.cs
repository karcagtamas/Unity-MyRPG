using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    [Header("Movement details")]
    [SerializeField] protected float moveSpeed = 3.5f;
    [SerializeField] private float jumpForce = 8f;

    private float xinput;
    private bool canJump = true;

    private InputAction inputAction;

    protected override void Awake()
    {
        base.Awake();
        inputAction = new InputAction(
            type: InputActionType.Value,
            binding: "<Keyboard>/a"
        );
        inputAction.AddCompositeBinding("1DAxis")
            .With("Negative", "<Keyboard>/a")
            .With("Positive", "<Keyboard>/d");
        inputAction.Enable();
    }

    protected override void Update()
    {
        base.Update();
        HandleInput();
    }

    protected override void HandleMovement()
    {
        Move(canMove ? xinput : 0, moveSpeed);
    }

    protected override void HandleAnimations()
    {
        base.HandleAnimations();
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    private void HandleInput()
    {
        xinput = inputAction.ReadValue<float>();

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            TryToJump();
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            HandleAttack();
        }
    }

    public override void EnableMovement(bool enable)
    {
        base.EnableMovement(enable);
        canJump = enable;
    }

    private void TryToJump()
    {
        if (isGrounded && canJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
}
