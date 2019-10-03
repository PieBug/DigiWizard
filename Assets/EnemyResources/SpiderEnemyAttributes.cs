using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( fileName = "newSpiderEnemyAttributes", menuName = "SpiderEnemyAttributes")]
public class SpiderEnemyAttributes : ScriptableObject
{
    public bool docile;
    public bool pacifist;

    [Range(-1f, 1f)]
    public float viewEngage;
    [Range(-1f, 1f)]
    public float viewPounce;
    public float sightRange;
    public float attackRange;

    public float rotSpeed;

    public float attackTime;
    public float recoupTime;
    public float lungeDistance;
    public float lungeSpeed;

    public float jitterRate;
    public float jitterRange;
}
