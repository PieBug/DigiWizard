using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicSystem : MonoBehaviour {
    // Start is called before the first frame update
    public GameObject LeftWand;
    public GameObject RightWand;
    float buttonPressedTime = 10.0f;

    // Shooting Variables // 
    public int wandDamage = 1;


    void Start()
    {
        LeftWand = GameObject.Find("LeftWand");
        RightWand = GameObject.Find("RightWand");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Input.GetButtonDown("Fire2") || Input.GetButtonDown("Fire2") && Input.GetButtonDown("Fire1"))
        {
            buttonPressedTime = Time.time;
            print("Both wands were pressed!");

        }

        if (Input.GetButtonDown("Fire1") && !Input.GetButtonDown("Fire2"))
        {
            buttonPressedTime = Time.time;
            print(LeftWand.name);
            //shootMagic();
        }

        if (Input.GetButtonDown("Fire2") && !Input.GetButtonDown("Fire1"))
        {
            buttonPressedTime = Time.time;
            print(RightWand.name);
        }


        /*
       void shootMagic(string Magic, int damage)
       {

       }

       */

    }
}
