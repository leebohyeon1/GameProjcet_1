using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyStats;

[CreateAssetMenu(fileName = "EnemyType", menuName = "Enemy/enemyType")]
public class EnemyType : ScriptableObject
{
    [Header("�̵�")]
    public float Speed = 5f;
    public float boundarySpeed = 2f;
    public float rotationSpeed = 2f;

    [Header("����")]
    public float attackDistance = 3f;
    public float attackSpeed = 2;

    [Header("���� ����")]
    public EnemyState enemyState;
    public EnemyPersonality enemyPersonality;
}
