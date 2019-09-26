using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackSystem : MonoBehaviour {
    // Variables //
    public float timeBetweenAttacks = 0.5f;
    public int attackAmt = 10;
    GameObject player;
    PlayerHealthAndDeathManager playerHealth;
    bool isPlayerRange;
    float timer;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = player.GetComponent<PlayerHealthAndDeathManager>(); 
    }

    // When player enters the box collision
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerRange = true;
        }
    }

    // When player exits the box collision
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            isPlayerRange = false;
        }
    }

    // Attack will happen once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if ((isPlayerRange == true) && (timer >= timeBetweenAttacks))
        {
            AttackPlayer(); // Call attack function to damage the player
        }
    }

    void AttackPlayer()
    {
        timer = 0f; // Resetting timer so Enemy may damage player again
        if (playerHealth.currentHealth > 0)
        {
            playerHealth.DamagePlayer(attackAmt); // calling DamagePlayer function in "PlayerHealthAndDeathManager" and inputting enemy attackAmount.
        }

    }
}
