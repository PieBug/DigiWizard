using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
 * [NOTE:]
 * When placing this script inside an enemy, please make sure the enemy has a collider component and isTrigger is checked to True.
 * */


    


public class EnemyAttackSystem : MonoBehaviour {
    // Variables //
    public float timeBetweenAttacks = 0.5f;
    public int attackAmt = 10;
    GameObject player;
    PlayerHealthAndDeathManager playerHealth;
    bool isPlayerRange;
    float timer;

    void Start()
    {
        player = PlayerController.singleton.gameObject;
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

    private void OnDisable()
    {
        isPlayerRange = false;
    }

    // Attack will happen once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if ((isPlayerRange == true) && (timer >= timeBetweenAttacks))
        {
            AttackPlayer(); // Call attack function to damage the player
        }
        if(playerHealth == null)
        {
            playerHealth = PlayerController.singleton.gameObject.GetComponent<PlayerHealthAndDeathManager>();
            isPlayerRange = false;
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
