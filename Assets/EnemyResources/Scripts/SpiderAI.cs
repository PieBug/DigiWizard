using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpiderAI : BaseAI
{
    public SpiderEnemyAttributes attributes;
    public NavMeshAgent agent;
    public Animator animator;
    public AudioSource audioSource2D;
    public AudioSource audioSource3D;
    public EnemyAttackSystem attackSphere;

    public State state;

    public enum State
    {
        idling,
        patroling,
        chasing,
        searching,
        preparingPounce,
        pouncing,
        recouping,
        closeQuaters,
        dead
    }

    private Vector3 lastRecalcLocation;
    private Vector3 playersLastKnownLocation;

    private float distanceCovered;

    private Coroutine walkRoutine;
    private Coroutine pounceRoutine;

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
            case State.chasing:
                UpdateDistanceToAndDotProductBetweenPlayer();
                break;
            default:
                break;
        }

        switch (state)
        {
            case State.idling:
                if (CanSeePlayer(attributes.sightRange, attributes.viewEngage) && !attributes.docile)
                {
                    state = State.chasing;
                    walkRoutine = StartCoroutine(JitterWalkToPlayer());
                    agent.isStopped = false;
                }
                break;
            case State.chasing:
                //bool pathRecalulated = false;
                if (distanceToPlayer > attributes.sightRange || attributes.docile)
                {
                    state = State.idling;
                    agent.isStopped = true;
                    StopCoroutine(walkRoutine);
                }
                if (distanceToPlayer < attributes.attackRange)
                {
                    if (attributes.pacifist && agent.isStopped == false)
                    {
                        agent.isStopped = true;
                        StopCoroutine(walkRoutine);
                    }
                    else if (FacingPlayer(attributes.viewPounce))
                    {
                        pounceRoutine = StartCoroutine(Pounce());
                    }
                }
                else
                {
                    if (attributes.pacifist && agent.isStopped == true)
                    {
                        agent.isStopped = false;
                        walkRoutine = StartCoroutine(JitterWalkToPlayer());
                    }
                //    if (CanSeePlayer() && Vector3.Distance(player.transform.position, playersLastKnownLocation) > 1f)
                //    {
                //        pathRecalulated = true;
                //        playersLastKnownLocation = player.transform.position;
                //        agent.destination = playersLastKnownLocation + Random.insideUnitSphere;
                        
                //    }
                }
                //if(agent.pathStatus == NavMeshPathStatus.PathComplete && pathRecalulated)
                //{
                //    Debug.Log("Going Idle");
                //    state = State.idling;
                //}
                break;
            case State.preparingPounce:
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up), attributes.rotSpeed * Time.deltaTime);
                break;
            case State.pouncing:
                float delta = attributes.lungeSpeed * Time.deltaTime;
                agent.Move(transform.forward * delta);
                distanceCovered += delta;
                break;
            case State.dead:
                break;
        }
    }

    private IEnumerator Pounce()
    {
        //Debug.Log(name + " is resting on his haunches.");
        audioSource3D.PlayOneShot(attributes.preparePounceClip);
        agent.isStopped = true;
        state = State.preparingPounce;
        animator.SetTrigger("pounce");
        yield return new WaitForSeconds(attributes.attackTime);
        //Debug.Log(name + " pounces ferociously OwO.");
        audioSource2D.PlayOneShot(attributes.pounceClip);
        distanceCovered = 0f;
        state = State.pouncing;
        yield return new WaitUntil(() => distanceCovered > attributes.lungeDistance);
        //Debug.Log(name + " is recouping from that phat pounce.");
        state = State.recouping;
        yield return new WaitForSeconds(attributes.recoupTime);
        //Debug.Log(name + " is back on the prowl.");
        agent.isStopped = false;
        state = State.chasing;
    }

    private IEnumerator JitterWalkToPlayer()
    {
        agent.destination = player.transform.position;
        yield return new WaitForSeconds(Random.Range(0, 1f));
        while (true)
        {
            yield return new WaitForSeconds(attributes.jitterRate);
            bool right = Random.Range(0f, 1f) < 0.5f;
            Vector3 direction = right ? transform.right : -transform.right;
            agent.destination = transform.position + direction * attributes.jitterRange;
            yield return new WaitUntil(() => agent.pathStatus == NavMeshPathStatus.PathComplete);
            agent.destination = player.transform.position;
        }
    }

    
}
