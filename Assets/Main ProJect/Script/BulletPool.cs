using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance;

    public GameObject bulletPrefab;
    public Transform bulletParent; // 총알의 부모 객체

    public int bulletAmount;
    public List<GameObject> bullets;

    void Awake()
    {
        Instance = this;
    }

    public void SpawnBullet()
    {
        bullets = new List<GameObject>();
        for (int i = 0; i < bulletAmount; i++) //bulletAmount의 값에 따라 생성되는 총알 관리
        {
            GameObject obj = Instantiate(bulletPrefab, bulletParent); 
            obj.SetActive(false);
            bullets.Add(obj);
        }
    }

    public GameObject GetBullet()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            if (!bullets[i].activeInHierarchy)
            {
                return bullets[i];
            }
        }
        return null;
    }
 }

