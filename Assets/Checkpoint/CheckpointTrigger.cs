using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    private bool triggered = false;
    [HideInInspector]
    public Material triggeredMaterial;
    [HideInInspector]
    public Checkpoint checkpoint;

    private void OnTriggerEnter(Collider other)
    {
        if(!triggered && other.gameObject == PlayerController.singleton.gameObject)
        {
            triggered = true;
            checkpoint.SetCheckpoint();
            GetComponent<MeshRenderer>().material = triggeredMaterial;
        }
        
    }
}
