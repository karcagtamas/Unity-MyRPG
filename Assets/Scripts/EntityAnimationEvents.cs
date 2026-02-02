using UnityEngine;

public class EntityAnimationEvents : MonoBehaviour
{
    private Entity player;

    void Awake()
    {
        player = GetComponentInParent<Entity>();
    }

    public void DamageTargets() => player.DamageTargets();

    private void DisableMovementAndJump() => player.EnableMovementAndJump(false);

    private void EnableMovementAndJump() => player.EnableMovementAndJump(true);
}
