using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Alice_Ablity : MonoBehaviour
{
    AbilityManager abilityManager;

    public int ExpCount;

    [SerializeField] GameObject ElenaObject;

    [SerializeField] GameObject RepositionBtn;

    void Awake()
    {
        abilityManager = GetComponentInParent<AbilityManager>();
    }
    public void ActivateAlice()
    {
        int playerId = GameManager.Instance.PlayerId;
        SkillData skillData = AbilityManager.Instance.skillData[playerId];

        StartCoroutine(SkillCoroutine(skillData));
    }

    public IEnumerator SkillCoroutine(SkillData skillData)
    {
        if (!abilityManager.isSkillActive && ExpCount >= 4)
        {
            abilityManager.isSkillActive = true;

            Debug.Log("¾Ù¸®½º & ¾Ù·¹³ª ½ºÅ³");

            abilityManager.skillBackgroundOb.SetActive(true);
            abilityManager.skillBackgroundImage.DOColor(Color.white, 0.2f)
                .OnComplete(() =>
                {
                    abilityManager.skillBackgroundImage.color = new Color(0f, 0.3f, 1f, 0f);
                });


            ElenaObject.SetActive(true);
            RepositionBtn.SetActive(true);

            EffectManager.instance.PlayParticle(ParticlesType.Astralo);

            //Ä³ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
            abilityManager.sprite.color = Color.cyan;

            // ï¿½ï¿½Å³ È°ï¿½ï¿½È­ ï¿½Ã°ï¿½
            float timer = skillData.activeTime;

            //ï¿½ï¿½ï¿½ï¿½
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hi);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Skill);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.GasSound);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Bell);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Whistle);
            while (timer > 0f)
            {
                abilityManager.skillCoolTimeImage.fillAmount = timer / skillData.activeTime;
                abilityManager.skillCoolTimeImage.color = Color.cyan;
                timer -= Time.deltaTime;
                yield return null;
            }
            //ï¿½ï¿½Å³ ï¿½É·ï¿½ ï¿½ï¿½ï¿½ï¿½


            ElenaObject.SetActive(false);
            RepositionBtn.SetActive(false);
            ExpCount = 0;
            abilityManager.skillBackgroundOb.SetActive(false);

            abilityManager.sprite.color = new Color(1, 1, 1);
            timer = skillData.coolTime;

            EffectManager.instance.PlayParticle(ParticlesType.End);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.GasSound);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.ByeBye);
            //ï¿½ï¿½Å¸ï¿½ï¿½
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

    public void GetAliceAblity()
    {
        if (ExpCount < 5)
        {
            abilityManager.skillCoolTimeImage.color = Color.cyan;
            abilityManager.skillCoolTimeImage.fillAmount = (float)ExpCount / 4f;
            if (abilityManager.skillCoolTimeImage.fillAmount == 1)
            {
                AudioManager.instance.PlaySfx(AudioManager.Sfx.max);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.oh);
            }
        }
    }
}
