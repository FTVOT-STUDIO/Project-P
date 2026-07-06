using System.Collections;
using UnityEngine;

public class ChesedSkill : MonoBehaviour
{
    public int damage;
    Transform player;
    [HideInInspector] public CapsuleCollider2D CapsuleCollider2D;

    void OnEnable()
    {
        CapsuleCollider2D.enabled = true;
        transform.position = player.position;
    }

    void Awake()
    {
        CapsuleCollider2D = GetComponent<CapsuleCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        transform.position = player.position;
    }
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Enemy"))
        {
            EnemyMove enemy = collider.GetComponent<EnemyMove>();

            StartCoroutine(SkillActive(enemy));
        }

    }

    IEnumerator SkillActive(EnemyMove enemy)
    {
        enemy.OnEnemyDamaged(damage);
        enemy.isFainting = true;
        yield return new WaitForSeconds(10f);
        enemy.isFainting = false;
    }
}
