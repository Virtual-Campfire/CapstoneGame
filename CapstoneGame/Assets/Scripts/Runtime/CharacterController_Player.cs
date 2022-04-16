using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;
using UnityEngine.UI;

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
    public Vector3 moveIntent, moveVector, rotVector, jumpVector, nextPosition, pullSource;
    public float moveSpeed = 1, jumpPower = 2, airControl = .5f;
    public bool isGrounded = false, isJumping = false, moveLock = false, rotLock = false, pulled = false;

    // Health variables
    DamageKnockback health;
    float prevHealth;
    
    Color defaultColour;
    [SerializeField]
    GameObject flashingMesh, flashingMesh2;
    Material flashingMat;

    // Respawn effect
    [SerializeField]
    ParticleSystem respawnParticles;

    // Variables for melee attack and tools (Note: melee attack works on the "hitbox" physics layer)
    public BoxCollider meleeHurtbox;
    public GameObject meleeVisualRoot;
    bool isMeleeing = false;
    float meleeStartTime;
    Quaternion meleeAngle;
    public float meleeDuration = .5f, meleeCooldown = .5f, meleeBufferTime = .5f, meleeDamage = 2, meleeBaseDamage = 2, meleeKnockback = 2, meleeBaseKnockback = 2, enhancedDamage = 6, enhancedKnockback = 5;
    public bool specialMelee = false;

    // Instrument effect variables
    [SerializeField]
    public float currentResource, maxResource = 1, resourceRecoveryMult = .25f, breakExhaustAtPercent = 100;
    bool playingInstrument = false, exhausted = false;
    float lastSting;

    // UI variables
    [SerializeField]
    Health healthUI;
    [SerializeField]
    MagicBarScript magicUI;
    [SerializeField]
    GameObject rhythmMechanics, rhythmUI, magicBar, meleeUI, sirenUI;
    [SerializeField]
    Image meleeIcon, sirenIcon;
    [SerializeField]
    GameObject gameoverUI, winUI;

    Color meleeIconColor, sirenIconColour;

    // Variables used with revised instrument / weapons system
    [SerializeField]
    ParticleSystem effectRadiusParticles, slashParticles;
    public float AOEEffectRadius = 5;
    public bool playingSiren;

    enum EquipID
    {
        Siren,
        Poison,
        Roar,
        Requiem,
        AOE
    };

    public int instrumentHeld = 0, instrumentsCollected;

    // Array keeps track of what instruments the player has possession of
    public bool[] instrumentStates = new bool[4] { false, false, false, false };

    public GameObject effectRadiusIndicator;

    // Checkpoint variables
    public Vector3 lastCheckpoint;

    // Audio variables
    [SerializeField]
    StudioEventEmitter slashSpeaker, jumpSpeaker, violinSpeaker;

    FMOD.Studio.Bus MasterBus;

    void Awake()
    {
        MasterBus = RuntimeManager.GetBus("Bus:/");

        rb = GetComponent<Rigidbody>();

        capsule = GetComponent<CapsuleCollider>();

        floorLayer = 1 << LayerMask.NameToLayer("Floor");
        wallLayer = 1 << LayerMask.NameToLayer("Wall");

        health = GetComponent<DamageKnockback>();

        meleeHurtbox.gameObject.SetActive(false);

        currentResource = maxResource;

        // Set default checkpoint location
        lastCheckpoint = transform.position;

        // Set UI defaults
        healthUI.numOfHearths = (int)health.maxHealth;
        healthUI.health = (int)health.currentHealth;
        magicUI.SetMaxMagic((int)maxResource);
        meleeUI = GameObject.Find("Primary Attack");
        sirenUI = GameObject.Find("Secondary Attack");
        gameoverUI = GameObject.Find("Game Over UI");
        winUI = GameObject.Find("Win UI");
        gameoverUI.SetActive(false);
        winUI.SetActive(false);

        meleeIconColor = meleeIcon.color;
        sirenIconColour = sirenIcon.color;

        // Recount inventory upon waking up
        AddToInventory(-1);

        // Variables for flashing when damaged or healed
        flashingMat = new Material(flashingMesh.GetComponent<SkinnedMeshRenderer>().material);
        defaultColour = flashingMat.color;
        flashingMesh.GetComponent<SkinnedMeshRenderer>().material = flashingMat;
        flashingMesh2.GetComponent<SkinnedMeshRenderer>().material = flashingMat;

        // Get animator
        anim = GetComponentInChildren<Animator>();

        // Reset health change check
        prevHealth = health.currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //// Pressing escape returns to the menu (temporary)
        //if (Input.GetKeyUp(KeyCode.Escape))
        //{
        //    SceneManager.LoadScene(0);
        //}

        // Upon taking damage
        if (prevHealth > health.currentHealth)
        {
            // Colour of material flashes red
            flashingMat.color = Color.red;
        }
        else if (prevHealth < health.currentHealth)
        {
            // Colour of material flashes green
            flashingMat.color = Color.green;
        }

        // Return to original colour
        flashingMat.color = Color.Lerp(flashingMat.color, defaultColour, Time.deltaTime * 2);
        

        // Update UI elements
        healthUI.health = (int)health.currentHealth;
        magicUI.SetMagic((int)currentResource);

        // Check player inputs
        moveIntent = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //if (Input.GetButtonDown("Jump"))
        //{
        //    isJumping = true;
        //}

        // Check if melee weapon is equipped and available and last melee attack and melee buffer time (so that players can queue up an attack if they click right before ending their last)
        if (instrumentStates[instrumentHeld] && Input.GetButtonDown("Fire1") && Time.fixedTime > meleeStartTime + meleeDuration + meleeCooldown - meleeBufferTime)
        {
            // Rinvy+++---------------------------------------------------
            meleeIndex = Random.Range(0, 3);
            isMeleeing = true;
        }

        // When player presses instrument button
        if (instrumentStates[instrumentHeld] && Input.GetButton("Fire2"))
        {
            playingInstrument = true;
        }
        else
        {
            playingInstrument = false;
        }

        // Save current health
        prevHealth = health.currentHealth;
    }

    // Rinvy+++---------------------------------------------------
    int meleeIndex = 0;

    void FixedUpdate()
    {
        #region Movement
        
        // Ground ray is drawn a little bit above player character's foot
        groundRay = new Ray(groundRayBase.position + Vector3.up * 0.5f, Vector3.down);

        // Calculate the angle of the normal that the player is standing on (angle rounded to nearest significant digit; for use in later functionality)
        float groundAngle = Mathf.Round(Vector3.Angle(groundRayHit.normal, groundRay.direction));

        // Check if ground collision is below
        // Added conditional so ground collision is only checked when moving down
        if (Physics.Raycast(groundRay, out groundRayHit, 1f, floorLayer | wallLayer, QueryTriggerInteraction.Ignore) && moveVector.y <= 0)
        {
            // If player's health is depleted
            if (health.currentHealth <= 0)
            {
                // Only updates whether or not death animation is playing if on the ground
                anim.SetFloat("Health", health.currentHealth);

                StartCoroutine(DeathSequence());
            }

            // Reset jump vector to avoid edge cases with landing velocity
            jumpVector = Vector3.zero;

            // Unlock rotation while grounded
            rotLock = false;

            // Set grounded flag on
            isGrounded = true;

            // Readout for angle of normal that is currently stood on
            // Debug.Log("Hit!" + ", " + groundAngle % 60 + ", " + groundAngle);

            // Do not snap player to ground if being pulled
            if (!pulled)
            {
                // Move character by distance of ground collision so that they are standing on top of the ground collision point
                rb.position = new Vector3(transform.position.x, groundRayHit.point.y + 1.05f, transform.position.z);
            }

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
        nextPosition = rb.position + moveVector * Time.fixedDeltaTime;
        #endregion

        #region Combat

        // Check if player's melee attack overlaps with other characters
        if (meleeHurtbox.gameObject.activeSelf)
        {
            // Show slashing visual effect
            slashParticles.Play();

            Collider[] temp = Physics.OverlapBox(meleeHurtbox.transform.position + transform.forward * meleeHurtbox.center.z, meleeHurtbox.size, meleeHurtbox.gameObject.transform.rotation, 1 << 14);

            foreach (Collider item in temp)
            {
                // Added check to catch edge cases related to hitting enemies who are about to be destroyed by something else
                if (item != null)
                {
                    if (item.transform.GetComponentInChildren<DamageKnockback>() && item.gameObject.tag == "Enemy")
                    {
                        if (specialMelee)
                        {
                            // Gain resource bar charge from hit
                            AddResource(.25f);

                            // Enhance damage and knockback
                            meleeDamage = enhancedDamage;
                            meleeKnockback = enhancedKnockback;
                        }

                        // Apply melee effects
                        item.transform.GetComponentInChildren<DamageKnockback>().ApplyDamage(rb.transform.position, meleeDamage, meleeKnockback);
                    }
                }
            }

            // Secondary check for puzzle piece activation
            Collider[] temp2 = Physics.OverlapBox(meleeHurtbox.transform.position + transform.forward * meleeHurtbox.center.z, meleeHurtbox.size, meleeHurtbox.gameObject.transform.rotation);

            foreach (Collider item in temp2)
            {
                if (item != null)
                {
                    // If any of the items hit are sound stones (with their statues in place) get the script that handles the whole puzzle to check which statue will be activated by the attack
                    if (item.GetComponentInParent<SoundSequencePuzzle>() && item.GetComponentInParent<SoundSequencePuzzle>().constructed)
                    {
                        item.GetComponentInParent<SoundSequencePuzzle>().ActivateStone();
                    }
                }
            }
        }

        // If previous melee attack has finished
        if (Time.fixedTime > meleeStartTime + meleeDuration + meleeCooldown)
        {
            // If holding an instrument
            if (instrumentsCollected > 0)
            {
                // Lighten melee icon
                meleeIcon.color = meleeIconColor;
                // Reset icon size to its default
                meleeIcon.transform.localScale = new Vector3(1f, 1f, 1f);
            }

            // If trying to melee
            if (isMeleeing)
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
        }

        // If within duration of melee attack
        if (Time.fixedTime < meleeStartTime + meleeDuration)
        {
            // The angle the weapon will be rotating to for this physics update
            Quaternion tempRot = Quaternion.Euler(0, Mathf.Lerp(-60, 60, (Time.fixedTime - meleeStartTime) / meleeDuration), 0);

            // Rotate weapon based on time since attacking
            meleeVisualRoot.transform.rotation = (Quaternion.Euler(meleeAngle.eulerAngles + tempRot.eulerAngles));

            // Darken melee icon
            meleeIcon.color = new Color(.2f, .2f, .2f);
            // Shrink icon
            meleeIcon.transform.localScale = new Vector3(.8f, .8f, .8f);
        }
        else
        {
            // End of melee attack, reset melee attribute defaults
            specialMelee = false;
            meleeDamage = meleeBaseDamage;
            meleeKnockback = meleeBaseKnockback;

            // Unlock character rotation
            rotLock = false;

            // Make weapon swipe model/effect disappear
            meleeHurtbox.gameObject.SetActive(false);
        }
        
        // If magic bar has refilled to a specific pecentage after exhaustion, allow use of magic once again
        if (currentResource >= maxResource * (breakExhaustAtPercent / 100))
        {
            exhausted = false;
        }

        // Instrument ability only works when magic resources are above 0 and player has not recently exhausted magic resources
        if (playingInstrument && currentResource > 0 && !exhausted)
        {
            // AOE instrument abilities
            if (instrumentHeld == (int)EquipID.Siren || instrumentHeld == (int)EquipID.AOE)
            {
                // Siren behaviour
                if (instrumentHeld == (int)EquipID.Siren)
                {
                    // Siren flag for putting to sleep certain characters
                    playingSiren = true;

                    // Show instrument effect particles
                    effectRadiusParticles.Play();

                    if (Time.time > lastSting + 1)
                    {
                        // Play violin sounds
                        violinSpeaker.Play();

                        // Set time until next violin sting to avoid sound fatigue until a looping sound is available
                        lastSting = Time.time;
                    }

                    // Darken siren icon
                    sirenIcon.color = Color.gray;
                    // Shrink icon
                    sirenIcon.transform.localScale = new Vector3(.8f, .8f, .8f);

                    // Drain resource store
                    AddResource(-Time.deltaTime * 1.5f);
                }
            }

            // If exhausting magic resources, darken siren icon and lock magic until bar refills to max
            if (currentResource == 0)
            {
                sirenIcon.color = new Color(.1f, .1f, .1f);

                exhausted = true;
            }

            // Visual effect for instrument effect radius
            effectRadiusIndicator.SetActive(true);
            effectRadiusIndicator.transform.localScale = new Vector3(AOEEffectRadius * 2 - Mathf.Cos(Time.fixedTime * 5) * .25f, AOEEffectRadius * 2 - Mathf.Cos(Time.fixedTime * 5) * .25f, AOEEffectRadius * 2 - Mathf.Cos(Time.fixedTime * 5) * .25f);
        }
        else
        {
            // When not playing an instrument

            // Disable siren flag by default
            playingSiren = false;

            // Stop instrument effect particles
            effectRadiusParticles.Stop();

            // If not exhausted, set icon colour to default
            if (!exhausted)
            {
                // If holding an instrument
                if (instrumentsCollected > 0)
                {
                    // Lighten siren icon
                    sirenIcon.color = sirenIconColour;
                    // Reset icon size to its default
                    sirenIcon.transform.localScale = new Vector3(1f, 1f, 1f);
                }
            }

            effectRadiusIndicator.SetActive(false);
        }
        #endregion

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
        else
        {
            rb.velocity = Vector3.zero;
            moveVector = Vector3.zero;
            nextPosition = rb.position;

            // If being pulled
            if (pulled)
            {
                // Move character based on pull
                rb.position = Vector3.Lerp(rb.position, pullSource, Time.fixedDeltaTime * 2);
            }
        }

        // When not playing an instrument or exhausted magic resources
        if (!playingInstrument || exhausted)
        {
            // Recover resource over time
            AddResource(Time.deltaTime * resourceRecoveryMult);
        }

        // Animator sends variables (do not update variables if playing death animation)
        if (health.currentHealth > 0)
        {
            anim.SetFloat("Speed", moveVector.magnitude);
            // Rinvy+++---------------------------------------------------
            anim.SetBool("IsGrounded", isGrounded);
            anim.SetBool("IsMeleeing", isMeleeing);
            anim.SetInteger("AttackIndex", meleeIndex);
            //anim.SetBool("IsPlayingIns", playingInstrument);
        }
    }

    public void AddResource(float amount)
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
    }

    // Note: Add function to reevaluate the instrumentNum (number of instruments collected) when picking up an instrument here

    // Function used for swapping between instruments in a cyclic fashion
    void SwapInstrumentID(int value)
    {
        
    }

    // Used to start equipment switching actions
    void CheckEquipID()
    {
        switch (instrumentHeld)
        {
            case (int)EquipID.Siren:
                instrumentHeld = (int)EquipID.Siren;
                // Additional weapon switch processes
                break;
            case (int)EquipID.Poison:
                instrumentHeld = (int)EquipID.Poison;
                break;
            case (int)EquipID.Roar:
                instrumentHeld = (int)EquipID.Roar;
                break;
            case (int)EquipID.Requiem:
                instrumentHeld = (int)EquipID.Requiem;
                break;
        }
    }

    // Adds an instrument to the inventory (inputting -1 or other negative values only recounts the inventory)
    public void AddToInventory(int instrumentID)
    {
        if (instrumentID > -1 && instrumentStates[instrumentID] == false)
        {
            // Add instrument ID to inventory
            instrumentStates[instrumentID] = true;
        }

        // Recount instruments collected
        instrumentsCollected = 0;

        foreach (bool item in instrumentStates)
        {
            if (item)
            {
                instrumentsCollected += 1;
            }
        }

        // If not carrying any instruments, hide rhythm mechanics and show unequipped warning message on HUD
        if (instrumentsCollected == 0)
        {
            if (meleeUI && sirenUI)
            {
                // Hide UI elements for abilities
                meleeUI.SetActive(false);
                sirenUI.SetActive(false);
            }

            // Hide rhythm mechanics
            MeshRenderer[] notes1 = rhythmMechanics.GetComponentsInChildren<MeshRenderer>();

            foreach (MeshRenderer item in notes1)
            {
                item.enabled = false;
            }

            // Deactivate trigger object scripts for rhythm hits and spark effect
            TriggerObject[] notes2 = rhythmMechanics.GetComponentsInChildren<TriggerObject>();

            foreach (TriggerObject item in notes2)
            {
                item.enabled = false;
            }

            // Hide UI portion related to music
            rhythmUI.SetActive(false);
            magicBar.SetActive(false);
        }
        // If carrying instruments, show rhythm mechanics and hide unequipped warning message on HUD
        else
        {
            if (meleeUI && sirenUI)
            {
                // Show UI elements for abilities
                meleeUI.SetActive(true);
                sirenUI.SetActive(true);
            }

            // Show rhythm mechanics
            MeshRenderer[] notes1 = rhythmMechanics.GetComponentsInChildren<MeshRenderer>();

            foreach (MeshRenderer item in notes1)
            {
                item.enabled = true;
            }

            // Activate trigger object scripts for rhythm hits and spark effect
            TriggerObject[] notes2 = rhythmMechanics.GetComponentsInChildren<TriggerObject>();

            foreach (TriggerObject item in notes2)
            {
                item.enabled = true;
            }

            // Reveal UI portion related to music
            rhythmUI.SetActive(true);
            magicBar.SetActive(true);
        }
    }

    // Used to return player to a safe location after going out-of-bounds or into a damage source with behaviour requiring a position reset
    public void ReturnFromOutOfBounds()
    {
        // Variables pullSource and pulled are both changed externally by the pitfall at the moment
        moveLock = true;
        rotLock = true;
        StartCoroutine(TeleportToCheckpoint());
    }

    IEnumerator TeleportToCheckpoint()
    {
        // Wait before moving player back to a checkpoint
        yield return new WaitForSeconds(1);

        // Make player invulnerable for a brief period
        health.invulnerableUntil = Time.fixedTime + 2;

        // Teleport just above checkpoint
        rb.position = lastCheckpoint + Vector3.up * 3;

        // Set character to float down after teleporting to checkpoint
        pullSource = lastCheckpoint;

        // Play respawn particle effects
        respawnParticles.Play();

        // Wait, then unlock movement if alive
        yield return new WaitForSeconds(2);
        
        moveLock = false;
        rotLock = false;

        // Release player from pull
        pulled = false;
    }

    public IEnumerator DeathSequence()
    {
        // Prevents some edge cases if both sequences are somehow called one after the other
        StopCoroutine(WinSequence());

        moveLock = true;
        rotLock = true;

        // Add in game over UI
        if (gameoverUI != null)
        {
            gameoverUI.SetActive(true);
        }

        //Stop playing music
        MasterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        yield return new WaitForSeconds(3);

        SceneManager.LoadScene(0);
    }

    public IEnumerator WinSequence()
    {
        // Prevents some edge cases if both sequences are somehow called one after the other
        StopCoroutine(DeathSequence());

        // Add in game over UI
        if (winUI != null)
        {
            winUI.SetActive(true);
        }

        //Stop playing music
        MasterBus.stopAllEvents(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        yield return new WaitForSeconds(3);

        SceneManager.LoadScene(0);
    }

    void OnDrawGizmos()
    {
        // Debug effect for violin radius
        if (playingInstrument && currentResource > 0)
        {
            Gizmos.DrawWireSphere(transform.position, AOEEffectRadius);
        }
    }
}
