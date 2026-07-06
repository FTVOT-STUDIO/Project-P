using System.Collections;
using UnityEngine;

public class LightningTile : MonoBehaviour
{
    public PlayerMove player;

    public float waitingTime;
    public float activeTime;

    Animator anim;
    Coroutine activeCoroutine;


    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    IEnumerator ActiveLightningTile()
    {
        gameObject.tag = "Untagged";

        anim.SetBool("isActive", false);
        yield return new WaitForSeconds(waitingTime);

        gameObject.tag = "Spike";
        anim.SetBool("isActive", true);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Electricshock);

        yield return new WaitForSeconds(activeTime);

        AudioManager.instance.StopSfx(AudioManager.Sfx.Electricshock);
        activeCoroutine = StartCoroutine(ActiveLightningTile());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (activeCoroutine == null)
            {
                activeCoroutine = StartCoroutine(ActiveLightningTile());
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
            {
                return;
            }
            if (activeCoroutine != null)
            {
                StopCoroutine(activeCoroutine);
                activeCoroutine = null; // 코루틴 상태 초기화
                AudioManager.instance.StopSfx(AudioManager.Sfx.Electricshock); // 효과음 중지
                anim.SetBool("isActive", false);
            }
        }
    }
}
