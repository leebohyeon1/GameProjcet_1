using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyStats;

[CreateAssetMenu(fileName = "EnemyType", menuName = "Enemy/enemyType")]
public class EnemyType : ScriptableObject
{
    [Header("이동")]
    public float Speed = 5f;
    public float boundarySpeed = 2f;
    public float rotationSpeed = 2f;

    [Header("공격")]
    public float attackDistance = 3f;
    public float attackSpeed = 2;

    [Header("현재 상태")]
    public EnemyState enemyState;
    public EnemyPersonality enemyPersonality;
}
