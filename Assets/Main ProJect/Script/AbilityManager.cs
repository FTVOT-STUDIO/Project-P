using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
[System.Serializable]
public class SkillData
{
    public string name;
    public float activeTime;
    public float coolTime;
}

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager Instance;

    [Header("# Skill Data")]
    public SkillData[] skillData;


    [Header("# Player Info")]
    public PlayerMove player;
    public GameObject[] SkillIndex;
    [Header("# Sprite")]
    public Image skillBackgroundImage;
    public GameObject skillBackgroundOb;
    public Image skillButtonImage;
    public Sprite[] skillSprite;
    public Image skillCoolTimeImage;
    public Button skillButton;

    [Header("# Active")]
    public bool isSkillActive = false;

    public bool isinvincibility = false;

    [Header("# Animation & Sprite")]
    [SerializeField] Animator anim;

    public SpriteRenderer sprite;
    public Image actionBtnImage;
    public Animator actionBtnAnime;

    //==========================================

    Astralo_Ablity AstraloAblity;
    Chaser_Ablity ChaserAblity;
    ScalarUI_Ablity ScalarUIAblity;
    DrTrigger_Ablity DrTriggerAblity;
    TanZeta_Ablity TanZetaAblity;
    [HideInInspector] public Chesed_Abilty ChesedAbilty;
    [HideInInspector] public Womputer_Ablity WomputerAblity;
    [HideInInspector] public Alice_Ablity AliceAblity;
    [HideInInspector] public Warden_Ablity WardenAblity;
    Angela_Ablity AngelaAblity;


    void Awake()
    {
        Instance = this;
        actionBtnImage = actionBtnAnime.GetComponent<Image>();
        actionBtnAnime = actionBtnAnime.GetComponent<Animator>();

    }
    public void GetComponentScripts()
    {
        AstraloAblity = GetComponentInChildren<Astralo_Ablity>();
        ChaserAblity = GetComponentInChildren<Chaser_Ablity>();
        ScalarUIAblity = GetComponentInChildren<ScalarUI_Ablity>();
        DrTriggerAblity = GetComponentInChildren<DrTrigger_Ablity>();
        TanZetaAblity = GetComponentInChildren<TanZeta_Ablity>();
        ChesedAbilty = GetComponentInChildren<Chesed_Abilty>();
        WomputerAblity = GetComponentInChildren<Womputer_Ablity>();
        AliceAblity = GetComponentInChildren<Alice_Ablity>();
        WardenAblity = GetComponentInChildren<Warden_Ablity>();
        AngelaAblity = GetComponentInChildren<Angela_Ablity>();
    }
    void Start()
    {
        player = GameManager.Instance.Player;
        skillButtonImage.sprite = skillSprite[GameManager.Instance.PlayerId];
    }

    public void SetBtnSprite()
    {
        int skillID = GameManager.Instance.PlayerId;

        if (skillID < skillSprite.Length)
        {
            skillButtonImage.sprite = skillSprite[skillID];
        }
    }

    public void Init()
    {
        int skillID = GameManager.Instance.PlayerId;

        QuestUI.Instance.CollectItem(1);

        GameManager.Instance.skillCount++;

        switch (skillID)
        {
            case 0:
                AstraloAblity.ActivateAstral();
                break;
            case 1:
                ChaserAblity.ActivateChaser();
                break;
            case 2:
                ScalarUIAblity.ActivateScalar();
                break;
            case 3:
                DrTriggerAblity.ActivateDrTrigger();
                break;
            case 4:
                TanZetaAblity.ActivateTanZeta();
                break;
            case 5:
                ChesedAbilty.ActivateChesed();
                break;
            case 6:
                WomputerAblity.ActivateWomputer();
                break;
            case 7:
                AliceAblity.ActivateAlice();
                break;
            case 8:
                WardenAblity.ActivateWarden();
                break;
            case 9:
                AngelaAblity.ActivateAngela();
                break;
        }
    }

    void Update()
    {
        Action_Btn_Set_Color();
        if (!GameManager.Instance.isStart)
            return;
        if (Input.GetKeyDown(KeyCode.N) && !isSkillActive)
        {
            Init();
        }

    }

    void Action_Btn_Set_Color()
    {
        Color default_Color = Color.white;
        Color CoolTime_Color = new Color(0.4f, 0.4f, 0.4f);

        if (isSkillActive)
        {
            actionBtnImage.color = CoolTime_Color;
            skillButton.interactable = false;
        }
        else if (AliceAblity != null && GameManager.Instance.PlayerId == 7 && AliceAblity.ExpCount < 4)
        {
            actionBtnImage.color = CoolTime_Color;
            skillButton.interactable = false;
        }
        else
        {
            actionBtnImage.color = default_Color;
            //actionBtnAnime.enabled = true;
            skillButton.interactable = true;
        }
    }
}

