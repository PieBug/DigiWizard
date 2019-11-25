using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellProjectile : MonoBehaviour {
    public float speed;
    public GameObject explosion;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (speed != 0)
        {
            transform.position += transform.forward * (speed * Time.deltaTime);
        }
        else
        {
            Debug.Log("No speed given for fire projectile.");
        }
    }

    /**
    void OnCollisionEnter(Collision collision)
    {
        EnemyHealthAndDeathManager enemyHealth = collision.gameObject.GetComponentInParent<EnemyHealthAndDeathManager>();
        if (enemyHealth != null)
        {
            Instantiate(explosion, transform.position, transform.rotation);
            Destroy(gameObject);
            //print("success in spell projectile");
        }
    }
    **/
}
