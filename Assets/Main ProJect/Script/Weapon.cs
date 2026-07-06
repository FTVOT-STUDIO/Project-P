using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float coolTime;
    private float curTime;
    public bool isfire = false;
    public GameObject bulletPos;
    SpriteRenderer Player_Sprite;

    void Awake()
    {
        Player_Sprite = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        if (GameManager.Instance.PlayerId == 6)
        {
            bulletPos.gameObject.SetActive(true);
        }
    }

    void Update()
    {
        DirBulletPos();

        if (AbilityManager.Instance.WomputerAblity == null)
            return;
        if ((isfire || Input.GetKeyDown(KeyCode.J)) && AbilityManager.Instance.WomputerAblity.isWeaponActive)
        {
            if (curTime <= 0)
            {
                Shoot();
                curTime = coolTime;
            }
        }
        curTime -= Time.deltaTime;
        isfire = false;
    }

    void DirBulletPos()
    {
        int direction = Player_Sprite.flipX == false ? 1 : -1;
        bulletPos.transform.localPosition = new Vector3(direction, bulletPos.transform.localPosition.y, bulletPos.transform.localPosition.z);
    }

    void Shoot()
    {
        GameObject bullet = PoolManager.Instance.Get(1);
        if (bullet != null)
        {
            bullet.transform.position = bulletPos.transform.position;
            bullet.SetActive(true);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Explode2);
        }
    }
}
