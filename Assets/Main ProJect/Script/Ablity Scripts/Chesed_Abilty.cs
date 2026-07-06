using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using DG.Tweening;

public class Chesed_Abilty : MonoBehaviour
{
    AbilityManager abilityManager;

    public GameObject chesedSkill;

    void Awake()
    {
        abilityManager = GetComponentInParent<AbilityManager>();
    }
    public void ActivateChesed()
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
            //            Debug.Log("헤세드");

            abilityManager.skillBackgroundOb.SetActive(true);
            abilityManager.skillBackgroundImage.DOColor(Color.white, 0.2f)
                .OnComplete(() =>
                {
                    abilityManager.skillBackgroundImage.color = new Color(0f, 0.3f, 1f, 0f);
                });

            //스킬 능력
            chesedSkill.SetActive(true);
            chesedSkill.GetComponent<ChesedSkill>().CapsuleCollider2D.enabled = true;

            //이펙트 실행
            EffectManager.instance.PlayParticle(ParticlesType.chesed);


            //캐릭터 색상 변경
            abilityManager.sprite.color = new Color(1f, 0.6f, 0.7f);

            // 스킬 활성화 시간
            float timer = skillData.activeTime;

            //사운드
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Skill);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.ASTLSkill);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Crush);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.GasSound);
            while (timer > 0f)
            {
                abilityManager.skillCoolTimeImage.fillAmount = timer / skillData.activeTime;
                abilityManager.skillCoolTimeImage.color = new Color(1f, 0.6f, 0.7f);
                //  EffectManager.instance.Effects[6].transform.position = player.transform.position;
                timer -= Time.deltaTime;
                yield return null;
            }


            chesedSkill.GetComponent<ChesedSkill>().CapsuleCollider2D.enabled = false;

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
            //스킬 능력 끝남
            //chesedSkill.SetActive(false);
            abilityManager.isSkillActive = false;
            abilityManager.skillCoolTimeImage.fillAmount = 0f;
        }
    }
}
