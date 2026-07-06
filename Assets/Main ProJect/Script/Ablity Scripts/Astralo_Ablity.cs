using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.VisualScripting;

public class Astralo_Ablity : MonoBehaviour
{
    AbilityManager abilityManager;

    void Awake()
    {
        abilityManager = GetComponentInParent<AbilityManager>();
    }
    public void ActivateAstral()
    {
        int playerId = GameManager.Instance.PlayerId;
        SkillData skillData = abilityManager.skillData[playerId];

        StartCoroutine(SkillCoroutine(skillData));

    }

    public IEnumerator SkillCoroutine(SkillData skillData)
    {
        if (!abilityManager.isSkillActive)
        {
            //스킬 실행 했을때
            abilityManager.isSkillActive = true;
            Debug.Log("아스트랄로 스킬 활성화");

            abilityManager.skillBackgroundOb.SetActive(true);
            abilityManager.skillBackgroundImage.DOColor(Color.white, 0.2f)
                .OnComplete(() =>
                {
                    abilityManager.skillBackgroundImage.color = new Color(0f, 0.3f, 1f, 0f);
                });

            //스킬 능력
            abilityManager.player.PlayerAttack *= 2;
            abilityManager.player.Jump *= 1.5f;
            //이펙트 실행

            EffectManager.instance.PlayParticle(ParticlesType.Astralo);

            //캐릭터 색상 변경
            abilityManager.sprite.color = new Color(0f, 0.9f, 1f);

            // 스킬 활성화 시간
            float timer = skillData.activeTime;

            //사운드
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Skill);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.ASTLSkill);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.GasSound);
            while (timer > 0f)
            {
                abilityManager.skillCoolTimeImage.fillAmount = timer / skillData.activeTime;
                abilityManager.skillCoolTimeImage.color = new Color(0.2f, 0.2f, 0.8f, 0.8f);
                timer -= Time.deltaTime;
                yield return null;
            }
            //스킬 능력 끝남
            abilityManager.player.PlayerAttack /= 2;
            abilityManager.player.Jump /= 1.5f;

            abilityManager.skillBackgroundOb.SetActive(false);

            abilityManager.sprite.color = new Color(1, 1, 1);
            timer = skillData.coolTime;

            EffectManager.instance.PlayParticle(ParticlesType.End);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.GasSound);

            //쿨타임
            while (timer > 0)
            {
                abilityManager.skillCoolTimeImage.fillAmount = timer / skillData.coolTime;
                timer -= Time.deltaTime;
                abilityManager.skillCoolTimeImage.color = new Color(1, 1, 1, 0.8f);
                yield return null;
            }
            abilityManager.isSkillActive = false;
            abilityManager.skillCoolTimeImage.fillAmount = 0f;
        }
    }
}
