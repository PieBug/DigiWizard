using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public MagicSystem magicSystem;
    public GameObject fireExplosion;
    public int colCounter = 3;
    public string power;
    // Collision counter //
    void OnCollisionEnter(Collision col)
    {
        colCounter--;
        
        if (colCounter == 0)
        {
            Instantiate(fireExplosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        EnemyHealthAndDeathManager enemyHealth = col.gameObject.GetComponentInParent<EnemyHealthAndDeathManager>();
        if (enemyHealth != null)
        {
            print("hit enemy");
            magicSystem.ElementDamageManager(power, enemyHealth);
            
        }
    }
}

