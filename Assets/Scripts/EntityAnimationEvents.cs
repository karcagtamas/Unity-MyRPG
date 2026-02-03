using UnityEngine;

public class EntityAnimationEvents : MonoBehaviour
{
    private Entity player;

    void Awake()
    {
        player = GetComponentInParent<Entity>();
    }

    public void DamageTargets() => player.DamageTargets();

    private void DisableMovementAndJump() => player.EnableMovement(false);

    private void EnableMovementAndJump() => player.EnableMovement(true);
}
