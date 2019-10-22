using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class PopUpAdAI : BaseAI
{
    public PopUpAdAttributes attributes;

    public State state;

    public enum State
    {
        idling,
        hiding,
        chasingAndShooting,
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
            yield return new WaitUntil(() => agent.pathStatus == NavMeshPathStatus.PathComplete);
            agent.destination = player.transform.position;
        }
    }

    private IEnumerator ShootAtPlayer()
    {
        while (true)
        {
            //Spawn projectile
            GameObject projectile = Instantiate(attributes.projectile);
            //Aim projectile at player
            projectile.transform.rotation = Quaternion.LookRotation(normalBetweenPlayer, Vector3.up);
            //Wait
            yield return new WaitForSeconds(attributes.fireRate);
        }
    }
}
