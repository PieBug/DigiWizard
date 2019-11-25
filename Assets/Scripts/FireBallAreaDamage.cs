using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallAreaDamage : MonoBehaviour
{
    public MagicSystem magicSystem;
    public GameObject fireExplosion;
    // Collision counter //
    void OnCollisionEnter(Collision col)
    {
        EnemyHealthAndDeathManager enemyHealth = col.gameObject.GetComponentInParent<EnemyHealthAndDeathManager>();
        if (enemyHealth != null)
        {
            print("sphere");
            magicSystem.ElementDamageManager("fire", enemyHealth);
            Instantiate(fireExplosion, transform.position, transform.rotation);
            Destroy(gameObject);

        }
    }
    /**
    private void OnTriggerEnter(Collider other)
    {
        print("box");
    }
    **/
}
