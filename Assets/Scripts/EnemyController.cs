using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour {


    GameObject target;
    NavMeshAgent agent;


    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindGameObjectWithTag("Player");
        agent.SetDestination(target.transform.position);
    }

    void Start()
    {
        
       
    }


    void Update()
    {
        
    }
}

