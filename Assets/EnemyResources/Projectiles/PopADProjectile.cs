using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopADProjectile : Projectile
{
    public override void Fire(Quaternion direction)
    {
        transform.rotation = direction;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position + (speed * transform.forward) * Time.deltaTime;
        RaycastHit raycastHit;
        bool hit = Physics.Raycast(transform.position, transform.forward, out raycastHit, speed * Time.deltaTime, hitMask, QueryTriggerInteraction.Ignore);
        if (hit)
        {
            Hit(raycastHit.collider.gameObject);
        }
    }
    /**
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject == PlayerController.singleton.gameObject)
        {
            Hit(PlayerController.singleton.gameObject);
        }
    }
    **/
}
