using System.Collections;
using UnityEngine;

public class BoxStatus : MonoBehaviour
{
    public enum BoxType
    {
        Move, Jump, DobbleJump, Portal, Heal, Shield,
        Hook, Fainting, Boost, Slow, Bomb, TreasureChest, BreakBox, DiamondOre, SilicateOre, GravityBox, ReGravityBox
    }
    public BoxType type;
    public ParticleSystem particle;

    public float value = 0;
    public GameObject portal;
    public GameObject Coll;
    bool isOpend;
    Transform trans;
    Animator anim;

    void OnEnable()
    {
        trans.transform.localScale = Vector3.one;

        switch (type)
        {
            case BoxType.TreasureChest:
                anim.SetBool("Open", false);
                isOpend = false;
                break;
        }
    }

    void Awake()
    {
        particle = GetComponentInChildren<ParticleSystem>();
        trans = GetComponent<Transform>();
        anim = GetComponent<Animator>();



    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        PlayerMove player = collider.GetComponentInParent<PlayerMove>();

        if (player != null && player.gameObject.layer == 10 && collider.CompareTag("Coll"))
        {
            switch (type)
            {
                case BoxType.Jump:
                    anim.SetBool("isColl", true);
                    Vector2 upVelocity = new Vector2(0, value);
                    player.rigid.AddForce(upVelocity, ForceMode2D.Impulse);
                    break;
                case BoxType.DobbleJump:
                    anim.SetBool("isColl", true);
                    player.maxJumps = 2;
                    break;
                case BoxType.GravityBox:
                    player.isGravity = !player.isGravity;
                    player.SetChangeGravity();
                    anim.SetBool("isColl", true);
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.Gravity);
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.selecOff);
                    break;
                case BoxType.ReGravityBox:
                    player.isGravity = !player.isGravity;
                    player.SetChangeGravity();
                    anim.SetBool("isColl", true);
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.Gravity);
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.selecOff);
                    break;
            }
        }

        if (collider.CompareTag("Coll") || collider.CompareTag("Bullet") || collider.CompareTag("Skill"))

        {
            switch (type)
            {
                case BoxType.BreakBox:
                    StartCoroutine(StepBreakBox());
                    break;
                case BoxType.DiamondOre:
                    anim.SetBool("isColl", true);

                    value--;

                    int randomValue = Random.Range(0, 2);

                    if (randomValue < 1)
                        AudioManager.instance.PlaySfx(AudioManager.Sfx.StoneBreak0);
                    else
                        AudioManager.instance.PlaySfx(AudioManager.Sfx.StoneBreak1);

                    GameManager.Instance.stagePoint += 100;

                    if (value < 1)
                    {
                        AudioManager.instance.PlaySfx(AudioManager.Sfx.StoneBreak2);
                        gameObject.SetActive(false);
                    }
                    break;
                case BoxType.SilicateOre:
                    anim.SetBool("isColl", true);

                    value--;

                    randomValue = Random.Range(0, 1);

                    if (randomValue < 1)
                        AudioManager.instance.PlaySfx(AudioManager.Sfx.StoneBreak0);
                    else
                        AudioManager.instance.PlaySfx(AudioManager.Sfx.StoneBreak1);

                    GameManager.Instance.stagePoint += 300;

                    if (value < 1)
                    {
                        AudioManager.instance.PlaySfx(AudioManager.Sfx.StoneBreak2);
                        gameObject.SetActive(false);
                    }
                    break;
            }
        }

        if (collider.CompareTag("Player"))
        {
            switch (type)
            {
                case BoxType.Heal:
                    if (GameManager.Instance.healthPoint < Charskil.HealthPoint) // ���� ü���� �ִ� ü�º��� ���� ���� ȸ��
                    {
                        GameManager.Instance.healthPoint++; // ü�� ȸ��
                        gameObject.SetActive(false);
                        QuestUI.Instance.CollectItem(2);
                        GameManager.Instance.Hit.gameObject.SetActive(true);
                        GameManager.Instance.Hit.color = new Color(1, 8, 0, 0.2f);
                        AudioManager.instance.PlaySfx(AudioManager.Sfx.Heal);
                    }
                    break;
                case BoxType.Shield:
                    if (GameManager.Instance.shieldPoint < 5)
                    {
                        QuestUI.Instance.CollectItem(4);
                        GameManager.Instance.shieldPoint++;
                        AudioManager.instance.PlaySfx(AudioManager.Sfx.GetShiled);
                        gameObject.SetActive(false);

                    }
                    break;
                case BoxType.Hook:
                    gameObject.SetActive(false);
                    GrapplingHook.instance.hookButton.SetActive(true);
                    GrapplingHook.instance.hookInfo.SetActive(true);
                    GrapplingHook.instance.hasGrapplingHook = true;
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.Infomation);
                    break;
                case BoxType.Slow:
                    QuestUI.Instance.CollectItem(0);
                    StartCoroutine(SlowBox());
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.GetShiled);

                    break;
                case BoxType.Boost:
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.speedUp);
                    QuestUI.Instance.CollectItem(8);
                    StartCoroutine(BoostBox());
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.GetShiled);
                    break;
                case BoxType.Fainting:
                    StartCoroutine(FaintingBox());
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.KetherA);
                    //     AudioManager.instance.PlaySfx(AudioManager.Sfx.KetherB);
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.GetShiled);
                    break;
                case BoxType.Bomb:
                    StartCoroutine(BombBox());
                    break;

            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerMove player = collision.gameObject.GetComponentInParent<PlayerMove>();

        if (collision.gameObject.CompareTag("Player"))
        {
            switch (type)
            {
                case BoxType.Portal:
                    anim.SetBool("isColl", true);
                    int dirVec = player.inputVec.x > 0 ? -2 : 2;
                    Vector3 warpPos = new Vector3(portal.transform.position.x + 1 * dirVec, portal.transform.position.y + 2f, portal.transform.position.z);
                    collision.transform.position = warpPos;
                    AudioManager.instance.PlaySfx(AudioManager.Sfx.Portal);
                    break;
                case BoxType.TreasureChest:
                    if (!isOpend)
                    {
                        StartCoroutine(OpenTreasureBox());
                        anim.SetBool("Open", true);
                        isOpend = true;
                        AudioManager.instance.PlaySfx(AudioManager.Sfx.TreasureOpen);
                    }
                    break;
            }

        }

    }


    void OnCollisionExit2D(Collision2D collision)
    {
        anim.SetBool("isColl", false);
    }



    IEnumerator SlowBox()
    {
        gameObject.transform.localScale = Vector3.zero;
        GameManager.Instance.Player.maxSpeed *= 0.5f;
        yield return new WaitForSeconds(5f);

        GameManager.Instance.Player.maxSpeed *= 2f;
        yield return new WaitForSeconds(1f);
        gameObject.transform.localScale = Vector3.one;
    }


    IEnumerator BoostBox()
    {
        gameObject.transform.localScale = Vector3.zero;
        GameManager.Instance.Player.maxSpeed *= 1.5f;

        yield return new WaitForSeconds(6f);
        GameManager.Instance.Player.maxSpeed /= 1.5f;
        yield return new WaitForSeconds(1f);
        gameObject.transform.localScale = Vector3.one;
    }

    IEnumerator FaintingBox()
    {
        Coll.SetActive(true);
        particle.Play();
        yield return new WaitForSeconds(0.1f);
        trans.transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(0.5f);
        FaintingBox faintingBox = Coll.GetComponent<FaintingBox>();
        faintingBox.coll.enabled = false;
        yield return new WaitForSeconds(10f);
        Coll.SetActive(false);
        gameObject.transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(1f);
        gameObject.transform.localScale = Vector3.one;
    }

    IEnumerator BombBox()
    {

        particle.Play();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.ExpoldeBox);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Explode2);
        Coll.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        trans.transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(0.3f);
        Coll.SetActive(false);
        gameObject.transform.localScale = Vector3.zero;
        yield return new WaitForSeconds(1f);
        gameObject.transform.localScale = Vector3.one;
    }

    IEnumerator OpenTreasureBox()
    {
        yield return new WaitForSeconds(0.3f);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Reward);
        for (int i = 0; i < 10; i++)
        {
            GameObject coins = PoolManager.Instance.Get(Random.Range(10, 12));
            Vector2 randomDirection = Random.insideUnitCircle.normalized;
            float distance = Random.Range(0.5f, 1f);
            coins.transform.position = new Vector3(trans.position.x + randomDirection.x * distance,
            trans.position.y + 2, trans.position.z);


            Rigidbody2D rb = coins.GetComponent<Rigidbody2D>();
            float force = Random.Range(100, 300);
            rb.AddForce(randomDirection * force);
        }

        yield return new WaitForSeconds(4f);

        gameObject.SetActive(false);



    }

    IEnumerator StepBreakBox()
    {
        anim.SetTrigger("Smashed");

        AudioManager.instance.PlaySfx(AudioManager.Sfx.BreakBox);

        yield return new WaitForSeconds(0.2f);
        gameObject.SetActive(false);
    }

}
