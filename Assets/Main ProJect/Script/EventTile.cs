using UnityEngine;

public class EventTile : MonoBehaviour
{
    public enum eventTile { Slow, Speed }

    public eventTile tileType;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coll"))
        {
            PlayerMove player = collision.GetComponentInParent<PlayerMove>();

            switch (tileType)
            {
                case eventTile.Slow:
                    QuestUI.Instance.CollectItem(0);
                    player.maxSpeed *= 0.3f;
                    break;
                case eventTile.Speed:
                    QuestUI.Instance.CollectItem(8);
                    player.maxSpeed *= Mathf.CeilToInt(2);
                    break;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Coll"))
        {
            PlayerMove player = collision.GetComponentInParent<PlayerMove>();

            switch (tileType)
            {
                case eventTile.Slow:
                    player.maxSpeed /= 0.3f;
                    break;
                case eventTile.Speed:
                    player.maxSpeed /= 2;
                    break;
            }
        }
    }
}
