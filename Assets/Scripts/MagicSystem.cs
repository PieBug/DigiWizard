using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicSystem : MonoBehaviour
{
    //Input Variables//
    bool updateRightLine = false;
    bool updateLeftLine = false;
    bool updateMidLine = false;
    bool activateRamLine = false;

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

    // Combo Magics //
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
    public WaitForSeconds ramDrainSpeed = new WaitForSeconds(0.7f);
    public int lightningRamDepletion;
    public LineRenderer RlaserLine;
    public LineRenderer LlaserLine; 
    public LineRenderer MlaserLine; 
    private WaitForSeconds rayDuration = new WaitForSeconds(.07f);

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

    int regenCounter = 1;
    bool regenRam = false;

    //------------------------------

    // RAM System //
    //public bool cancelPenalty = false;
    //public bool penaltyRunning;
    public Slider ramSlider;
    public int ramAmount = 100;
    public float nextRamFire;
    public float regenWait;
    public float ramFireRate = 3.0f;
    private WaitForSeconds ramDuration = new WaitForSeconds(7.0f);
    //bool IsRamPenalty = false;
    private IEnumerator ramPenaltyCoroutine;
    private IEnumerator ramLineRendererCoroutine;

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

    float buttonPressedTime = 0f;
    public float waitTime = 0.1f;
    bool casting = false;
    bool que1;
    bool que2;
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
        print(ramAmount);
        // Buffer for holding down buttons
        bool fire1 = Input.GetMouseButtonDown(0);
        bool fire2 = Input.GetMouseButtonDown(1);
        que1 = que1 || fire1;
        que2 = que2 || fire2;
        if (fire1 || fire2 && casting == false)
        {
            casting = true;
        }

        if (casting == true)
        {
            buttonPressedTime += Time.deltaTime;
            if (buttonPressedTime > waitTime)
            {
                // COMBO POWERS
                // BOTH BUTTONS //
                if (que1 && que2 && Time.time > nextFire && ramAmount != 0 && ramAmount > 0)
                {
                    //waitTime += Time.deltaTime;
                    //print(waitTime);
                    nextFire = Time.time + fireRate; // Making sure player does not constantly fire

                    if ((Lelement == "fire" && Relement == "ice") || (Lelement == "ice" && Relement == "fire"))
                    {
                        ShootElement(MidPosition, ComboIceFire, "fireice", fireiceRamDepletion, redParticle); // combo ram need to re-do
                    }
                    else if ((Lelement == "ice" && Relement == "lighting") || (Lelement == "lighting" && Relement == "ice"))
                    {
                        if (ramAmount >= icelightRamDepletion)
                        {
                            ComboLightIce.SetActive(true);
                            ShootElement(MidPosition, iceProjectile, "icelight", icelightRamDepletion, blueParticle); // combo ram need to re-do
                        }
                    }
                    else if ((Lelement == "fire" && Relement == "lighting") || (Lelement == "lighting" && Relement == "fire"))
                    {
                        if (ramAmount >= lightfireRamDepletion)
                        {
                            updateMidLine = true;
                            activateRamLine = true;
                        }
                        else
                        {
                            print("less than ram amount");
                        }
                    }
                    
                } // End Both Button
                // LEFT BUTTON // 
                if (que1 && Time.time > nextFire && ramAmount != 0 && ramAmount > 0)
                {
                    //waitTime += Time.time;
                    //print(waitTime);
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
                            if (ramAmount >= lightningRamDepletion)
                            {
                                updateLeftLine = true;
                                activateRamLine = true;
                            }
                            else
                            {
                                print("less than ram amount");
                            }
                            break;
                    }
                    
                } // END Left Mouse Button Down

                // RIGHT BUTTON // 
                if (que2 && Time.time > nextFire && ramAmount != 0 && ramAmount > 0)
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
                            if (ramAmount >= lightningRamDepletion)
                            {
                                updateRightLine = true;
                                activateRamLine = true;
                            }
                            else
                            {
                                print("less than ram amount");
                            }
                            break;
                    }
                    
                } // END Right Mouse Button Down
                que1 = que2 = casting = false;
                buttonPressedTime = 0f;
            }
            
        } // end casting time
        if ((Input.anyKey == false) || !(Input.GetMouseButton(0)) && !(Input.GetMouseButton(1)) || (Input.GetMouseButtonUp(1)) && (Input.GetMouseButtonUp(0)))
        {
            updateLeftLine = false;
            activateRamLine = false;
            LlaserLine.enabled = false;
            updateRightLine = false;
            RlaserLine.enabled = false;
            ComboLightIce.SetActive(false);
            updateMidLine = false;
            MlaserLine.enabled = false;
        }

        // Ram regeneration system //
        if (ramAmount <= 0 && (Time.time > regenWait) && playerMovement.isPlayerMoving == true || regenRam == true && (Time.time > regenWait) && playerMovement.isPlayerMoving)
        { // && regenRam == true
            //int regenCounter = 1;
            print (regenCounter);
            regenCounter *=2;
            regenWait = Time.time + 1;
            if (!(ramAmount >= 100))
            {
                ramAmount += regenCounter;
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
                regenCounter = 1;
                playerMovement.isPlayerMoving = false;
                //regenRam = false;
            }
        }

        // Ram hitting zero //
        if (ramAmount <= 0)
        {
            updateLeftLine = false;
            updateRightLine = false;
            updateMidLine = false;
            activateRamLine = false;
            MlaserLine.enabled = false;
            RlaserLine.enabled = false;
            LlaserLine.enabled = false;
            //-------------------------
            //regenRam = true;
        }

        // making sure player always have enough ram to feel strong
        if (ramAmount < 5)
        {
            regenRam = true;
        }
        else if (ramAmount == 100)
        {
            //regenCounter = 1;
            regenRam = false;
        }

    } // end UPDATE

    // Fixed Update //
    private void FixedUpdate()
    {
        if (updateLeftLine == true)
        {
            ActivateLineRenderer(Lwand, "lighting", lightingDMG, LlaserLine);
            if (activateRamLine == true)
            {
                RamDepletion(lightningRamDepletion); // Starting lightning RamDepletion
                StartLineCoroutine();
            }
        }
        else if (updateRightLine == true)
        {
            ActivateLineRenderer(Rwand, "lighting", lightingDMG, RlaserLine);
            if (activateRamLine == true)
            {
                RamDepletion(lightningRamDepletion); // Starting lightning RamDepletion
                StartLineCoroutine();
            }
        }
        else if (updateMidLine == true)
        {
            ActivateLineRenderer(MidPosition, "lightfire", lightfireDMG, MlaserLine);
            if (activateRamLine == true)
            {
                RamDepletion(lightfireRamDepletion);
                StartLineCoroutine();
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
                // Switching HUD images
                L_Lightning.enabled = true;
                L_Fire.enabled = false;
                L_Ice.enabled = false;
            }
            else if ((Lelement == "lighting" && Relement == "ice") || (Lelement == "ice" && Relement == "lighting"))
            {
                Lelement = "fire";
                // Switching HUD images
                L_Lightning.enabled = false;
                L_Fire.enabled = true;
                L_Ice.enabled = false;
            }
            else if ((Lelement == "fire" && Relement == "lighting") || (Lelement == "lighting" && Relement == "fire"))
            {
                Lelement = "ice";
                // Switching HUD images
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
    } // end FIXED UPDATE

    // Element Damage Manager //
    public void ElementDamageManager(string element, EnemyHealthAndDeathManager enemyH, float modifier = 1f)
    {
        if (element == "fire" && enemyH != null)
        {
            enemyH.DamageEnemy((int)(fireDMG * modifier));
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
            enemyH.DamageEnemy((int)(fireiceDMG * modifier));
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
        if (!(ramAmount <= 0) && ramAmount >= damageAmt)
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

    // Particle Coroutine //
    private IEnumerator InstantiateParticle(GameObject particle, GameObject elementObj)
    {
        
        if (elementObj != null)
        {
            //Destroy(elementObj, 0.3f);
            yield return new WaitForSeconds(0.6f);
            Instantiate(particle, elementObj.transform.position, Quaternion.identity);
            Destroy(elementObj);
        }
    }

    // Line Renderer Ram Coroutine //
    private void StartLineCoroutine()
    {
        ramLineRendererCoroutine = LineRamDeplete();
        StartCoroutine(ramLineRendererCoroutine);
    }
    private void StopLineCoroutine()
    {
        activateRamLine = true;
        StopCoroutine(ramLineRendererCoroutine);
    }
    private IEnumerator LineRamDeplete()
    {
        activateRamLine = false;
        yield return ramDrainSpeed;
        StopLineCoroutine();
    }

    // Freezing AI //
    private void FreezeAIEnemy(BaseAI ai)
    {
        if (ai != null)
        {
            ai.IceAI(0.5f, 0.5f); // Icing the AI w/ Ice power and Freezing it
            StartCoroutine(EnemyFreezeCoroutine());
        }
    }
    private IEnumerator EnemyFreezeCoroutine()
    {
        //print("Enemy Coroutine start");
        yield return new WaitForSeconds(10);
        //print("Enemy Coroutine is over");
    }

    // Element Magic Shooting //
    public void ShootElement(Transform wandPosition, GameObject Element, string power, int ramAmt, GameObject particleToInstantiate)
    {
        if (ramAmount >= ramAmt)
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
                    Destroy(elementToShoot, 0.3f); //0.3
                    Instantiate(particle, hitObj.point, Quaternion.identity);
                }
                else
                {
                    RamDepletion(ramAmt);
                    Destroy(elementToShoot, 0.3f); //0.3
                    Instantiate(particle, hitObj.point, Quaternion.identity);
                }
            }
            else
            {
                //print("Did not hit");
                var position = ray.GetPoint(shootRange);
                //Vector3 position = ray.GetPoint(shootRange);
                Vector3 destintion = elementToShoot.transform.position - position;
                //Quaternion rotationDestination = Quaternion.LookRotation(-destintion);
                elementToShoot.transform.localRotation = Quaternion.Lerp(elementToShoot.transform.rotation, cam.transform.rotation, 1);
                RamDepletion(ramAmt);
                StartCoroutine(InstantiateParticle(particleToInstantiate, elementToShoot));
            }
        }
    }

    private void ActivateLineRenderer(Transform wand, string element, int damage, LineRenderer line)
    {
        LinerRendererShoot(wand, element, damage, line);
    }

    // Coroutine ShotEffect()
    private IEnumerator ShotEffect(LineRenderer laserLine)
    {
        laserLine.enabled = true; // When shot, laserline is enabled and coroutine is waiting for .07 seconds until it enables the laser from game view
        yield return rayDuration;
        laserLine.enabled = false;
    }

    // FireBall Projectile //
    private void FireShoot(Transform wandE)
    {
        if (ramAmount >= fireRamDepletion)
        {
            GameObject fireCopy;
            fireCopy = Instantiate(fireProjectile, wandE.transform.position, cam.transform.rotation);
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

    // Lightning Line Renderer
    private void LinerRendererShoot(Transform wand, string element, int RamDepleteAmt, LineRenderer Line)
    {
        Line.enabled = true;
        Vector3 camShootingPoint = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0)); // Aiming point of the ray -> will be set to the middle position of the fps camera. Takes position of the camera and converts it to world space. 

        RaycastHit hitObject; // Object that is hit with our ray; object must have a collider on
        Line.SetPosition(0, wand.position); // starting position of the laserline is set to current position of the tip of the wand where the ray will shoot from
        GameObject particle;
        if (Physics.Raycast(camShootingPoint, cam.transform.forward, out hitObject, shootRange)) // Raycast is used to determine where the end of the ray will be, and deals force/damage to the object hit. Physics Raycast returns a bool. [camShootingPoin:] point in the world space where ray will begin [fpsCam:] Direction of the ray [Out - keyword:] Allows us to store information from a function + it's return type of the object hit. ex: Information like Rigidbody, collider, & surfacenormal of object hit. [shootRange:] How far ray goes.
        {
            Line.SetPosition(1, hitObject.point); // if raycast returns true and an object is hit, we're setting the 2nd position of the laser line to that object point
            particle = Instantiate(yellowParticle, hitObject.point, Quaternion.identity);
            Destroy(particle, 0.07f);

            enemyHealth = hitObject.collider.GetComponentInParent<EnemyHealthAndDeathManager>();  // getting script from the object hit
            enemyMonster = hitObject.collider.GetComponent<BaseAI>();

            if (enemyHealth != null || enemyMonster != null) // checking to make sure the hit object is an enemy type with script "EnemyHealthAndDamageManager" attached
            {
                ElementDamageManager(element, enemyHealth); // if "EnemyHealthAndDamageManager" exists, then pass in the element
                particle = Instantiate(yellowParticle, hitObject.point, Quaternion.identity);
                Destroy(particle, 0.07f);
            }
            else
            {
                // nothing happens
            }

        }
        else // Raycast returns false
        {
            // nothing was hit
            Line.SetPosition(1, (camShootingPoint + (cam.transform.forward * shootRange))); // if nothing is hit, then the ray will just shoot 50 units away from the camera

            // Bullet Cloning //
            particle = Instantiate(yellowParticle, (camShootingPoint + (cam.transform.forward * shootRange)), Quaternion.identity);
            Destroy(particle, 0.75f);
        }
    }
}
