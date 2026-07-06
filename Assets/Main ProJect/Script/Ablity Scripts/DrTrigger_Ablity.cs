using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using DG.Tweening;

public class DrTrigger_Ablity : MonoBehaviour
{
    AbilityManager abilityManager;

    void Awake()
    {
        abilityManager = GetComponentInParent<AbilityManager>();

    }
    public void ActivateDrTrigger()
    {
        int playerId = GameManager.Instance.PlayerId;
        SkillData skillData = abilityManager.skillData[playerId];

        StartCoroutine(SkillCoroutine(skillData));
    }

    public IEnumerator SkillCoroutine(SkillData skillData)
    {
        if (!AbilityManager.Instance.isSkillActive && GameManager.Instance.healthPoint != 5)
        {
            // 스킬 활성화 했을때
            abilityManager.isSkillActive = true;

            abilityManager.skillButton.interactable = false;

            Debug.Log("Dr.트리거");

            abilityManager.skillBackgroundOb.SetActive(true);
            abilityManager.skillBackgroundImage.DOColor(Color.green, 0.4f)
                 .OnComplete(() =>
                 {
                     abilityManager.skillBackgroundImage.color = new Color(0.8f, 0f, 1f, 0f);
                 });

            //스킬 능력
            if (GameManager.Instance.healthPoint <= 4)
            {
                GameManager.Instance.healthPoint++;
                QuestUI.Instance.CollectItem(2);
                GameManager.Instance.UpdateHealthUI();
            }

            Vector3 playerPosition = abilityManager.player.transform.position;
            EffectManager.instance.PlayParticle(ParticlesType.End);

            //캐릭터 색상 변경
            abilityManager.sprite.color = new Color(0.9f, 0.1f, 0.9f);


            // 스킬 활성화 시간
            float timer = skillData.activeTime;
            //사운드
            AudioManager.instance.PlaySfx(AudioManager.Sfx.GasSound);

            while (timer > 0f)
            {
                abilityManager.skillCoolTimeImage.fillAmount = timer / skillData.activeTime;
                abilityManager.skillCoolTimeImage.color = new Color(0.5f, 0.2f, 0.9f, 0.8f);
                timer -= Time.deltaTime;
                yield return null;
            }

            abilityManager.skillBackgroundOb.SetActive(false);

            abilityManager.player.spriteRenderer.color = new Color(1, 1, 1);

            //sprite.color = new Color(1, 1, 1);

            timer = skillData.coolTime;

            playerPosition = abilityManager.player.transform.position;
            EffectManager.instance.PlayParticle(ParticlesType.End);

            AudioManager.instance.PlaySfx(AudioManager.Sfx.GasSound);


            //쿨타임
            while (timer > 0)
            {
                // isSkillCoolTime = true;
                abilityManager.skillCoolTimeImage.fillAmount = timer / skillData.coolTime;
                timer -= Time.deltaTime;
                abilityManager.skillCoolTimeImage.color = new Color(1, 1, 1, 0.8f);
                yield return null;
            }

            abilityManager.skillButton.interactable = true;
            abilityManager.isSkillActive = false;
            // isSkillCoolTime = false;
            abilityManager.skillCoolTimeImage.fillAmount = 0f;
        }
    }
}
