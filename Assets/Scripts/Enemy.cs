using UnityEngine;

public class Enemy : Entity
{
    protected override void Update()
    {
        HandleCollision();
        HandleAnimations();
        HandleMovement();
        HandleFlip();
    }

    protected override void HandleMovement()
    {
        base.Move(canMove ? facingDir : 0);
    }
}
