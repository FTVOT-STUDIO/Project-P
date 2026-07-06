using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeEnemy : EnemyMove
{
    private Transform player;
    public float detectRange = 8f;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    void Start()
    {
        player = GameManager.Instance.Player.transform;
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= detectRange && !isFainting)
        {
            MoveTowardsPlayer();
        }
    }

    void LateUpdate()
    {
        isFaintined();
    }

    void MoveTowardsPlayer()
    {
        if (player.transform.position.y > transform.position.y + 1 || player.transform.position.y < transform.position.y - 1)
            return;

        nextMove = player.position.x > transform.position.x ? 1 : -1;
        if (nextMove != 0)
        {
            spriteRenderer.flipX = nextMove == -1;

        }

        rigid.velocity = new Vector2(Speed * nextMove, rigid.velocity.y);
    }
}