using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage;
    public float speed;
    public bool isRotateWeapon;

    void Update()
    {
        if (isRotateWeapon)
        {
            transform.Rotate(Vector3.back, speed * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && isRotateWeapon)
        {
            EnemyMove enemy = other.GetComponent<EnemyMove>();
            if (enemy.isHit)
                return;

            enemy.OnEnemyDamaged(damage);

            AudioManager.instance.PlaySfx(AudioManager.Sfx.Attack);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.metalBreak);

        }
    }
}
