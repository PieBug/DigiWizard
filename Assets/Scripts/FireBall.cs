using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private MagicSystem magicSystem;
    public GameObject fireExplosion;
    public int colCounter = 3;
    public string power;
    public float radius;
    public LayerMask enemyMask;

    private void Start()
    {
        magicSystem = PlayerController.singleton.GetComponent<MagicSystem>();
    }
    // Collision counter //
    void OnCollisionEnter(Collision col)
    {

        colCounter--;
        if (colCounter == 0)
        {
            Explode();
            return;
        }
        EnemyHealthAndDeathManager enemyHealth = col.gameObject.GetComponentInParent<EnemyHealthAndDeathManager>();
        if (enemyHealth != null)
        {
            Explode();
        }
    }

    private void Explode()
    {
        Instantiate(fireExplosion, transform.position, transform.rotation);
        Collider[] hitObjs = Physics.OverlapSphere(transform.position, radius, enemyMask);
        for (int i = 0; i < hitObjs.Length; i++)
        {
            float distanceFromRadius = Vector3.Distance(transform.position, hitObjs[i].transform.position);
            float modNum = Mathf.Max((radius - distanceFromRadius), 0f) / radius;
            EnemyHealthAndDeathManager enemyManager = hitObjs[i].GetComponentInParent<EnemyHealthAndDeathManager>();
            if (enemyManager != null)
            {
                magicSystem.ElementDamageManager(power, enemyManager, modNum);
            }
        }
        Destroy(gameObject);
    }
}

