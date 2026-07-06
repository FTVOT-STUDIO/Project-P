using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Angela_Ablity : MonoBehaviour
{
    AbilityManager abilityManager;

    public GameObject[] weaponTrans;

    void Awake()
    {
        abilityManager = GetComponentInParent<AbilityManager>();
    }
    public void ActivateAngela()
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

            abilityManager.skillBackgroundOb.SetActive(true);
            abilityManager.skillBackgroundImage.DOColor(Color.white, 0.1f)
                .OnComplete(() =>
                {
                    abilityManager.skillBackgroundImage.color = new Color(0f, 0.3f, 1f, 0f);
                });

            //스킬 능력
            for (int i = 0; i < weaponTrans.Length; i++)
            {
                if (i == 0 || i == 1)
                {
                    weaponTrans[i].transform.DOLocalMoveX(weaponTrans[i].transform.localPosition.x * 2, 0.5f);
                }
                else if (i == 2 || i == 3)
                {

                    weaponTrans[i].transform.DOLocalMoveY(weaponTrans[i].transform.localPosition.y * 2, 0.5f);
                }
            }

            foreach (GameObject index in weaponTrans)
            {
                index.gameObject.transform.DOScale(index.gameObject.transform.localScale * 1.5f, 1.5f);
            }

            weaponTrans[0].GetComponentInParent<AngelaBullet>().bulletSpeed *= 2;

            abilityManager.player.maxSpeed *= 1.2f;

            //이펙트 실행

            EffectManager.instance.PlayParticle(ParticlesType.Astralo);

            //캐릭터 색상 변경
            abilityManager.sprite.color = new Color(0.45f, 0.45f, 0.9f);

            // 스킬 활성화 시간
            float timer = skillData.activeTime;

            //사운드
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Skill);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.angleaActive);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Trans);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.GasSound);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.peeee);

            while (timer > 0f)
            {
                abilityManager.skillCoolTimeImage.fillAmount = timer / skillData.activeTime;
                abilityManager.skillCoolTimeImage.color = new Color(0.45f, 0.45f, 0.9f, 0.8f);
                timer -= Time.deltaTime;
                yield return null;
            }

            //스킬 능력 끝남

            for (int i = 0; i < weaponTrans.Length; i++)
            {
                if (i == 0 || i == 1)
                {

                    weaponTrans[i].transform.DOLocalMoveX(weaponTrans[i].transform.localPosition.x / 2, 1f);
                }
                else if (i == 2 || i == 3)
                {

                    weaponTrans[i].transform.DOLocalMoveY(weaponTrans[i].transform.localPosition.y / 2, 1f);
                }
            }

            foreach (GameObject index in weaponTrans)
            {
                index.gameObject.transform.DOScale(index.gameObject.transform.localScale / 1.5f, 1.5f);
            }

            weaponTrans[0].GetComponentInParent<AngelaBullet>().bulletSpeed /= 2;

            abilityManager.player.maxSpeed /= 1.2f;


            abilityManager.skillBackgroundOb.SetActive(false);

            abilityManager.sprite.color = new Color(1, 1, 1);
            timer = skillData.coolTime;

            EffectManager.instance.PlayParticle(ParticlesType.End);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.GasSound);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.peeee);
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
