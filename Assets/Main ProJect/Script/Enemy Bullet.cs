
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float speed;

    Rigidbody2D rb;
    TrailRenderer trail;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        trail = GetComponent<TrailRenderer>();
    }
    void OnEnable()
    {
        rb.velocity = Vector2.zero;
        trail.Clear();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            PlayerMove player = collider.GetComponent<PlayerMove>();
            if (QuestManager.Instance.isReflect)
            {
                gameObject.SetActive(false);
                return;
            }
            player.OnDamaged(collider.transform.position);
            gameObject.SetActive(false);
        }
    }

    public void Initialize(Vector3 targetPosition)
    {
        Vector2 direction = (targetPosition - transform.position).normalized;

        rb.velocity = direction * speed;

        Invoke("DeActiveBullet", 5f);
    }

    void DeActiveBullet()
    {
        gameObject.SetActive(false);
        rb.velocity = Vector2.zero;
        trail.Clear();
    }
}
