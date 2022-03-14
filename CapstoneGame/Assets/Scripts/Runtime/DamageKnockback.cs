using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adam B.
// Applies damage and knockback when called from other scripts and manages visual health bar, if one is present
public class DamageKnockback : MonoBehaviour
{
    [Tooltip("Do not enable if death is handled by other script(s).")]
    public bool handleDeath = false;
    public bool ignoreKnockback = false;

    public float currentHealth = 1, maxHealth = 1, invulnTime = 1, deathDelay = 0;

    LayerMask targetLayer;
    float invulnerableUntil;

    [SerializeField]
    GameObject healthBar;
    public bool showHealthBar = false;
    Vector3 healthBarSize;


    public GameObject PiggyToHide;
    public GameObject PiggyToShow;


    // Start is called before the first frame update
    void Start()
    {
        // If a health bar exists, record and set initial parameters
        if (healthBar != null)
        {
            healthBarSize = healthBar.transform.localScale;
            healthBar.SetActive(false);
        }
    }

    // This is meant for applying actual damage/knockback effects (healing effects should not be done with this overload)
    public void ApplyDamage(Vector3 sourcePosition, float damage, float knockback)
    {
        if (Time.fixedTime > invulnerableUntil)
        {
            SetInvulnTime();

            // Knockback application is ignored if entity ignores knockback (for example, barriers like fallen logs are "enemies", but shouldn't be knocked back)
            if (!ignoreKnockback)
            {
                // Clip knockback Y axis to object's axis
                sourcePosition = new Vector3(sourcePosition.x, transform.position.y, sourcePosition.z);

                // Apply knockback
                transform.position += (transform.position - sourcePosition).normalized * knockback;
            }

            ChangeHPValue(damage);
        }

        DoBeforeDead();
    }

    // This overload of the function only applies damage; no knockback is applied (healing calculations can be done here, as there are supporting conditionals)
    public void ApplyDamage(float damage)
    {
        // When healing, invulnerability time isn't considered
        if (damage < 0)
        {
            ChangeHPValue(damage);
        }
        else if (Time.fixedTime > invulnerableUntil)
        {
            SetInvulnTime();

            ChangeHPValue(damage);
        }

        DoBeforeDead();
    }

    void SetInvulnTime()
    {
        // Set invulnerability time
        invulnerableUntil = Time.fixedTime + invulnTime;
    }

    void ChangeHPValue(float damage)
    {
        // Apply damage
        currentHealth -= damage;

        // Cap health at 0
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        // Cap health at max
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // Conditional update of enemy health scripts
        if (GetComponent<EnemyState>())
        {
            GetComponent<EnemyState>().HP = currentHealth;
        }

        // Adjust health bar size based on current compared to total
        if (healthBar != null)
        {
            healthBar.transform.localScale = new Vector3(healthBarSize.x / (1 / (currentHealth / maxHealth)), healthBarSize.y, healthBarSize.z);
        }

        // Show health bar if damaged or overhealed
        if (healthBar != null)
        {
            if (currentHealth != maxHealth && showHealthBar)
            {
                healthBar.SetActive(true);
            }
            else
            {
                healthBar.SetActive(false);
            }
        }
    }

    void CheckIfDead()
    {
        // If this script overrides death check
        Destroy(gameObject);
    }

    void DoBeforeDead() 
    {
        if (currentHealth <= 0)
        {
            if (PiggyToHide != null && PiggyToShow != null)
            {
                PiggyToShow.SetActive(true);
                PiggyToHide.SetActive(false);
            }

            if (handleDeath)
            {
                Invoke("CheckIfDead", deathDelay);
            }
        }
    }
}
