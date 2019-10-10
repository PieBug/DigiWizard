using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "newSpiderEnemyAttributes", menuName = "Enemy Attributes/Spider Enemy Attributes")]
public class SpiderEnemyAttributes : ScriptableObject
{
    [Header("Behavior")]
    public bool docile;
    public bool pacifist;
    [Header("Senses")]
    [Range(-1f, 1f)]
    public float viewEngage;
    [Range(-1f, 1f)]
    public float viewPounce;
    public float sightRange;
    public float attackRange;
    [Header("Attacks")]
    public float rotSpeed;

    public float attackTime;
    public float recoupTime;
    public float lungeDistance;
    public float lungeSpeed;

    public float jitterRate;
    public float jitterRange;
    [Header("Sounds")]
    public AudioClip preparePounceClip;
    public AudioClip pounceClip;
    public AudioClip deathClip;

    [Header("Drops")]
    public GameObject normalDrop;
}
