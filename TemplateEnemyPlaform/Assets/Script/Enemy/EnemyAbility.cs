using UnityEngine;

public abstract class EnemyAbility : MonoBehaviour
{
    [Header("Habilidade")]
    [SerializeField]
    private float cooldown = 2f;
    [SerializeField]
    private float minRange = 0f;
    [SerializeField]
    private float maxRange = 1.5f;

    protected EnemyController enemy;
    protected EnemyVisionController vision;
    private float cooldownTimer;
    [SerializeField]
    private string animationTrigger;

    protected virtual void Awake()
    {
        enemy = GetComponent<EnemyController>();
        vision = GetComponent<EnemyVisionController>();
    }

    private void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUse()
    {
        if (cooldownTimer > 0f)
            return false;

        if (!vision.CanSeePlayer)
            return false;

        float distance = vision.DistanceToPlayer;
        return distance >= minRange && distance <= maxRange;
    }

    public void Use()
    {
        cooldownTimer = cooldown;
        if (!string.IsNullOrEmpty(animationTrigger))
            enemy.anim.SetTrigger(animationTrigger);

        OnUse();
    }

    // Cada habilidade implementa o que faz
    protected abstract void OnUse();

    // Mantive o nome FinishAttack para os seus Animation Events continuarem funcionando
    public void FinishAttack()
    {
        enemy.rb.linearVelocity = new Vector2(0f, enemy.rb.linearVelocity.y);
        enemy.SetState(EnemyState.Idle);
    }
}