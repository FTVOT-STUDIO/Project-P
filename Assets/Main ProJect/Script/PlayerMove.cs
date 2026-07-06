using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;


public class PlayerMove : MonoBehaviour
{
    [Header("# Game Manager")]
    public GameManager gameManager;

    [Header("# Player Core")]
    public Vector2 inputVec;
    public float maxSpeed;
    public float Jump;
    public int jumpCount = 0;
    public int maxJumps = 2;
    public bool isLive;
    public bool isGround;
    public bool isGravity;
    public Transform coll;
    public GameObject parachute;

    [Header("# Player Damage")]
    public float PlayerAttack;

    bool isHit;
    public bool isDash;
    bool isDead;
    bool isJump;

    [Header("# Button Event")]
    public bool isSlip;
    public bool inputLeft = false;
    public bool inputRight = false;
    public bool inputJump = false;
    public bool isMovementSlider;

    [Header("# Hook Event")]
    public float maxForce = 10f;

    [Header("# Item Event")]
    public GameObject HealBox;

    [Header("# Skill Event")]
    public AbilityManager ability;

    [Header("# Player ID Anim")]
    public RuntimeAnimatorController[] animCon;

    [Header("# PixelPerfectCamera")]
    public PixelPerfectCamera pixelPerfectCamera;

    [Header("# UI Elenments")]

    public Slider onMoveSlider;
    public Toggle onChangeMovementToggle;
    public GameObject buttonMovement;
    public GameObject sliderMovement;
    public Image sliderHandler;
    public Sprite[] sliderSprites;

    [HideInInspector] public Rigidbody2D rigid;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    [HideInInspector] public Animator anim;
    [HideInInspector] public CapsuleCollider2D capsuleCollider2D;
    GrapplingHook grappling;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        grappling = GetComponent<GrapplingHook>();

        maxSpeed = 1f;
        gameManager.healthPoint = 1;
        PlayerAttack = 1f;

        onMoveSlider.onValueChanged.AddListener(OnSliderValueChanged);

