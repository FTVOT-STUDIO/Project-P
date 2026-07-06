using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTile : MonoBehaviour
{
    PlayerMove player;

    void Start()
    {
        player = GameManager.Instance.Player;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.rigid.drag *= 4;
            player.rigid.gravityScale /= 2;
            AudioManager.instance.PlaySfx(AudioManager.Sfx.InWater);
            AudioManager.instance.AudioDivingEffect(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.rigid.drag /= 4;
            player.rigid.gravityScale *= 2;
            AudioManager.instance.PlaySfx(AudioManager.Sfx.OutWater);
            AudioManager.instance.AudioDivingEffect(false);
        }
    }
}
