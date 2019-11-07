using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtScript : MonoBehaviour
{
    public GameObject target;

    void Start()
    {
        //target = GameObject.Find("PlayerCharacter");
    }
    void Update()
    {
        // Rotate the camera every frame so it keeps looking at the target
        transform.LookAt(target.transform);

        // Same as above, but setting the worldUp parameter to Vector3.left in this example turns the camera on its side
        transform.LookAt(target.transform, Vector3.left);
    }
}
