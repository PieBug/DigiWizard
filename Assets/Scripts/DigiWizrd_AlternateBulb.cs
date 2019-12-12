using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigiWizrd_AlternateBulb : LightningReaction
{

    [SerializeField]
    GameObject platform;

    bool isActivated = false;
    bool running = false;
    public Vector3 platformSpawnPoint = new Vector3(0, 0, 0);

    public override void React()
    {
        Debug.Log("Triggered");
        if (isActivated == false)
        {
            if (running == false)
            {
                isActivated = true; //Once Activated, script can not be run again.
                platform.GetComponent<ObjectMovementToggleScript>().move = true; //Alow the selected platform to run it's Movement script, which moves it back and forth in a pendulum motion.
                StartCoroutine(TriggerWait());
            }

        }
        else
        {
            if (running == false)
            {
                isActivated = false; //Once Activated, script can not be run again.
                platform.GetComponent<ObjectMovementToggleScript>().move = false; //Alow the selected platform to run it's Movement script, which moves it back and forth in a pendulum motion.
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

