using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private static Checkpoint currentCheckpoint;
    [HideInInspector]
    public GameObject trigger;
    [HideInInspector]
    public GameObject respawn;
    [HideInInspector]
    public GameObject player;
    [Tooltip("This helps the pointer determine where to point to next")]
    public Checkpoint nextCheckpoint;
    public UnityEvent onCheckpoint;

    private void Start()
    {
        if (!Application.isEditor)
        {
            trigger.GetComponent<MeshRenderer>().enabled = false;
            foreach(MeshRenderer renderer in respawn.GetComponentsInChildren<MeshRenderer>()){
                renderer.enabled = false;
            }  
        }    
    }

    public void SetCheckpoint()
    {
        onCheckpoint.Invoke();
        currentCheckpoint = this;
    }

    public static void Respawn()
    {
        Instantiate(currentCheckpoint.player, currentCheckpoint.respawn.transform.position, currentCheckpoint.respawn.transform.rotation);
        PlayerController.singleton.gameObject.GetComponentInChildren<CharacterCamera>().Look(currentCheckpoint.respawn.transform.eulerAngles);
    }

    public static Vector3 GetGoal()
    {
        if (currentCheckpoint?.nextCheckpoint)
        {
            return currentCheckpoint.nextCheckpoint.transform.position;
        }
        else
        {
            return Vector3.zero;
        }
        
    }
}
