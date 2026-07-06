using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelAnime : MonoBehaviour
{
    Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void SetAnime()
    {
        anim.SetTrigger("Change");
    }

    public void OnScaleAnime()
    {
        anim.SetTrigger("OnChange");
    }

    public void OffScaleAnime()
    {
        anim.SetTrigger("OffChange");
    }

    public void OffObject()
    {
        gameObject.SetActive(false);
    }
}
