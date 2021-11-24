using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

// Adam B.
// Script to control player character, including general movement and collision
public class CharacterController_Player : MonoBehaviour
{
    // Physics variables
    Rigidbody rb;
    CapsuleCollider capsule;
    float gravityFac = 9.81f;
    Ray groundRay;
    RaycastHit groundRayHit;
    public Transform groundRayBase;

    // Layer masks involved in character movement
    LayerMask floorLayer, wallLayer;

    // Variables for movement control factors
    Vector3 moveIntent, moveVector, rotVector, jumpVector;
    public float moveSpeed = 1, jumpPower = 2, airControl = .5f;
    [SerializeField]
    bool isGrounded = false, isJumping = false, moveLock = false, rotLock = false;

    // Health variables
    DamageKnockback health;

    // Variables for melee attack and tools (Note: melee attack works on the "hitbox" physics layer)
    public BoxCollider meleeHurtbox;
    public GameObject meleeVisualRoot;
    bool isMeleeing = false;
    float meleeStartTime;
    Quaternion meleeAngle;
    public float meleeDuration = .5f, meleeCooldown = .5f, meleeBufferTime = .5f, meleeDamage = 1, meleeBaseDamage = 1, meleeKnockback = 3, meleeBaseKnockback = 3;

    public GameObject violinRadiusIndicator;
    bool isViolinning = false;
    float violinRadius = 5;

    // Audio variables
    [SerializeField]
    StudioEventEmitter slashSpeaker, jumpSpeaker, groundedSpeaker, hitSpeaker;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        capsule = GetComponent<CapsuleCollider>();
        
        floorLayer = 1 << LayerMask.NameToLayer("Floor");
        wallLayer = 1 << LayerMask.NameToLayer("Wall");

        health = GetComponent<DamageKnockback>();

        meleeHurtbox.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Once player health is depleted, reset scene (temporary solution)
        if (health.currentHealth <= 0)
        {
            // Temporary fix to stop music duplication on level reload
            BeatScroller.level1Music.Stop();

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // Check player inputs
        moveIntent = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if(Input.GetButtonDown("Jump"))
        {
            isJumping = true;
        }
        
        // Check last melee attack and melee buffer time (so that players can queue up an attack if they click right before ending their last)
        if (Input.GetButtonDown("Fire1") && Time.fixedTime > meleeStartTime + meleeDuration + meleeCooldown - meleeBufferTime)
        {
            isMeleeing = true;
        }

        if (Input.GetButton("Fire2"))
        {
            isViolinning = true;
        }
        else
        {
            isViolinning = false;
        }
    }

