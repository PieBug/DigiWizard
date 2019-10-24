using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class PopUpAdAI : BaseAI
{
    public PopUpAdAttributes attributes;
    public GameObject projectileLauncher;
    public ParticleSystem chargeParticles;

    public State state;

    public enum State
    {
        idling,
        hiding,
        chasingAndShooting,
        justShooting,
        closeQuaters,
        dead
    }

    private Coroutine walkRoutine;
    private Coroutine shootRoutine;

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
            case State.hiding:
            case State.chasingAndShooting:
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
                    state = State.chasingAndShooting;
                    walkRoutine = StartCoroutine(ChasePlayer());
                    shootRoutine = StartCoroutine(ShootAtPlayer());
                }
                break;
            case State.hiding:
                if(CanSeePlayer(attributes.revealRange, attributes.viewEngage) && !attributes.docile)
                {
                    state = State.chasingAndShooting;
                    //Take a quick shot at the player
                    walkRoutine = StartCoroutine(ChasePlayer());
                }
                break;
            case State.chasingAndShooting:
                if (distanceToPlayer > attributes.sightRange || attributes.docile)
                {
                    state = State.idling;
                    agent.isStopped = true;
                    StopCoroutine(walkRoutine);
                    StopCoroutine(shootRoutine);
                }
                break;
        }
    }

    private IEnumerator ChasePlayer()
    {
        agent.destination = player.transform.position;
        yield return new WaitForSeconds(Random.Range(0, 1f));
        while (true)
        {
            agent.destination = player.transform.position;
            if(agent.pathStatus == NavMeshPathStatus.PathPartial)
            {
                Debug.Log("Cannot reach player. Stop when I'm close enough.");
                agent.stoppingDistance = attributes.playerInaccessibleStoppingDistance;
                state = State.justShooting;
                
            }
            else
            {
                agent.stoppingDistance = attributes.normalStoppingDistance;
            }
            yield return new WaitForSeconds(1f);
            agent.isStopped = true;
            yield return new WaitForSeconds(1f);
            agent.isStopped = false;
        }
    }

    private IEnumerator ShootAtPlayer()
    {
        yield return new WaitForSeconds(Random.value);
        while (true)
        {
            //Spawn projectile
            Projectile projectile = Instantiate(attributes.projectile).GetComponent<Projectile>();
            //Aim projectile at player
            projectile.transform.position = projectileLauncher.transform.position;
            projectileLauncher.transform.LookAt(player.transform);
            projectile.Fire(projectileLauncher.transform.rotation);
            //Wait
            yield return new WaitForSeconds(attributes.fireRate);
            chargeParticles.Play();
            yield return new WaitUntil(() => !chargeParticles.IsAlive());
        }
    }
}
