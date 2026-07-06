using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TanZetaSkill : MonoBehaviour
{
    public int damage;

    Transform player;

    void OnEnable()
    {
        transform.position = player.position;
    }

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        transform.position = player.position;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collided object has the tag "Enemy"
        if (collision.CompareTag("Enemy"))
        {
            EnemyMove enemy = collision.GetComponent<EnemyMove>();
            StartCoroutine(enemy.DeathEffect());
        }
    }
}
