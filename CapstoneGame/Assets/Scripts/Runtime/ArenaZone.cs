using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaZone : MonoBehaviour
{
    [SerializeField]
    GameObject[] gates, Enemies;

    // Whether the arena has been activated
    bool completed = false;

    // Start is called before the first frame update
    void Start()
    {
        // Set gates lowered by default
        LowerGates();

        // Set enemies to be deactivated until player enters arena
        foreach (GameObject item in Enemies)
        {
            item.SetActive(false);
        }
    }

    void FixedUpdate()
    {
        bool enemiesExist = false;

        foreach (GameObject item in gates)
        {
            if (item != null)
            {
                enemiesExist = true;
            }
        }

        if (!enemiesExist)
        {
            // Set arena as completed and lower gates
            completed = true;
            LowerGates();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Raise gates upon first entry by player
        if (!completed && other.gameObject.tag == "Player")
        {
            // Set arena as completed and lower gates
            completed = true;
            RaiseGates();

            // Activate enemies (maybe replace this with a call for an enemy spawner later)
            foreach (GameObject item in Enemies)
            {
                item.SetActive(true);
            }
        }
    }

    void RaiseGates()
    {
        foreach (GameObject item in gates)
        {
            item.SetActive(true);
        }
    }

    void LowerGates()
    {
        foreach (GameObject item in gates)
        {
            item.SetActive(false);
        }
    }
}