    void FixedUpdate()
    {
        // Ground ray is drawn a little bit above player character's foot
        groundRay = new Ray(groundRayBase.position + Vector3.up * 0.5f, Vector3.down);
        
        // Calculate the angle of the normal that the player is standing on (angle rounded to nearest significant digit; for use in later functionality)
        float groundAngle = Mathf.Round(Vector3.Angle(groundRayHit.normal, groundRay.direction));

        // Check if ground collision is below
        // Added conditional so ground collision is only checked when moving down
        if (Physics.Raycast(groundRay, out groundRayHit, 1f, floorLayer | wallLayer, QueryTriggerInteraction.Ignore) && moveVector.y <= 0)
        {
            // Reset jump vector to avoid edge cases with landing velocity
            jumpVector = Vector3.zero;

            // Unlock rotation while grounded
            rotLock = false;

            // Set grounded flag on
            isGrounded = true;

            // Readout for angle of normal that is currently stood on
            Debug.Log("Hit!" + ", " + groundAngle % 60 + ", " + groundAngle);
            
            // Move character by distance of ground collision so that they are standing on top of the ground collision point
            rb.position = new Vector3(transform.position.x, groundRayHit.point.y + 1.05f, transform.position.z);
            
            // Draw red line where ground detection is being checked (collision has already happened)
            Debug.DrawRay(groundRayBase.position + Vector3.up * 0.5f, groundRay.direction, Color.red);

            // Movement vector is reset
            moveVector = Vector3.zero;

            // Player's movement intent is added
            moveVector += moveIntent.normalized * moveSpeed;

            // Update character's facing direction
            if (moveIntent.magnitude > 0)
            {
                rotVector = new Vector3(0, Mathf.Rad2Deg * Mathf.Atan2(moveVector.x, moveVector.z), 0);
            }

            // Rudimentary jumping (jumping, in this case, is additive onto current movement vector)
            if (isJumping)
            {
                print("Jump!");
                
                // Record velocity at moment before jumping
                jumpVector = moveVector;

                // Add jump velocity and reset jump control flag
                moveVector += transform.up * jumpPower;
                isJumping = false;

                // Play jump sound
                jumpSpeaker.Play();
            }
        }
        else
        {
            // Lock rotation while airborne
            rotLock = true;

            // Set grounded flag off
            isGrounded = false;
            
            // Set jumping variable to false to prevent complete jump buffering for after landing (partial jump buffering might be wanted later)
            isJumping = false;
            
            // Add initial velocity from before jump began
            moveVector = new Vector3(jumpVector.x, moveVector.y, jumpVector.z);

            // Player's movement intent is added, multiplied by air control factor
            moveVector += moveIntent.normalized * (moveSpeed * airControl);

            // Add gravity factor when not standing on a surface
            moveVector += Vector3.down * gravityFac * Time.deltaTime;

            // Cap falling speed at gravity factor per second
            if (moveVector.y < -gravityFac)
            {
                moveVector = new Vector3(moveVector.x, -gravityFac, moveVector.z);
            }

            // Draw red line where ground detection is being checked (collision hasn't happened yet)
            Debug.DrawRay(groundRayBase.position + Vector3.up * 0.5f, groundRay.direction, Color.green);
        }

        // Draw current rotation direction
        Debug.DrawRay(groundRayBase.position, transform.forward, Color.blue);

        // Calculate position character is trying to move to for this physics update
        Vector3 nextPosition = rb.position + moveVector * Time.fixedDeltaTime;

        // Check if damage source overlaps character
        if (meleeHurtbox.gameObject.activeSelf)
        {
            Collider[] temp = Physics.OverlapBox(meleeHurtbox.transform.position + transform.forward * meleeHurtbox.center.z, meleeHurtbox.size, meleeHurtbox.gameObject.transform.rotation, 1 << 14);
            
            foreach (Collider item in temp)
            {
                if (item.GetComponent<DamageKnockback>() && item.gameObject.tag == "Enemy")
                {
                    item.GetComponent<DamageKnockback>().ApplyDamage(rb.transform.position, meleeDamage, meleeKnockback);
                }
            }
        }

        // If not interrupted, rotate to final rotation angle
        if (!rotLock)
        {
            // Zero-out velocity just in case unexpected physics interactions occur
            rb.angularVelocity = Vector3.zero;

            // Apply final rotation calculation
            rb.MoveRotation(Quaternion.Euler(rotVector));
        }

        // If not interrupted, move to destination
        if (!moveLock)
        {
            // Zero-out velocity just in case unexpected physics interactions occur
            rb.velocity = Vector3.zero;

            // Apply final movement calculation
            rb.MovePosition(nextPosition);
        }

        // If trying to melee and previous melee attack has finished
        if (isMeleeing && Time.fixedTime > meleeStartTime + meleeDuration + meleeCooldown)
        {
            // Reset variables
            meleeStartTime = Time.fixedTime;
            
            // Get angle to derive the arc the weapon will cover when swiping
            meleeAngle = transform.rotation;

            // Reset input flag
            isMeleeing = false;
            
            // Lock character in place during attack (to allow for hover or other movement effects to take over)
            rotLock = true;

            // Make weapon swipe model/effect appear
            meleeHurtbox.gameObject.SetActive(true);

            // Play slash sound
            slashSpeaker.Play();
        }

        // If within duration of melee attack
        if (Time.fixedTime < meleeStartTime + meleeDuration)
        {
            // The angle the weapon will be rotating to for this physics update
            Quaternion tempRot = Quaternion.Euler(0, Mathf.Lerp(-60, 60, (Time.fixedTime - meleeStartTime) / meleeDuration), 0);
            
            // Rotate weapon based on time since attacking
            meleeVisualRoot.transform.rotation = (Quaternion.Euler(meleeAngle.eulerAngles + tempRot.eulerAngles));
        }
        else
        {
            // Unlock character movement
            rotLock = false;
            
            // Make weapon swipe model/effect disappear
            meleeHurtbox.gameObject.SetActive(false);

            // Reset melee attribute defaults
            meleeDamage = meleeBaseDamage;
            meleeKnockback = meleeBaseKnockback;
        }

        // Violin behaviour
        if (isViolinning)
        {
            // Check for hearing actors within violin's range
            Collider [] temp = Physics.OverlapSphere(transform.position, violinRadius, 1 << 14);

            // Call each hearing actor's hearing response function
            foreach (Collider item in temp)
            {
                if (item.gameObject.GetComponent<HearWithinRadius>())
                {
                    // Send nearby, hearing actors this player's position
                    item.gameObject.GetComponent<HearWithinRadius>().CheckHearing(rb.transform.position);
                }
            }

            // Visual effect for violin sound radius
            violinRadiusIndicator.SetActive(true);
            violinRadiusIndicator.transform.localScale = new Vector3(violinRadius * 2 - Mathf.Cos(Time.fixedTime * 5) * .25f, violinRadius * 2 - Mathf.Cos(Time.fixedTime * 5) * .25f, violinRadius * 2 - Mathf.Cos(Time.fixedTime * 5) * .25f);
        }
        else
        {
            violinRadiusIndicator.SetActive(false);
        }
    }

    void OnDrawGizmos()
    {
        // Debug effect for violin radius
        if (isViolinning)
        {
            Gizmos.DrawWireSphere(transform.position, violinRadius);
        }
    }
}
