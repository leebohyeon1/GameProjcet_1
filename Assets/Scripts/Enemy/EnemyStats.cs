using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : MonoBehaviour
{
    public enum EnemyPersonality
    {
        Cautious,   // ���ɽ����� ����
        Aggressive, // �������� ����
        Cold        // ��ö�� ����
    }
    //=========================================================

    public EnemyType enemyType;

    [Header("�̵�")]
    public float Speed;
    public float boundarySpeed;
    public float rotationSpeed;

    [Header("����")]
    public float attackDistance;
    public float attackSpeed;

    [Header("���� ����")]
    public EnemyState enemyState;
    public EnemyPersonality enemyPersonality;
    //==========================================================


    void Start()
    {    
        enemyState = EnemyState.Chase;
        Speed = enemyType.Speed;
        SetEnemy();

      
    }

    void OnEnable()
    {
        enemyPersonality = GetRandomPersonality();
    }
    //==========================================================

    void SetEnemy()
    {
        boundarySpeed = enemyType.boundarySpeed;
        rotationSpeed = enemyType.rotationSpeed;
        attackDistance = enemyType.attackDistance;
        attackSpeed = enemyType.attackSpeed;
        enemyPersonality = enemyType.enemyPersonality;
    }
    EnemyPersonality GetRandomPersonality()
    {
        int personalityCount = System.Enum.GetValues(typeof(EnemyPersonality)).Length;
        int randomIndex = Random.Range(0, personalityCount);
        return (EnemyPersonality)randomIndex;
    }
}
