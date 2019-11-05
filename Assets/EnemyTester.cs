using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTester : MonoBehaviour
{
    public Vector2 count;
    public Vector2 space;
    public GameObject enemyToTest;
    public GameObject obstacle;
    // Start is called before the first frame update
    void Start()
    {
        for(var i = 0; i < count.x; ++i)
        {
            for(var j = 0; j < count.y; ++j)
            {
                if((i + j) % 2 == 0)
                {
                    GameObject obj = Instantiate(obstacle, new Vector3(i * space.x, 0, j * space.y), Quaternion.identity);
                    obj.transform.localScale *= (Random.value * 0.9f) + 0.1f;
                }
                else
                {
                    Instantiate(enemyToTest, new Vector3(i * space.x, 0, j * space.y), Quaternion.identity);
                }
            }
        }
    }
}
