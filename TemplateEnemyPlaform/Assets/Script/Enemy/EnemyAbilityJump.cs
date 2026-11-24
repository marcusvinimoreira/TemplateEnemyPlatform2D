using UnityEngine;

public class EnemyAbilityJump : EnemyAbility
{
    [Header("Pulo")]
    [SerializeField]
    private float jumpForce = 8f;
    [SerializeField]
    private float jumpSpeed = 5f;

    public override bool CanUse()
    {
        if (!enemy.OnGround)
            return false;

        return base.CanUse();
    }

    protected override void OnUse()
    {
        float dirX = Mathf.Sign(vision.Player.position.x - transform.position.x);

        enemy.Flip(dirX);
        enemy.rb.linearVelocity = Vector2.zero;
        enemy.rb.AddForce(new Vector2(dirX * jumpSpeed, jumpForce), ForceMode2D.Impulse);
    }
}