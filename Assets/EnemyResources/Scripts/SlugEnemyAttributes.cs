using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newSlugEnemyAttributes", menuName = "Enemy Attributes/Slug Enemy Attributes")]
public class SlugEnemyAttributes : ScriptableObject
{
    [Header("Behavior"), Tooltip("Don't spawn baby spiders")]
    public bool infertile;
    public bool brave;
    [Header("Senses")]
    [Range(-1f, 1f)]
    public float viewRunaway;
    public float sightRange;
    [Range(0f, 1f)]
    public float distanceBias = 1f;
    [Range(0f, 1f)]
    public float dot1Bias = 1f;
    [Range(0f, 1f)]
    public float dot2Bias = 1f;
    [Header("Attacks")]
    public GameObject child;
    public float defensiveBirthRate;

    public float rotSpeed;

    public float burstRate;
    public float burstRange;
    public float burstSpeedIncrease;
    [Header("Sounds")]
    public AudioClip patheticRunawayClip;
    public AudioClip deathClip;

    [Header("Drops")]
    public GameObject normalDrop;
}
