using UnityEngine;

public class Summonor : MonoBehaviour
{
    public float moveSpeed = 5f;
    
    public float telDistance = 5f;
    public float distance;
    public Rigidbody2D target;

    SpriteRenderer sprite;
    Rigidbody2D rb;
    Animator anim;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
       
    }

    void Update()
    {
        Vector2 dirVec = target.position - rb.position;
        Vector2 nextVec = dirVec.normalized * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + nextVec);
        rb.velocity = Vector2.zero;
    }

    void LateUpdate()
    {
        sprite.flipX = target.position.x < transform.position.x;
    }

}
