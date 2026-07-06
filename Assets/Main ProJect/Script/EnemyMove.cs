
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMove : MonoBehaviour
{
    public EnemyData EnemyData;
    public Rigidbody2D rigid;
    protected Animator anim;
    protected SpriteRenderer spriteRenderer;
    protected Collider2D Coll;
    protected int nextMove;
    public float Speed;
    [HideInInspector] public bool isHit;
    public bool isLive;
    public bool isFainting;
    [SerializeField] Transform HpBar;
    [SerializeField] GameObject Fainting;
    private bool previousFainting = false;

    //public float damage; // ëª¬ìŠ¤?„° ê³µê²©? ¥
    [HideInInspector] public float EnemyHealth; // ëª¬ìŠ¤?„° ì²´ë ¥
    float maxHealth;

    protected virtual void Awake()
    {
        Speed = EnemyData.Speed;

        EnemyHealth = EnemyData.Health;

        isLive = true;

        maxHealth = EnemyData.Health;
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        Coll = GetComponent<Collider2D>();

        anim.runtimeAnimatorController = EnemyData.anim;
        Invoke("Think", 0.3f);
    }

    protected virtual void OnEnable()
    {
        Speed = EnemyData.Speed;

        EnemyHealth = EnemyData.Health;

        rigid.velocity = Vector2.zero;
        isLive = true;
        HpBar.localScale = new Vector3(1, 1, 1);
        spriteRenderer.color = new Color(1, 1, 1, 1f);
        spriteRenderer.flipY = false;
        Coll.enabled = true;
        isFainting = false;
    }

    void LateUpdate()
    {
        isFaintined();
    }

    void FixedUpdate()
    {
        // Move
        if (isFainting || !isLive)
            return;

        rigid.velocity = new Vector2(Speed * nextMove, rigid.velocity.y);


        EnemyRay();
    }

    protected void isFaintined()
    {
        if (isFainting && !previousFainting)
        {
            Fainting.SetActive(true);
            QuestUI.Instance.CollectItem(3);
        }

        Fainting.SetActive(isFainting);

        previousFainting = isFainting;
    }

    void EnemyRay()
    {
        // Raycast for front detection
        Vector2 frontVec = new Vector2(rigid.position.x + nextMove * 0.5f, rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));

        RaycastHit2D rayHitFront = Physics2D.Raycast(frontVec, Vector3.down, 2, LayerMask.GetMask("Platform"));

        // Raycast for forward detection
        Vector2 forwardVec = new Vector2(transform.position.x + nextMove * 0.5f, transform.position.y);
        Debug.DrawRay(forwardVec, Vector2.right * nextMove, Color.red);

        RaycastHit2D rayHitForward = Physics2D.Raycast(forwardVec, Vector2.right * nextMove, 0.5f, LayerMask.GetMask("Platform"));

        if (rayHitFront.collider == null || rayHitForward.collider != null)
        {
            Turn();
        }
    }


    //?ž¬ê·? ?•¨?ˆ˜
    void Think()
    {
        //Set Next Active
        nextMove = Random.Range(-1, 2);

        //Sprite Animation
        anim.SetInteger("Walk Speed", nextMove);

        //Flip Spirte
        if (nextMove != 0)
            spriteRenderer.flipX = nextMove == -1;

        //Recurisve
        float nextThinkTime = Random.Range(1f, 4f);
        Invoke("Think", nextThinkTime);
    }

    void Turn()
    {
        nextMove *= -1;
        spriteRenderer.flipX = nextMove == -1;

        CancelInvoke();
        Invoke("Think", 2);
    }

    void DeActive()
    {
        gameObject.SetActive(false);
    }



    public virtual void OnEnemyDamaged(float damage)
    {
        if (isHit)
            return;

        EnemyHealth -= damage;

        if (EnemyHealth > 1)
        {
            isHit = true;

            GameManager.Instance.stagePoint += 30;

            rigid.AddForce(Vector2.up * 4, ForceMode2D.Impulse);

            float healthRatio = Mathf.Clamp01(EnemyHealth / maxHealth);

            HpBar.localScale = new Vector3(healthRatio, 1, 1);

            StartCoroutine(DamageEffect());

            if (isHit) // ÃÄ¸ÂÀ»¶§¸¸ È¿°úÀ½ Àç»ý
            {
                // print("Hit!");
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Attack);
            }


            if (!QuestManager.Instance.isSlow)
                return;
            StartCoroutine(SlowEffect());
        }
        else if (EnemyHealth < 1)
        {
            StartCoroutine(DeathEffect());
            CheckAchieve();
        }
    }

    IEnumerator DamageEffect()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        StartCoroutine(FaintingOn());
        yield return new WaitForSeconds(0.5f); // ¹«Àû½Ã°£
        isHit = false;
        spriteRenderer.color = Color.white;

    }

    IEnumerator SlowEffect()
    {
        Speed *= 0.5f;
        yield return new WaitForSeconds(3f);
        Speed *= 2f;
    }

    public virtual IEnumerator DeathEffect()
    {
        isLive = false;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Attack);
        QuestUI.Instance.CollectItem(6);
        GameManager.Instance.stagePoint += 100;
        GameManager.Instance.KillCount++;
        DropItem();
        GameManager.Instance.currentStreak++;
        if (AbilityManager.Instance.AliceAblity != null)
        {
            AbilityManager.Instance.AliceAblity.ExpCount++;
            AbilityManager.Instance.AliceAblity.GetAliceAblity();
        }

        if (GameManager.Instance.PlayerId == 8)
        {
            GameManager.Instance.Player.PlayerAttack += 0.5f;
            float currentValue = float.Parse(GameManager.Instance.StackCount.GetComponentInChildren<Text>().text);
            currentValue += 0.5f;
            GameManager.Instance.StackCount.GetComponentInChildren<Text>().text = currentValue.ToString("F1");
        }
        HpBar.localScale = new Vector3(0, 0, 0);
        spriteRenderer.color = new Color(1, 1, 1, 0.2f);
        spriteRenderer.flipY = true;
        Coll.enabled = false;
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        yield return new WaitForSeconds(5f);
        DeActive(); // ë¹„í™œ?„±?™”
    }

    void CheckAchieve()
    {
        if (GameManager.Instance.healthPoint == 1)
        {
            QuestUI.Instance.CollectItem(5);
        }
    }

    void DropItem()
    {
        if (QuestManager.Instance.isHealth)
        {
            GameObject healBox = PoolManager.Instance.Get(8);

            healBox.transform.position = transform.position;
        }

        if (QuestManager.Instance.isShiled)
        {
            GameObject healBox = PoolManager.Instance.Get(9);

            healBox.transform.position = transform.position;
        }
    }

    IEnumerator FaintingOn()
    {
        if (QuestManager.Instance.isFaintined)
        {
            isFainting = true;

            yield return new WaitForSeconds(5f);

            isFainting = false;
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Repos"))
        {
            StartCoroutine(DeathEffect());
        }
    }
}
