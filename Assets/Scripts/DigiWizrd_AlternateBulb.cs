using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigiWizrd_AlternateBulb : MonoBehaviour
{

    [SerializeField]
    GameObject platform;

    bool isActivated = false;
    public Vector3 platformSpawnPoint = new Vector3(0, 0, 0);

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag.Equals("Thunder")) //When object is hit by another object with the Thunder tag, play script
        {
            if (isActivated == false)
            {
                isActivated = true; //Once Activated, script can not be run again.
                platform.GetComponent<ObjectMovementToggleScript>().move = true; //Alow the selected platform to run it's Movement script, which moves it back and forth in a pendulum motion.
            }

            if (isActivated == true)
            {
                isActivated = false; //Once Activated, script can not be run again.
                platform.GetComponent<ObjectMovementToggleScript>().move = false; //Alow the selected platform to run it's Movement script, which moves it back and forth in a pendulum motion.
            }
        }

    }
}

