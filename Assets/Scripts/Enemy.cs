using UnityEngine;

public class Enemy : Entity
{
    private bool playerDetected;

    [Header("Movement details")]
    [SerializeField] protected float moveSpeed = 3.5f;

    protected override void Update()
    {
        base.Update();
        HandleAttack();
    }

    protected override void HandleAttack()
    {
         if (playerDetected)
        {
            anim.SetTrigger("attack");
        }
    }

    protected override void HandleMovement()
    {
        base.Move(canMove ? facingDir : 0, moveSpeed);
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();

        playerDetected = Physics2D.OverlapCircle(attackPoint.position, attackRadius, whatIsTarget);
    }

    protected override void Die()
    {
        base.Die();
        UI.instance.AddKillCount();
    }
}
