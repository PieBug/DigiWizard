using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovementScript : MonoBehaviour
{
    public float x1 = 0;
    public float y1 = 0;
    public float x2 = 0;
    public float y2 = 0;
    public float z1 = 0;
    public float z2 = 0;

    private Vector3 pos1 = new Vector3(0, 0, 0);
    private Vector3 pos2 = new Vector3(0, 0, 0);
    public float speed = 1.0f;

    private void Start()
    {
        pos1 = new Vector3(x1, y1, z1);
        pos1 = new Vector3(x2, y2, z2);
    }

    void Update()
    {
        transform.position = Vector3.Lerp(pos1, pos2, (Mathf.Sin(speed * Time.time) + 1.0f) / 2.0f);
    }
}
