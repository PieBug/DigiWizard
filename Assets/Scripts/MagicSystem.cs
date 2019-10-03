using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicSystem : MonoBehaviour {
    // Start is called before the first frame update
    public GameObject LeftWand;
    public GameObject RightWand;

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
    private LineRenderer RlaserLine; // takes array of two points and draws a line between each one in the game view
    private LineRenderer LlaserLine; // takes array of two points and draws a line between each one in the game view
    private float nextFire; // Holds time when player can fire again after firing

    // Wand Elements //
    string Lelement = "fire";
    string Relement = "ice";

    public Material fireMaterial;
    public Material lightingMaterial;
    public Material iceMaterial;

    // RAM System //
    public Slider ramSlider;
    public int ramAmount = 100;
    public float nextRamFire;
    public float ramFireRate = 1.0f;

    //---------------------------------------------------------------------------------------------//

    void Start()
    {
        LeftWand = GameObject.Find("LeftWand");
        RightWand = GameObject.Find("RightWand");

        LlaserLine = LeftWand.GetComponent<LineRenderer>();
        RlaserLine = RightWand.GetComponent<LineRenderer>();
        fpsCam = GetComponentInChildren<Camera>();

    } // end Start

    // Update is called once per frame
    void Update()
    {

        // LEFT BUTTON // 
        if (Input.GetButtonDown("Fire1") && !Input.GetButtonDown("Fire2") && Time.time > nextFire && ramAmount != 0)
        {
            string element = Lelement; // storing the element information

            nextFire = Time.time + fireRate; // making sure player does not constantly fire
            StartCoroutine(ShotEffect(LlaserLine)); // Calling the coroutine ShotEffect function to enable laser line

            Vector3 camShootingPoint = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0)); // Aiming point of the ray -> will be set to the middle position of the fps camera. Takes position of the camera and converts it to world space. 

            RaycastHit hitObject; // Object that is hit with our ray; object must have a collider on
            LlaserLine.SetPosition(0, LwandEnd.position); // starting position of the laserline is set to current position of the tip of the wand where the ray will shoot from

            if (Physics.Raycast(camShootingPoint, fpsCam.transform.forward, out hitObject, shootRange)) // Raycast is used to determine where the end of the ray will be, and deals force/damage to the object hit. Physics Raycast returns a bool. [camShootingPoin:] point in the world space where ray will begin [fpsCam:] Direction of the ray [Out - keyword:] Allows us to store information from a function + it's return type of the object hit. ex: Information like Rigidbody, collider, & surfacenormal of object hit. [shootRange:] How far ray goes.
            {
                LlaserLine.SetPosition(1, hitObject.point); // if raycast returns true and an object is hit, we're setting the 2nd position of the laser line to that object point

                EnemyHealthAndDeathManager enemyHealth = hitObject.collider.GetComponent<EnemyHealthAndDeathManager>(); // getting script from the object hit
                if (enemyHealth != null) // checking to make sure the hit object is an enemy type with script "EnemyHealthAndDamageManager" attached
                {
                    enemyHealth.DamageEnemy(wandDamage); // if "EnemyHealthAndDamageManager" exists, then call the damage function and pass in wand damage
                    RamDepletion();
                }
            }
            else // Raycast returns false
            {
                LlaserLine.SetPosition(1, (camShootingPoint + (fpsCam.transform.forward * shootRange))); // if nothing is hit, then the ray will just shoot 50 units away from the camera
            }
        }

        // RIGHT BUTTON //
        if (Input.GetButtonDown("Fire2") && !Input.GetButtonDown("Fire1") && Time.time > nextFire && ramAmount != 0) 
        {
            string element = Relement; // storing the element information

            nextFire = Time.time + fireRate;  // Prevent player from spamming fire button
            StartCoroutine(ShotEffect(RlaserLine)); // enabling the liner renderer 

            Vector3 camShootingPoint = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));  // Getting the mid point of the camera

            RaycastHit hitObject;  // Raycast var storing information of the object hit
            RlaserLine.SetPosition(0, RwandEnd.position); // setting starting position of the laser


            if (Physics.Raycast(camShootingPoint, fpsCam.transform.forward, out hitObject, shootRange))
            {
                RlaserLine.SetPosition(1, hitObject.point); // setting end position of laser to the object hit

                EnemyHealthAndDeathManager enemyHealth = hitObject.collider.GetComponent<EnemyHealthAndDeathManager>(); // creating object of enemyhealthmanager
                if (enemyHealth != null)
                {
                    // if EnemyHealthAndDamaageManager exists, then damage enemy
                    enemyHealth.DamageEnemy(wandDamage);
                    RamDepletion();
                }
            }
            else
            {
                // if it does not exist then cast the ray in a forward direction from the camera middle point
                RlaserLine.SetPosition(1, (camShootingPoint + (fpsCam.transform.forward * shootRange)));
            }
        }

        // BOTH BUTTONS //
        if ((Input.GetButtonDown("Fire1") && Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire2") && Input.GetButtonDown("Fire1")) && Time.time > nextFire && ramAmount != 0)
        {
            nextFire = Time.time + fireRate; // Making sure player does not constantly fire
            StartCoroutine(ShotEffect(LlaserLine)); // Calling to able the line renderer 
            StartCoroutine(ShotEffect(RlaserLine)); // Calling to able the line renderer 
            Vector3 camShootingPoint = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0)); // Point where ray will shoot at

            RaycastHit hitObject; // Variable Raycast that will store the information of the object that is hit.

            // Setting initial starting position of te lasers to the tip of the designated wands.
            LlaserLine.SetPosition(0, LwandEnd.position);
            RlaserLine.SetPosition(0, RwandEnd.position);


            // RayCast(origin, direction, out: inserting information of object hit from raycast, distance)
            if (Physics.Raycast(camShootingPoint, fpsCam.transform.forward, out hitObject, shootRange))
            {
                // Point stores the Vector3 transformation of the object hit in the game view
                LlaserLine.SetPosition(1, hitObject.point);
                RlaserLine.SetPosition(1, hitObject.point);

                // Creating object of EnemyHealthDamageManager and inserting it with the hitObject
                EnemyHealthAndDeathManager enemyHealth = hitObject.collider.GetComponent<EnemyHealthAndDeathManager>();
                if (enemyHealth != null)
                {
                    // if it exsists, then insert wand damage
                    enemyHealth.DamageEnemy(wandDamage);
                    RamDepletion();
                }
            }
            else
            {
                // If a object is not hit, then just cast the laser line in forward direction pointing originating from the camera middle point
                LlaserLine.SetPosition(1, (camShootingPoint + (fpsCam.transform.forward * shootRange)));
                RlaserLine.SetPosition(1, (camShootingPoint + (fpsCam.transform.forward * shootRange)));
            }
        }


        // Magic Switching System //

        // Q KEY: Left Wand //
        if (Input.GetKeyDown(KeyCode.Q) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            if ((Lelement == "fire" && Relement == "ice") || (Lelement == "ice" && Relement == "fire"))
            {
                Lelement = "lighting";
                LlaserLine.material = new Material(lightingMaterial);
                print(Lelement);
            }
            else if ((Lelement == "lighting" && Relement == "ice") || (Lelement == "ice" && Relement == "lighting"))
            {
                Lelement = "fire";
                LlaserLine.material = new Material(fireMaterial);
                print(Lelement);
            }
            else if ((Lelement == "fire" && Relement == "lighting") || (Lelement == "lighting" && Relement == "fire"))
            {
                Lelement = "ice";
                LlaserLine.material = new Material(iceMaterial);
                print(Lelement);
            }
        }

        // E KEY: Right Wand //
        if (Input.GetKey(KeyCode.E) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            if ((Lelement == "fire" && Relement == "ice") || (Lelement == "ice" && Relement == "fire"))
            {
                Relement = "lighting";
                RlaserLine.material = new Material(lightingMaterial);
                print(Relement);
            }
            else if ((Lelement == "lighting" && Relement == "ice") || (Lelement == "ice" && Relement == "lighting"))
            {
                Relement = "fire";
                RlaserLine.material = new Material(fireMaterial);
                print(Relement);
            }
            else if ((Lelement == "fire" && Relement == "lighting") || (Lelement == "lighting" && Relement == "fire"))
            {
                Relement = "ice";
                RlaserLine.material = new Material(iceMaterial);
                print(Relement);
            }
        }

        if (!(Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")) && (Time.time > nextRamFire) && (ramAmount < 100))
        {
            nextRamFire = Time.time + ramFireRate;
            ramAmount += 1;
            print(ramAmount);
            ramSlider.value = ramAmount;
        }

    } // end update

    // Deplete ram slider bar //
    private void RamDepletion()
    {
        ramAmount -= 5;
        print(ramAmount);
        ramSlider.value = ramAmount;
    }

    // Coroutine ShotEffect()
    private IEnumerator ShotEffect(LineRenderer laserLine) 
    {
        laserLine.enabled = true; // When shot, laserline is enabled and coroutine is waiting for .07 seconds until it enables the laser from game view
        yield return rayDuration;
        laserLine.enabled = false;
    }
}
