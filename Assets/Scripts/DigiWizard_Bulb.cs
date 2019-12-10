using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigiWizard_Bulb : MonoBehaviour
{

    [SerializeField]
    GameObject platform;

    bool isActivated = false;
    public Vector3 platformSpawnPoint1 = new Vector3(0, 0, 0);
    public Vector3 platformSpawnPoint2 = new Vector3(0, 0, 0);

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag.Equals("Thunder")) //When object is hit by another object with the Thunder tag, play script
        {
            print("Target Hit Registered");

            if (isActivated == false)
            {
                isActivated = true; //Once Activated, script can not be run again.
                platform.transform.position = platformSpawnPoint1; //Move the pre selected platform to new coordinates 
            }

            if (isActivated == true)
            {
                isActivated = false; //Once Activated, script can not be run again.
                platform.transform.position = platformSpawnPoint2; //Move the pre selected platform to new coordinates 
            }
        }

    }
}

