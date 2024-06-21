using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class EnemyStats : MonoBehaviour
{
    [Header("ü��")]
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
        }
    }

    [Header("�̵�")]
    public float speed;

    [Header("����")]
    public float closeDistanceAttackRange;
    public float longDistanceAttackRange;
    public float attackCooldown;
    public float[] damage;
   
    [Header("���� ����")]
    public EnemyState enemyState;
    public bool isAttack;

    public Transform Point;
    //==========================================================

    void Start()
    {
        curHP = maxHP;
    }

    void OnEnable()
    {
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, new Vector3(closeDistanceAttackRange * 2, 0, closeDistanceAttackRange * 2) );
        Gizmos.color = Color.black;
        Gizmos.DrawWireCube(transform.position, new Vector3(longDistanceAttackRange * 2, 0, longDistanceAttackRange * 2));
    }
    //==========================================================


  
}
