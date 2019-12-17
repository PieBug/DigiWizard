using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigiWizard_Bulb : LightningReaction
{

    [SerializeField]
    GameObject platform1;

    bool isActivated = false;
    bool running = false;
    public Vector3 platformSpawn1Point1 = new Vector3(0, 0, 0);
    public Vector3 platformSpawn1Point2 = new Vector3(0, 0, 0);

    public override void React()
    {
        if (isActivated == false)
        {
            if (running == false)
            {
                isActivated = true; //Script is Activated
                platform1.transform.position = platformSpawn1Point2; //Move the pre selected platform to new coordinates 
                StartCoroutine(TriggerWait());
            }

        }
        else
        {
            if (running == false)
            {
                isActivated = false; //Script is Deactivated
                platform1.transform.position = platformSpawn1Point1; //Move the pre selected platform to new coordinates 
                StartCoroutine(TriggerWait());
            }
        }
    }

    IEnumerator TriggerWait()
    {
        running = true;
        yield return new WaitForSeconds(1f);
        running = false;
    }
}

