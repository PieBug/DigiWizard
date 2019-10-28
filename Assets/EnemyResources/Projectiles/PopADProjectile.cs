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
        rigidbody.MovePosition(transform.position + (speed * transform.forward) * Time.deltaTime);
    }
}
