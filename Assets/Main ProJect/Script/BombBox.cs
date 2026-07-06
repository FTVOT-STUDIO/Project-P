using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBox : MonoBehaviour
{
    [HideInInspector] public Collider2D coll;
    [SerializeField] float damage;
    void OnEnable()
    {
        coll.enabled = true;
    }
    void Awake()
    {
        coll = GetComponent<Collider2D>();
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyMove enemy = other.gameObject.GetComponent<EnemyMove>();

        if (other.gameObject.CompareTag("Enemy"))
        {
            BombEnemy(enemy, damage);
            //            print(enemy.EnemyHealth);
        }
    }

    void BombEnemy(EnemyMove enemy, float damage)
    {
        enemy.OnEnemyDamaged(damage);

        enemy.rigid.AddForce(Vector2.up * 9, ForceMode2D.Impulse);

    }
}
