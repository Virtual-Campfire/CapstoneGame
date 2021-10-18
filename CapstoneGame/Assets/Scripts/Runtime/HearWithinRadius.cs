using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Adam B.
// A simple script to be called when a sound source calls its function(s); used for hearing enemies and actors that approach sound sources
public class HearWithinRadius : MonoBehaviour
{
    NavMeshAgent agent;
    public float hearingRadius;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Makes nav mesh agent try to move to the position of the sound that called this script
    public void CheckHearing(Vector3 soundPosition)
    {
        agent.SetDestination(soundPosition);
    }
}
