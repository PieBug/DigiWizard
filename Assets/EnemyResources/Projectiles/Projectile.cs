using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public int damage;
    public LayerMask hitMask;
    public Rigidbody rigidbody;

    public virtual void Fire(Vector3 direction)
    {

    }
    public virtual void Fire(Quaternion direction)
    {

    }

    protected virtual void Hit(GameObject hit)
    {
        PlayerHealthAndDeathManager healthManager = hit.gameObject.GetComponent<PlayerHealthAndDeathManager>();
        if (healthManager != null)
        {
            healthManager.DamagePlayer(damage);
        }
        Destroy(gameObject);
    }
}
