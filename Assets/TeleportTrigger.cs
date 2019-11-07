using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    PlayerController player;
    public Transform teleportationDestination;
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerController.singleton;
    }

    private void OnTriggerEnter(Collider other)
    {
        player.transform.position = teleportationDestination.position;
        player.transform.rotation = teleportationDestination.rotation;
    }
}
