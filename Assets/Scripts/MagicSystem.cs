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
    public Transform MidPosition; // Marks the tip of the wand where spell will shoot from
    public GameObject fireProjectile; // Holds fire prefab
    public GameObject iceProjectile; // Holds ice prefab

    // GameObject lightingCopy;
    public GameObject lightingRightLine;
    public GameObject lightingLeftLine;

    // Combo Magics //
    public GameObject ComboFireLight;
    public GameObject ComboIceFire;
    public GameObject ComboLightIce;

    // Particle Feedback //
    public GameObject blueParticle;
    public GameObject redParticle;
    public GameObject yellowParticle;

    //public GameObject bullet; 
    string Lelement; // string to store current element in LEFT hand
    string Relement; // string to store current element in RIGHT hand

    // Element Damages //
    public int lightingDMG; // lighting damage 
    public int fireDMG; // fire damage 
    public int iceDMG; // ice damage

    // Fire attributes //
    public float fireBallForce;
    public float fireBallArch;
    public float playerInfluenceOnFireBall;
    public int fireRamDepletion;

    // Ice attributes //
    public int iceRamDepletion;

    // Lightning Attributes //
    bool confirmLight = true;
    public WaitForSeconds ramDrainSpeed = new WaitForSeconds(0.2f);
    public int lightningRamDepletion;
    //------------------------------
            // COMBOS //

    // FireIce Attributes //
    public int fireiceRamDepletion;
    public int fireiceDMG;

    // IceLightning Attributes //
    public int icelightRamDepletion;
    public int icelightDMG;

    // LightningFire Attributes //
    public int lightfireRamDepletion;
    public int lightfireDMG;
    bool confirmLightFire = true;

    //------------------------------

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
    private IEnumerator ramLightningCoroutine;
    private IEnumerator ramLightFireCoroutine;

    // Spider AI //
    BaseAI enemyMonster; // WIP freezing spider AI
    EnemyHealthAndDeathManager enemyHealth; // damaging enemy

    // Player Controller // 
    PlayerController playerMovement;
    Rigidbody rbPlayer;

    // Magic Switch Image //
    public Image R_Fire;
    public Image R_Ice;
    public Image R_Lightning;
    public Image L_Fire;
    public Image L_Ice;
    public Image L_Lightning;



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
        // LEFT BUTTON // 
        if (Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1) && Time.time > nextFire && ramAmount != 0 && ramAmount > 0)
        {
            nextFire = Time.time + fireRate; // making sure player does not constantly fire
            string element = Lelement; // storing the element information
            switch (element)
            {
                case "fire":
                    FireShoot(MidPosition);
                    break;
                case "ice":
                    ShootElement(MidPosition, iceProjectile, element, iceRamDepletion, blueParticle);
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
            switch (element)
            {
                case "fire":
                    FireShoot(MidPosition);
                    break;
                case "ice":
                    ShootElement(MidPosition, iceProjectile, element, iceRamDepletion, blueParticle);
                    break;
                case "lighting":
                    ActivateLighting(Rwand);
                    break;
            }
        }
        // COMBO POWERS
        // BOTH BUTTONS //
        if ((Input.GetMouseButtonDown(1) && Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(0) && Input.GetMouseButtonDown(1)) && Time.time > nextFire && ramAmount != 0 && ramAmount > 0)
        {
            nextFire = Time.time + fireRate; // Making sure player does not constantly fire

            if ((Lelement == "fire" && Relement == "ice") || (Lelement == "ice" && Relement == "fire"))
            {
                ShootElement(MidPosition, ComboIceFire, "fireice", fireiceRamDepletion, redParticle); // combo ram need to re-do
            }
            else if ((Lelement == "ice" && Relement == "lighting") || (Lelement == "lighting" && Relement == "ice"))
            {
                ComboLightIce.SetActive(true);
                ShootElement(MidPosition, iceProjectile, "icelight", icelightRamDepletion, blueParticle); // combo ram need to re-do
            }
            else if ((Lelement == "fire" && Relement == "lighting") || (Lelement == "lighting" && Relement == "fire"))
            {
                ComboFireLight.SetActive(true);
                confirmLightFire = true;
                StartLightFireCor();
            }
        }

        // BOTH BUTTONS //
        if ((Input.GetMouseButtonUp(1) && Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(0) && Input.GetMouseButtonUp(1)))
        {
            /**
            if ((Lelement == "fire" && Relement == "ice") || (Lelement == "ice" && Relement == "fire"))
            {}
            **/
            if ((Lelement == "ice" && Relement == "lighting") || (Lelement == "lighting" && Relement == "ice"))
            {
                ComboLightIce.SetActive(false);
            }
            else if (ComboFireLight == true)
            {
                ComboFireLight.SetActive(false);
                confirmLightFire = false;
                StopLightFire();
            }
        }

        // Ram regeneration system //
        if (!(Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2")) && (Time.time > nextRamFire) && IsRamPenalty == false && confirmLight == true)
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

    // Fixed Update //
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
                // Switching images
                L_Lightning.enabled = true;
                L_Fire.enabled = false;
                L_Ice.enabled = false;
            }
            else if ((Lelement == "lighting" && Relement == "ice") || (Lelement == "ice" && Relement == "lighting"))
            {
                Lelement = "fire";
                // Switching images
                L_Lightning.enabled = false;
                L_Fire.enabled = true;
                L_Ice.enabled = false;
            }
            else if ((Lelement == "fire" && Relement == "lighting") || (Lelement == "lighting" && Relement == "fire"))
            {
                Lelement = "ice";
                // Switching images
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
                // Switching images
                R_Lightning.enabled = true;
                R_Fire.enabled = false;
                R_Ice.enabled = false;
            }
            else if ((Lelement == "lighting" && Relement == "ice") || (Lelement == "ice" && Relement == "lighting"))
            {
                Relement = "fire";
                // Switching images
                R_Lightning.enabled = false;
                R_Fire.enabled = true;
                R_Ice.enabled = false;
            }
            else if ((Lelement == "fire" && Relement == "lighting") || (Lelement == "lighting" && Relement == "fire"))
            {
                Relement = "ice";
                // Switching images
                R_Lightning.enabled = false;
                R_Fire.enabled = false;
                R_Ice.enabled = true;
            }
        }

        // Lightning Ram Depletion // 
        if (updateRightLightBool && confirmLight)
        {
            RamDepletion(lightningRamDepletion);
            StartLightningCor();

        }
        if (updateLeftLightBool && confirmLight)
        {
            RamDepletion(lightningRamDepletion);
            StartLightningCor();
        }

        // Light FIRE Ram Depletion // 
        if (confirmLightFire)
        {
            RamDepletion(lightfireRamDepletion);
            StartLightFireCor();

        }

    } // end FIXED UPDATE

    // Element Damage Manager //
    public void ElementDamageManager(string element, EnemyHealthAndDeathManager enemyH)
    {
        if (element == "fire" && enemyH != null)
        {
            enemyH.DamageEnemy(fireDMG);
        }
        if (element == "ice" && enemyH != null)
        {
            enemyH.DamageEnemy(iceDMG);  
            FreezeAIEnemy(enemyH.ai); // Freezing AI
        }
        if (element == "lighting" && enemyH != null)
        {
            enemyH.DamageEnemy(lightingDMG);
        }

        if (element == "fireice" && enemyH != null)
        {
            enemyH.DamageEnemy(fireiceDMG);
        }

        if (element == "icelight" && enemyH != null)
        {
            enemyH.DamageEnemy(icelightDMG);
        }

        if (element == "lightfire" && enemyH != null)
        {
            enemyH.DamageEnemy(lightfireDMG);
        }
    } 

    // Deplete ram //
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

    // Add Ram for pick up //
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

    // RAM penalty //
    void StartPenalty()
    {
        //print("Starting penalty");
        ramPenaltyCoroutine = RamPenalty();
        StartCoroutine(ramPenaltyCoroutine);
        cancelPenalty = false;
    }

    private IEnumerator RamPenalty()
    {
        if (cancelPenalty == false)
        {
            penaltyRunning = true;
            yield return ramDuration;
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

    // Particle Coroutine //
    private IEnumerator InstantiateParticle(GameObject particle, GameObject elementObj)
    {
        //print("Inside particle coroutine");
        yield return new WaitForSeconds(0.9f);
        if (elementObj != null)
        {
            Instantiate(particle, elementObj.transform.position, elementObj.transform.rotation);
            Destroy(elementObj, 0.3f);
        }
    }

    // Lightning Ram Coroutine //

    private void StartLightningCor()
    {
        ramLightningCoroutine = LightningRamDeplete();
        StartCoroutine(ramLightningCoroutine);
    }
    private void StopLighting()
    {
        confirmLight = true;
        StopCoroutine(ramLightningCoroutine);
    }
    private IEnumerator LightningRamDeplete()
    {
        confirmLight = false;
        yield return ramDrainSpeed;
        StopLighting();
    }

    // Light FIRE Ram Coroutine //

    private void StartLightFireCor()
    {
        ramLightFireCoroutine = LightFireRamDeplete();
        StartCoroutine(ramLightFireCoroutine);
    }
    private void StopLightFire()
    {
        confirmLightFire = true;
        StopCoroutine(ramLightFireCoroutine);
    }
    private IEnumerator LightFireRamDeplete()
    {
        confirmLightFire = false;
        yield return new WaitForSeconds(0.1f);
        //StopLightFire();
    }

    // Freezing AI //
    private void FreezeAIEnemy(BaseAI ai)
    {
        if (ai != null)
        {
            ai.IceAI(0.5f, 0.5f);
            //print("Slowing enemy");
            StartCoroutine(EnemyFreezeCoroutine());
            //ai.IceAI(1f, 1f);
            //print("Freezing enemy");
            StartCoroutine(EnemyFreezeCoroutine());
            //ai.ResetAI();
            //print("Un freezing enemy");
        }
    }
    private IEnumerator EnemyFreezeCoroutine()
    {
        //print("Enemy Coroutine start");
        yield return new WaitForSeconds(5);
        //print("Enemy Coroutine is over");
    }

    // Element Magic Shooting //
    public void ShootElement(Transform wandPosition, GameObject Element, string power, int ramAmt, GameObject particleToInstantiate)
    {
        Transform wand = wandPosition;
        Quaternion BulletRotation = Quaternion.LookRotation(cam.transform.forward);
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 pointTo = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hitObj;
        GameObject elementToShoot = Instantiate(Element, wand.transform.position, Quaternion.identity);
        GameObject particle = particleToInstantiate;
        if (Physics.Raycast(ray, out hitObj, shootRange))
        {
            Vector3 desitnation = elementToShoot.transform.position - hitObj.point;
            //Quaternion rotationDestination = Quaternion.LookRotation(-desitnation);
            elementToShoot.transform.localRotation = Quaternion.Lerp(elementToShoot.transform.rotation, cam.transform.rotation, 1);
            RamDepletion(ramAmt);
            enemyHealth = hitObj.collider.GetComponentInParent<EnemyHealthAndDeathManager>(); // getting script from the object hit
            enemyMonster = hitObj.collider.GetComponent<BaseAI>();
            if (enemyHealth != null) // checking to make sure the hit object is an enemy type with script "EnemyHealthAndDamageManager" attached
            {
                ElementDamageManager(power, enemyHealth); // if "EnemyHealthAndDamageManager" exists, then pass in the element
                RamDepletion(ramAmt);
            }
            else
            {
                RamDepletion(ramAmt);
                StartCoroutine(InstantiateParticle(particle, elementToShoot));
            }
        }
        else
        {
            //print("Did not hit");
            var position = ray.GetPoint(shootRange);
            Vector3 destintion = elementToShoot.transform.position - position;
            //Quaternion rotationDestination = Quaternion.LookRotation(-destintion);
            elementToShoot.transform.localRotation = Quaternion.Lerp(elementToShoot.transform.rotation, cam.transform.rotation, 1);
            RamDepletion(ramAmt);
            StartCoroutine(InstantiateParticle(particle, elementToShoot));
        }
    } 

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
            // continously updating the position of the laser to the wand
            lightingLeftLine.transform.position = wandE.transform.position;
        }
        else if (updateRightLightBool == true)
        {
            // continously updating the position of the laser to the wand
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
        fireCopy = Instantiate(fireProjectile, wandE.transform.position, cam.transform.rotation);
        //fireCopy.GetComponent<FireBall>().magicSystem = this;
        if (fireCopy != null)
        {
            RamDepletion(fireRamDepletion);
            Rigidbody rbFireBall;
            rbFireBall = fireCopy.GetComponent<Rigidbody>();
            rbFireBall.AddForce((cam.transform.forward + new Vector3(0, fireBallArch, 0)).normalized * fireBallForce, ForceMode.VelocityChange);
            Vector3 v = rbFireBall.velocity;
            v += playerMovement.playerVelocity * playerInfluenceOnFireBall;
            rbFireBall.velocity = v;
        }
    } 
}
