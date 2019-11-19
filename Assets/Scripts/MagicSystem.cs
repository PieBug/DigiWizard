using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicSystem : MonoBehaviour
{
    //Input Variables//
    bool leftPressed;
    bool rightPressed;
    bool updateRightLightBool;
    bool updateLeftLightBool;

    // Shooting Variables // 
    public float shootRange = 10f; // How long rays are shot
    public float fireRate = .25f; // How often player can fire weapon
    private Camera cam; // Player camera
    private float nextFire; // Holds time when player can fire again after firing

    // Wand Objects //
    public Transform Lwand; // Marks the tip of the wand where spell will shoot from
    public Transform Rwand; // Marks the tip of the wand where spell will shoot from
    public Transform ComboPosition; // Marks the tip of the wand where spell will shoot from
    public GameObject fireBall; // Holds fire prefab
    public GameObject iceBall; // Holds ice prefab
    public GameObject lightingBall; // Holds lighting prefab
    string Lelement; // string to store current element in LEFT hand
    string Relement; // string to store current element in RIGHT hand
    int lightingDMG; // lighting damage 
    int fireDMG; // fire damage 
    public float fireBallForce;
    public float fireBallArch;
    public float playerInfluenceOnFireBall;
    int iceDMG; // ice damage

   // GameObject lightingCopy;
    public GameObject lightingRightLine;
    public GameObject lightingLeftLine;

    string LastHitElement; // obtains the element last shot
    int LightingCounter; // counting lighting -> if shot consecutively, lighting does more damage

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
    BaseAI enemyMonster; // WIP freezing spider AI
    EnemyHealthAndDeathManager enemyHealth; // damaging enemy

    // Player Controller // 
    PlayerController playerMovement;
    Vector3 PlayerVel;
    Vector3 PlayerDir;
    Vector3 PlayerTranslate;
    Rigidbody rbPlayer;

    // Magic Switch Image //
    public Image R_Fire;
    public Image R_Ice;
    public Image R_Lightning;
    public Image L_Fire;
    public Image L_Ice;
    public Image L_Lightning;

    // Combo Magics //
    public GameObject ComboFireLight;
    public GameObject ComboIceFire;
    public GameObject ComboLightIce;

    //---------------------------------------------------------------------------------------------//

    void Start()
    {
        Lelement = "fire";
        Relement = "ice";
        L_Fire.enabled = true;
        R_Ice.enabled = true;
        cam = GetComponentInChildren<Camera>();
        playerMovement = this.GetComponent<PlayerController>();
        rbPlayer = this.GetComponent<Rigidbody>();
    } // end Start

    // Update is called once per frame
    void Update()
    {
        PlayerVel = this.transform.position;
        PlayerDir = playerMovement.direction;
        PlayerTranslate = rbPlayer.worldCenterOfMass.normalized;

        
        // LEFT BUTTON // 
        if (Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && Time.time > nextFire && ramAmount != 0 && ramAmount > 0)
        {
            nextFire = Time.time + fireRate; // making sure player does not constantly fire
            string element = Lelement; // storing the element information
            LastHitElement = Lelement;
            switch (element)
            {
                case "fire":
                    FireShoot(Lwand);
                    break;
                case "ice":
                    ShootElement(Lwand, iceBall, element);
                    break;
                case "lighting":
                    ActivateLighting(Lwand);
                    break;
            }
        }

        // RIGHT BUTTON // 
        if (Input.GetMouseButtonDown(1) && !Input.GetMouseButtonDown(0) && Time.time > nextFire && ramAmount != 0 && ramAmount > 0)
        {
            nextFire = Time.time + fireRate; // making sure player does not constantly fire
            string element = Relement; // storing the element information
            LastHitElement = Relement;
            switch (element)
            {
                case "fire":
                    FireShoot(Rwand);
                    break;
                case "ice":
                    ShootElement(Rwand, iceBall, element);
                    break;
                case "lighting":
                    ActivateLighting(Rwand);
                    break;
            }
        }

        // BOTH BUTTONS //
        if ((Input.GetMouseButtonDown(1) && Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(0) && Input.GetMouseButtonDown(1)) && Time.time > nextFire && ramAmount != 0 && ramAmount > 0)
        {
            LastHitElement = "";
            nextFire = Time.time + fireRate; // Making sure player does not constantly fire
            print("Both buttons were pressed");

            if ((Lelement == "fire" && Relement == "ice") || (Lelement == "ice" && Relement == "fire"))
            {
                print("Fire and ice");
                //GameObject fireiceClone = Instantiate(ComboIceFire, ComboPosition.transform.position, Quaternion.identity);
                ShootElement(ComboPosition, ComboIceFire, "ice");
            }
            else if ((Lelement == "ice" && Relement == "lighting") || (Lelement == "lighting" && Relement == "ice"))
            {
                print("Ice and lightning");
                ComboLightIce.SetActive(true);
                ShootElement(ComboPosition, iceBall, "ice");
            }
            else if ((Lelement == "fire" && Relement == "lighting") || (Lelement == "lighting" && Relement == "fire"))
            {
                print("fire and lightning");
                ComboFireLight.SetActive(true);
            }
        }

        // BOTH BUTTONS //
        if ((Input.GetMouseButtonUp(1) && Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(0) && Input.GetMouseButtonUp(1)))
        {
            LastHitElement = "";
            //nextFire = Time.time + fireRate; // Making sure player does not constantly fire
            // && Time.time > nextFire && ramAmount != 0 && ramAmount > 0

            if ((Lelement == "fire" && Relement == "ice") || (Lelement == "ice" && Relement == "fire"))
            {
                print("BUTTON UP Fire and ice");

            }
            else if ((Lelement == "ice" && Relement == "lighting") || (Lelement == "lighting" && Relement == "ice"))
            {
                print("BUTTON UP Ice and lightning");
                ComboLightIce.SetActive(false);
            }
            else if (ComboFireLight == true)
            {
                print("BUTTON UP fire and lightning");
                ComboFireLight.SetActive(false);
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
        }

        // Ram penalty system //
        if (ramAmount == 0 || ramAmount < 0)
        {
            
            if (updateLeftLightBool == true)
            {
                lightingLeftLine.SetActive(false);
                updateLeftLightBool = false;
            }
            else if (updateRightLightBool == true)
            {
                lightingRightLine.SetActive(false);
                updateLeftLightBool = false;
            }
            IsRamPenalty = true;
            StartPenalty();
        }

        if (updateRightLightBool)
        {
            UpdateLighting(Rwand);
        }
        if (updateLeftLightBool)
        {
            UpdateLighting(Lwand);
        }

    } // end UPDATE

    private void FixedUpdate()
    {
        // RIGHT BUTTON // 
        if (Input.GetMouseButtonUp(1))
        {
            if (updateRightLightBool == true)
            {
                EnableLighting(Rwand);
            }
        }
        // LEFT BUTTON //
        if (Input.GetMouseButtonUp(0))
        {
            if (updateLeftLightBool == true)
            {
                EnableLighting(Lwand);
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
                L_Lightning.enabled = true;
                L_Fire.enabled = false;
                L_Ice.enabled = false;
            }
            else if ((Lelement == "lighting" && Relement == "ice") || (Lelement == "ice" && Relement == "lighting"))
            {
                Lelement = "fire";
                L_Lightning.enabled = false;
                L_Fire.enabled = true;
                L_Ice.enabled = false;
            }
            else if ((Lelement == "fire" && Relement == "lighting") || (Lelement == "lighting" && Relement == "fire"))
            {
                Lelement = "ice";
                L_Lightning.enabled = false;
                L_Fire.enabled = false;
                L_Ice.enabled = true;
            }
        }
        // E KEY: Right Wand //
        if (Input.GetKey(KeyCode.E) && Time.time > nextFire)
        {
            nextFire = Time.time + fireRate;
            if ((Lelement == "fire" && Relement == "ice") || (Lelement == "ice" && Relement == "fire"))
            {
                Relement = "lighting";
                R_Lightning.enabled = true;
                R_Fire.enabled = false;
                R_Ice.enabled = false;
            }
            else if ((Lelement == "lighting" && Relement == "ice") || (Lelement == "ice" && Relement == "lighting"))
            {
                Relement = "fire";
                R_Lightning.enabled = false;
                R_Fire.enabled = true;
                R_Ice.enabled = false;
            }
            else if ((Lelement == "fire" && Relement == "lighting") || (Lelement == "lighting" && Relement == "fire"))
            {
                Relement = "ice";
                R_Lightning.enabled = false;
                R_Fire.enabled = false;
                R_Ice.enabled = true;
            }
        }
    } // end FIXED UPDATE

    // Creating elemental damages //
    public void ElementDamageManager(string element, EnemyHealthAndDeathManager enemyH)
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
            }
        }
        if (element == "ice" && enemyH != null)
        {
            iceDMG = 1;
            enemyH.DamageEnemy(iceDMG);  // Does very little damage
            FreezeAIEnemy(enemyH.ai);
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
    public void RamDepletion(int damageAmt)
    {
        if (ramAmount > 0 && !(ramAmount <= 0))
        {
            ramAmount -= damageAmt;
        }
        else if (ramAmount < 0)
        {
            ramAmount = 0;
        }
        ramSlider.value = ramAmount;
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
    }
    // Starting Penalty //
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

    // Cancel Penalty // 
    public void CancelPenaltyCoroutine()
    {
        cancelPenalty = true;
        penaltyRunning = false;
        IsRamPenalty = false;

        StopCoroutine(ramPenaltyCoroutine);
    }

    private void FreezeAIEnemy(BaseAI ai)
    {
        if (ai != null)
        {
            ai.IceAI(0.5f, 0.5f);
            print("Slowing enemy");
            StartCoroutine(EnemyFreezeCoroutine());
            //ai.IceAI(1f, 1f);
            print("Freezing enemy");
            StartCoroutine(EnemyFreezeCoroutine());
            //ai.ResetAI();
            print("Un freezing enemy");
        }
    }
    private IEnumerator EnemyFreezeCoroutine()
    {
        print("Enemy Coroutine start");
        yield return new WaitForSeconds(5);
        print("Enemy Coroutine is over");
    }

    public void ShootElement(Transform wandPosition, GameObject Element, string power)
    {
        Transform wand = wandPosition;
        Quaternion BulletRotation = Quaternion.LookRotation(cam.transform.forward);
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 pointTo = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hitObj;
        GameObject elementToShoot = Instantiate(Element, wand.transform.position, Quaternion.identity);
        if (Physics.Raycast(ray, out hitObj, shootRange))
        {
            Vector3 desitnation = elementToShoot.transform.position - hitObj.point;
            Quaternion rotationDestination = Quaternion.LookRotation(-desitnation);
            elementToShoot.transform.localRotation = Quaternion.Lerp(elementToShoot.transform.rotation, cam.transform.rotation, 1);
            RamDepletion(3);
            enemyHealth = hitObj.collider.GetComponentInParent<EnemyHealthAndDeathManager>(); // getting script from the object hit
            enemyMonster = hitObj.collider.GetComponent<BaseAI>();

            if (enemyHealth != null) // checking to make sure the hit object is an enemy type with script "EnemyHealthAndDamageManager" attached
            {
                ElementDamageManager(power, enemyHealth); // if "EnemyHealthAndDamageManager" exists, then pass in the element
                RamDepletion(3);
            }
        }
        else
        {
            var position = ray.GetPoint(shootRange);
            Vector3 destintion = elementToShoot.transform.position - position;
            Quaternion rotationDestination = Quaternion.LookRotation(-destintion);
            elementToShoot.transform.localRotation = Quaternion.Lerp(elementToShoot.transform.rotation, cam.transform.rotation, 1);
            RamDepletion(3);
            Destroy(elementToShoot, 0.6f);
        }
    } // End shootElement

    private void LightingShoot(Transform wandE)
    {
        GameObject elementToShoot;
        elementToShoot = Instantiate(lightingBall, wandE.transform);

    } // End LightingShoot

    private void ActivateLighting(Transform wandE)
    {
        if (wandE.name == "RightWandEnd")
        {
            updateRightLightBool = true;
            lightingRightLine.SetActive(true);
        }
        else if (wandE.name == "LeftWandEnd")
        {
            updateLeftLightBool = true;
            lightingLeftLine.SetActive(true);
        }
    }
    
    private void UpdateLighting(Transform wandE)
    {
        if (updateLeftLightBool == true)
        {
            lightingLeftLine.transform.position = wandE.transform.position;
        }
        else if (updateRightLightBool == true)
        {
            lightingRightLine.transform.position = wandE.transform.position;
        }
    }

    private void EnableLighting(Transform wandE)
    {
        if (wandE.name == "RightWandEnd")
        {
            updateRightLightBool = false;
            lightingRightLine.SetActive(false);
        }
        else if (wandE.name == "LeftWandEnd")
        {
            updateLeftLightBool = false;
            lightingLeftLine.SetActive(false);
        }
    }

    // FireBall Projectile //
    private void FireShoot(Transform wandE)
    {
        GameObject fireCopy;
        fireCopy = Instantiate(fireBall, wandE.transform.position, cam.transform.rotation);
        fireCopy.GetComponent<FireBall>().magicSystem = this;
        if (fireCopy != null)
        {
            RamDepletion(4);
            Rigidbody rbFireBall;
            rbFireBall = fireCopy.GetComponent<Rigidbody>();
            rbFireBall.AddForce((cam.transform.forward + new Vector3(0, fireBallArch, 0)).normalized * fireBallForce, ForceMode.VelocityChange);
            Vector3 v = rbFireBall.velocity;
            v += playerMovement.playerVelocity * playerInfluenceOnFireBall;
            rbFireBall.velocity = v;
        }
    } // End FireShoot
}

// NOTES: //
// Spider AI Frozen float - to increase or decrease speed


