﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMagic : MonoBehaviour {
    public float speed = 20;
    public float firerate;
    public int shootRange = 80;
    public MagicManager csp;
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /**
        cam = csp.returnCam();
        Vector3 camShootingPoint = cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        if (speed != 0)
        {
            RaycastHit hitObject;
            transform.position += transform.forward * (speed * Time.deltaTime);
            if (Physics.Raycast(camShootingPoint, cam.transform.forward, out hitObject, shootRange))
            {
                transform.localRotation = Quaternion.Lerp(transform.localRotation, hitObject.transform.rotation, Time.time * speed);
            }
                
        }
        else
        {
            Debug.Log("No speed given for fire projectile.");
        }
    **/
    }
}
