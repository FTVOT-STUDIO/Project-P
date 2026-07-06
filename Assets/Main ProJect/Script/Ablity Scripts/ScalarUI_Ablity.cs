using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScalarUI_Ablity : MonoBehaviour
{
    AbilityManager abilityManager;

    void Awake()
    {

        abilityManager = GetComponentInParent<AbilityManager>();
    }

    public void ActivateScalar()
    {
        int playerId = GameManager.Instance.PlayerId;
        SkillData skillData = abilityManager.skillData[playerId];

        StartCoroutine(SkillCoroutine(skillData));
    }

    public IEnumerator SkillCoroutine(SkillData skillData)
    {
        if (!abilityManager.isSkillActive)
        {
            // 스킬 활성화 했을때
            abilityManager.isSkillActive = true;

            Debug.Log("스칼라우이");

            abilityManager.skillBackgroundOb.SetActive(true);
            abilityManager.skillBackgroundImage.DOColor(Color.red, 0.4f)
                .OnComplete(() =>
                {
                    abilityManager.skillBackgroundImage.color = new Color(0.8f, 0f, 1f, 0f);
                });

            //스킬 능력
            abilityManager.isinvincibility = true;

            abilityManager.player.PlayerAttack *= 2;

            EffectManager.instance.PlayParticle(ParticlesType.ScalarUI);
            EffectManager.instance.PlayParticle(ParticlesType.ScalarUICurrunt);


            //캐릭터 색상 변경
            abilityManager.sprite.color = new Color(0.9f, 0.1f, 0.9f);

            foreach (Image image in GameManager.Instance.UIHealthl)
            {
                image.color = new Color(0.2f, 0.2f, 0.2f); // 원하는 색상으로 변경
            }

            // 스킬 활성화 시간
            float timer = skillData.activeTime;

            //사운드
            AudioManager.instance.PlaySfx(AudioManager.Sfx.GasSound);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.DemonSfx);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Zap);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.VoidSFX);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.heartbeat);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Ghost);
            while (timer > 0f)
            {
                abilityManager.skillCoolTimeImage.fillAmount = timer / skillData.activeTime;
                abilityManager.skillCoolTimeImage.color = new Color(0.5f, 0.2f, 0.9f, 0.8f);
                timer -= Time.deltaTime;
                yield return null;
            }


            //능력 끝남
            abilityManager.isinvincibility = false;

            abilityManager.player.PlayerAttack /= 2;

            abilityManager.skillBackgroundOb.SetActive(false);

            abilityManager.sprite.color = new Color(1, 1, 1);
            timer = skillData.coolTime;

            foreach (Image image in GameManager.Instance.UIHealthl)
            {
                image.color = new Color(1f, 1f, 1f); // 원하는 색상으로 변경
            }

            EffectManager.instance.StopParticle(ParticlesType.ScalarUICurrunt);
            EffectManager.instance.PlayParticle(ParticlesType.End);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.GasSound);
            AudioManager.instance.StopSfx(AudioManager.Sfx.Ghost);
            AudioManager.instance.StopSfx(AudioManager.Sfx.heartbeat);
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
