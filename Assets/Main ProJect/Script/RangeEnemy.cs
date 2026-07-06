using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangeEnemy : EnemyMove
{
    public Transform firePoint;
    public float fireRange = 8f;
    private Transform player;
    private float fireCooldown = 2f;
    private float lastFireTime = 0;
    [SerializeField] SpriteRenderer weapon;
    ParticleSystem particle;
    //List<GameObject> bulletList = new List<GameObject>();

    protected override void OnEnable()
    {

        weapon.color = new Color(1, 1, 1, 1);
        base.OnEnable();
    }

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        player = GameManager.Instance.Player.transform;
        particle = GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= fireRange && !isFainting && isLive)
        {
            MoveTowardsPlayer();

            // 미사일 발사
            if (Time.time > lastFireTime + fireCooldown)
            {
                FireMissile();
                lastFireTime = Time.time;
            }
        }
    }

    void LateUpdate()
    {
        isFaintined();

        if (nextMove != 0)
        {
            weapon.flipX = nextMove == -1;
            weapon.flipY = nextMove == -1;
        }

    }

    void MoveTowardsPlayer()
    {
        nextMove = player.position.x > transform.position.x ? 1 : -1;
        if (nextMove != 0)
        {
            spriteRenderer.flipX = nextMove == -1;

        }
        rigid.velocity = new Vector2(Speed * nextMove, rigid.velocity.y);
    }

    void FireMissile()
    {
        particle.Play();
        anim.SetBool("Attack", true);
    }

    public void ShootBullet()
    {
        // 오브젝트 풀링에서 총알 가져오기
        GameObject bullet = PoolManager.Instance.Get(0);
        if (bullet != null) // bullet이 null이 아닐 때만 진행
        {
            bullet.transform.position = firePoint.position;
            bullet.transform.rotation = firePoint.rotation;
            bullet.SetActive(true);
            bullet.GetComponent<EnemyBullet>().Initialize(player.position);
        }
        AudioManager.instance.PlaySfx(AudioManager.Sfx.NoaAttack);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.fireBall);
    }

    public void ArrangeShootSound()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.soul);
    }


    public void OffAnim()
    {
        anim.SetBool("Attack", false);
    }


    public override IEnumerator DeathEffect()
    {
        StartCoroutine(base.DeathEffect());
        weapon.color = new Color(1, 1, 1, 0.2f);
        yield return null;
    }
}
