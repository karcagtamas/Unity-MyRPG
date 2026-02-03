using UnityEngine;

public class ObjectToProtect : Entity
{
    [Header("Extra details")]
    [SerializeField] private Transform player;

    protected override void Awake()
    {
        base.Awake();
        player = FindFirstObjectByType<Player>()?.transform;
    }

    protected override void Update()
    {
        HandleFlip();
    }

    protected override void HandleFlip()
    {
        if (player == null)
        {
            return;
        }

        // Should flip the object to right (to the player)
        if (player.transform.position.x > transform.position.x && !facingRight)
        {
            Flip();
        }
        // Should flip the character to left (to the player)
        else if (player.transform.position.x < transform.position.x && facingRight)
        {
            Flip();
        }
    }
}
