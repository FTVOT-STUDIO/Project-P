using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "EnemyData", order = int.MaxValue)]
public class EnemyData : ScriptableObject
{
    [Header("# Name")]
    public string EnemyName;
    [Header("# Animation")]
    public RuntimeAnimatorController anim;
    [Header("# Health")]
    public float Health;
    [Header("# Speed")]
    public float Speed;
}
