using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderAI : MonoBehaviour
{
    public NavMeshAgent agent;
    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = PlayerController.singleton;
    }

    // Update is called once per frame
    void Update()
    {
        
        agent.destination = player.transform.position;
    }
}
