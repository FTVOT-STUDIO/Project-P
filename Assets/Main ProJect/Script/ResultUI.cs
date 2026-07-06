using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections;

[System.Serializable]
public class ResultCharData
{
    public string name;
    [TextArea]
    public string text;
    [TextArea]
    public string failText;

    public Sprite portrait;
}

public class ResultUI : MonoBehaviour
{
    public ResultCharData[] charDatas;

    public Sprite[] rankBadge;

    public Image rankImage;

    public Text charText;
    public Text killCountText;
    public Text jumpCountText;
    public Text skillCountText;
    public Text GrapplingCountText;
    public Text QuestCountText;

    public Text resultText;
    public Color clearTextColor;
    public Color unClearTextColor;

    public GameObject failEmoji;

    public Image portrait;


    void OnEnable()
    {
        int id = GameManager.Instance.PlayerId;
        this.portrait.sprite = charDatas[id].portrait;

        if (GameManager.Instance.Player.isLive)
            this.charText.text = charDatas[id].text.ToString();
        else
            this.charText.text = charDatas[id].failText.ToString();


        AudioManager.instance.PlaySfx(AudioManager.Sfx.Hologram0);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Hologram1);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.TextEffect);
        AudioManager.instance.PlaySfx(AudioManager.Sfx.punch);

        if (GameManager.Instance.Player.isLive)
            AudioManager.instance.PlaySfx(AudioManager.Sfx.LoadMark);
        else
            AudioManager.instance.PlaySfx(AudioManager.Sfx.die);

        SetResultAdjText();
        SetRankBadge();

        StartCoroutine(OutPutPauseAudio());

    }

    void SetResultAdjText()
    {
        resultText.text = GameManager.Instance.Player.isLive ? "탐사 대성공!" : "탐사 중단!";
        resultText.color = GameManager.Instance.Player.isLive ? clearTextColor : unClearTextColor;

        GameManager.Instance.ClearScore.DOText("점수:" + GameManager.Instance.UIScore.text, 1f, scrambleMode: ScrambleMode.Custom, scrambleChars: "00").SetEase(Ease.InFlash).SetUpdate(true);
        killCountText.DOText("처치 횟수:" + GameManager.Instance.KillCount, 1f, scrambleMode: ScrambleMode.Custom, scrambleChars: "00").SetEase(Ease.InFlash).SetUpdate(true).SetDelay(0.3f);
        jumpCountText.DOText("점프 횟수:" + GameManager.Instance.jumpCount, 1f, scrambleMode: ScrambleMode.Custom, scrambleChars: "00").SetEase(Ease.InFlash).SetUpdate(true).SetDelay(0.3f);
        skillCountText.DOText("스킬 사용 횟수:" + GameManager.Instance.skillCount, 1f, scrambleMode: ScrambleMode.Custom, scrambleChars: "00").SetEase(Ease.InFlash).SetUpdate(true).SetDelay(0.3f);
        GrapplingCountText.DOText("그래플링 횟수:" + GameManager.Instance.grapplingCount, 1f, scrambleMode: ScrambleMode.Custom, scrambleChars: "00").SetEase(Ease.InFlash).SetUpdate(true).SetDelay(0.3f);
        QuestCountText.DOText("완료한 퀘스트:" + GameManager.Instance.qeustClearCount, 1f, scrambleMode: ScrambleMode.Custom, scrambleChars: "00").SetEase(Ease.InFlash).SetUpdate(true).SetDelay(0.3f);

        if (!GameManager.Instance.Player.isLive)
        {
            portrait.color = new Color(0.5f, 0.5f, 0.5f, 1);
            failEmoji.SetActive(true);
        }
    }

    void SetRankBadge()
    {
        int score = GameManager.Instance.totalPoint + GameManager.Instance.stagePoint;

        if (score >= 50000 && GameManager.Instance.Player.isLive)
        {
            rankImage.sprite = rankBadge[0];
        }
        else if (score >= 40000)
        {
            rankImage.sprite = rankBadge[1];
        }
        else if (score >= 30000)
        {
            rankImage.sprite = rankBadge[2];
        }
        else if (score >= 20000)
        {
            rankImage.sprite = rankBadge[3];
        }
        else if (score >= 10000)
        {
            rankImage.sprite = rankBadge[4];
        }
        else if (score >= 0)
        {
            rankImage.sprite = rankBadge[5];
        }
    }


    IEnumerator OutPutPauseAudio()
    {
        GameManager.Instance.Pause();
        yield return new WaitForSecondsRealtime(0.2f);
        AudioManager.instance.GameStopEffect(true);
    }

}
