using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExplosion : MonoBehaviour
{
    float counter;
    float time = 0.6f;
    // Start is called before the first frame update
    void Start()
    {
        counter = time;
        
    }

    // Update is called once per frame
    void Update()
    {
        counter -= Time.deltaTime;
        if (counter <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
