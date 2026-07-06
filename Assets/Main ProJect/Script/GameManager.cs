using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    [Header("# Test Mode")]
    public bool testMode;
    public static GameManager Instance;
    [Header("# Player ID")]
    [Range(0, 9)]
    public int PlayerId;
    [Header("=========[Player]")]
    [Range(1, 999)] public int healthPoint;
    public PlayerMove Player;
    public GameObject RocketShadow;
    public bool isStart;
    // ===========================
    public int KillCount;
    public int jumpCount;
    public int skillCount;
    public int grapplingCount;
    public int qeustClearCount;
    // ===========================
    public int currentStreak;
    public int shieldPoint;
    public Vector3 set_Position;
    public bool isPlayerUseGravity;
    public GameObject SwordGroup;
    public GameObject WeaponGroup;
    [Header("=========[Stage]")]
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public GameObject[] Stages;

    [Header("=========[UI]")]

    [Header("Screen Transition")]
    public GameObject screenTransition;

    [Header("# Health & Shield")]
    public List<Image> UIHealthl;
    public List<Image> UIShield;
    [Header("# Game Score")]
    public Text UIScore;
    public Text MaxScore;
    public Text UIStage;
    public Text ClearScore;
    public Text RetryScore;
    public Text KillScore;
    public Text KillCountText;
    [Header("# UI Button")]
    public GameObject RestartButton;
    public GameObject GameClearButton;
    public GameObject bulletButton;
    public ButtonManager btnManger;
    public GameObject hookBtn;
    [Header("# In Game HUD")]
    public GameObject HUD;
    public GameObject pickUI;
    public UIButtonManager UiManager;
    public Image Hit;
    public GameObject StackCount;
    public GameObject WeaponStackCount;
    [Header("# Save & Load Mark")]
    public GameObject Repos_Mark;
    [HideInInspector] public GameObject Loc_Mark;

    void Awake()
    {
        Instance = this;
        isStart = false;
        if (!PlayerPrefs.HasKey("MaxScore"))
        {
            PlayerPrefs.SetInt("MaxScore", 0);
        }

        MaxScore.text = PlayerPrefs.GetInt("MaxScore").ToString();

        btnManger.InitData();
    }

    void Start()
    {
        AudioManager.instance.PlayBgm(AudioManager.Bgm.MainTheme0);
        screenTransition.SetActive(true);
        int maxScore = PlayerPrefs.GetInt("MaxScore", 0);
        MaxScore.text = PlayerPrefs.GetInt("MaxScore").ToString();

        shieldPoint = 0;

        if (testMode)
        {
            TestMode();
        }
    }

    public void SetID(int id)
    {
        PlayerId = id;
    }

    public void GameStart(int stageID)
    {
        AudioManager.instance.StopBgm();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.StartSfx);

        stageIndex = stageID;
        // 게임 시작 코루틴
        StartCoroutine(GameStartRoutin());
    }

    IEnumerator GameStartRoutin()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.AirPlain);

        yield return new WaitForSeconds(2f);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Falling);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.GetShiled);
        AbilityManager.Instance.SkillIndex[PlayerId].SetActive(true);

        AbilityManager.Instance.GetComponentScripts();

        Player.gameObject.SetActive(true);
        Player.parachute.SetActive(true);
        Stages[stageIndex].SetActive(true);
        RocketShadow.SetActive(true);

        UIStage.text = "STAGE " + stageIndex;

        Resume();
        UpdateHealthUI();
        SetPlayerWeapon(); // 플레이어 무기 초기화

        Player.rigid.gravityScale /= 6;

        AbilityManager.Instance.SetBtnSprite();



        switch (stageIndex)
        {
            case 0:
                AudioManager.instance.PlayBgm(AudioManager.Bgm.TheVoid);
                break;
            case 2:
                AudioManager.instance.PlayBgm(AudioManager.Bgm.PracticeTheme);
                break;

        }


        yield return new WaitForSeconds(3.5f);
        Player.rigid.gravityScale *= 6;
        HUD.SetActive(true);
        Player.parachute.SetActive(false);
        RocketShadow.SetActive(false);
        isStart = true;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.HUDon);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Unequip);
    }

    void SetPlayerWeapon()
    {
        if (PlayerId == 8)
        {
            StackCount.SetActive(true);
            SwordGroup.SetActive(true);
        }
        if (PlayerId == 9)
        {
            WeaponGroup.SetActive(true);
            WeaponStackCount.SetActive(true);
        }

    }

    void TestMode()
    {
        AbilityManager.Instance.SkillIndex[PlayerId].SetActive(true);

        AbilityManager.Instance.GetComponentScripts();

        Player.gameObject.SetActive(true);
        HUD.SetActive(true);
        UIStage.text = "STAGE " + stageIndex;

        if (PlayerId == 8)
        {
            StackCount.SetActive(true);
            SwordGroup.SetActive(true);
        }
        if (PlayerId == 9)
        {
            WeaponGroup.SetActive(true);
            WeaponStackCount.SetActive(true);
        }
        Resume();
        UpdateHealthUI();
        AbilityManager.Instance.SetBtnSprite();
        AudioManager.instance.PlayBgm(AudioManager.Bgm.PracticeTheme);
    }


    void Update()
    {
        UIScore.text = (totalPoint + stagePoint).ToString();

        KillCountText.text = KillCount.ToString();
        UpdateHealthUI();

        UpdateShieldUI();


    }

    public void NextStage()
    {
        if (stageIndex < Stages.Length - 1)
        {
            Destroy(Stages[stageIndex]);

            stageIndex++;
            Stages[stageIndex].SetActive(true);
            NextStagePosition();
            if (Player.isGravity)
            {
                Player.isGravity = !Player.isGravity;
                Player.SetChangeGravity();
                Player.capsuleCollider2D.enabled = true;
            }
            set_Position = Vector3.zero;
            Loc_Mark.transform.position = set_Position;


            AudioManager.instance.PlaySfx(AudioManager.Sfx.Infomation);
            UIStage.text = "STAGE " + stageIndex;
        }
        else
        {
            //Pause();
            //  ClearScore.DOText("점수:" + UIScore.text, 1f, scrambleMode: ScrambleMode.Custom, scrambleChars: "00").SetEase(Ease.InFlash).SetUpdate(true);
            //ClearScore.text = "점수:" + UIScore.text;
            // KillScore.text = "처치 수:" + KillCountText.text;
            SaveMaxScore();
            GameClearButton.SetActive(true);
        }
        totalPoint += stagePoint;
        stagePoint = 0;
    }

    public void HealthDown()
    {
        if (AbilityManager.Instance != null && AbilityManager.Instance.isinvincibility == true)
        {
            return;
        }

        if (healthPoint > 0)
        {

            if (shieldPoint > 0)
            {
                shieldPoint--;
            }
            else
                healthPoint--;

            UpdateHealthUI();
            HealthUp();

            if (healthPoint == 0)
            {
                Player.OnDie();
                Player.isLive = false;
                //Text btnText = RestartButton.GetComponentInChildren<Text>();
                //RetryScore.text = "게임오버:" + UIScore.text;
                SaveMaxScore();
                GameClearButton.SetActive(true);
                AudioManager.instance.StopBgm();
                foreach (var image in UIHealthl)
                {
                    image.gameObject.SetActive(false);
                }
            }
        }
    }
    public void UpdateHealthUI()
    {
        // 캐릭?��?�� ?��?�� 체력?�� 맞게 ?��미�??�? ?��?��?��
        for (int i = 0; i < healthPoint; i++)
        {
            int index = Mathf.Min(i, UIHealthl.Count - 1);
            UIHealthl[index].gameObject.SetActive(true);
        }
        for (int i = 0; i < UIHealthl.Count; i++)
        {
            if (i < healthPoint)
            {
                UIHealthl[i].gameObject.SetActive(true);
            }
            else
            {
                UIHealthl[i].gameObject.SetActive(false);
            }
        }
        HealthUp();
    }

    void HealthUp()
    {
        if (PlayerId == 3)
        {
            switch (healthPoint)
            {
                case 5:
                    Player.PlayerAttack = 10f;
                    break;
                case 4:
                    Player.PlayerAttack = 8f;
                    break;
                case 3:
                    Player.PlayerAttack = 6f;
                    break;
                case 2:
                    Player.PlayerAttack = 4f;
                    break;
                case 1:
                    Player.PlayerAttack = 2f;
                    break;
            }
        }
        //Debug.Log("공격?��:" + Player.PlayerAttack);
    }

    public void UpdateShieldUI()
    {

        for (int i = 0; i < shieldPoint; i++)
        {
            int index = Mathf.Min(i, UIShield.Count - 1);
            UIShield[index].gameObject.SetActive(true);
        }
        for (int i = 0; i < UIShield.Count; i++)
        {
            if (i < shieldPoint)
            {
                UIShield[i].gameObject.SetActive(true);
            }
            else
            {
                UIShield[i].gameObject.SetActive(false);
            }
        }
    }

    void NextStagePosition()
    {
        Player.transform.position = new Vector3(0, 0, 0);
        Player.VelocityZero();
    }

    public void Restart()
    {
        Resume();
        SaveMaxScore();
        SceneManager.LoadScene("MainScenes");

    }

    public void Pause()
    {
        Time.timeScale = 0;
    }

    public void Resume()
    {
        Time.timeScale = 1;
    }

    public void GameQuit()
    {
        Application.Quit();
    }

    private void UpdateMaxScoreUI()
    {
        MaxScore.text = PlayerPrefs.GetInt("MaxScore").ToString();
    }

    private void SaveMaxScore()
    {
        //?��?��?��?�� 최�?? ?��?�� ????��
        int score = int.Parse(UIScore.text);
        int maxScore = Mathf.Max(score, PlayerPrefs.GetInt("MaxScore"));
        PlayerPrefs.SetInt("MaxScore", maxScore);

        UpdateMaxScoreUI();
    }


    public void SetPlayerLocation()
    {
        if (Player.isGravity)
        {
            isPlayerUseGravity = true;
        }
        else
            isPlayerUseGravity = false;


        set_Position = Player.transform.position;
        print(set_Position);
        if (Loc_Mark == null)
        {

            Loc_Mark = Instantiate(Repos_Mark, set_Position, Quaternion.identity);

        }
        else
        {

            Loc_Mark.SetActive(true);

            Loc_Mark.transform.position = set_Position;
        }


        AudioManager.instance.PlaySfx(AudioManager.Sfx.SaveMark);
    }

    public void LoadPlayerLocation()
    {
        if (Loc_Mark == null)
            return;

        CheckPlayerCurGravity();

        Player.transform.position = set_Position;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.LoadMark);
    }

    public void CheckPlayerCurGravity()
    {

        if (isPlayerUseGravity && !Player.isGravity)
        {
            Player.isGravity = true;
            Player.SetChangeGravity();
        }
        else if (!isPlayerUseGravity && Player.isGravity)
        {
            Player.isGravity = false;
            Player.SetChangeGravity();
        }
    }
}
