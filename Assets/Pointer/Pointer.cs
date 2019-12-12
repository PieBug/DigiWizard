using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    private Quaternion targetDirection;

    // Update is called once per frame
    void Update()
    {
        targetDirection = Quaternion.LookRotation((PlayerController.singleton.transform.position - Checkpoint.GetGoal()).normalized, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetDirection, 0.1f);
        //Debug.Log(targetDirection);
    }
}
