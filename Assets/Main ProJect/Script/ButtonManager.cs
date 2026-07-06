using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    [Header("Setting")]
    public GameObject Setting;
    public UISettingBar SettingBar;
    public GameObject TouchMiss;

    public Toggle URPToggle;
    public GameObject URP;

    private Button btn;

    bool isSetActive = false;
    bool isPause = false;

    void Awake()
    {
        btn = GetComponent<Button>();
    }

    public void InitData()
    {
        if (PlayerPrefs.HasKey("URPToggle"))
        {
            int urpState = PlayerPrefs.GetInt("URPToggle");
            URPToggle.isOn = urpState == 1;
        }
        else
        {
            URPToggle.isOn = false;
        }
    }

    public void IsSetActive()
    {
        isSetActive = !isSetActive;

        if (isSetActive)
        {
            Setting.SetActive(true);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Button);
        }
        else
        {
            SettingBar.anim.SetTrigger("DeActive");
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Button);
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("MainScenes");
        Time.timeScale = 1;
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Button);
    }

    public void Pause()
    {
        isPause = !isPause;

        if (isPause)
        {
            Time.timeScale = 0;
            btn.interactable = false;
            TouchMiss.SetActive(true);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Button);
        }
        else
        {
            Time.timeScale = 1;
            btn.interactable = true;
            TouchMiss.SetActive(false);
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Button);
        }
    }

    public void SetURP()
    {
        PlayerPrefs.SetInt("URPToggle", URPToggle.isOn ? 1 : 0);
        PlayerPrefs.Save();
        URP.SetActive(URPToggle.isOn);

    }
}
