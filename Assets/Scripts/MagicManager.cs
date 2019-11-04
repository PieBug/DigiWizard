using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicManager : MonoBehaviour{
    // GameObjects //
    public GameObject fireBall;
    public GameObject iceBall;
    public GameObject lightingBall;

    // Wands // 
    public Transform Lwand; 
    public Transform Rwand;
    string RightHandElement;
    string LeftHandElement;

    // Fire Elements //
    public int forwardBounce = 2;
    public float bounceHeight = 0.2f;

    // Ice Elements //
    public float shootRange = 10f;

    // Variable for cam // 
    public Camera cam;


    // Start is called before the first frame update
    void Start()
    {
        RightHandElement = "lighting";
        LeftHandElement = "fire";
        cam = GetComponentInChildren<Camera>();
    } // End Start

    // Update is called once per frame
    void Update()
    {
         // Left Mouse //
        if ((Input.GetButtonDown("Fire1") && !Input.GetButtonDown("Fire2")))
        {
            //FireShoot(Lwand);
            ShootElement(Rwand, lightingBall);
        }
        // Right Mouse //
        else if ((Input.GetButtonDown("Fire2") && !Input.GetButtonDown("Fire1")))
        {
            ShootElement(Rwand, iceBall);
        }
        // Both Right and Left mouse // 
        if ((Input.GetButton("Fire1") && Input.GetButton("Fire2")))
        {
            print("pressed both!!!");
        }
        // E is pressed //
        if (Input.GetKeyDown(KeyCode.Q))
        {
            switchElementInHand("Q");
        }
        // Q is pressed //
        if (Input.GetKeyDown(KeyCode.E))
        {
            switchElementInHand("E");
        }
    } // End Update //
    private void FireShoot(Transform wandE)
    {
        GameObject fireCopy = Instantiate(fireBall, wandE.transform.position, Quaternion.identity);
        if (fireCopy != null)
        {

            //ballCopy.GetComponent<Rigidbody>().AddForceAtPosition(fpsCam.transform.forward * 350, wand.transform.position);
            fireCopy.GetComponent<Rigidbody>().AddForceAtPosition(cam.transform.forward * 200, wandE.transform.position);

            //ballCopy.GetComponent<Rigidbody>().velocity = transform.up * bounceHeight;
            fireCopy.GetComponent<Rigidbody>().AddForce(0, 5, 0, ForceMode.VelocityChange);
        }
    } // End FireShoot
    // Functions to call
    /**
    
    private void IceShoot(Transform wandE)
    {
        Vector3 camShootingPoint = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        iceCopy = Instantiate(iceBall, wandE.transform.position, Quaternion.identity);
        
        if (iceCopy != null)
        {
            start = true;
            //iceCopy.GetComponent<Rigidbody>().AddForceAtPosition(fpsCam.transform.forward * 200, wandE.transform.position);
            //iceCopy.GetComponent<Rigidbody>().AddForce(fpsCam.transform.forward * 200, ForceMode.Acceleration);
        }
        else
        {
            start = false;
        }
     
    } //  End IceShoot
    
    private void LightingShoot(Transform wandE, Vector3 hitObj)
    {
        InstantiateLighting(wandE, hitObj);
        StartCoroutine(waitFor(0.5f));
        InstantiateLighting(wandE, hitObj);
        StartCoroutine(waitFor(0.5f));
        InstantiateLighting(wandE, hitObj);
    }

    private void InstantiateLighting(Transform wandE, Vector3 HitObj)
    {
        lightingCopy = Instantiate(lightingBall, wandE.transform.position, Quaternion.identity);
    }
    private IEnumerator waitFor(float num)
    {
        yield return new WaitForSeconds(num);  
    }
    **/
    public void ShootElement(Transform wandPosition, GameObject Element )
    {
        Transform wand = wandPosition;
        Quaternion BulletRotation = Quaternion.LookRotation(cam.transform.forward);
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitObj;

        GameObject elementToShoot = Instantiate(Element, wand.transform.position, Quaternion.identity);

        if (Physics.Raycast(ray, out hitObj, shootRange))
        {
            Vector3 desitnation = elementToShoot.transform.position - hitObj.point;
            Quaternion rotationDestination = Quaternion.LookRotation(-desitnation);

            elementToShoot.transform.localRotation = Quaternion.Lerp(elementToShoot.transform.rotation, rotationDestination, 1);
        }
        else
        {
            var position = ray.GetPoint(shootRange);
            Vector3 destintion = elementToShoot.transform.position - position;
            Quaternion rotationDestination = Quaternion.LookRotation(-destintion);
            elementToShoot.transform.localRotation = Quaternion.Lerp(elementToShoot.transform.rotation, rotationDestination, 1);
        }
    } // End shootElement

    // SWitching out elements in hand for when Q or E is pressed.
    private void switchElementInHand(string key)
    {
        if (key == "E")
        {
            if ((RightHandElement == "fire" && LeftHandElement == "ice") || (LeftHandElement == "fire" && RightHandElement == "ice"))
            {
                RightHandElement = "lighting";
                print ("E This is right hand:" + RightHandElement);
            }
            else if ((RightHandElement == "lighting" && LeftHandElement == "ice") || (LeftHandElement == "lighting" && RightHandElement == "ice"))
            {
                RightHandElement = "fire";
                print("E This is right hand:" + RightHandElement);
            }
            else if ((RightHandElement == "fire" && LeftHandElement == "lighting") || (LeftHandElement == "fire" && RightHandElement == "lighting"))
            {
                RightHandElement = "ice";
                print("E This is right hand:" + RightHandElement);
            }
        }
        else if (key == "Q")
        {
            if ((RightHandElement == "fire" && LeftHandElement == "ice") || (LeftHandElement == "fire" && RightHandElement == "ice"))
            {
                LeftHandElement = "lighting";
                print("Q This is left hand: " + LeftHandElement);
            }
            else if ((RightHandElement == "lighting" && LeftHandElement == "ice") || (LeftHandElement == "lighting" && RightHandElement == "ice"))
            {
                LeftHandElement = "fire";
                print("Q This is left hand: " + LeftHandElement);
            }
            else if ((RightHandElement == "fire" && LeftHandElement == "lighting") || (LeftHandElement == "fire" && RightHandElement == "lighting"))
            {
                LeftHandElement = "ice";
                print("Q This is left hand: " + LeftHandElement);
            }
        }
    } // End Switch Elements

} // END SCRIPT //

