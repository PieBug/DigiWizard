using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class PopUpAdAI : BaseAI
{
    public GameObject mesh;
    public PopUpEnemyAttributes attributes;
    private PopUpEnemyAttributes baseAttributes;
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
        if (state == State.hiding)
        {
            animator.SetBool("hide", true);
        }
        attributes = Instantiate(attributes);
        baseAttributes = Instantiate(attributes);
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();
        //Get distance to player
        switch (state)
        {
            case State.idling:
            case State.hiding:
            case State.chasingAndShooting:
            case State.justShooting:
            case State.closeQuaters:
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
                    Alert();
                }
                break;
            case State.hiding:
                if(CanSeePlayer(attributes.revealRange, attributes.viewEngage) && !attributes.docile)
                {
                    Alert();
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
                if (distanceToPlayer < attributes.backupRange)
                {
                    state = State.closeQuaters;
                    agent.updateRotation = false;
                }
                break;
            case State.closeQuaters:
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(player.transform.position - transform.position, Vector3.up), attributes.rotSpeed * Time.deltaTime);
                if (distanceToPlayer > attributes.backupRange)
                {
                    state = State.chasingAndShooting;
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
            if(state == State.chasingAndShooting || state == State.justShooting)
            {
                agent.destination = player.transform.position;
                agent.updateRotation = true;
            }
            else if(state == State.closeQuaters)
            {
                agent.destination = transform.position + (-normalBetweenPlayer * agent.stoppingDistance * 2);
            }
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
            yield return new WaitForSeconds(attributes.stopTime);
            agent.isStopped = true;
            yield return new WaitForSeconds(attributes.stopTime);
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
            audioSource3D.PlayOneShot(attributes.shootClip);
            yield return new WaitUntil(() => !chargeParticles.IsAlive());
        }
    }

    public override void IceAI(float linearIceFactor, float angularIceFactor)
    {
        base.IceAI(linearIceFactor, angularIceFactor);
        Color color = Color.Lerp(new Color(0f, 0.97f, 1f), Color.white, linearIceFactor);
        material.SetColor("_BaseColor", color);
        //material.SetColor("_BaseColor", Color.blue);

        attributes.rotSpeed = baseAttributes.rotSpeed * angularIceFactor;
    }

    public override void Alert()
    {
        if(state != State.dead)
        {
            animator.SetBool("hide", false);
            state = State.chasingAndShooting;
            if (walkRoutine == null) walkRoutine = StartCoroutine(ChasePlayer());
            if (shootRoutine == null) shootRoutine = StartCoroutine(ShootAtPlayer());
        }
    }
}
