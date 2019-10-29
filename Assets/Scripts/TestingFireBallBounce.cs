using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingFireBallBounce : MonoBehaviour{
    //Variables//
    public GameObject fireBall;
    public Transform wand;
    private GameObject ballCopy;
    public int forwardBounce = 2;
    public float bounceHeight = 0.2f;

    Vector3 camShootingPoint;
    private Camera fpsCam;

    private bool start = false;

    // Start is called before the first frame update
    void Start()
    {
        fpsCam = GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ballCopy = Instantiate(fireBall, wand.transform.position, Quaternion.identity);
            if (ballCopy != null)
            {
                
                //ballCopy.GetComponent<Rigidbody>().AddForceAtPosition(fpsCam.transform.forward * 350, wand.transform.position);
                start = true;
                // ballCopy.GetComponent<Rigidbody>().AddForceAtPosition(fpsCam.transform.forward * 100, wand.transform.position);

                //ballCopy.GetComponent<Rigidbody>().velocity = transform.up * bounceHeight;
                ballCopy.GetComponent<Rigidbody>().AddForce(0, 10, 0, ForceMode.VelocityChange);
            }
            else
            {
                start = false;
            }
            
        }
        if (start == true && ballCopy != null)
        {
            //ballCopy.GetComponent<Rigidbody>().AddForceAtPosition(fpsCam.transform.forward * forwardBounce, wand.transform.position);
            
        }
    }


}

