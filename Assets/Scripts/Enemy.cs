using UnityEngine;

public class Enemy : Entity
{
    private bool playerDetected;

    protected override void Update()
    {
        HandleCollision();
        HandleAnimations();
        HandleMovement();
        HandleFlip();
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
        base.Move(canMove ? facingDir : 0);
    }

    protected override void HandleCollision()
    {
        base.HandleCollision();

        playerDetected = Physics2D.OverlapCircle(attackPoint.position, attackRadius, whatIsTarget);
    }
}
