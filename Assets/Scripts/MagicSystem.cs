using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSystem : MonoBehaviour {
    // Start is called before the first frame update
    private GameObject LeftWand;
    private GameObject RightWand;
    //float buttonPressedTime = 10.0f;

    // Shooting Variables // 
    public int wandDamage = 1; // Amount of damage
    public float fireRate = .25f; // How often player can fire weapon
    public float shootRange = 50f; // How long rays are shot
    //public float hitForce = 100f; // How much force hitting game objects with a rigidbody
    public Transform LwandEnd; // Marks the tip of the wand where ray will shoot from
    public Transform RwandEnd; // Marks the tip of the wand where ray will shoot from
    private Camera fpsCam;
    private WaitForSeconds rayDuration = new WaitForSeconds(.07f); // How long ray will remain in game view
    //private AudioSource bulletSound;  // Only use this once we get audio sounds
    private LineRenderer laserLine; // takes array of two points and draws a line between each one in the game view
    private float nextFire; // Holds time when player can fire again after firing

    void Start()
    {
        LeftWand = GameObject.Find("LeftWand");
        RightWand = GameObject.Find("RightWand");

        laserLine = GetComponent<LineRenderer>();
        fpsCam = GetComponentInChildren<Camera>();

    } // end Start

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButtonDown("Fire1") && !Input.GetButtonDown("Fire2") && Time.time > nextFire) 
        {
            nextFire = Time.time + fireRate; // making sure player does not constantly fire
            StartCoroutine(ShotEffect()); // Calling the coroutine ShotEffect function to enable laser line

            Vector3 camShootingPoint = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0)); // Aiming point of the ray -> will be set to the middle position of the fps camera. Takes position of the camera and converts it to world space. 

            RaycastHit hitObject; // Object that is hit with our ray; object must have a collider on
            laserLine.SetPosition(0, LwandEnd.position); // laser starting position is set to current position of the tip of the wand where the ray will shoot from

            if (Physics.Raycast(camShootingPoint, fpsCam.transform.forward, out hitObject, shootRange)) // Raycast is used to determine where the end of the ray will be, and deals force/damage to the object hit. Physics Raycast returns a bool. [camShootingPoin:] point in the world space where ray will begin [fpsCam:] Direction of the ray [Out - keyword:] Allows us to store information from a function + it's return type of the object hit. ex: Information like Rigidbody, collider, & surfacenormal of object hit. [shootRange:] How far ray goes.
            {
                laserLine.SetPosition(1, hitObject.point); // if raycast returns true and an object is hit, we're setting the 2nd position of the laser line to that object point

                EnemyHealthAndDamageManager enemyHealth = hitObject.collider.GetComponent<EnemyHealthAndDamageManager>(); // getting script from the object hit
                if (enemyHealth != null) // checking to make sure the hit object is an enemy type with script "EnemyHealthAndDamageManager" attached
                {
                    enemyHealth.damageEnemy(wandDamage); // if "EnemyHealthAndDamageManager" exists, then call the damage function and pass in wand damage
                }
            }
            else // Raycast returns false
            {
                laserLine.SetPosition(1, (camShootingPoint + (fpsCam.transform.forward * shootRange))); // if nothing is hit, then the ray will just shoot 50 units away from the camera
            }
        }

        if (Input.GetButtonDown("Fire2") && !Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate; 
            StartCoroutine(ShotEffect());

            Vector3 camShootingPoint = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0)); 

            RaycastHit hitObject; 
            laserLine.SetPosition(0, RwandEnd.position); 

            if (Physics.Raycast(camShootingPoint, fpsCam.transform.forward, out hitObject, shootRange))
            {
                laserLine.SetPosition(1, hitObject.point); 

                EnemyHealthAndDamageManager enemyHealth = hitObject.collider.GetComponent<EnemyHealthAndDamageManager>(); 
                if (enemyHealth != null) 
                {
                    enemyHealth.damageEnemy(wandDamage); 
                }
            }
            else 
            {
                laserLine.SetPosition(1, (camShootingPoint + (fpsCam.transform.forward * shootRange))); 
            }
        }

        if ((Input.GetButtonDown("Fire1") && Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire2") && Input.GetButtonDown("Fire1")) && Time.time > nextFire)
        {

            print("Both left and right fire was hit!");
        }

    } // end update


    // Coroutine ShotEffect()
    private IEnumerator ShotEffect() 
    {
        laserLine.enabled = true; // When shot, laserline is enabled and coroutine is waiting for .07 seconds until it enables the laser from game view
        yield return rayDuration;
        laserLine.enabled = false;
    }
}
