using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElenaLogic : MonoBehaviour
{
    public Transform player;
    public float speed;
    public float jumpForce;
    public float attack;
    public float playerDistance;
    public float teleportDistance;
    public float detectRange;

    public LayerMask layerMask;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isFollowingTarget = true;
    private Transform trans;
    private Animator anim;
    private bool isJumping = false;

    void OnEnable()
    {
        trans.position = new Vector3(player.position.x, player.position.y + 3, player.position.z);
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        trans = GetComponent<Transform>();
    }

    void Update()
    {
        DetectEnemies();
        DetectPlatform();
    }

    void FixedUpdate()
    {
        if (isFollowingTarget)
        {
            FollowTarget();
        }
    }

    void LateUpdate()
    {
        float moveSpeed = Mathf.Abs(rb.velocity.x);
        anim.SetFloat("Move", moveSpeed);
    }

    void DetectPlatform()
    {
        Vector2 rayVec = new Vector2(transform.position.x, transform.position.y - 0.15f);
        int dirVec = spriteRenderer.flipX ? 1 : -1;
        RaycastHit2D hit = Physics2D.Raycast(rayVec, Vector2.left * dirVec, 0.5f, layerMask);
        Debug.DrawRay(rayVec, Vector2.left * 0.5f * dirVec, Color.green);

        if (hit && !isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Jump);
            isJumping = true;
        }
    }

    void FollowTarget()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > teleportDistance)
        {
            trans.position = player.position;
            return;
        }

        if (distanceToPlayer > playerDistance)
        {
            Vector2 direction = (player.position - transform.position).normalized;

            float movement = direction.x * speed * Time.fixedDeltaTime;
            rb.velocity = new Vector2(movement, rb.velocity.y);

            spriteRenderer.flipX = direction.x < 0;
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    }

    void DetectEnemies()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer > teleportDistance)
        {
            trans.position = player.position;
            return;
        }

        Collider2D[] enemiesInRange = Physics2D.OverlapCircleAll(transform.position, detectRange);
        foreach (Collider2D enemy in enemiesInRange)
        {
            if (enemy.CompareTag("Enemy"))
            {
                if (transform.position.y <= enemy.transform.position.y - 1f)
                    return;
                isFollowingTarget = false;
                TargetOnEnemy(enemy.transform);
                return;
            }
        }

        isFollowingTarget = true;
    }

    void TargetOnEnemy(Transform enemy)
    {
        Vector2 directionToEnemy = (enemy.position - transform.position).normalized;
        float distanceToEnemy = Vector2.Distance(transform.position, enemy.position);

        if (!isJumping)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Jump);
            isJumping = true;
        }
        else
        {
            float movement = directionToEnemy.x * speed * Time.deltaTime;
            rb.velocity = new Vector2(movement, rb.velocity.y);
            spriteRenderer.flipX = directionToEnemy.x < 0;
        }
    }

    void OnAttack(Transform enemy)
    {
        rb.AddForce(Vector2.up * 30, ForceMode2D.Impulse);

        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnEnemyDamaged(attack);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Attack);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform")) // Î∞îÎã•?óê ?ãø?úºÎ©?
        {
            isJumping = false; // ?†ê?îÑ ?ÉÅ?Éú Ï¥àÍ∏∞?ôî
        }

        EnemyMove enemy = collision.gameObject.GetComponent<EnemyMove>();
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (rb.velocity.y < 0 || transform.position.y > collision.transform.position.y + 0.5f && enemy.isHit == false)
            {
                OnAttack(collision.transform);
            }
        }
    }

    IEnumerator StayState(Collider2D collider, EnemyMove enemy)
    {
        yield return new WaitForSeconds(1f);

        if (collider.gameObject.CompareTag("Enemy") && enemy.transform.position.y < trans.position.y)
        {
            rb.AddForce(Vector2.up, ForceMode2D.Impulse);
        }
        else
            yield break;
    }

    void OnTriggerStay2D(Collider2D collider)
    {
        anim.SetBool("isGround", true);

        EnemyMove enemy = collider.gameObject.GetComponent<EnemyMove>();

        StartCoroutine(StayState(collider, enemy));
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        anim.SetBool("isGround", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }

    public void SetReposition()
    {
        Vector3 player = GameManager.Instance.Player.transform.position;
        transform.position = new Vector3(player.x, player.y + 1, player.z);
    }
}