        if (PlayerPrefs.HasKey("ToggleState"))
        {
            int toggleState = PlayerPrefs.GetInt("ToggleState");
            onChangeMovementToggle.isOn = toggleState == 1;
        }

    }

    void Start()
    {
        UIButtonManager ui = gameManager.UiManager.GetComponent<UIButtonManager>();
        ui.Init();

    }

    void OnEnable()
    {
        isLive = true;
        SetPlayerStats();

        if (!gameManager.testMode)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
        }

        //rigid.AddForce(Vector2.up * 30, ForceMode2D.Impulse);
    }

    void Update()
    {
        if (!GameManager.Instance.isStart)
            return;

        if (isMovementSlider)
        {
            inputVec.x = Mathf.RoundToInt(onMoveSlider.value);

        }
        else
            inputVec.x = (inputLeft ? -1f : 0f) + (inputRight ? 1f : 0f) + Input.GetAxisRaw("Horizontal");

        if (grappling.isAttach)
        {
            jumpCount = 0;
            //isJump = false;
            anim.SetBool("isGround", false);

        }
        else
        {
            if (isDead)
                return;  //?��?�� 로직

            if ((inputJump || Input.GetButtonDown("Jump")) && jumpCount < maxJumps)
            {

                int curGravity = isGravity ? -1 : 1;

                inputJump = false;

                rigid.AddForce(Vector2.up * Jump * curGravity, ForceMode2D.Impulse);
                jumpCount++;

                AudioManager.instance.PlaySfx(AudioManager.Sfx.Jump);

                gameManager.jumpCount++;

                if (QuestManager.Instance.isDobbleJump)
                    return;
                if (jumpCount == 2)
                {
                    maxJumps = 1;
                }

            }
        }
    }

    void FixedUpdate()
    {
        if (!GameManager.Instance.isStart)
            return;
        //?��?��
        if (isDead) return;
        {
            if (!isDash)
            {


                if (grappling.isAttach)
                {
                    if (Mathf.Abs(rigid.velocity.x) < maxForce)
                    {
                        rigid.AddForce(Vector2.right * inputVec * 100 * Time.fixedDeltaTime, ForceMode2D.Impulse);
                    }

                }
                if (isSlip == true)
                {
                    if (Mathf.Abs(rigid.velocity.x) < maxSpeed)
                    {
                        rigid.AddForce(Vector2.right * inputVec * maxSpeed);
                    }
                }
                else
                {
                    if (grappling.isAttach)
                        return;
                    Vector2 nextVec = inputVec * maxSpeed * Time.fixedDeltaTime;

                    rigid.velocity = new Vector2(nextVec.x, rigid.velocity.y);
                }
            }
        }
    }

    void LateUpdate()
    {
        if (!GameManager.Instance.isStart)
            return;
        if (inputVec.x != 0)
        {
            spriteRenderer.flipX = inputVec.x < 0;
        }

        ChangeSliderSprite();

        anim.SetFloat("Move", inputVec.magnitude);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyBullet")
        {
            EnemyMove enemy = collision.gameObject.GetComponent<EnemyMove>();

            if (!isHit && !isDead)
            {

                if (rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
                {
                    OnAttack(enemy);
                }
                else
                {
                    if (enemy.isFainting || ability.isinvincibility)
                    {
                        return;
                    }

                    if (QuestManager.Instance.isReflect)
                    {
                        float randomIndex = Random.Range(0f, 1f);
                        if (randomIndex <= 0.7f)
                        {
                            OnAttack(enemy);
                            return;
                        }
                    }
                    OnDamaged(collision.transform.position);

                }
            }
        }
    }

    void OnCollisionStay2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("Spike"))
        {
            if (ability.isinvincibility || isHit)
                return;
            OnDamaged(collision2D.transform.position);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Finish")
        {
            // next stage
            gameManager.NextStage();
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {

        if (!collision.CompareTag("Skill") && !collision.CompareTag("GetBox") && !collision.CompareTag("Lightning") && !collision.CompareTag("LimitColl"))
        {
            isGround = true;
            jumpCount = 0;
            anim.SetBool("isGround", true);
        }

    }

    void OnTriggerExit2D()
    {
        anim.SetBool("isGround", false);
        isGround = false;
    }

    public void SetChangeGravity()
    {
        spriteRenderer.flipY = isGravity;
        coll.localScale = new Vector3(coll.localScale.x, coll.localScale.y * -1, coll.localScale.z);
        rigid.gravityScale *= -1;
    }


    void OnAttack(EnemyMove enemy)
    {
        jumpCount = 0;

        rigid.AddForce(Vector2.up * 20, ForceMode2D.Impulse);

        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();
        enemyMove.OnEnemyDamaged(PlayerAttack);

        if (QuestManager.Instance.isDrain)
        {
            float randomIndex = Random.Range(0f, 1f);

            if (randomIndex < 0.5f && gameManager.healthPoint < Charskil.HealthPoint)
            {
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Drain);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Heal);
                gameManager.Hit.gameObject.SetActive(true);
                gameManager.Hit.color = new Color(1, 0, 0, 0.3f);
                gameManager.healthPoint++;
            }
        }

    }

    public void OnDamaged(Vector2 targetPos)
    {

        if (gameManager.PlayerId == 5)
        {
            float chesedPassive = Random.Range(0f, 1f);

            if (chesedPassive <= 0.3f)
            {
                gameManager.Hit.gameObject.SetActive(true);
                gameManager.Hit.color = new Color(1, 0.5f, 1, 0.3f);
                VelocityZero();
                DamagedDir(targetPos);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.evasion);
                return;

            }
        }

        //Anime
        anim.SetTrigger("doDamaged");

        //Change layer
        gameObject.layer = 11;

        //Alpha
        if (!ability.isSkillActive)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        }

        rigid.velocity = Vector2.zero;

        DamagedDir(targetPos);

        isHit = true;


        UpdateHealthAndShield();

        Invoke("OffDamaged", 1);

        //Invoke("IsHitOff", 0.3f);

        gameManager.Hit.gameObject.SetActive(true);
    }


    public void OffDamaged()
    {
        isHit = false;

        gameObject.layer = 10;
        //?�� �?�?
        if (!ability.isSkillActive)
        {
            spriteRenderer.color = new Color(1, 1, 1, 1);
        }
    }

    void IsHitOff()
    {
        isHit = false;
    }

    public void OnDie()

    {
        if (!ability.isSkillActive)
        {
            //Alpha
            spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        }
        //Flip Y
        spriteRenderer.flipY = true;
        //Collider off
        capsuleCollider2D.enabled = false;
        //Die effect
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);

        VelocityZero();

        isDead = true;



    }

    void UpdateHealthAndShield()
    {
        if (gameManager.shieldPoint != 0)
        {
            gameManager.Hit.color = new Color(0, 0, 1, 0.2f);
        }
        else
            gameManager.Hit.color = new Color(1, 0, 0, 0.2f);

        if (gameManager.shieldPoint != 0)
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.ShiledHit);
        }
        else
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);



        gameManager.HealthDown();

    }

    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }

    void SetPlayerStats()
    {
        anim.runtimeAnimatorController = animCon[gameManager.PlayerId];
        maxSpeed *= Charskil.Speed;
        gameManager.healthPoint = Charskil.HealthPoint;
        PlayerAttack *= Charskil.Damaged;
    }

    void DamagedDir(Vector2 targetPos)
    {
        int impactAmount = 15;
        int getShiledImpactAmount = 8;

        rigid.velocity = Vector2.zero;

        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;

        int amount = gameManager.shieldPoint > 0 ? getShiledImpactAmount : impactAmount;

        rigid.AddForce(new Vector2(dirc, 1) * amount, ForceMode2D.Impulse);
    }

    public void EqualsCamera()
    {
        if (pixelPerfectCamera.assetsPPU < 44)
        {
            pixelPerfectCamera.assetsPPU += 2;
        }
    }

    public void MinusCamera()
    {
        if (pixelPerfectCamera.assetsPPU > 8)
        {
            pixelPerfectCamera.assetsPPU -= 2;
        }
    }

    void OnSliderValueChanged(float value)
    {
        inputVec.x = value;

    }

    public void OnPointerUp()
    {
        onMoveSlider.value = 0;
        inputVec.x = 0;
    }

    public void OnChangeMovementSystem()
    {
        if (onChangeMovementToggle.isOn)
        {
            isMovementSlider = true;
            buttonMovement.SetActive(false);
            sliderMovement.SetActive(true);
        }
        else
        {
            isMovementSlider = false;
            buttonMovement.SetActive(true);
            sliderMovement.SetActive(false);
        }

    }

    void ChangeSliderSprite()
    {
        int index = (inputVec.x > 0) ? 1 : (inputVec.x < 0) ? 2 : 0;

        sliderHandler.sprite = sliderSprites[index];

        PlayerPrefs.SetInt("ToggleState", onChangeMovementToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
    }
}
