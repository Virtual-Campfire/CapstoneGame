using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float Range=30;

    public Camera cam;

    public UnityEngine.AI.NavMeshAgent agent;


    public bool PlayLure;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
      


      //  if (Input.GetMouseButtonDown(0))
     //   {


     //       Ray ray = cam.ScreenPointToRay(Input.mousePosition);
     //       RaycastHit hit;

    ////        if (Physics.Raycast(ray, out hit))
    //        {
    //            agent.SetDestination(hit.point);
    //        }
    //    }



        if (Input.GetKey(KeyCode.G))
        {

            PlayLure = true;

        }
        else { PlayLure = false; }
    }



    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
