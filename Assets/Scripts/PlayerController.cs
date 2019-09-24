﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    // Variables //
    public static PlayerController singleton { get; private set; }
    public float walkSpeed = 10.0f;
    public float jumpSpeed = 10;
    private Vector3 playerTransformation = Vector3.zero;
    [HideInInspector]
    public Rigidbody rigidbody;

    private bool jumpPressed;
    private bool onGround;
    private int groundContacts;
    private int groundMask;

    private void Awake()
    {
        //Set singleton
        if (this != singleton && singleton == null)
        {
            singleton = this;
        }
        else
        {
            Destroy(this);
        }
    }
    // Locking and removing cursor from view on spawned. //
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rigidbody = GetComponent<Rigidbody>();
        groundMask = LayerMask.GetMask("Default");
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Check if there is a floor beneath player
        if (Physics.CheckSphere(transform.position + Vector3.down * 1.1f, 0.5f, groundMask))
        {
            onGround = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        //Check if there is not a floor beneath player
        if (!Physics.CheckSphere(transform.position + Vector3.down * 1.1f, 0.5f, groundMask))
        {
            onGround = false;
        }
    }

    // Update is called once per frame //
    void Update()
    {
        float zAxis = Input.GetAxis("Vertical") * walkSpeed; // Getting the z axis and multiplying by player's speed.
        float xAxis = Input.GetAxis("Horizontal") * walkSpeed; // Getting the x axis and multiplying by player's speed.

        zAxis *= Time.deltaTime; // Ensuring smooth transitioning per updated frame
        xAxis *= Time.deltaTime; // Ensuring smooth transitioning per updated frame

        // Move the transformation of the player accordingly to the updated z and x axis variables
        playerTransformation = new Vector3 (xAxis, 0 , zAxis);
        //transform.Translate(playerTransformation);



        // If player presses ESC, the player's cursor will enable. 
        if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None;
        }
        // Player Jumping
        if (Input.GetButtonDown("Jump") && onGround)
        {
            //Handle Jump in FixedUpdate()
            jumpPressed = true;
        }

        transform.Translate(playerTransformation);
    }

    private void FixedUpdate()
    {
        if (jumpPressed)
        {
            jumpPressed = false;
            onGround = false;
            Vector3 velocity = rigidbody.velocity;
            velocity.y = 0f;
            rigidbody.velocity = velocity;
            rigidbody.AddForce(Vector3.up * jumpSpeed, ForceMode.VelocityChange);
        }
        
    }
}
