using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AngelaBullet : MonoBehaviour
{
    public float bulletSpeed;

    public int StackCount;

    public int stackLv;

    public Text stackText;

    void Awake()
    {
        StackCount = 0;
    }

    void OnEnable()
    {
        transform.position = GameManager.Instance.Player.transform.position;
    }

    void Update()
    {
        transform.position = GameManager.Instance.Player.transform.position;
        transform.Rotate(Vector3.back * bulletSpeed * Time.deltaTime);

        StackCount = GameManager.Instance.currentStreak;
        if (StackCount >= 10)
        {
            StackCount = GameManager.Instance.currentStreak = 0;
            bulletSpeed *= 1.1f;
            stackLv++;

        }

        stackText.text = $"{StackCount}/10/.Lv{stackLv}";

    }

}
