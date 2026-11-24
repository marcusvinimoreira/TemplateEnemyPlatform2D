using UnityEngine;

public class EnemyAbilityController : MonoBehaviour
{
    [SerializeField]
    private EnemyController enemy;
    [SerializeField]
    private EnemyAbility[] abilities; // a ordem define a prioridade

    void Awake()
    {
        if (enemy == null)
            enemy = GetComponent<EnemyController>();
    }

    void FixedUpdate()
    {
        if (enemy.State == EnemyState.Attack)
            return;

        for (int i = 0; i < abilities.Length; i++)
        {
            if (abilities[i].CanUse())
            {
                enemy.SetState(EnemyState.Attack);
                abilities[i].Use();
                return;
            }
        }
    }
}