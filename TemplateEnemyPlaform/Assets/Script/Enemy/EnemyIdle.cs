using UnityEngine;

public class EnemyIdle : MonoBehaviour
{
    [SerializeField]
    private EnemyController enemyController;
    [SerializeField]
    private EnemyVisionController vision;

    void Awake()
    {
        if (enemyController == null)
            enemyController = GetComponent<EnemyController>();

        if (vision == null)
            vision = GetComponent<EnemyVisionController>();
    }

    void FixedUpdate()
    {
        if (enemyController.State == EnemyState.Attack)
            return;

        enemyController.rb.linearVelocity = new Vector2(0f, enemyController.rb.linearVelocity.y);
        enemyController.SetState(EnemyState.Idle);

        if (vision.CanSeePlayer)
        {
            float dirX = vision.Player.position.x - transform.position.x;
            enemyController.Flip(dirX);
        }
    }
}