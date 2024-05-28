using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : MonoBehaviour
{
    public enum EnemyPersonality
    {
        Cautious,   // 조심스러운 성격
        Aggressive, // 저돌적인 성격
        Cold        // 냉철한 성격
    }
    //=========================================================

    public EnemyType enemyType;

    [Header("이동")]
    public float Speed;
    public float boundarySpeed;
    public float rotationSpeed;

    [Header("공격")]
    public float attackDistance;
    public float attackSpeed;

    [Header("현재 상태")]
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
