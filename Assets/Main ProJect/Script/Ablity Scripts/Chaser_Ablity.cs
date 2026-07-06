using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Chaser_Ablity : MonoBehaviour
{
    AbilityManager abilityManager;

    void Awake()
    {
        abilityManager = GetComponentInParent<AbilityManager>();
    }
    public void ActivateChaser()
    {
        int playerId = GameManager.Instance.PlayerId;
        SkillData skillData = abilityManager.skillData[playerId];

        StartCoroutine(SkillCoroutine(skillData));
    }

    public IEnumerator SkillCoroutine(SkillData skillData)
    {
        if (!abilityManager.isSkillActive)
        {
            // 스킬 실행 중임을 표시
            abilityManager.isSkillActive = true;

            //            Debug.Log("최이서 스킬 활성화");

            // 스킬 배경 이미지 활성화
            abilityManager.skillBackgroundOb.SetActive(true);

            // 스킬 배경 이미지 색상 변경 애니메이션
            abilityManager.skillBackgroundImage.DOColor(Color.white, 0.2f)
                .OnComplete(() =>
                {
                    abilityManager.skillBackgroundImage.color = new Color(0.2f, 0.7f, 0.4f, 0f);
                });

            // 캐릭터의 이동 방향 설정
            Flesh();

            // 후크 설정
            if (GrapplingHook.instance.isAttach)
            {
                GrapplingHook.instance.isAttach = false;
                GrapplingHook.instance.isHookActive = false;
                GrapplingHook.instance.hook.GetComponent<Hooking>().joint2D.enabled = false;
                GrapplingHook.instance.hook.gameObject.SetActive(false);
            }

            // 캐릭터의 이동 속도 증가
            GameManager.Instance.Player.maxSpeed *= 1.2f;
            //업적
            QuestUI.Instance.CollectItem(8);
            // 캐릭터 색상 변경
            abilityManager.sprite.color = new Color(0f, 0.5f, 0f);

            // 이펙트 실행
            EffectManager.instance.PlayParticle(ParticlesType.Chaser);
            // EffectManager.instance.PlayParticle(2, playerPosition);
            // 스킬 사용 사운드 재생
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Lighting);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Freeze);

            // 스킬 활성화 시간 설정
            float timer = skillData.activeTime;

            while (timer > 0f)
            {
                // 스킬 쿨타임 이미지 업데이트
                abilityManager.skillCoolTimeImage.fillAmount = timer / skillData.activeTime;
                abilityManager.skillCoolTimeImage.color = new Color(0.2f, 0.8f, 0.2f, 0.8f);
                timer -= Time.deltaTime;
                yield return null;
            }

            // 스킬 끝나면 캐릭터 이동 속도 원래대로 돌리기
            GameManager.Instance.Player.maxSpeed /= 1.2f;

            // 스킬 배경 이미지 비활성화
            abilityManager.skillBackgroundOb.SetActive(false);

            // 캐릭터 색상 원래대로 돌리기
            abilityManager.sprite.color = new Color(1, 1, 1);

            // 쿨타임 시작
            timer = skillData.coolTime;

            // 스킬 사용 이펙트 실행
            EffectManager.instance.PlayParticle(ParticlesType.End);
            // EffectManager.instance.PlayParticle(0, playerPosition);
            // 스킬 쿨타임 사운드 재생
            AudioManager.instance.PlaySfx(AudioManager.Sfx.GasSound);

            // 쿨타임 진행
            while (timer > 0)
            {
                // 스킬 쿨타임 이미지 업데이트
                abilityManager.skillCoolTimeImage.fillAmount = timer / skillData.coolTime;
                timer -= Time.deltaTime;
                abilityManager.skillCoolTimeImage.color = new Color(1, 1, 1, 0.8f);
                yield return null;
            }

            // 스킬 사용 종료
            abilityManager.isSkillActive = false;
            abilityManager.skillCoolTimeImage.fillAmount = 0f;

            yield return null;
        }
    }

    void Flesh()
    {
        Vector2 moveDirection = GameManager.Instance.Player.transform.right;
        if (GameManager.Instance.Player.spriteRenderer.flipX)
        {
            moveDirection = -moveDirection;
        }

        // 순간이동 범위 설정
        float moveDistance = 8f;

        // 플레이어의 이동 방향에 따라 여러 방향으로 레이를 쏩니다.
        Vector2[] directions = { moveDirection, Quaternion.Euler(0, 0, 45) * moveDirection, Quaternion.Euler(0, 0, -45) * moveDirection };

        foreach (Vector2 direction in directions)
        {
            // 이동할 위치에 벽이 있는지 검사
            RaycastHit2D hit = Physics2D.Raycast(GameManager.Instance.Player.transform.position, direction, moveDistance, LayerMask.GetMask("Platform", "Box"));
            if (hit.collider != null)
            {
                // 충돌이 발생한 경우 이동 거리를 충돌 지점까지로 조정
                moveDistance = Mathf.Min(moveDistance, hit.distance);
            }
        }

        // 이동 거리만큼 플레이어를 순간이동합니다.
        GameManager.Instance.Player.transform.Translate(moveDirection * moveDistance);
    }
}
