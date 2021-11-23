using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adam B.
// Makes object align itself with camera rotation (good for health bars)
public class FaceCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;

        // Set 90 degree rotational offset
        transform.rotation = Quaternion.Euler(transform.rotation.x + 90, transform.rotation.y, transform.rotation.z);
    }
}
