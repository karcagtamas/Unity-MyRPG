using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Entity : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;
    protected Collider2D col;
    protected SpriteRenderer sr;
    private InputAction inputAction;

    [Header("Health")]
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private int currentHealth = 1;
    [SerializeField] private Material damageMaterial;
    [SerializeField] private float damageFeedbackDuration = .2f;
    private Coroutine damageFeedbackCoroutine;


    [Header("Attack details")]
    [SerializeField] protected float attackRadius;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected LayerMask whatIsTarget;


    [Header("Movement details")]
    [SerializeField] protected float moveSpeed = 3.5f;
    [SerializeField] private float jumpForce = 8f;
    protected int facingDir = 1;
    private float xinput;
    protected bool facingRight = true;
    protected bool canMove = true;
    private bool canJump = true;

    [Header("Collision details")]
    [SerializeField] private float groundCheckDistance = 1.35f;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;


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
        col = GetComponent<Collider2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        
        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        HandleCollision();
        HandleInput();
        HandleMovement();
        HandleAnimations();
        HandleFlip();
    }

    public void DamageTargets()
    {
        var targetColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, whatIsTarget);
        
        foreach (var targetCollider in targetColliders)
        {
            targetCollider.GetComponent<Entity>().TakeDamage();
        }
    }

    private void TakeDamage()
    {
        currentHealth -= 1;
        DamageFeedback();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void DamageFeedback()
    {
        if (damageFeedbackCoroutine != null)
        {
            StopCoroutine(damageFeedbackCoroutine);
        }
        damageFeedbackCoroutine = StartCoroutine(DamageFeedbackCoroutine());
    }

    public void EnableMovementAndJump(bool enable)
    {
        canMove = enable;
        canJump = enable;
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

    protected void HandleAnimations()
    {
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    protected virtual void HandleMovement()
    {
        Move(canMove ? xinput : 0);
    }

    protected virtual void HandleFlip()
    {
        // Should flip the character to right
        if (rb.linearVelocity.x > 0 && !facingRight)
        {
            Flip();
        }
        // Should flip the character to left
        else if (rb.linearVelocity.x < 0 && facingRight)
        {
            Flip();
        }
    }

    protected virtual void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    protected virtual void HandleAttack()
    {
        if (isGrounded)
        {
            anim.SetTrigger("attack");
        }
    }

    private void TryToJump()
    {
        if (isGrounded && canJump)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    protected void Move(float by)
    {
        rb.linearVelocity = new Vector2(by * moveSpeed, rb.linearVelocity.y);
    }

    [ContextMenu("Flip")]
    protected void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingDir = -facingDir;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

    protected virtual void Die()
    {
        anim.enabled = false;
        col.enabled = false;
        rb.gravityScale = 12;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 15);
    }

    private IEnumerator DamageFeedbackCoroutine()
    {
        var originalMaterial = sr.material;
        sr.material = damageMaterial;
        yield return new WaitForSeconds(damageFeedbackDuration);
        sr.material = originalMaterial;
    }
}
