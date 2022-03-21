using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adam B.
// Simple script to apply damage to player from projectiles and determine whether or not it will disappear when hitting obstacles
public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField]
    int damage = 1;
    [SerializeField]
    [Tooltip("Does the projectile pass through colliders that aren't enemies or players?")]
    bool passthrough;
    
    void OnTriggerEnter(Collider other)
    {
        // When hitting player, do damage
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<DamageKnockback>().ApplyDamage(damage);
        }

        // If projectile is not set to pass through obstacles
        if (other.gameObject.tag != "Enemy" && other.gameObject.tag != "Player" && !passthrough)
        {
            // Destroy the projectile
            Destroy(gameObject);
        }
    }
}
