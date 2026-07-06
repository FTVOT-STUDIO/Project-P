using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Womputer_Ablity : MonoBehaviour
{
    AbilityManager abilityManager;
    [SerializeField] GameObject weapon;

    public bool isWeaponActive = false;

    void Awake()
    {
        abilityManager = GetComponentInParent<AbilityManager>();
    }

    public void ActivateWomputer()
    {
        int playerId = GameManager.Instance.PlayerId;
        SkillData skillData = abilityManager.skillData[playerId];

        StartCoroutine(SkillCoroutine(skillData));
    }

    public IEnumerator SkillCoroutine(SkillData skillData)
    {
        Debug.Log("��ǻ��");

        abilityManager.isSkillActive = true;

        abilityManager.skillButton.interactable = false;

        if (!isWeaponActive)
        {
            GameManager.Instance.shieldPoint++;
            QuestUI.Instance.CollectItem(4);
        }

        isWeaponActive = !isWeaponActive;

        GameManager.Instance.Player.anim.SetBool("Weapon", isWeaponActive);

        GameManager.Instance.Player.anim.SetTrigger("WeaponChange");

        EffectManager.instance.PlayParticle(ParticlesType.End);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.GasSound);

        weapon.SetActive(isWeaponActive);

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Reboot);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Camera);

        float timer = skillData.activeTime;

        if (isWeaponActive)
        {

            abilityManager.player.PlayerAttack -= 2;
        }
        else
        {

            abilityManager.player.PlayerAttack += 2;
        }

        while (timer > 0f)
        {
            abilityManager.skillCoolTimeImage.fillAmount = timer / skillData.activeTime;
            abilityManager.skillCoolTimeImage.color = new Color(1, 1, 1, 0.8f);
            timer -= Time.deltaTime;
            yield return null;
        }

        abilityManager.isSkillActive = false;

        abilityManager.skillBackgroundOb.SetActive(false);

        abilityManager.player.spriteRenderer.color = new Color(1, 1, 1);

        timer = skillData.coolTime;

        EffectManager.instance.PlayParticle(ParticlesType.End);

        abilityManager.skillButton.interactable = true;

        // isSkillCoolTime = false;
        abilityManager.skillCoolTimeImage.fillAmount = 0f;
    }



}
