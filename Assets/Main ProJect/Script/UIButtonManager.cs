using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonManager : MonoBehaviour
{

    GameObject player;
    PlayerMove PlayerMove;
    GrapplingHook GrapplingHook;
    Weapon Weapon;

    public void Init()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        PlayerMove = player.GetComponent<PlayerMove>();
        GrapplingHook = player.GetComponent<GrapplingHook>();
        Weapon = player.GetComponent<Weapon>();
    }

    public void LeftClick()
    {
        PlayerMove.inputLeft = true;
    }

    public void LeftUp()
    {
        PlayerMove.inputLeft = false;
    }

    public void RightClick()
    {
        PlayerMove.inputRight = true;
    }

    public void RightUp()
    {
        PlayerMove.inputRight = false;
    }

    public void JumpClick()
    {
        PlayerMove.inputJump = true;
    }

    public void JumpUp()
    {
        PlayerMove.inputJump = false;
    }

    public void HookClick()
    {
        GrapplingHook.HookClick = true;
    }

    public void WeaponClick()
    {
        Weapon.isfire = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && GameManager.Instance.hookBtn.activeSelf)
        {
            GrapplingHook.HookClick = true;
        }
    }
}
