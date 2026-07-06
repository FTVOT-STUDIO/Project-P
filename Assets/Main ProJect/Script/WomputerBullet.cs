using UnityEngine;

public class WomputerBullet : MonoBehaviour
{
    public float speed;
    public int damage;
    public float lifeTime = 1.5f;

    private float currentLifeTime;
    private Vector3 direction;
    private SpriteRenderer sprite;
    private PlayerMove player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMove>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        SetDirection();
        currentLifeTime = 0f;
    }

    void Update()
    {
        MoveBullet();
        currentLifeTime += Time.deltaTime;

        if (currentLifeTime >= lifeTime)
        {
            DisableBullet();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy"))
        {
            DisableBullet();
        }

        EnemyMove enemy = collision.GetComponent<EnemyMove>();
        if (enemy != null)
        {
            if (enemy.isHit)
                return;
            enemy.OnEnemyDamaged(damage);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Attack);
            DisableBullet();
        }

    }



    void DisableBullet()
    {
        gameObject.SetActive(false);
    }

    void SetDirection()
    {
        direction = player.spriteRenderer.flipX ? Vector3.left : Vector3.right;
        if (player.spriteRenderer.flipX)
        {
            sprite.flipX = true;
        }
        else
        {
            sprite.flipX = false;
        }
    }

    void MoveBullet()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }
}
