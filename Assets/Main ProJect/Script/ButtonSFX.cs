using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    public void StartSfx()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.StartSfx);
    }

    public void ButtonSfx()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Button);
    }

    public void MenuSfx()
    {
        AudioManager.instance.PlaySfx(AudioManager.Sfx.MenuSfx);
    }
}
