using System.Collections;
using UnityEngine;

public enum EnemyState { Idle, Walk, Attack }

public class EnemyController : MonoBehaviour
{
    [Header("Status do inimigo")]
    [SerializeField, Min(1f)]
    protected float health = 3f;

    public bool invulnerable = false;

    [Header("Referências")]
    public Animator anim;
    public Rigidbody2D rb;
    public SpriteRenderer sprite;
    [SerializeField]
    private EnemyVisionController vision;

    [Header("Chão")]
    [SerializeField]
    private bool needsGround = true;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private float groundRaycastLength = 0.6f;

    [Header("Morte")]
    [SerializeField]
    private GameObject dieEnemy;

    private Coroutine damageCoroutine;
    private bool isDead;

    [Header("Ataque")]
    [SerializeField]
    private float maxAttackTime = 1f;
    private float attackTimer;

    public float Health => health;
    public bool IsDead => isDead;
    public EnemyState State { get; private set; }
    public bool OnGround { get; private set; }

    void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        if (anim == null)
            anim = GetComponent<Animator>();

        if (sprite == null)
            sprite = GetComponent<SpriteRenderer>();

        if (vision == null)
            vision = GetComponent<EnemyVisionController>();
    }

    private void Update()
    {
        if (needsGround)
            OnGround = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastLength, groundLayer);
        else
            OnGround = true;
        if (State == EnemyState.Attack)
        {
            attackTimer -= Time.deltaTime;

            if (attackTimer <= 0f)
                State = EnemyState.Idle;
        }
        AnimatorEnemy();
    }

    public void SetState(EnemyState newState)
    {
        if (newState == EnemyState.Attack && State != EnemyState.Attack)
            attackTimer = maxAttackTime;

        State = newState;
    }

    public void Flip(float directionX)
    {
        if (directionX == 0f)
            return;

        sprite.flipX = directionX < 0f;
    }

    public void DamageEnemy(float damageHit)
    {
        if (isDead || invulnerable || damageHit <= 0f)
            return;

        health -= damageHit;

        if (health <= 0f)
        {
            health = 0f;
            Die();
            return;
        }

        if (damageCoroutine != null)
            StopCoroutine(damageCoroutine);

        damageCoroutine = StartCoroutine(DamageFeedback());
    }

    void Die()
    {
        if (isDead)
            return;

        isDead = true;

        if (dieEnemy != null)
            Instantiate(dieEnemy, transform.position, transform.rotation);

        Destroy(gameObject);
    }

    private IEnumerator DamageFeedback()
    {
        if (sprite == null)
            yield break;

        sprite.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        if (sprite != null)
            sprite.color = Color.white;

        damageCoroutine = null;
    }

    void AnimatorEnemy()
    {
       // anim.SetBool("Idle", State == EnemyState.Idle);
        anim.SetBool("Walk", State == EnemyState.Walk);
       // anim.SetBool("Attack", State == EnemyState.Attack);
    }

    private void OnDrawGizmosSelected()
    {
        if (!needsGround)
            return;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundRaycastLength);
    }
}