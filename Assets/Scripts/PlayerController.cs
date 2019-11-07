using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    // Variables //
    public static PlayerController singleton { get; private set; }
    public float walkSpeed = 10f;
    public float accelerationWalk = 1f;
    public float deaccelerationWalk = 1f;
    public float accelerationAir = 1f;
    public float deaccelerationAir = 1f;
    public float jumpSpeed = 10;
    public LayerMask solidLayerMask;
    public LayerMask groundMask;
    public Vector3 direction = Vector3.zero;
    public Vector3 playerVelocity = Vector3.zero;
    [HideInInspector]
    new public Rigidbody rigidbody;
    [HideInInspector]
    new public CapsuleCollider collider;
    public float zAxis;
    public float xAxis;

    private bool jumpPressed;
    private bool onGround;
    private int groundContacts;
    

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
        collider = GetComponent<CapsuleCollider>();
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
        zAxis = Input.GetAxis("Vertical") * walkSpeed; // Getting the z axis and multiplying by player's speed.
        xAxis = Input.GetAxis("Horizontal") * walkSpeed; // Getting the x axis and multiplying by player's speed.

        //zAxis *= Time.deltaTime; // Ensuring smooth transitioning per updated frame
        //xAxis *= Time.deltaTime; // Ensuring smooth transitioning per updated frame

        // Set the desired direction of the player accordingly to the updated z and x axis variables
        direction = transform.localRotation * new Vector3 (xAxis, 0 , zAxis);
        direction = direction.normalized;
        //Get player velocity with y component set to zero.
        Vector3 playerVelocityNoGravity = new Vector3(playerVelocity.x, 0, playerVelocity.z);
        //Determine whether or not to accelerate
        bool accelerate = Vector3.Dot(direction,playerVelocityNoGravity) > 0.8f;
        //Declare and set the target speed
        Vector3 targetVelocity;
        targetVelocity = direction * walkSpeed;

        //Calculate Velocity
        float acceleration = onGround  ? accelerationWalk : accelerationAir;
        float deacceleration = onGround ? deaccelerationWalk : deaccelerationAir;
        playerVelocity = Vector3.Lerp(playerVelocity, targetVelocity, (accelerate ? acceleration : deacceleration) * Time.deltaTime);

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

        //transform.Translate(playerTransformation);
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
        //Check for approaching collision
        RaycastHit hit_info;
        while (Physics.CapsuleCast(transform.position + collider.center + (Vector3.up * collider.height * 0.15f),
                transform.position + collider.center + (Vector3.up * collider.height * -0.15f), collider.radius,
                playerVelocity.normalized, out hit_info, playerVelocity.magnitude,
                solidLayerMask))
        {
            //If normal has a significant y component, break the while loop
            if (hit_info.normal.y > 0.5f /*|| (!hit_info.transform.gameObject.isStatic && onGround)*/)
            {
                break;
            }
            Vector3 hit_normal_perpendicular = Quaternion.AngleAxis(90f, Vector3.up) * hit_info.normal;
            hit_normal_perpendicular *= (Vector3.Angle(hit_normal_perpendicular, playerVelocity.normalized) > 90f) ? (-1) : (1);


            //Calculate clamp
            Vector3 hitplayerVelocity = (hit_info.point - transform.position);
            hitplayerVelocity.y = 0;
            hitplayerVelocity = hitplayerVelocity.normalized;
            float clamp = Vector3.Dot(hitplayerVelocity, playerVelocity.normalized);
            clamp = (1f - Mathf.Abs(clamp)) * Mathf.Sign(clamp);
            clamp = Mathf.Clamp(clamp * 1.2f, -1f, 1f);

            playerVelocity = hit_normal_perpendicular * Mathf.Sign(clamp) * playerVelocity.magnitude * clamp;
            playerVelocity.y = 0;
        }
        //Translate the player
        rigidbody.MovePosition(rigidbody.position + (playerVelocity));
    }
}
