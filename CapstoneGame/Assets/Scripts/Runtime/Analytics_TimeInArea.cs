using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

// Adam B.
// Sends analytic events telling how long player objects spent in an area trigger
public class Analytics_TimeInArea : MonoBehaviour
{
    // Name included in the event to be sent
    [SerializeField]
    string areaName;

    // The layer that object must be on to trigger entry / exit calls
    //LayerMask playerLayer;

    // The time that player entered the 
    float timeEntered, timeExited;

    // Dictionary relevant to sending events
    Dictionary<string, object> customParams = new Dictionary<string, object>();

    // Start is called before the first frame update
    void Start()
    {
        // Parameter default values
        customParams.Add("area_name", "Unknown");
        customParams.Add("time_entered", 0);
        customParams.Add("time_inside", 0);

        // Get the layer that the player is on
        //playerLayer = LayerMask.GetMask("Player");
    }

    void OnTriggerEnter(Collider other)
    {
        // Log player's time of entry into area
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player entering area!");
            timeEntered = Time.time;
        }
    }
    
    void OnTriggerExit(Collider other)
    {
        // Log players time of exit out of area
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player exiting area!");
            //Analytics event (only triggers if player is in area for more than a second)
            if (Time.time - timeEntered > 1)
            {
                timeExited = Time.time - timeEntered;

                // Parameters for the event are filled in
                customParams["area_name"] = areaName;
                customParams["time_entered"] = timeEntered;
                customParams["time_inside"] = timeExited;

                // Event data is sent
                AnalyticsResult result = Analytics.CustomEvent(areaName, customParams);
                if (result == AnalyticsResult.Ok)
                {
                    Debug.Log("Analytics event sent for " + areaName + "!");
                }
                else
                {
                    Debug.Log("Analytics event failed for " + areaName + "!");
                }
            }
        }
    }
}
