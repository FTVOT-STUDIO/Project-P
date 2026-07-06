using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BackGroundFollower : MonoBehaviour
{
    PlayerMove player;

    void Start()
    {
        player = GameManager.Instance.Player;
    }

    void LateUpdate()
    {
        transform.position = new Vector2(player.transform.position.x, player.transform.position.y);
    }
}
