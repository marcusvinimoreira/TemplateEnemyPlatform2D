using UnityEngine;

public class EnemyAbilityDash : EnemyAbility
{
    [Header("Dash")]
    [SerializeField]
    private float dashSpeed = 8f;

    protected override void OnUse()
    {
        float dirX = Mathf.Sign(vision.Player.position.x - transform.position.x);

        enemy.Flip(dirX);
        enemy.rb.linearVelocity = new Vector2(dirX * dashSpeed, enemy.rb.linearVelocity.y);
    }
}