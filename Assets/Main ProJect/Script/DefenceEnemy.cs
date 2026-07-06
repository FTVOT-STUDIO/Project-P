using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenceEnemy : EnemyMove
{
    public override void OnEnemyDamaged(float damage)
    {
        float evasion = Random.Range(0f, 1f);
        if (evasion <= 0.8f && !isFainting)
        {
            StartCoroutine(DamageEffect());
            return;
        }

        base.OnEnemyDamaged(damage);
    }

    void LateUpdate()
    {
        isFaintined();
    }

    IEnumerator DamageEffect()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.miss);

        spriteRenderer.color = Color.blue;

        yield return new WaitForSeconds(0.1f);

        spriteRenderer.color = Color.white;

    }
}
