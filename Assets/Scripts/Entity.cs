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
    

    [Header("Health")]
    [SerializeField] private int maxHealth = 1;
    [SerializeField] private int currentHealth = 1;
    [SerializeField] private Material damageMaterial;
    [SerializeField] private float damageFeedbackDuration = .1f;
    private Coroutine damageFeedbackCoroutine;

    [Header("Attack details")]
    [SerializeField] protected float attackRadius;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected LayerMask whatIsTarget;

    [Header("Collision details")]
    [SerializeField] private float groundCheckDistance = 1.35f;
    [SerializeField] private LayerMask whatIsGround;
    protected bool isGrounded;

    // Direction details
    protected int facingDir = 1;
    protected bool facingRight = true;
    protected bool canMove = true;


    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        anim = GetComponentInChildren<Animator>();
        
        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        HandleCollision();
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

    public virtual void EnableMovement(bool enable)
    {
        canMove = enable;
    }

    protected virtual void HandleAnimations()
    {
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
    }

    protected virtual void HandleMovement() { }

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

    protected void Move(float by, float speed)
    {
        rb.linearVelocity = new Vector2(by * speed, rb.linearVelocity.y);
    }

    [ContextMenu("Flip")]
    public void Flip()
    {
        transform.Rotate(0, 180, 0);
        facingRight = !facingRight;
        facingDir = -facingDir;
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

        Destroy(gameObject, 3);
    }

    private IEnumerator DamageFeedbackCoroutine()
    {
        var originalMaterial = sr.material;
        sr.material = damageMaterial;
        yield return new WaitForSeconds(damageFeedbackDuration);
        sr.material = originalMaterial;
    }
}
