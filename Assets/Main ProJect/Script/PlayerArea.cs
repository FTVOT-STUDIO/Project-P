using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArea : MonoBehaviour
{
    public enum WorldArea { Ground, UnderGround, Cave, Sky }

    public WorldArea worldArea;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            switch (worldArea)
            {
                case WorldArea.Ground:
                    return;
                case WorldArea.Cave:
                    AudioManager.instance.AudioCaveEffect(true);
                    break;
            }
        }

    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            switch (worldArea)
            {
                case WorldArea.Ground:
                    return;
                case WorldArea.Cave:
                    AudioManager.instance.AudioCaveEffect(false);
                    break;
            }
        }
    }
}
