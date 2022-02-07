using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adam B.
// Used to kill enemies going into the out-of-bounds zone, damage player and respawn them at the last checkpoint, or handle obstructions to the zone
public class OOBZone : MonoBehaviour
{
    [SerializeField]
    float damage;

    [SerializeField]
    bool inescapable;

    // Function for out-of-bounds damage is intended to activate for only one frame, but will loop again if character is somehow still there
    void OnTriggerStay(Collider other)
    {
        // Check if entity inside zone has a health system
        if (other.GetComponent<DamageKnockback>())
        {
            // Call player to respawn if this is an inescapable out-of-bounds
            if (inescapable)
            {
                if (other.GetComponent<EnemyState>())
                {
                    other.GetComponent<DamageKnockback>().ApplyDamage(1000);
                    Debug.Log("Enemy destroyed by pitfall.");
                }

                if (other.GetComponent<CharacterController_Player>())
                {
                    other.GetComponent<CharacterController_Player>().TeleportToCheckpoint();
                }
            }

            // Damage entities with health in the zone
            other.GetComponent<DamageKnockback>().ApplyDamage(damage);
        }
        else if (other.tag != "DoNotDestroy")
        {
            // Destroy any other object so zone doesn't get obstructed by unintended entities (certain tagged objects are allowed in case we want obstructions used for puzzles)
            Destroy(other);
        }
    }
}
