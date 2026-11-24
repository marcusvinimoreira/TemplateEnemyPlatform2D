using UnityEngine;

public class EnemyChaserBase : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField]
    private EnemyVisionController vision;
    [SerializeField]
    private EnemyController enemyController;

    [Header("Movimentação")]
    [SerializeField, Min(0f)]
    private float speed = 2f;
    [SerializeField, Min(0f)]
    private float stopDistance = 1.5f;

    void Awake()
    {
        if (vision == null)
            vision = GetComponent<EnemyVisionController>();

        if (enemyController == null)
            enemyController = GetComponent<EnemyController>();
    }

    void FixedUpdate()
    {
        // Durante o ataque, quem manda é a habilidade
        if (enemyController.State == EnemyState.Attack)
            return;

        bool shouldChase = vision.CanSeePlayer && enemyController.OnGround && vision.DistanceToPlayer > stopDistance;

        if (!shouldChase)
        {
            Stop();
            enemyController.SetState(EnemyState.Idle);
            return;
        }

        float moveDir = Mathf.Sign(vision.Player.position.x - transform.position.x);

        enemyController.rb.linearVelocity = new Vector2(moveDir * speed, enemyController.rb.linearVelocity.y);
        enemyController.Flip(moveDir);
        enemyController.SetState(EnemyState.Walk);
    }

    void Stop()
    {
        enemyController.rb.linearVelocity = new Vector2(0f, enemyController.rb.linearVelocity.y);
    }
}