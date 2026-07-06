using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.U2D;

public class TanZeta_Ablity : MonoBehaviour
{
    AbilityManager abilityManager;

    void Awake()
    {
        abilityManager = GetComponentInParent<AbilityManager>();
    }

    [SerializeField] GameObject tanZeta;
    public void ActivateTanZeta()
    {
        int playerId = GameManager.Instance.PlayerId;
        SkillData skillData = abilityManager.skillData[playerId];

        StartCoroutine(SkillCoroutine(skillData));
    }

    public IEnumerator SkillCoroutine(SkillData skillData)
    {
        if (!abilityManager.isSkillActive)
        {
            // 스킬 사용 시작
            abilityManager.isSkillActive = true;

            tanZeta.gameObject.SetActive(true);

            abilityManager.skillButton.interactable = false;

            AudioManager.instance.PlaySfx(AudioManager.Sfx.GasSound);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.flaunch);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Explode);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Explode2);

            Debug.Log("탄제타");

            abilityManager.skillBackgroundOb.SetActive(true);
            abilityManager.skillBackgroundImage.DOColor(Color.black, 0.2f)
                .OnComplete(() =>
                {
                    abilityManager.skillBackgroundImage.color = new Color(0.8f, 0.2f, 1f, 0f);
                });

            Vector3 playerPosition = abilityManager.player.transform.position;
            EffectManager.instance.PlayParticle(ParticlesType.End);
            EffectManager.instance.PlayParticle(ParticlesType.TanZeta);

            //캐릭터 색상 변경
            abilityManager.sprite.color = new Color(0.9f, 0.1f, 0.9f);


            // 스킬 활성화 시간
            float timer = skillData.activeTime;
            //사운드
            AudioManager.instance.PlaySfx(AudioManager.Sfx.GasSound);

            // 활성화 타이머
            while (timer > 0f)
            {
                abilityManager.skillCoolTimeImage.fillAmount = timer / skillData.activeTime;
                abilityManager.skillCoolTimeImage.color = new Color(0.5f, 0.2f, 0.9f, 0.8f);
                timer -= Time.deltaTime;
                yield return null;
            }

            tanZeta.gameObject.SetActive(false);

            abilityManager.skillBackgroundOb.SetActive(false);

            abilityManager.player.spriteRenderer.color = new Color(1, 1, 1);


            timer = skillData.coolTime;

            playerPosition = abilityManager.player.transform.position;
            EffectManager.instance.PlayParticle(ParticlesType.End);

            AudioManager.instance.PlaySfx(AudioManager.Sfx.GasSound);


            //쿨타임 타이머
            while (timer > 0)
            {
                // isSkillCoolTime = true;
                abilityManager.skillCoolTimeImage.fillAmount = timer / skillData.coolTime;
                timer -= Time.deltaTime;
                abilityManager.skillCoolTimeImage.color = new Color(1, 1, 1, 0.8f);
                yield return null;
            }

            // 스킬 사용 종료
            abilityManager.isSkillActive = false;
            abilityManager.skillButton.interactable = true;
            abilityManager.skillCoolTimeImage.fillAmount = 0f;
        }
    }
}
