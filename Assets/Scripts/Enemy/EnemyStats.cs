using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

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
    [Header("체력")]
    public float maxHP = 100f;
    [SerializeField]
    private float currentHP;
    public float curHP
    { 
        get { return currentHP; } 
        set 
        { 
            currentHP = value;          
            if (currentHP <= 0 )
            {
                currentHP = 0;
                enemyState = EnemyState.Die;
                FindObjectOfType<PlayerMovement>().UnlockTarget();
                EventManager.Instance.PostNotification(EVENT_TYPE.ENEMY_STATE,this,EnemyState.Die);
            }
            Debug.Log(currentHP);
        }
    }

    [Header("이동")]
    public float speed;
    public float boundarySpeed;
    public float rotationSpeed;

    [Header("공격")]
    public float attackDistance;
    public float attackTime;
    public float damage = 10;

    [Header("현재 상태")]
    public EnemyState enemyState;
    public EnemyPersonality enemyPersonality;
    public bool isAttack;

    public Transform Point;
    //==========================================================


    void Start()
    {    
        enemyState = EnemyState.Chase;
        speed = enemyType.Speed;
        curHP = maxHP;
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
        attackTime = enemyType.attackTime;
        enemyPersonality = enemyType.enemyPersonality;
    }
    EnemyPersonality GetRandomPersonality()
    {
        int personalityCount = System.Enum.GetValues(typeof(EnemyPersonality)).Length;
        int randomIndex = Random.Range(0, personalityCount);
        return (EnemyPersonality)randomIndex;
    }

    public void TakeDamage(float damage)
    {
        if(curHP > 0)
        {
            curHP -= damage;
        }
        
    }
}
