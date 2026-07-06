using System.Collections;
using DG.Tweening;
using UnityEngine;

public class FallingTile : MonoBehaviour
{
    private Vector3 positionVec;

    Rigidbody2D rb;
    Collider2D coll;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        positionVec = transform.position;
    }

    IEnumerator DropTile()
    {
        yield return new WaitForSeconds(0.2f);

        rb.gravityScale = 12;
        coll.enabled = false;

        yield return new WaitForSeconds(2);

        rb.velocity = Vector2.zero;
        rb.gravityScale = 0;
        transform.DOMove(positionVec, 2f);
        coll.enabled = true;

    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMove player = other.gameObject.GetComponent<PlayerMove>();
            if (player.transform.position.y > positionVec.y)
            {
                StartCoroutine(DropTile());
            }

        }
    }
}
