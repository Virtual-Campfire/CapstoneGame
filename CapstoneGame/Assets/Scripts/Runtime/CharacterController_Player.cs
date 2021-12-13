using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

// Adam B.
// Script to control player character, including general movement and collision
public class CharacterController_Player : MonoBehaviour
{
    // Character animator variables
    [SerializeField]
    Animator anim;
    
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
    public float meleeDuration = .5f, meleeCooldown = .5f, meleeBufferTime = .5f, meleeDamage = 0, meleeBaseDamage = 0, meleeKnockback = 2, meleeBaseKnockback = 2;
    public bool specialMelee = false;

    // Instrument effect variables
    public GameObject resourceBar;
    Vector3 resourceBarSize;
    [SerializeField]
    float currentResource, maxResource = 1, resourceRecoveryMult = .25f;
    bool playingInstrument = false;
    float effectRadius = 5;

    // Will be used later with revised instrument / weapons system
    public bool hasAOE, hasLure;
    public bool equippedAOE = true, equippedLure;

    public GameObject effectRadiusIndicator;
    

    // Audio variables
    [SerializeField]
    StudioEventEmitter slashSpeaker, jumpSpeaker;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        capsule = GetComponent<CapsuleCollider>();
        
        floorLayer = 1 << LayerMask.NameToLayer("Floor");
        wallLayer = 1 << LayerMask.NameToLayer("Wall");

        health = GetComponent<DamageKnockback>();

        meleeHurtbox.gameObject.SetActive(false);

        currentResource = maxResource;

        resourceBarSize = resourceBar.transform.localScale;
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
            playingInstrument = true;
        }
        else
        {
            playingInstrument = false;
        }
    }

    void FixedUpdate()
    {
        // Animator sends variables
        anim.SetFloat("Speed", moveVector.magnitude);

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
                    if (specialMelee)
                    {
                        // Gain resource bar charge from hit
                        AddResource(.25f);

                        // Add to knockback
                        meleeKnockback = 5;
                    }

                    // Apply melee effects
                    item.GetComponent<DamageKnockback>().ApplyDamage(rb.transform.position, meleeDamage, meleeKnockback);
                }
            }
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
            
            // Lock character's facing direction during attack
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
            // End of melee attack, deactivate any special effects
            specialMelee = false;
            meleeDamage = meleeBaseDamage;
            meleeKnockback = meleeBaseKnockback;

            // Unlock character rotation
            rotLock = false;
            
            // Make weapon swipe model/effect disappear
            meleeHurtbox.gameObject.SetActive(false);

            // Reset melee attribute defaults
            meleeDamage = meleeBaseDamage;
            meleeKnockback = meleeBaseKnockback;
        }

        // Instrument ability only works when resources are above 0
        if (playingInstrument && currentResource > 0)
        {
            // Instrument ability type
            if (equippedAOE)
            {
                // Check for actors within effect's range
                Collider[] temp = Physics.OverlapSphere(transform.position, effectRadius, 1 << 14);

                foreach (Collider item in temp)
                {
                    if (item.tag == "Enemy" && item.gameObject.GetComponent<DamageKnockback>())
                    {
                        // Apply damage to each enemy in the radius
                        item.gameObject.GetComponent<DamageKnockback>().ApplyDamage(transform.position, 5, 1);

                        // Drain resource store
                        AddResource(-Time.fixedDeltaTime);
                    }
                }

                // Drain resource bar
                currentResource -= Time.fixedDeltaTime;
            }

            // Visual effect for instrument effect radius
            effectRadiusIndicator.SetActive(true);
            effectRadiusIndicator.transform.localScale = new Vector3(effectRadius * 2 - Mathf.Cos(Time.fixedTime * 5) * .25f, effectRadius * 2 - Mathf.Cos(Time.fixedTime * 5) * .25f, effectRadius * 2 - Mathf.Cos(Time.fixedTime * 5) * .25f);
        }
        else
        {
            effectRadiusIndicator.SetActive(false);
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

        // When not playing an instrument
        if (!playingInstrument)
        {
            // Recover resource over time
            AddResource(Time.fixedDeltaTime * resourceRecoveryMult);
        }

        // Update resource bar size
        resourceBar.transform.localScale = new Vector3(resourceBarSize.x / (1 / (currentResource / maxResource)), resourceBarSize.y, resourceBarSize.z);
    }

    void AddResource(float amount)
    {
        // Add value to resource variable
        currentResource += amount;

        // Cap resource at max limit
        if (currentResource >= maxResource)
        {
            currentResource = maxResource;
        }

        // Cap resource at min limit
        if (currentResource < 0)
        {
            currentResource = 0;
        }

        // Show resource bar if not full
        if (resourceBar != null)
        {
            if (currentResource != maxResource)
            {
                resourceBar.SetActive(true);
            }
            else
            {
                resourceBar.SetActive(false);
            }
        }
    }

    void OnDrawGizmos()
    {
        // Debug effect for violin radius
        if (playingInstrument && currentResource > 0)
        {
            Gizmos.DrawWireSphere(transform.position, effectRadius);
        }
    }
}
