using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private float moveSpeed = 3.5f;
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
        xinput = inputAction.ReadValue<float>();
        rb.linearVelocity = new Vector2(xinput * moveSpeed, rb.linearVelocity.y);
    }
}
