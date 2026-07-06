using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Warden_Ablity : MonoBehaviour
{
    AbilityManager abilityManager;
    public PlayerMove player;
    bool isVTAttack;
    public Sprite[] SkillIcons;
    public GameObject[] cuttingSword;
    public GameObject SwordGroup;

    void Awake()
    {
        abilityManager = GetComponentInParent<AbilityManager>();
    }

    public void ActivateWarden()
    {
        int playerId = GameManager.Instance.PlayerId;
        SkillData skillData = abilityManager.skillData[playerId];

        StartCoroutine(SkillCoroutine(skillData));
    }

    public IEnumerator SkillCoroutine(SkillData skillData)
    {

        abilityManager.skillBackgroundOb.SetActive(true);

        abilityManager.skillBackgroundImage.color = new Color(1f, 0f, 0f, 0);

        isVTAttack = !isVTAttack;

        abilityManager.isSkillActive = true;

        abilityManager.skillButton.interactable = false;

        EffectManager.instance.PlayParticle(ParticlesType.Warden);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.GasSound);

        float timer = skillData.activeTime;

        Vector2 dashDirection;


        dashDirection = player.spriteRenderer.flipX ? Vector2.left : Vector2.right;

        int curGravity = GameManager.Instance.Player.isGravity ? -1 : 1;

        if (isVTAttack)
        {
            AudioManager.instance.PlaySfx(AudioManager.Sfx.SwordH);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.SwordThrow);
            player.anim.SetTrigger("Horizontal");
            cuttingSword[1].SetActive(true);
            if (!player.isGround)
            {
                player.rigid.AddForce(Vector2.up * 25 * curGravity, ForceMode2D.Impulse);
            }
            cuttingSword[1].transform.localScale = new Vector3(player.spriteRenderer.flipX ? -3 : 3, cuttingSword[1].transform.localScale.y, cuttingSword[1].transform.localScale.z);
            yield return new WaitForSeconds(0.1f);
            cuttingSword[1].SetActive(false);
        }
        else
        {
            player.isDash = true;
            AudioManager.instance.PlaySfx(AudioManager.Sfx.SwordV);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.SwordThrow);
            player.anim.SetTrigger("Vertical");
            cuttingSword[0].SetActive(true);
            cuttingSword[0].transform.localScale = new Vector3(player.spriteRenderer.flipX ? -2 : 2, cuttingSword[1].transform.localScale.y, cuttingSword[1].transform.localScale.z);

            if (!player.isGround)
            {
                player.rigid.AddForce(Vector2.down * 60 * curGravity, ForceMode2D.Impulse);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Lighting);
            }
            else
            {
                player.rigid.AddForce(dashDirection * 10, ForceMode2D.Impulse);
            }

            yield return new WaitForSeconds(0.1f);
            cuttingSword[0].SetActive(false);
            player.isDash = false;
        }


        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        abilityManager.isSkillActive = false;

        abilityManager.skillBackgroundOb.SetActive(false);

        abilityManager.player.spriteRenderer.color = new Color(1, 1, 1);

        EffectManager.instance.PlayParticle(ParticlesType.End);

        abilityManager.skillButton.interactable = true;

        abilityManager.skillCoolTimeImage.fillAmount = 0f;
    }

    void LateUpdate()
    {
        if (isVTAttack)
        {
            if (player.isGround)
                abilityManager.skillButtonImage.sprite = SkillIcons[1];
            else
                abilityManager.skillButtonImage.sprite = SkillIcons[2];
        }
        else
        {
            if (player.isGround)
                abilityManager.skillButtonImage.sprite = SkillIcons[0];
            else
                abilityManager.skillButtonImage.sprite = SkillIcons[3];
        }
    }
}
