using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newSlugEnemyAttributes", menuName = "Enemy Attributes/Slug Enemy Attributes")]
public class SlugEnemyAttributes : ScriptableObject
{
    [Header("Behavior"), Tooltip("Don't spawn baby spiders")]
    public bool infertile;
    [Header("Senses")]
    [Range(-1f, 1f)]
    public float viewRunaway;
    [Range(-1f, 1f)]
    public float sightRange;
    [Header("Attacks")]
    public float defensiveBirthRange;

    public float rotSpeed;

    public float burstRate;
    public float burstRange;
    [Header("Sounds")]
    public AudioClip patheticRunawayClip;
    public AudioClip deathClip;

    [Header("Drops")]
    public GameObject normalDrop;
}
