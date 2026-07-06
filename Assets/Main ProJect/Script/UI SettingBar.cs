using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISettingBar : MonoBehaviour
{
    public Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Active()
    {
        gameObject.SetActive(true);
    }

    public void DeActive()
    {
        gameObject.SetActive(false);
    }
}
