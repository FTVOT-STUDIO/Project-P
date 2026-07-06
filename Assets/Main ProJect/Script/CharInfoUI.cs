using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[System.Serializable]
public class CharInfos
{
    public string charName;
    [TextArea] public string skillName;
    [TextArea] public string charInfo;
    [TextArea] public string SkillInfo;
    public Sprite portrait;
    public Sprite skillIcon;
    public Sprite sdIllrust;
    public VideoClip videoClips;
    [HideInInspector] public float damage;
    [HideInInspector] public int health;
    [HideInInspector] public float speed;
}

public class CharInfoUI : MonoBehaviour
{
    public CharInfos[] charData;

    public Text Name;
    public Text SkillName;
    public Text Info;
    public Text SkillInfo;

    public Text Damage;
    public Text Health;
    public Text Speed;

    public Image Portrait;
    public Image SkillIcon;
    public Image SDIllrust;
    public VideoPlayer Video;

    void OnEnable()
    {
        UpdateStateUI();
        AudioManager.instance.PlaySfx(AudioManager.Sfx.StartSfx);
    }

    public void UpdateStateUI()
    {
        Name.text = charData[GameManager.Instance.PlayerId].charName;
        SkillName.text = charData[GameManager.Instance.PlayerId].skillName;
        Info.text = charData[GameManager.Instance.PlayerId].charInfo;
        SkillInfo.text = charData[GameManager.Instance.PlayerId].SkillInfo;

        charData[GameManager.Instance.PlayerId].damage = Charskil.Damaged;
        charData[GameManager.Instance.PlayerId].health = Charskil.HealthPoint;
        charData[GameManager.Instance.PlayerId].speed = Charskil.Speed;

        Damage.text = charData[GameManager.Instance.PlayerId].damage.ToString();
        Health.text = charData[GameManager.Instance.PlayerId].health.ToString();
        Speed.text = charData[GameManager.Instance.PlayerId].speed.ToString();

        Portrait.sprite = charData[GameManager.Instance.PlayerId].portrait;
        SkillIcon.sprite = charData[GameManager.Instance.PlayerId].skillIcon;
        SDIllrust.sprite = charData[GameManager.Instance.PlayerId].sdIllrust;

        Video.clip = charData[GameManager.Instance.PlayerId].videoClips;
    }
}


