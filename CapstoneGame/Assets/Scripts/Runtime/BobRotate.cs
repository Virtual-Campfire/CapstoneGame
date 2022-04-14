using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adam B.
// Applies a bob and rotate effect to an object (this should not be used with physics objects, as the updates are done based on framerate and are meant to be for visual-only elements)
public class BobRotate : MonoBehaviour
{
    Vector3 objectLoc;

    [SerializeField]
    GameObject target;

    [SerializeField]
    float bobHeight = 0.25f, spinsPerSecond = 0.5f;

    void Awake()
    {
        objectLoc = target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the target object over time
        target.transform.rotation = Quaternion.Euler(0, Time.time * 180, 0);

        // Bob the target object up and down over time
        target.transform.position = new Vector3(objectLoc.x, objectLoc.y + bobHeight * Mathf.Sin(Time.time * spinsPerSecond * 2 * Mathf.PI), objectLoc.z);
    }
}
