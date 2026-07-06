using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingSword : MonoBehaviour
{
    bool soundPlayed = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        EnemyMove enemy = other.gameObject.GetComponent<EnemyMove>();
        if (other.CompareTag("Enemy"))
        {
            // 적에게 대미지를 입힙니다.
            enemy.OnEnemyDamaged(GameManager.Instance.Player.PlayerAttack);
            enemy.rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);


            if (QuestManager.Instance.isDrain)
            {
                float randomIndex = Random.Range(0f, 1f);

                if (GameManager.Instance.healthPoint < Charskil.HealthPoint)
                {
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.Drain);
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.Heal);
                    GameManager.Instance.Hit.gameObject.SetActive(true);
                    GameManager.Instance.Hit.color = new Color(1, 0, 0, 0.3f);
                    GameManager.Instance.healthPoint++;
                }
            }


            // 소리가 재생되지 않았다면 소리 재생
            if (!soundPlayed)
            {
                AudioManager.instance.PlaySfx(AudioManager.Sfx.SwordHit);
                soundPlayed = true; // 소리가 재생되었음을 기록
            }

        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            soundPlayed = false;
        }
    }

    void LateUpdate()
    {
        AbilityManager.Instance.WardenAblity.SwordGroup.transform.position = GameManager.Instance.Player.transform.position;
    }
}
