using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public int damage;
    public Rigidbody rigidbody;

    public virtual void Fire(Vector3 direction)
    {

    }
    public virtual void Fire(Quaternion direction)
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        PlayerHealthAndDeathManager healthManager = collision.gameObject.GetComponent<PlayerHealthAndDeathManager>();
        if (healthManager != null)
        {
            healthManager.DamagePlayer(damage);
        }
        Destroy(gameObject);
    }
}
