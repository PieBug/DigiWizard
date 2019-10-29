using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    private int colCounter;
    // Collision counter
    void OnCollisionEnter(Collision col)
    {
        
        if (col.gameObject.tag == "Ground" && colCounter != 5) // If collided gameObject does not have game tag "Ground"
        {
            colCounter++;
            print(colCounter);
        }
        if (colCounter == 3)
        {
           Destroy(gameObject);
        }
    }
}

