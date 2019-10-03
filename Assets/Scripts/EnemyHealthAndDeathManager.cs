using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthAndDeathManager : MonoBehaviour
{
    public int enemyHealth = 10; // Setting default enemy health

    public void DamageEnemy(int damageAmount) // public function that takes in damage amount from user
    {
        enemyHealth -= damageAmount;  // subtracting enemy health with damage amount

        if (enemyHealth <= 0)  // if enemy health is less or equal to 0, then detroy the enemy from game view
        {
            Kill();
        }
    }

    public virtual void Kill()
    {
        Destroy(gameObject);
        print("Enemy died");
    }
}
