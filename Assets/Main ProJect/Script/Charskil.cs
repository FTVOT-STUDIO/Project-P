using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charskil : MonoBehaviour
{
    public static float Speed
    {
        get
        {
            switch (GameManager.Instance.PlayerId)
            {
                case 0:
                    return 320f;
                case 1:
                    return 500f;
                case 2:
                    return 340f;
                case 3:
                    return 320f;
                case 4:
                    return 350f;
                case 5:
                    return 444;
                case 6:
                    return 400f;
                case 7:
                    return 350f;
                case 8:
                    return 400;
                case 9:
                    return 400;
                default:
                    return GameManager.Instance.Player.maxSpeed;
            }
        }
    }
    public static int HealthPoint
    {
        get
        {
            switch (GameManager.Instance.PlayerId)
            {
                case 0:
                    return 3;
                case 1:
                    return 2;
                case 2:
                    return 1;
                case 3:
                    return 5;
                case 4:
                    return 9999;
                case 5:
                    return 4;
                case 6:
                    return 3;
                case 7:
                    return 3;
                case 8:
                    return 2;
                case 9:
                    return 3;
                default:
                    return GameManager.Instance.healthPoint;
            }
        }
    }

    public static float Damaged
    {
        get
        {
            switch (GameManager.Instance.PlayerId)
            {
                case 0:
                    return 3;
                case 1:
                    return 4;
                case 2:
                    return 10;
                case 3:
                    if (HealthPoint >= 5)
                    {
                        return 10;
                    }
                    else if (HealthPoint >= 3)
                    {
                        return 8;
                    }
                    else if (HealthPoint >= 1)
                    {
                        return 6;
                    }
                    break;
                case 4:
                    return 0.1f; // Í≥µÍ≤©?†•?ù¥ 0.1?ù∏ Í≤ΩÏö∞Î•? 0?úºÎ°? ?Ñ§?†ï
                case 5:
                    return 3;
                case 6:
                    return 3;
                case 7:
                    return 5;
                case 8:
                    return 2;
                case 9:
                    return 1;
                default:
                    return GameManager.Instance.Player.PlayerAttack;
            }
            return 0;
        }
    }
}
