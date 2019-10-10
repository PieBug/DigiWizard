using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class SlugAI : BaseAI
{
    public SlugEnemyAttributes attributes;
    public NavMeshAgent agent;
    //public Animator animator;
    public AudioSource audioSource2D;
    public AudioSource audioSource3D;
    //public EnemyAttackSystem attackSphere;
    private PlayerController player;
    public State state;

    public enum State
    {
        idling,
        patroling,
        fleeing,
        birthing,
        closeQuaters,
        dead
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
