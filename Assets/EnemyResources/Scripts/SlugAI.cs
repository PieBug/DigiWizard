using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class SlugAI : BaseAI
{
    public SlugEnemyAttributes attributes;
    public SlugEnemyAttributes baseAttributes;
    //public EnemyAttackSystem attackSphere;
    public State state;
    public int area;

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
                    Alert();
                }
                break;
            case State.fleeing:
                if (distanceToPlayer > attributes.sightRange)
                {
                    state = State.idling;
                    agent.isStopped = true;
                    attributes.distanceBias = Mathf.Lerp(attributes.dot1Bias, baseAttributes.distanceBias, 0.1f);
                    attributes.dot1Bias = Mathf.Lerp(attributes.dot1Bias, baseAttributes.dot1Bias, 0.1f);
                    StopCoroutine(walkRoutine);
                    StopCoroutine(birthRoutine);
                }
                //SetFleeDestination(20, 2.5f);
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
            if(state == State.fleeing)
            {
                //Debug.Log(name + "is bursting.");
                //state = State.bursting;
                //agent.speed = baseSpeed + attributes.burstSpeedIncrease;
                SetFleeDestination(20, 2.5f);
                //yield return new WaitUntil(() => agent.pathStatus == NavMeshPathStatus.PathComplete);
                state = State.fleeing;
                agent.speed = baseSpeed;
            }
        }
    }

    private IEnumerator BirthSpiders()
    {
        yield return new WaitForSeconds(Random.Range(0, 1f));
        while (true)
        {
            yield return new WaitForSeconds(attributes.defensiveBirthRate);
            if(state == State.fleeing)
            {
                //Debug.Log(name + "is birthing.");
                Instantiate(attributes.child, transform.position, Quaternion.LookRotation(normalBetweenPlayer,Vector3.up));
                animator.SetTrigger("birth");
                state = State.birthing;
                //agent.isStopped = true;
                yield return new WaitForSeconds(1f);
                //agent.isStopped = false;
                state = State.fleeing;
            }
        }
    }

    private void SetFleeDestination(int attempts, float step)
    {
        NavMeshPath path = new NavMeshPath();
        Vector3 target = new Vector3();
        float bestAverage = 0f;
        List<GameObject> retreatPoints = SlugRetreatPoint.GetRetreatPointsInArea(area);
        foreach (GameObject retreatPoint in retreatPoints)
        {
            float distance = Vector3.Distance(player.transform.position, retreatPoint.transform.position);
            float dot1 = Vector3.Dot(normalBetweenPlayer, Vector3.Normalize(player.transform.position - retreatPoint.transform.position));
            float dot2 = Vector3.Dot(Vector3.Cross(Vector3.up, normalBetweenPlayer), Vector3.Normalize(transform.position - retreatPoint.transform.position));
            dot2 = Mathf.Abs(dot2);
            float average = ((distance * attributes.distanceBias) + ((dot1 * distance) * attributes.dot1Bias + (dot2 * distance) * attributes.dot2Bias)) / 3f;
            if (average > bestAverage)
            {
                bestAverage = average;
                target = retreatPoint.transform.position;
            }
            //retreatPoint.GetComponentInChildren<TextMesh>().text = average.ToString();
        }
        agent.destination = target;
    }

    public override void IceAI(float linearIceFactor, float angularIceFactor)
    {
        base.IceAI(linearIceFactor, angularIceFactor);
        Color color = Color.Lerp(new Color(0f, 0.97f, 1f), Color.white, linearIceFactor);
        material.SetColor("_BaseColor", color);
        attributes.burstSpeedIncrease = baseAttributes.burstSpeedIncrease * linearIceFactor;
        attributes.rotSpeed = baseAttributes.burstSpeedIncrease * angularIceFactor;
        attributes.defensiveBirthRate = baseAttributes.defensiveBirthRate * linearIceFactor;
    }

    public override void Alert()
    {
        if(state == State.idling)
        {
            state = State.fleeing;
            audioSource3D.PlayOneShot(attributes.patheticRunawayClip);
            if (walkRoutine != null) StopCoroutine(walkRoutine);
            walkRoutine = StartCoroutine(FleeFromPlayer());
            if (birthRoutine != null) StopCoroutine(birthRoutine);
            if (!attributes.infertile)
                birthRoutine = StartCoroutine(BirthSpiders());
            agent.isStopped = false;
        }
    }
}
