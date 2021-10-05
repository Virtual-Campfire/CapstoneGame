using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adam B.
// Script to control player character, including general movement and collision
public class CharacterController_Player : MonoBehaviour
{
    // Physics variables
    Rigidbody rb;
    float gravityFac = 9.81f;
    Ray groundRay;
    RaycastHit groundRayHit;
    public Transform groundRayBase;
    // Layer masks involved in character movement
    LayerMask floorLayer;

    Vector3 moveIntent, moveVector;
    public float moveSpeed = 1;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        floorLayer = LayerMask.NameToLayer("Floor");
    }

    // Update is called once per frame
    void Update()
    {
        // Check player input
        moveIntent = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    }

    void FixedUpdate()
    {
        // Ground ray is drawn a little bit above player character's foot
        groundRay = new Ray(groundRayBase.position + Vector3.up * 0.5f, Vector3.down);

        // Calculate the angle of the normal that the player is standing on (angle rounded to nearest significant digit; for use in later functionality)
        float groundAngle = Mathf.Round(Vector3.Angle(groundRayHit.normal, groundRay.direction));

        // Check if ground collision is below
        if (Physics.Raycast(groundRay, out groundRayHit, 1f, 1 << floorLayer, QueryTriggerInteraction.Ignore))
        {
            Debug.Log("Hit!" + ", " + groundAngle % 60 + ", " + groundAngle);
            // Move character by distance of ground collision so that they are standing on top of the ground collision point
            rb.position = new Vector3(transform.position.x, groundRayHit.point.y + 1, transform.position.z);
            Debug.DrawRay(groundRayBase.position + Vector3.up * 0.5f, groundRay.direction, Color.red);

            // Movement vector is reset
            moveVector = Vector3.zero;

            // Player's movement intent is added
            moveVector += moveIntent * moveSpeed;
            
        }
        else
        {
            // Add gravity factor when not standing on a surface
            moveVector += Vector3.down * gravityFac * Time.deltaTime;
            Debug.DrawRay(groundRayBase.position + Vector3.up * 0.5f, groundRay.direction, Color.green);


        }

        // Final movement calculation
        rb.MovePosition(rb.position + moveVector * Time.fixedDeltaTime);
    }
}
