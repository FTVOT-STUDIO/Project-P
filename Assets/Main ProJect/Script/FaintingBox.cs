using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaintingBox : MonoBehaviour
{
    [HideInInspector] public Collider2D coll;
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
            StartCoroutine(FaintingEnemy(enemy));
        }
    }

    IEnumerator FaintingEnemy(EnemyMove enemy)
    {

        enemy.isFainting = true;

        enemy.rigid.AddForce(Vector2.up * 9, ForceMode2D.Impulse);

        yield return new WaitForSeconds(5f);

        enemy.isFainting = false;

    }
}
