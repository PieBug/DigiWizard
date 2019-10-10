using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAI : MonoBehaviour
{
    protected PlayerController player;

    //Which encounter does the enemy belong to.
    public int encounter;

    //Where is the player in relation to me
    protected float distanceToPlayer;
    protected float dotProductBetweenPlayer;

    //What damage states am I in
    public float frozen;


    protected void Start()
    {
        player = PlayerController.singleton;
    }

    protected void UpdateDistanceToAndDotProductBetweenPlayer()
    {
        Vector3 difference = PlayerController.singleton.transform.position - transform.position;
        distanceToPlayer = difference.magnitude;
        dotProductBetweenPlayer = Vector3.Dot(transform.forward, difference.normalized);
    }
}
