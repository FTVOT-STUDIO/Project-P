using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomAudio : MonoBehaviour
{
    void Start()
    {
        PlayCustomAudio();
    }

    void OnEnable()
    {
        PlayCustomAudio();
    }

    // ?„?˜?˜ ?˜¤?””?˜¤ ?´ë¦½ì„ ????¥?•  ë³??ˆ˜
    public AudioManager.Sfx customSfx; // ?‚¬?š©?ê°? ì§?? •?•œ ?˜¤?””?˜¤

    public void PlayCustomAudio()
    {
        AudioManager.instance.PlaySfx(customSfx);
    }
}
