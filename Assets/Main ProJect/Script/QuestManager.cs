    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    [Header("# UI")]
    public Slider coolDownSlider;
    public ToggleGroup toggleGroup;
    public Toggle[] toggles;
    public Image[] Routes;
    public Text infoText;
    public Text coolDownText;
    [Header("# Particle")]
    public ParticleSystem particle;
    public GameObject particleObj;
    [Header("# Flag Var")]
    public bool isToggleActive;
    public bool isSlow;
    public bool isHealth;
    public bool isShiled;
    public bool isFaintined;
    public bool isReflect;
    public bool isDobbleJump;
    public bool isDrain;

    public Animator PortraitAnim;

    float originalCoolTime;
    bool isCooldown = false;

    void Awake()
    {
        Instance = this;
        infoText.text = "공명할 세피라를 선택해봐요!\n각 세피라 별로 특별한\n능력을 부여받을 수 있어요!";

    }

    private void Start()
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            int index = i;
            toggles[i].onValueChanged.AddListener(delegate { ToggleChanged(index); });
        }


    }

    void Update()
    {

    }

    void ToggleChanged(int index)
    {
        if (isCooldown) return; // If on cooldown, do nothing

        // ���� ���� ����� ��Ȱ��ȭ(interactable=false) ��Ŵ
        if (toggles[index].isOn)
        {
            ActivateAbility(index);
            //toggles[index - 1].interactable = false; // �ش� ����� ��Ȱ��ȭ
            toggles[index].interactable = false;
            StartCoroutine(CooldownCoroutine());
        }
        else
        {
            DeactivateAbility(index);
            toggles[index].interactable = true;
        }
    }
    void ActivateAbility(int index)
    {
        isToggleActive = true;
        particleObj.SetActive(true);
        Color selectColor;
        var mainModule = particle.main;
        PortraitAnim.SetTrigger("Change");
        switch (index)
        {
            case 0:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.FireGun);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.select);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.KetherA);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.KetherB);
                isSlow = true;
                infoText.text = "<공허의 왕관/케테르>\n몬스터를 피격할 시,피격당한\n몬스터의 이동 속도를\n5초동안 감소시킬 수 있어요!";
                selectColor = new Color(0.7f, 0, 1);
                mainModule.startColor = new Color(0.7f, 0, 1);
                // particle.Play();
                break;
            case 1:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.FireGun);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.select);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.flaunch);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Hokma);
                int playerId = GameManager.Instance.PlayerId;
                originalCoolTime = AbilityManager.Instance.skillData[playerId].coolTime;
                AbilityManager.Instance.skillData[playerId].coolTime *= 0.5f;
                infoText.text = "<금단의 지혜/호크마>\n스킬 쿨타임이\n50% 감소되요!!";
                selectColor = new Color(0, 1, 0);
                mainModule.startColor = new Color(0, 1, 0);
                //particle.Play();
                break;
            case 2:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.FireGun);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.select);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Binah);
                isHealth = true;
                infoText.text = "<생명의 이해/비나>\n몬스터를 처치할 시,처치\n위치에 회복 박스를\n드랍해요!";
                selectColor = Color.yellow;
                mainModule.startColor = Color.yellow;
                // particle.Play();
                break;
            case 3:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.FireGun);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.select);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Chesed);
                isFaintined = true;
                infoText.text = "<자비와 영광의 도시/헤세드>\n몬스터를 피격할 시,피격당한\n몬스터를 5초 동안 기절시켜요!";
                selectColor = new Color(1, 0.5f, 1);
                mainModule.startColor = new Color(1, 0.5f, 1);
                //particle.Play();
                break;
            case 4:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.FireGun);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.select);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Gebura);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Reboot);
                isShiled = true;
                infoText.text = "<전자의 힘/게부라>\n몬스터를 처치할 시 처치\n위치에 방패 박스를\n드랍해요!";
                selectColor = new Color(0, 0.9f, 1);
                mainModule.startColor = new Color(0, 0.9f, 1);
                //particle.Play();
                break;
            case 5:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.FireGun);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.select);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.TipheretA);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.TipheretB);
                isReflect = true;
                infoText.text = "<진실의 아름다움/티페레트>\n50%의 확률로 몬스터의 피해는\n반사시키고,투사체에\n맞을 시 파괴시켜요!";
                selectColor = new Color(1, 0.1f, 0.4f);
                mainModule.startColor = new Color(1, 0.1f, 0.4f);
                // particle.Play();
                break;
            case 6:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.SwordHit);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.FireGun);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.select);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Netzach);
                GameManager.Instance.Player.PlayerAttack *= 2f;
                isDrain = true;
                infoText.text = "<살상의 승리/네차흐>\n플레이어의 공격력을\n2배 증가시키고,적 공격시\n50% 확률로 생명력을 회복해요!";
                selectColor = Color.red;
                mainModule.startColor = Color.red;
                // particle.Play();
                break;
            case 7:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.FireGun);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.select);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.HodA);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.HodB);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.HodC);
                GrapplingHook.instance.searchRadius *= 2f;
                infoText.text = "<영광의 숙주/호드>\n그래플링의 탐색 범위가\n2배 증가해요!";
                selectColor = new Color(0, 0.7f, 0);
                mainModule.startColor = new Color(0, 0.7f, 0);
                //particle.Play();
                break;
            case 8:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.FireGun);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.select);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.yesodA);
                GameManager.Instance.Player.maxSpeed *= 1.2f;
                GameManager.Instance.Player.Jump *= 1.2f;
                infoText.text = "<가능성의 기초/예소드>\n플레이어의 속도와 점프력을\n20% 증가시켜요!";
                selectColor = Color.blue;
                mainModule.startColor = Color.blue;
                //  particle.Play();
                break;
            case 9:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.FireGun);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.heart);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.select);
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Malkuth);
                isDobbleJump = true;
                PlayerMove player = GameManager.Instance.Player;
                player.capsuleCollider2D.size = new Vector2(player.capsuleCollider2D.size.x * 0.7f,
                player.capsuleCollider2D.size.y * 1f);
                GameManager.Instance.Player.Jump *= 1.2f;
                GameManager.Instance.Player.maxJumps = 2;
                GameManager.Instance.Player.PlayerAttack *= 1.2f;
                GameManager.Instance.Player.maxSpeed *= 1.2f;
                infoText.text = "<찬란의 왕국/말쿠트>\n플레이어의 점프력,속도,공격력을\n20% 상승시키고 점프 최대 횟수가\n2회로 증가하며 히트박스를\n30% 감소시켜요!";
                selectColor = new Color(0, 1, 0.6f);
                mainModule.startColor = new Color(0, 1, 0.6f);
                // particle.Play();
                break;
            default:
                return;
        }
        particle.Play();

        StartCoroutine(ChangeRoutesColor(selectColor));

    }

    IEnumerator ChangeRoutesColor(Color newColor)
    {
        for (int i = 0; i < Routes.Length; i++)
        {
            Routes[i].color = newColor;
            yield return new WaitForSeconds(0.08f);
        }

    }

    void DeactivateAbility(int index)
    {
        isToggleActive = false;
        particleObj.SetActive(false);
        foreach (Image route in Routes)
        {
            route.color = Color.white;
        }

        PortraitAnim.SetTrigger("Change");

        string[] infoTexts = { "공명할 세피라를 선택해봐요!\n각 세피라 별로 특별한\n능력을 부여받을 수 있어요!", "해방시키지 못한 세피라는\n퀘스트 클리어를 통해\n해방시킬 수 있어요!" };

        infoText.text = infoTexts[Random.Range(0, infoTexts.Length)];

        switch (index)
        {
            case 0:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.selecOff);
                isSlow = false;
                break;
            case 1:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.selecOff);
                int playerId = GameManager.Instance.PlayerId;
                AbilityManager.Instance.skillData[playerId].coolTime = originalCoolTime;
                break;
            case 2:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.selecOff);

                isHealth = false;

                break;
            case 3:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.selecOff);
                ;
                isFaintined = false;

                break;
            case 4:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.selecOff);

                isShiled = false;

                break;
            case 5:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.selecOff);

                isReflect = false;

                break;
            case 6:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.selecOff);

                GameManager.Instance.Player.PlayerAttack /= 2f;
                isDrain = false;
                break;
            case 7:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.selecOff);

                GrapplingHook.instance.searchRadius /= 2f;

                break;
            case 8:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.selecOff);

                GameManager.Instance.Player.maxSpeed /= 1.2f;
                GameManager.Instance.Player.Jump /= 1.2f;

                break;
            case 9:
                AudioManager.instance.PlaySfx(AudioManager.Sfx.selecOff);

                isDobbleJump = false;
                PlayerMove player = GameManager.Instance.Player;
                player.capsuleCollider2D.size = new Vector2(player.capsuleCollider2D.size.x / 0.7f,
                player.capsuleCollider2D.size.y * 1f);
                GameManager.Instance.Player.Jump /= 1.2f;
                GameManager.Instance.Player.maxJumps = 1;
                GameManager.Instance.Player.PlayerAttack /= 1.2f;
                GameManager.Instance.Player.maxSpeed /= 1.2f;

                break;
        }
        particle.Stop();
        //  StartCoroutine(CooldownCoroutine());
    }

    IEnumerator CooldownCoroutine()
    {
        isCooldown = true;
        foreach (Toggle toggle in toggles)
        {
            toggle.interactable = false;
            toggle.GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0.5f);
        }

        float cooldown = 1f;

        coolDownSlider.maxValue = cooldown;
        coolDownSlider.value = cooldown;

        while (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            cooldown = Mathf.Max(cooldown, 0);
            coolDownSlider.value = cooldown;
            coolDownText.text = cooldown.ToString("F2");
            yield return null;
        }


        foreach (Toggle toggle in toggles)
        {
            toggle.interactable = true;
            toggle.GetComponentInChildren<Image>().color = Color.white;
        }
        isCooldown = false;
    }
}
