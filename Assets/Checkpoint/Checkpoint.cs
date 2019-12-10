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
    private Vector3 positon;
    public UnityEvent onCheckpoint;

    private void Start()
    {
        if (!Application.isEditor)
        {
            trigger.GetComponent<MeshRenderer>().enabled = false;
            respawn.GetComponent<MeshRenderer>().enabled = false;
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
}
