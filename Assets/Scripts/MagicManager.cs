using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicManager : MonoBehaviour{
    //Variables//
    public GameObject fireBall;
    public GameObject iceBall;
    public GameObject lightingBall;

    private GameObject fireCopy;
    private GameObject iceCopy;
    private GameObject lightingCopy;

    // Variables to obtain the wands & laserline // 
    public GameObject LeftWand;
    public GameObject RightWand;
    public Transform LwandEnd; 
    public Transform RwandEnd;

    // Fire Elements //
    public int forwardBounce = 2;
    public float bounceHeight = 0.2f;

    // Ice Elements //
    public float shootRange = 10f;

    // Variable for cam // 
    public Camera fpsCam;

    // MISC. Variables //
    RaycastHit hitObject;
    private bool start = false;

    // Start is called before the first frame update
    void Start()
    {
        LeftWand = GameObject.Find("LeftWand");
        RightWand = GameObject.Find("RightWand");
        fpsCam = GetComponentInChildren<Camera>();
    } // End Start

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Transform wandE = LwandEnd;
            FireShoot(wandE);
        }
        if (Input.GetButtonDown("Fire2"))
        {
            print("Right Wand");
            Transform wandE = RwandEnd;
            IceShoot(wandE);
        }
    } // End Update //



    // Functions to call
    private void FireShoot(Transform wandE)
    {
        fireCopy = Instantiate(fireBall, wandE.transform.position, Quaternion.identity);
        if (fireCopy != null)
        {

            //ballCopy.GetComponent<Rigidbody>().AddForceAtPosition(fpsCam.transform.forward * 350, wand.transform.position);
            start = true;
            fireCopy.GetComponent<Rigidbody>().AddForceAtPosition(fpsCam.transform.forward * 200, wandE.transform.position);

            //ballCopy.GetComponent<Rigidbody>().velocity = transform.up * bounceHeight;
            fireCopy.GetComponent<Rigidbody>().AddForce(0, 5, 0, ForceMode.VelocityChange);
        }
        else
        {
            start = false;
        }
    }
    private void IceShoot(Transform wandE)
    {
        Vector3 camShootingPoint = fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hitObject;
        iceCopy = Instantiate(iceBall, wandE.transform.position, Quaternion.identity);
        
        if (iceCopy != null & (Physics.Raycast(camShootingPoint, fpsCam.transform.forward, out hitObject, shootRange)))
        {
            start = true;
            iceCopy.GetComponent<Rigidbody>().AddForceAtPosition(fpsCam.transform.forward * 10, wandE.transform.position);
            iceCopy.transform.localRotation = hitObject.transform.rotation;
            iceCopy.transform.position += iceCopy.transform.forward * (30 * Time.deltaTime);
        }
        else
        {
            start = false;
        }

    }

} // END SCRIPT //

