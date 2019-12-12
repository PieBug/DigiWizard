using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigiWizard_Bulb : LightningReaction
{

    [SerializeField]
    GameObject platform;

    bool isActivated = false;
    bool running = false;
    public Vector3 platformSpawnPoint1 = new Vector3(0, 0, 0);
    public Vector3 platformSpawnPoint2 = new Vector3(0, 0, 0);

    public override void React()
    {
        if (isActivated == false)
        {
            if (running == false)
            {
                isActivated = true; //Script is Activated
                platform.transform.position = platformSpawnPoint2; //Move the pre selected platform to new coordinates 
                StartCoroutine(TriggerWait());
            }

        }
        else
        {
            if (running == false)
            {
                isActivated = false; //Script is Deactivated
                platform.transform.position = platformSpawnPoint1; //Move the pre selected platform to new coordinates 
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

