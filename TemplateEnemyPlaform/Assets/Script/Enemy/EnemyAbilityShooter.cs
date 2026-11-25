using UnityEngine;

public class EnemyAbilityShooter : EnemyAbility
{

    [Header("Shooter")]
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Transform posForFire;

    protected override void OnUse()
    {
        // Direção em relação ao inimigo: 1 = direita, -1 = esquerda
        float dirX = Mathf.Sign(vision.Player.position.x - transform.position.x);
        enemy.Flip(dirX);
        FlipPosForFire(dirX);

        GameObject cloneProjet = Instantiate(bullet, posForFire.position, Quaternion.identity);
        cloneProjet.GetComponent<BulletControler>().SetDirection(dirX);

    }

    private void FlipPosForFire(float directionX)
    {
        Vector3 pos = posForFire.localPosition;
        pos.x = Mathf.Abs(pos.x) * directionX;
        posForFire.localPosition = pos;
    }


}
