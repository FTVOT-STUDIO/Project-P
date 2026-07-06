using UnityEngine;

public class MonstorBox : MonoBehaviour
{
    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            anim.SetBool("isColl", true);

            int randomIndex = Random.Range(5, 8);
            int itemrandomIndex = Random.Range(13, 18);

            GameObject enemy = PoolManager.Instance.Get(randomIndex);
            Vector3 spawnPosition = transform.position;

            int randomValue = Random.Range(0, 2) == 0 ? 2 : -2;


            spawnPosition = new Vector2(transform.position.x + randomValue, transform.position.y);

            enemy.transform.position = spawnPosition;

            // 아이템 스폰 확률 코드 (주석 해제 시 필요)
            /*
            float spawnChance = Random.value;
            float spawnThreshold = 0.5f;

            if (spawnChance < spawnThreshold)
            {
                GameObject item = PoolManager.Instance.Get(itemrandomIndex);

                Vector3 itemSpawnPosition = spawnPosition;
                do
                {
                    itemSpawnPosition.x = transform.position.x + Random.Range(-3f, 3f);
                }
                while (Mathf.Abs(itemSpawnPosition.x) < 1f || Mathf.Abs(itemSpawnPosition.x) == 2f);

                item.transform.position = itemSpawnPosition;
            }
            */
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        anim.SetBool("isColl", false);
    }
}
