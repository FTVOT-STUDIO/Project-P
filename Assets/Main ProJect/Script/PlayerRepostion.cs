using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRepostion : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            if (AbilityManager.Instance.isinvincibility == true || GameManager.Instance.healthPoint > 0)
                Repostion();
            GameManager.Instance.HealthDown();
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
    }

    void Repostion()
    {
        if (GameManager.Instance.set_Position == Vector3.zero && GameManager.Instance.isPlayerUseGravity)
        {
            GameManager.Instance.Player.isGravity = false;
            GameManager.Instance.Player.SetChangeGravity();
            GameManager.Instance.Player.transform.position = Vector3.zero;
            GameManager.Instance.Player.VelocityZero();
        }
        else
        {
            GameManager.Instance.CheckPlayerCurGravity();
            GameManager.Instance.Player.transform.position = GameManager.Instance.set_Position;
            GameManager.Instance.Player.VelocityZero();
        }


    }


}
