using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Adam B.
// For forklift movement and (pathing behaviour works similar to how KC's AI patrol script does, and nodes are set up the same way) 
public class ForkliftBehaviour : MonoBehaviour
{
    NavMeshAgent Agent;

    [SerializeField]
    GameObject[] Nodes;

    [SerializeField]
    int nodeIndex;
    [SerializeField]
    [Tooltip("How many units away from the destination before considering it reached. A higher value makes the forklift turn to its next destination sooner.")]
    float pathingTolerance;

    [SerializeField]
    GameObject Violin;

    [SerializeField]
    GameObject GroupToHide, GroupToShow;

    [SerializeField]
    GameObject[] DissolveMeshes;
    [SerializeField]
    Material DissolveMat;
    Material DissolveMatInst;

    float controlCalculate;

    void Awake()
    {
        for (int i = 0; i < DissolveMeshes.Length; i++)
        {
            // Create an instance of the dissolve material
            DissolveMatInst = new Material(DissolveMat);
            
            // Apply that instance to the material component so properties are not modified from the instance instead of the global material
            DissolveMeshes[i].GetComponent<Renderer>().material.CopyPropertiesFromMaterial(DissolveMatInst);
        }

        Agent = GetComponent<NavMeshAgent>();
        nodeIndex = 0;

        // Go to the first node
        GoToNextNode();
    }

    // Update is called once per frame
    void Update()
    {
        // When near the destination node, change destination to next node in the array
        if (Vector3.Distance(Agent.nextPosition, Agent.destination) < pathingTolerance)
        {
            GoToNextNode();
        }

        if (GetComponent<DamageKnockback>().currentHealth == 0)
        {
            // Hide regular mesh, show dissolve mesh
            GroupToHide.SetActive(false);
            GroupToShow.SetActive(true);

            controlCalculate = controlCalculate + Time.deltaTime;

            // Modify material instance in each mesh
            foreach (GameObject item in DissolveMeshes)
            {
                item.GetComponent<Renderer>().material.SetFloat("_Control", controlCalculate);
            }
            
            // When dissolve is done, destroy forklift
            if (controlCalculate >= 1)
            {
                controlCalculate = 0;

                // Drop violin at position before disappearing
                Instantiate(Violin, transform.position, Quaternion.identity);

                Destroy(gameObject);
            }
        }
    }

    void GoToNextNode()
    {
        Agent.SetDestination(Nodes[nodeIndex].transform.position);

        // Follow next node (staying at final destination if it is reached)
        if (nodeIndex < Nodes.Length - 1)
        {
            nodeIndex++;
        }
    }
}
