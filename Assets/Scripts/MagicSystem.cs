using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSystem : MonoBehaviour {
    // Start is called before the first frame update
    public GameObject LeftWand;
    public GameObject RightWand;
    //float buttonPressedTime = 10.0f;

    // Shooting Variables // 
    public int wandDamage = 1; // Amount of damage
    public float fireRate = .25f; // How often player can fire weapon
    public float shootRange = 50f; // How long rays are shot
    public float hitForce = 100f; // How much force hitting game objects with a rigidbody
    public Transform rayEnd; // This will mark the B position where the ray will hit (from point A to point B)
    private Camera fpsCam;
    private WaitForSeconds rayDuration = new WaitForSeconds(.07f); // How long ray will remain in game view
    private AudioSource bulletSound;
    private LineRenderer laserLine; // takes array of two points and draws a line between each one in the game view
    private float nextFire; // Holds time when player can fire again after firing

    void Start()
    {
        LeftWand = GameObject.Find("LeftWand");
        RightWand = GameObject.Find("RightWand");

        laserLine = GetComponent<LineRenderer>();
        fpsCam = GetComponentInChildren<Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire2") && Input.GetButtonDown("Fire1"))
        {
            //buttonPressedTime = Time.time;
            print("Both wands were pressed!");

        }

        if (Input.GetButtonDown("Fire1") && !Input.GetButtonDown("Fire2"))
        {
            //buttonPressedTime = Time.time;
            print(LeftWand.name);
        }

        if (Input.GetButtonDown("Fire2") && !Input.GetButtonDown("Fire1"))
        {
            //buttonPressedTime = Time.time;
            print(RightWand.name);
        }

    }
}
