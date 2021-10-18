using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adam B.
// Applies damage and knockback when called from other scripts
public class DamageKnockback : MonoBehaviour
{
    LayerMask targetLayer;
    public string hostileTag;
    float invulnerableUntil;

    // Start is called before the first frame update
    void Start()
    {
        targetLayer = 1 << LayerMask.NameToLayer("Hitbox");
    }

    public void TakeDamage()
    {
        if (Time.fixedTime > invulnerableUntil)
        {
            print("Ouch++!");
            invulnerableUntil = Time.fixedTime + 1;
        }
    }

    public void TakeDamage(Vector3 sourcePosition, float knockback, float damage)
    {
        if (Time.fixedTime > invulnerableUntil)
        {
            print("Ouch++!");
            invulnerableUntil = Time.fixedTime + 1;

            // Clip knockback Y axis to object's axis
            sourcePosition = new Vector3(sourcePosition.x, transform.position.y, sourcePosition.z);

            // Apply knockback
            transform.position += (transform.position - sourcePosition).normalized * knockback;
        }
    }
}
