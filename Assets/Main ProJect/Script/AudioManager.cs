using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("#BGM ====")]
    public AudioClip[] bgmClips;
    public float bgmVolume;
    AudioSource bgmPlayer;
    int bgmIndex;

    [Header("#SFX ====")]
    public AudioClip[] sfxClip;
    public float sfxVolume;
    public int channels;
    AudioSource[] sfxPlayer;
    int channelIndex;

    [Header("#Audio Effect")]
    AudioLowPassFilter audioDivingEffect;
    AudioReverbFilter audioCaveFilter;
    AudioHighPassFilter audioHighPassFilter;

    public enum Bgm { MainTheme0, PracticeTheme = 3, TheVoid }

    public enum Sfx
    {
        Jump, Hit, Attack, Item, Portal, Jumping, Heal,
        Button, Titleaction, Hook, HookBack, HookAttach,
        Wire, isAttach, Infomation, Warning, ASTLSkill,
        Skill, GasSound, Lighting, Freeze, DemonSfx,
        VoidSFX, Zap, Laugh, heartbeat, Bird, Ghost, Reboot,
        Camera, Bullet, BulletDestroy, flaunch, Explode, Explode2, StartSfx, TextSfx, Llaunch, evasion, Piyoong,
        Crush, GetShiled, SaveMark, LoadMark, ShiledHit, Dead, Whistle, Bell, Siren, Hi, ByeBye, oh, max, NoaAttack
        , soul, fireBall, speedUp, select, KetherA, KetherB, Hokma, Binah, Chesed, Gebura, TipheretA, TipheretB, Netzach
        , HodA, HodB, yesodA, yesodB, Malkuth, MenuSfx, HodC, selecOff, heart, miss, Drain, TreasureOpen, Reward, ExpoldeBox,
        FireGun, SwordH, SwordV, SwordHit, SwordThrow, BeeBee, Beeeeeee, BellSound, Electricshock, angleaActive, peeee, Trans,
        BreakBox, metalBreak, StoneBreak0, StoneBreak1, StoneBreak2, InWater, OutWater, Hologram0, Hologram1, TextEffect, KO,
        punch, AirPlain, HUDon, Unequip, Gravity, Falling, die
    }

    void Awake()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        // 배경?�� 초기?��
        GameObject bgmObject = new GameObject("BgmPlayer");
        bgmObject.transform.parent = transform;
        bgmPlayer = bgmObject.AddComponent<AudioSource>();
        bgmPlayer.playOnAwake = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;

        // ?��과음 초기?��
        GameObject sfxObject = new GameObject("SfxPlayer");
        sfxObject.transform.parent = transform;
        sfxPlayer = new AudioSource[channels];

        audioDivingEffect = Camera.main.GetComponent<AudioLowPassFilter>();
        audioCaveFilter = Camera.main.GetComponent<AudioReverbFilter>();
        audioHighPassFilter = Camera.main.GetComponent<AudioHighPassFilter>();

        for (int index = 0; index < sfxPlayer.Length; index++)
        {
            sfxPlayer[index] = sfxObject.AddComponent<AudioSource>();
            sfxPlayer[index].playOnAwake = false;
            sfxPlayer[index].volume = sfxVolume;
        }
    }

    public void PlayBgm(Bgm bgm)
    {
        int index = (int)bgm;
        if (index < 0 || index >= bgmClips.Length) return;

        int randomIndex = 0;
        if (bgm == Bgm.MainTheme0)
        {
            randomIndex = Random.Range(0, 3);
        }

        bgmIndex = index;
        bgmPlayer.clip = bgmClips[bgmIndex + randomIndex];
        bgmPlayer.Play();
    }

    public void PlaySfx(Sfx sfx)
    {
        for (int index = 0; index < sfxPlayer.Length; index++)
        {
            int loopIndex = (index + channelIndex) % sfxPlayer.Length;

            if (sfxPlayer[loopIndex].isPlaying)
                continue;

            channelIndex = loopIndex;
            sfxPlayer[loopIndex].clip = sfxClip[(int)sfx];
            sfxPlayer[loopIndex].Play();
            break;
        }
    }
    public void StopBgm()
    {
        bgmPlayer.Stop();
        //bgmPlayer.clip = null;
    }

    public void StopSfx(Sfx sfx)
    {
        foreach (AudioSource sfxSource in sfxPlayer)
        {
            if (sfxSource.isPlaying && sfxSource.clip == sfxClip[(int)sfx])
            {

                sfxSource.Stop();
                //sfxSource.clip = null;
                break;
            }
        }
    }

    public void AudioDivingEffect(bool isPlay)
    {
        if (audioDivingEffect == null)
            return;
        audioDivingEffect.enabled = isPlay;
    }

    public void AudioCaveEffect(bool isPlay)
    {
        if (audioCaveFilter == null)
            return;
        audioCaveFilter.enabled = isPlay;
    }

    public void GameStopEffect(bool isPlay)
    {
        if (audioHighPassFilter == null)
            return;

        audioHighPassFilter.enabled = isPlay;
    }

    // Set individual volume for BGM
    public void SetBgmVolume(float volume)
    {
        bgmVolume = Mathf.Clamp(volume, 0f, 1f);
        bgmPlayer.volume = bgmVolume;
    }

    // Set individual volume for SFX
    public void SetSfxVolume(float volume)
    {
        sfxVolume = Mathf.Clamp(volume, 0f, 1f);
        foreach (AudioSource sfxSource in sfxPlayer)
        {
            if (sfxSource != null)
            {
                sfxSource.volume = sfxVolume;
            }
        }
    }

    public float GetBgmVolume()
    {
        return bgmPlayer.volume;
    }

    public float GetSfxVolume()
    {
        return sfxVolume;
    }

    void OnDestroy()
    {
        bgmPlayer = null;
        audioDivingEffect = null;
        audioCaveFilter = null;

        for (int i = 0; i < sfxPlayer.Length; i++)
        {
            sfxPlayer[i] = null;
        }
    }

}
