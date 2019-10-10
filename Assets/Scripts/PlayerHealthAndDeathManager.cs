using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthAndDeathManager : MonoBehaviour{
    // Variables //
    public int startingHealth;
    public int currentHealth;
    public Slider healthBar;
    PlayerController plyrController;
    //CharacterCamera plyrCam;
    bool isDead;
    bool isDamaged;
   

    // Awake gets called right a the beginning
    void Awake()
    {
        plyrController = GetComponent<PlayerController>();
        //plyrCam = GetComponent<CharacterCamera>();
        currentHealth = startingHealth;
    }

    // Public so that damagePlayer function can be called anywhere.
    public void DamagePlayer(int damageAmt)
    {
        isDamaged = true; 
        currentHealth -= damageAmt;
        healthBar.value = currentHealth; // Health slider in "HUD" will decrease as current health does.

        if (currentHealth <= 0 && !isDead) // If player health is zero and the player isn't already dead, call the Die() function
        {
            KillPlayer();
        }
    }

    public void HealPlayer(int healAmt)
    {
        isDamaged = false;
        currentHealth += healAmt;
        healthBar.value = currentHealth;

        if (currentHealth >= 100)
        {
            currentHealth = 100;
            healthBar.value = currentHealth;
        }

    }


    void KillPlayer()
    {
        isDead = true; 
        plyrController.enabled = false; // Stops the player from moving in the scene.
        //plyrCam.enabled = false;
        FindObjectOfType<GameManagerScript>().EndGame();
    }





    // Start is called before the first frame update
    void Start()
    {
        
    } // end start

    // Update is called once per frame
    void Update()
    {
        
    } // end update
} // end PlayerHealthAndDeathManager
