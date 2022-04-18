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
    [Tooltip("Set this to true if the forklift should disappear once it reaches its final movement node.")]
    bool disappearAtEndNode;
    [SerializeField]
    [Tooltip("Set this to true if the forklift should be active at all times and only start moving once triggered externally.")]
    public bool waitForTrigger;

    [SerializeField]
    int nextNodeIndex;
    [SerializeField]
    [Tooltip("How many units away from the destination before considering it reached. A higher value makes the forklift turn to its next destination sooner.")]
    float pathingTolerance;

    [SerializeField]
    GameObject Violin;

    [SerializeField]
    GameObject GroupToHide, GroupToShow;

    [SerializeField]
    GameObject[] DissolveMeshes, LightInMeshes;
    [SerializeField]
    Material DissolveMat, LightInMat;
    Material DissolveMatInst, LightInMatInst;

    float controlCalculate;

    void Awake()
    {
        for (int i = 0; i < DissolveMeshes.Length; i++)
        {
            // Create an instance of the dissolve material
            DissolveMatInst = new Material(DissolveMat);
            // Create an instance of the light-in material
            LightInMatInst = new Material(LightInMat);

            // Apply that instance to the material component so properties are not modified from the instance instead of the global material
            DissolveMeshes[i].GetComponent<Renderer>().material.CopyPropertiesFromMaterial(DissolveMatInst);
            LightInMeshes[i].GetComponent<Renderer>().material.CopyPropertiesFromMaterial(LightInMatInst);
        }

        Agent = GetComponent<NavMeshAgent>();
        nextNodeIndex = -1;

        if (!waitForTrigger)
        {
            // Go to the first node
            GoToNextNode();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // When near the destination node, change destination to next node in the array
        if (Vector3.Distance(Agent.nextPosition, Agent.destination) < pathingTolerance && !waitForTrigger)
        {
            GoToNextNode();
        }

        // Extra component check to avoid null refs if forklift doesn't have health
        if (GetComponent<DamageKnockback>())
        {
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
                foreach (GameObject item in LightInMeshes)
                {
                    item.GetComponent<Renderer>().material.SetFloat("_Control", controlCalculate);
                }

                // When death effect is done, destroy forklift
                if (controlCalculate >= 1)
                {
                    controlCalculate = 0;

                    // Drop violin at position before disappearing
                    Instantiate(Violin, transform.position, Quaternion.identity);

                    Destroy(gameObject);
                }
            }
        }
    }

    public void GoToNextNode()
    {
        // Follow next node (staying at final destination if it is reached)
        if (nextNodeIndex < Nodes.Length)
        {
            nextNodeIndex++;
        }

        if (nextNodeIndex == Nodes.Length)
        {
            if(disappearAtEndNode)
            {
                // Once the final node has been reached, if set to disappear, remove the forklift from play
                Destroy(gameObject);
            }
        }
        else
        {
            Agent.SetDestination(Nodes[nextNodeIndex].transform.position);
        }
    }
}
