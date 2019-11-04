using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicSystem : MonoBehaviour
{

    // Start is called before the first frame update
    public GameObject LeftWand;
    public GameObject RightWand;

    // Shooting Variables // 
    public int wandDamage = 1; // Amount of damage
    public float fireRate = .25f; // How often player can fire weapon
    public float shootRange = 10f; // How long rays are shot
    //public float hitForce = 100f; // How much force hitting game objects with a rigidbody
    public Transform Lwand; // Marks the tip of the wand where ray will shoot from
    public Transform Rwand; // Marks the tip of the wand where ray will shoot from
    private Camera cam;
    private WaitForSeconds rayDuration = new WaitForSeconds(.07f); // How long ray will remain in game view
    //private AudioSource bulletSound;  // Only use this once we get audio sounds
    private LineRenderer RlaserLine; // takes array of two points and draws a line between each one in the game view
    private LineRenderer LlaserLine; // takes array of two points and draws a line between each one in the game view
    private float nextFire; // Holds time when player can fire again after firing

    // Raycast visual indicator - bullet holes //
    public GameObject bullet;
    private GameObject bulletClone;

    // Wand Elements //
    string Lelement;
    string Relement;

    public Material fireMaterial;
    public Material lightingMaterial;
    public Material iceMaterial;
    public Material icefire;
    public Material icelight;
    public Material firelight;



    int lightingDMG;
    int fireDMG;
    int iceDMG;

    string LastHitElement;
    int LightingCounter;

    // RAM System //
    public bool cancelPenalty = false;
    public bool penaltyRunning;
    public Slider ramSlider;
    public int ramAmount = 100;
    public float nextRamFire;
    public float ramFireRate = 3.0f;
    private WaitForSeconds ramDuration = new WaitForSeconds(7.0f);
    bool IsRamPenalty = false;
    private IEnumerator ramPenaltyCoroutine;


    // Spider AI //
    BaseAI enemyMonster;
    EnemyHealthAndDeathManager enemyHealth;

    // Testing particle shooter // 
    public GameObject fireParticle;
    private GameObject fireBulletClone;
    private List<GameObject> fireBulletList = new List<GameObject>();
    public float fireSpeed = 20;

    // GameObjects //
    public GameObject fireBall;
    public GameObject iceBall;
    public GameObject lightingBall;

    //---------------------------------------------------------------------------------------------//

    void Start()
    {
        LeftWand = GameObject.Find("LeftWand");
        RightWand = GameObject.Find("RightWand");
        Lelement = "fire";
        Relement = "ice";

        cam = GetComponentInChildren<Camera>();

    } // end Start

    // Update is called once per frame
    void Update()
    {

        // LEFT BUTTON // 
        if (Input.GetButtonDown("Fire1") && !Input.GetButtonDown("Fire2") && Time.time > nextFire && ramAmount != 0 && ramAmount > 0)
        {
            string element = Lelement; // storing the element information
            LastHitElement = Lelement;

            nextFire = Time.time + fireRate; // making sure player does not constantly fire
            //StartCoroutine(ShotEffect(LlaserLine)); // Calling the coroutine ShotEffect function to enable laser line

            //ShootElement(Lwand, lightingBall);
            FireShoot(Lwand);

        }

        // RIGHT BUTTON //
        if (Input.GetButtonDown("Fire2") && !Input.GetButtonDown("Fire1") && Time.time > nextFire && ramAmount != 0 && ramAmount > 0)
        {

            string element = Relement; // storing the element information
            LastHitElement = Relement;

            nextFire = Time.time + fireRate;  // Prevent player from spamming fire button
            ShootElement(Rwand, iceBall);
        }

        // BOTH BUTTONS //
        if ((Input.GetButtonDown("Fire1") && Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire2") && Input.GetButtonDown("Fire1")) && Time.time > nextFire && ramAmount != 0 && ramAmount > 0)
        {


            LastHitElement = "";
            nextFire = Time.time + fireRate; // Making sure player does not constantly fire
            
        }


        // Magic Switching System //
        // Q KEY: Left Wand //
        if (Input.GetKeyDown(KeyCode.Q) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            if ((Lelement == "fire" && Relement == "ice") || (Lelement == "ice" && Relement == "fire"))
            {
                Lelement = "lighting";
            }
            else if ((Lelement == "lighting" && Relement == "ice") || (Lelement == "ice" && Relement == "lighting"))
            {
                Lelement = "fire";

            }
            else if ((Lelement == "fire" && Relement == "lighting") || (Lelement == "lighting" && Relement == "fire"))
            {
                Lelement = "ice";
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
                //print(Relement);
            }
            else if ((Lelement == "lighting" && Relement == "ice") || (Lelement == "ice" && Relement == "lighting"))
            {
                Relement = "fire";
                RlaserLine.material = new Material(fireMaterial);
                // print(Relement);
            }
            else if ((Lelement == "fire" && Relement == "lighting") || (Lelement == "lighting" && Relement == "fire"))
            {
                Relement = "ice";
                RlaserLine.material = new Material(iceMaterial);
                // print(Relement);
            }
        }
        // Ram regeneration system //
        if (!(Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")) && (Time.time > nextRamFire) && IsRamPenalty == false)
        {
            nextRamFire = Time.time + ramFireRate;

            if (!(ramAmount == 100) || !(ramAmount > 100))
            {
                ramAmount += 5;
                if (ramAmount > 100)
                {
                    ramAmount = 100;
                }
                ramSlider.value = ramAmount;
            }
            else if (ramAmount > 100)
            {
                ramAmount = 100;
                ramSlider.value = ramAmount;
            }
            // print(ramAmount);
        }

        // Ram penalty system //
        if (ramAmount == 0 || ramAmount < 0)
        {
            // print("Starting penalty");
            IsRamPenalty = true;
            StartPenalty();
        }

    } // end update


    // Creating elemental damages //

    void ElementDamageManager(string element, EnemyHealthAndDeathManager enemyH)
    {
        if (element == "fire" && enemyH != null)
        {
            if (LastHitElement == "ice")
            {
                fireDMG = 10;
                enemyH.DamageEnemy(fireDMG);
                print("success fire + ice");
            }
            else
            {
                // does regular damage
                fireDMG = 5;
                enemyH.DamageEnemy(fireDMG);
                //print("success fire");
            }
        }
        if (element == "ice" && enemyH != null)
        {
            iceDMG = 1;
            enemyH.DamageEnemy(iceDMG);  // Does very little damage
            FreezeAIEnemy();
        }
        if (element == "lighting" && enemyH != null)
        {
            if (LastHitElement == "lighting" != (LightingCounter == 5))
            {
                LightingCounter += 1;
                lightingDMG = 3 * LightingCounter;
                enemyH.DamageEnemy(lightingDMG);
            }
            else
            {
                LightingCounter = 1;
                // does damage + increase damage if you conseutively hit an enemy AI
                lightingDMG = 3;
                enemyH.DamageEnemy(lightingDMG);
            }
        }
    }

    // Deplete ram slider bar //
    public void RamDepletion()
    {
        if (ramAmount > 0 && !(ramAmount <= 0))
        {
            ramAmount -= 5;
        }
        else if (ramAmount < 0)
        {
            ramAmount = 0;
        }
        // print(ramAmount);
        ramSlider.value = ramAmount;
        //print(ramAmount);
    }

    // Add Ram //
    public void AddRam(int Amt)
    {
        ramAmount += Amt;
        ramSlider.value = ramAmount;

        if (ramAmount > 100)
        {
            ramAmount = 100;
            ramSlider.value = ramAmount;
        }
        //print(ramAmount);
    }

    void StartPenalty()
    {
        print("Starting penalty");
        ramPenaltyCoroutine = RamPenalty();
        StartCoroutine(ramPenaltyCoroutine);
        cancelPenalty = false;
    }

    // Ram Coroutine //
    private IEnumerator RamPenalty()
    {
        if (cancelPenalty == false)
        {
            penaltyRunning = true;
            // print (ramDuration);
            yield return ramDuration;
            // print ("Ram coroutine worked!");
            IsRamPenalty = false;
            penaltyRunning = false;
        }
    }

    public void CancelPenaltyCoroutine()
    {
        cancelPenalty = true;
        penaltyRunning = false;
        IsRamPenalty = false;

        // ramAmount = 40;
        // ramSlider.value = ramAmount;

        //print("Coroutine was stopped successfully");
        StopCoroutine(ramPenaltyCoroutine);
    }

    private void FreezeAIEnemy()
    {
        if (enemyMonster != null)
        {
            // StartCoroutine(EnemyFreezeCoroutine());
            enemyMonster.IceAI(1f, 1f);
            print("Slowing enemy");
            StartCoroutine(EnemyFreezeCoroutine());
            enemyMonster.IceAI(0f, 0f);
            print("Freezing enemy");
            StartCoroutine(EnemyFreezeCoroutine());
            enemyMonster.ResetAI();
            print("Un freezing enemy");
        }
    }
    private IEnumerator EnemyFreezeCoroutine()
    {
        print("Enemy Coroutine start");
        yield return new WaitForSeconds(5);
        print("Enemy Coroutine is over");
    }

    public void ShootElement(Transform wandPosition, GameObject Element)
    {
        Transform wand = wandPosition;
        Quaternion BulletRotation = Quaternion.LookRotation(cam.transform.forward);
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 pointTo = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hitObj;

        GameObject elementToShoot = Instantiate(Element, wand.transform.position, Quaternion.identity);

        if (Physics.Raycast(pointTo, cam.transform.forward, out hitObj, shootRange))
        {
            Vector3 desitnation = elementToShoot.transform.position - hitObj.point;
            Quaternion rotationDestination = Quaternion.LookRotation(-desitnation);
            elementToShoot.transform.localRotation = Quaternion.Lerp(elementToShoot.transform.rotation, rotationDestination, 1);
            RamDepletion();
        }
        else
        {
            var position = ray.GetPoint(shootRange);
            Vector3 destintion = elementToShoot.transform.position - position;
            Quaternion rotationDestination = Quaternion.LookRotation(-destintion);
            elementToShoot.transform.localRotation = Quaternion.Lerp(elementToShoot.transform.rotation, rotationDestination, 1);
            RamDepletion();
        }
    } // End shootElement

    private void FireShoot(Transform wandE)
    {
        GameObject fireCopy = Instantiate(fireBall, wandE.transform.position, Quaternion.identity);
        if (fireCopy != null)
        {

            //ballCopy.GetComponent<Rigidbody>().AddForceAtPosition(fpsCam.transform.forward * 350, wand.transform.position);
            fireCopy.GetComponent<Rigidbody>().AddForceAtPosition(cam.transform.forward * 300, wandE.transform.position);

            //ballCopy.GetComponent<Rigidbody>().velocity = transform.up * bounceHeight;
            fireCopy.GetComponent<Rigidbody>().AddForce(0, 5, 0, ForceMode.VelocityChange);
        }
    } // End FireShoot

}


// NOTES: //
// Spider AI Frozen float - to increase or decrease speed