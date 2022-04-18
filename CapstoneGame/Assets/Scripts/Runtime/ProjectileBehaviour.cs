using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adam B.
// Simple script to apply damage to player from projectiles and determine whether or not it will disappear when hitting obstacles
public class ProjectileBehaviour : MonoBehaviour
{
    [SerializeField]
    int damage = 1;

    void OnCollisionEnter(Collision collision)
    {
        // When hitting player or an enemy, do damage
        if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Enemy")
        {
            if (collision.gameObject.GetComponent<DamageKnockback>())
            {
                collision.gameObject.GetComponent<DamageKnockback>().ApplyDamage(damage);
            }
        }
        
        // Destroy the projectile
        Destroy(gameObject);
    }
}
