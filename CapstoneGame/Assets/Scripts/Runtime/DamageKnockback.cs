using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adam B.
// Applies damage and knockback when called from other scripts
public class DamageKnockback : MonoBehaviour
{
    [Tooltip("Do not enable if death is handled by other script(s).")]
    public bool handleDeath = false;

    public float currentHealth = 1, maxHealth = 1;

    LayerMask targetLayer;
    float invulnerableUntil;

    // Start is called before the first frame update
    void Start()
    {

    }

    // This is meant for testing without full effects
    public void TakeDamage()
    {
        if (Time.fixedTime > invulnerableUntil)
        {
            print("Ouch++!");
            invulnerableUntil = Time.fixedTime + 1;
        }
    }

    // THis is meant for applying actual damage/knockback effects
    public void ApplyDamage(Vector3 sourcePosition, float damage, float knockback)
    {
        if (Time.fixedTime > invulnerableUntil)
        {
            print("Ouch++!");
            invulnerableUntil = Time.fixedTime + 1;

            // Clip knockback Y axis to object's axis
            sourcePosition = new Vector3(sourcePosition.x, transform.position.y, sourcePosition.z);

            // Apply knockback
            transform.position += (transform.position - sourcePosition).normalized * knockback;

            // Apply damage
            currentHealth -= damage;
        }

        // If this script overrides death check
        if (handleDeath && currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}
