using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Collider collider;
    public GameObject pickup;
    public ParticleSystem particles;
    public AudioSource audioSource;

    public void MakeTemporary(float lifespan)
    {
        StartCoroutine(Blink(lifespan));
    }

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

    protected IEnumerator Blink(float lifespan)
    {
        yield return new WaitForSeconds(lifespan);
        //Start to despawn
        bool on = true;
        for (int i = 0; i < 100; ++i)
        {
            on = !on;
            if (pickup)
            {
                pickup.SetActive(on);
            }
            else
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.01f);
        }
        //Despawn
        Destroy(gameObject);
    }
}
