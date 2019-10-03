using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Collider collider;
    public GameObject pickup;
    public ParticleSystem particles;
    public AudioSource audioSource;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player = PlayerController.singleton;
        if (other.gameObject == player.gameObject && CanObtain(player))
        {
            Obtain(player);
        }
    }

    public virtual bool CanObtain(PlayerController player)
    {
        return true;
    }

    public virtual void Obtain(PlayerController player)
    {
        collider.enabled = false;
        Destroy(pickup);
        Destroy(gameObject, 10f);
        particles.Play();
        audioSource.Play();
    }
}
