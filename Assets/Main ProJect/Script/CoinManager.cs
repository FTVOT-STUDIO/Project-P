using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public enum CoinType { Bronze, Silver, Gold, Super_Bronze, Super_Silver, Super_Gold }

    public CoinType coinType;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            switch (coinType)
            {
                case CoinType.Super_Bronze:
                    GameManager.Instance.stagePoint += 200;
                    break;
                case CoinType.Super_Silver:
                    GameManager.Instance.stagePoint += 300;
                    break;
                case CoinType.Super_Gold:
                    GameManager.Instance.stagePoint += 400;
                    break;
            }
            gameObject.SetActive(false);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Item);
        }
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("Player"))
        {
            switch (coinType)
            {
                case CoinType.Bronze:
                    GameManager.Instance.stagePoint += 30;
                    break;
                case CoinType.Silver:
                    GameManager.Instance.stagePoint += 50;
                    break;
                case CoinType.Gold:
                    GameManager.Instance.stagePoint += 100;
                    break;
            }
            gameObject.SetActive(false);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Item);
        }
    }
}
