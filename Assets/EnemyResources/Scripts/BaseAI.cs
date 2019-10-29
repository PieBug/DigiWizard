﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public abstract class BaseAI : MonoBehaviour
{
    //public float slowDownAI = 1;

    protected PlayerController player;
    public NavMeshAgent agent;

    //Which encounter does the enemy belong to.
    public int encounter;

    //Where is the player in relation to me
    protected float distanceToPlayer;
    protected float dotProductBetweenPlayer;

    //What is my base speed
    protected float baseSpeed;
    protected Vector3 normalBetweenPlayer;
    protected float baseAngularSpeed;

    //Sound and animation
    public Animator animator;
    public AudioSource audioSource2D;
    public AudioSource audioSource3D;

    //What damage states am I in
    [HideInInspector]
    //public float slowDownAI = 1;


    protected void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = PlayerController.singleton;
        baseSpeed = agent.speed;
        baseAngularSpeed = agent.angularSpeed;
    }

    protected void UpdateDistanceToAndDotProductBetweenPlayer()
    {
        Vector3 difference = PlayerController.singleton.transform.position - transform.position;
        normalBetweenPlayer = difference.normalized;
        distanceToPlayer = difference.magnitude;
        dotProductBetweenPlayer = Vector3.Dot(transform.forward, difference.normalized);
    }

    protected bool CanSeePlayer(float sightRange, float view)
    {
        if (distanceToPlayer < sightRange && FacingPlayer(view))
            return !Physics.Linecast(transform.position, player.transform.position, LayerMask.GetMask("Default"));
        else
            return false;
    }

    protected bool FacingPlayer(float view, bool opposite = false)
    {
        return opposite == dotProductBetweenPlayer < view;
    }

    public virtual void IceAI(float slowSpeed, float slowAngularSpeed)
    {
        agent.speed = slowSpeed;
        agent.angularSpeed = slowAngularSpeed;
        print(agent.speed);
        print(agent.angularSpeed);
    }

    public virtual void ResetAI()
    {
        agent.speed = baseSpeed;
        agent.angularSpeed = baseAngularSpeed;
    }

    public abstract void Alert(); //Alert is not defined in BaseAI because it has no sensible implementation
}
