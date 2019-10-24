using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPopUpEnemyAttributes", menuName = "Enemy Attributes/Pop Up Enemy Attributes")]
public class PopUpAdAttributes : ScriptableObject
{
    [Header("Behavior")]
    public bool docile;
    public bool pacifist;
    [Header("Senses")]
    [Range(-1f, 1f)]
    public float viewEngage;
    [Range(-1f, 1f)]
    public float viewFire;
    public float sightRange;
    public float revealRange;
    [Header("Attacks")]
    public float rotSpeed;
    public GameObject projectile;
    public float fireRate;
    public float normalStoppingDistance;
    public float playerInaccessibleStoppingDistance;

    [Header("Sounds")]
    public AudioClip appearClip;
    public AudioClip shootClip;
    public AudioClip deathClip;

    [Header("Drops")]
    public GameObject normalDrop;
}
