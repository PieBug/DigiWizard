using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCamera : MonoBehaviour {
    // Variables //
    Vector2 mouseDirection; // Direction the mouse is pointing in
    Vector2 smoothV; // Vector to hold the smoothed mouse direction
    public float mouseSensitiveness = 5.0f;    
    public float mouseSmoothing = 2.0f;
    GameObject playerCharacter;


    // Start is called before the first frame update
    void Start()
    {
        playerCharacter = this.transform.parent.gameObject; // camera's parented gameObject aka Player -> needs to turn w/ cam direction
    }

    // Update is called once per frame
    void Update()
    {
        var mouseMovementDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y")); // Getting the mouse movement in the X and Y axis in delta time.
        mouseMovementDelta = Vector2.Scale(mouseMovementDelta, new Vector2(mouseSensitiveness * mouseSmoothing, mouseSensitiveness * mouseSmoothing)); // Multiplying the movement of the mouse with sentitivity and smoothing

        smoothV.x = Mathf.Lerp(smoothV.x, mouseMovementDelta.x, 1f / mouseSmoothing); // Lerp smoothV's x start position to mouseMovementDelta's x end position; 1f/2f = 0.5 mid point -> transitioning. Adds this into the x position of vector2 smoothV.
        smoothV.y = Mathf.Lerp(smoothV.y, mouseMovementDelta.y, 1f / mouseSmoothing); // Lerp from smoothV's y position, to mouseMovementDelta's y position; 1f/2f = 0.5 mid point -> transitioning. Adds this into the y position of vector2 smoothV.

        mouseDirection += smoothV; // mouseDirection inherits information from smoothV 

        
        transform.localRotation = Quaternion.AngleAxis(-mouseDirection.y, Vector3.right); // Local rotation of the camera -> mouse Y axis movement, rotating along x-rotation-axis = up and down. If mouseDirection.y is not negative, camera will move up and down in the opposite direction of the mouse.
        playerCharacter.transform.localRotation = Quaternion.AngleAxis(mouseDirection.x, playerCharacter.transform.up); // Local rotation of the player character -> mouse X axis movement, rotating along y-rotation-axis = side to side.


        /* Definitions:
            Quaternion.AngleAxis = Creates a rotation which rotates angle degrees around said axis.
            Vector3.right = (1,0,0) -> X-Axis
            Mathf.Lerp = lerps from position a to position b, in t amount of time. EX: Lerp(float a, float b, float t);
            Vector2 = Represents 2D positions in x and y axis.
            Var = a declared varible that inhereits it's "type" from assigned value. Only works locally.
         */
    }
}
