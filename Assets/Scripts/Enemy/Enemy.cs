using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private Rigidbody rb;
    private NavMeshAgent navMeshAgent;

    public Transform player;

    public float rotateSpeed = 20f;


    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //navMeshAgent.SetDestination(player.position);
        //if (Vector3.Distance(player.position, transform.position) < 7)
        //{
        //    navMeshAgent.ResetPath();
        //    transform.RotateAround(player.transform.position, new Vector3(0, 1, 0), rotateSpeed * Time.deltaTime);
        //}
    }


}
