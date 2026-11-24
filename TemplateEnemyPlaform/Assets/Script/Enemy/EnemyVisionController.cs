
using UnityEngine;

public class EnemyVisionController : MonoBehaviour
{
    [Header("Referências")]
    [SerializeField]
    private Transform player;

    public float DistanceToPlayer { get; private set; }
    [SerializeField]
    private Transform visionOrigin;

    [Header("Campo de visão")]
    [SerializeField, Range(0f, 360f)]
    private float fieldOfView = 180f;

    [SerializeField, Min(0f)]
    private float visionRange = 5f;

    [SerializeField, Min(0f)]
    private float awarenessRange = 1f;

    [SerializeField, Min(0f)]
    private float attackRange = 1.5f;

    [Header("Direção da visão")]
    [SerializeField]
    private Vector2 facingDirection = Vector2.right;

    [Header("Obstáculos")]
    [SerializeField]
    private LayerMask obstacleLayer;

    [Header("Estado da percepção")]
    [SerializeField]
    private bool canSeePlayer;

    [SerializeField]
    private bool isPlayerInAttackRange;

    private Vector2 lastKnownPlayerPosition;
    private bool hasLastKnownPosition;

    public Transform Player => player;
    public bool CanSeePlayer => canSeePlayer;
    public bool IsPlayerInAttackRange => isPlayerInAttackRange;

    public Vector2 LastKnownPlayerPosition => lastKnownPlayerPosition;

    public bool HasLastKnownPosition => hasLastKnownPosition;

    private void Awake()
    {
        if (visionOrigin == null)
            visionOrigin = transform;
    }

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
                player = playerObject.transform;
        }
    }

    private void Update()
    {
        CheckVision();
    }

    private void CheckVision()
    {
        canSeePlayer = false;
        isPlayerInAttackRange = false;

        if (player == null)
            return;

        Vector2 origin = visionOrigin.position;
        Vector2 playerPosition = player.position;
        Vector2 toPlayer = playerPosition - origin;

        float distance = toPlayer.magnitude;
        DistanceToPlayer = distance;

        if (distance > visionRange)
            return;

        // Perto o suficiente, o inimigo percebe o player mesmo fora do ângulo de visão
        bool insideAwareness = distance <= awarenessRange;

        if (!insideAwareness && distance > 0f && fieldOfView < 360f)
        {
            Vector2 direction = toPlayer.normalized;
            Vector2 facing = facingDirection.normalized;

            float angle = Vector2.Angle(facing, direction);

            if (angle > fieldOfView * 0.5f)
                return;
        }

        if (distance > 0f)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, toPlayer.normalized, distance, obstacleLayer);

            if (hit.collider != null)
                return;
        }

        canSeePlayer = true;

        lastKnownPlayerPosition = playerPosition;
        hasLastKnownPosition = true;

        isPlayerInAttackRange = distance <= attackRange;
    }
    public void SetFacingDirection(Vector2 direction)
    {
        if (direction.sqrMagnitude > 0f)
            facingDirection = direction.normalized;
    }

    private void OnValidate()
    {
        visionRange = Mathf.Max(0f, visionRange);
        attackRange = Mathf.Max(0f, attackRange);
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 origin = visionOrigin != null ? visionOrigin.position : transform.position;

        Vector2 facing = facingDirection.sqrMagnitude > 0f ? facingDirection.normalized : Vector2.right;

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(origin, visionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin, attackRange);

        if (fieldOfView >= 360f)
            return;

        Gizmos.color = Color.yellow;

        float halfAngle = fieldOfView * 0.5f;
        float baseAngle = Mathf.Atan2(facing.y, facing.x) * Mathf.Rad2Deg;

        Vector3 previousPoint = origin;

        int segments = Mathf.Max(2, Mathf.CeilToInt(fieldOfView / 5f));

        for (int i = 0; i <= segments; i++)
        {
            float angle = baseAngle - halfAngle + fieldOfView * i / segments;

            float radians = angle * Mathf.Deg2Rad;

            Vector3 point = origin + new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0f) * visionRange;

            if (i > 0)
                Gizmos.DrawLine(previousPoint, point);

            previousPoint = point;
        }

        float leftAngle = (baseAngle - halfAngle) * Mathf.Deg2Rad;

        float rightAngle = (baseAngle + halfAngle) * Mathf.Deg2Rad;

        Gizmos.DrawLine(origin, origin + new Vector3(Mathf.Cos(leftAngle), Mathf.Sin(leftAngle), 0f) * visionRange);

        Gizmos.DrawLine(origin, origin + new Vector3(Mathf.Cos(rightAngle), Mathf.Sin(rightAngle), 0f) * visionRange);
    }

    private void OnDrawGizmos()
    {
        if (player == null)
        {
            return;
        }

        if (!canSeePlayer)
        {
            return;
        }

        Vector3 origin = transform.position;

        if (visionOrigin != null)
        {
            origin = visionOrigin.position;
        }

        Gizmos.color = Color.blue;

        Gizmos.DrawLine(origin, player.position);
    }

}
