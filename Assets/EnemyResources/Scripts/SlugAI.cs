using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class SlugAI : BaseAI
{
    public SlugEnemyAttributes attributes;

    //public Animator animator;
    public AudioSource audioSource2D;
    public AudioSource audioSource3D;
    //public EnemyAttackSystem attackSphere;
    public State state;

    public enum State
    {
        idling,
        patroling,
        fleeing,
        bursting,
        birthing,
        closeQuaters,
        dead
    }

    private Coroutine walkRoutine;
    private Coroutine birthRoutine;


    // Start is called before the first frame update
    new void Start()
    {
        base.Start();


    }

    // Update is called once per frame
    void Update()
    {
        //Get distance to player
        switch (state)
        {
            case State.idling:
            case State.patroling:
            case State.fleeing:
                UpdateDistanceToAndDotProductBetweenPlayer();

                break;
            default:
                break;
        }

        switch (state)
        {
            case State.idling:
                if (CanSeePlayer(attributes.sightRange, attributes.viewRunaway) && !attributes.brave)
                {
                    state = State.fleeing;
                    walkRoutine = StartCoroutine(FleeFromPlayer());
                    agent.isStopped = false;
                }
                break;
            case State.fleeing:
                if (distanceToPlayer > attributes.sightRange)
                {
                    state = State.idling;
                    agent.isStopped = true;
                    StopCoroutine(walkRoutine);
                }
                SetFleeDestination(20, 2.5f);
                break;
        }
    }

    private IEnumerator FleeFromPlayer()
    {

        agent.destination = transform.position + (-normalBetweenPlayer * attributes.burstRange);
        yield return new WaitForSeconds(Random.Range(0, 1f));
        while (true)
        {
            yield return new WaitForSeconds(attributes.burstRate);
            state = State.bursting;
            agent.speed = baseSpeed + attributes.burstSpeedIncrease;
            SetFleeDestination(20, 2.5f);
            yield return new WaitUntil(() => agent.pathStatus == NavMeshPathStatus.PathComplete);
            state = State.fleeing;
            agent.speed = baseSpeed;
        }
    }

    private void SetFleeDestination(int attempts, float step)
    {
        NavMeshPath path = new NavMeshPath();
        Vector3 target = new Vector3();
        int i = 0;
        bool pathExists;
        do
        {
            i++;
            target = transform.position + (-normalBetweenPlayer * i * step);
            pathExists = agent.CalculatePath(target, path);
        } while (i < attempts && !pathExists);
        if (pathExists)
            agent.destination = target;
        else
            agent.destination = player.transform.position;
    }
    
}
