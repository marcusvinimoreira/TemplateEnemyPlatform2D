using UnityEngine;

public class BulletControler : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField]
    private float speed = 6f;
    [SerializeField]
    private float timeDestroy = 3f;
    [SerializeField]
    private SpriteRenderer sprite;

    private float directionX = 1f;

    void Awake()
    {
        if (sprite == null)
            sprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        Destroy(gameObject, timeDestroy);
    }

    // Chamado pelo inimigo logo depois do Instantiate
    public void SetDirection(float dirX)
    {
        directionX = dirX;

        if (sprite != null)
            sprite.flipX = dirX < 0f;
    }

    void Update()
    {
        transform.Translate(new Vector3(directionX * speed * Time.deltaTime, 0f, 0f), Space.World);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            Destroy(gameObject);
    }
}