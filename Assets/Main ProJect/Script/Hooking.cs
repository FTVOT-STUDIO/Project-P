using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hooking : MonoBehaviour
{
    GrapplingHook grappling;
    public DistanceJoint2D joint2D;

    void Start()
    {
        grappling = GameManager.Instance.Player.GetComponent<GrapplingHook>();
        joint2D = GetComponent<DistanceJoint2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Hook"))
        {
            joint2D.enabled = true;
            grappling.isAttach = true;
            AudioManager.instance.PlaySfx(AudioManager.Sfx.isAttach);
        }
    }
}
