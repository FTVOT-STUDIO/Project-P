using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [Header("Sliders")]
    public Slider mainMenuBgmVolumeSlider; // 메인 메뉴에서의 BGM 볼륨 조절 슬라이더
    public Slider inGameBgmVolumeSlider;   // 게임 내에서의 BGM 볼륨 조절 슬라이더
    public Slider mainMenuSfxVolumeSlider; // 메인 메뉴에서의 SFX 볼륨 조절 슬라이더
    public Slider inGameSfxVolumeSlider;   // 게임 내에서의 SFX 볼륨 조절 슬라이더

    private AudioManager audioManager;

    void Start()
    {
        audioManager = AudioManager.instance;

        if (audioManager == null)
        {
            Debug.LogError("AudioManager instance is not found. Please ensure that AudioManager is properly initialized.");
            return;
        }

        float savedBgmVolume = PlayerPrefs.GetFloat("BgmVolume", 1f);
        float savedSfxVolume = PlayerPrefs.GetFloat("SfxVolume", 1f);

        // 슬라이더 초기화 및 값 동기화 설정
        if (mainMenuBgmVolumeSlider != null && inGameBgmVolumeSlider != null)
        {
            mainMenuBgmVolumeSlider.value = savedBgmVolume;
            inGameBgmVolumeSlider.value = savedBgmVolume;

            mainMenuBgmVolumeSlider.onValueChanged.AddListener(delegate { SyncBgmSliders(mainMenuBgmVolumeSlider); });
            inGameBgmVolumeSlider.onValueChanged.AddListener(delegate { SyncBgmSliders(inGameBgmVolumeSlider); });
        }

        if (mainMenuSfxVolumeSlider != null && inGameSfxVolumeSlider != null)
        {
            mainMenuSfxVolumeSlider.value = savedSfxVolume;
            inGameSfxVolumeSlider.value = savedSfxVolume;

            mainMenuSfxVolumeSlider.onValueChanged.AddListener(delegate { SyncSfxSliders(mainMenuSfxVolumeSlider); });
            inGameSfxVolumeSlider.onValueChanged.AddListener(delegate { SyncSfxSliders(inGameSfxVolumeSlider); });
        }

        // 초기화 시 볼륨 설정
        audioManager.SetBgmVolume(savedBgmVolume);
        audioManager.SetSfxVolume(savedSfxVolume);
    }

    // BGM 슬라이더 값이 변경되었을 때 다른 슬라이더와 동기화
    private void SyncBgmSliders(Slider changedSlider)
    {
        float newVolume = changedSlider.value;
        mainMenuBgmVolumeSlider.value = newVolume;
        inGameBgmVolumeSlider.value = newVolume;
        audioManager.SetBgmVolume(newVolume);
        PlayerPrefs.SetFloat("BgmVolume", newVolume);
        PlayerPrefs.Save();
    }

    // SFX 슬라이더 값이 변경되었을 때 다른 슬라이더와 동기화
    private void SyncSfxSliders(Slider changedSlider)
    {
        float newVolume = changedSlider.value;
        mainMenuSfxVolumeSlider.value = newVolume;
        inGameSfxVolumeSlider.value = newVolume;
        audioManager.SetSfxVolume(newVolume);
        PlayerPrefs.SetFloat("SfxVolume", newVolume);
        PlayerPrefs.Save();
    }
}
