using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Adam B.
// Makes object align itself with camera rotation (good for health bars)
public class FaceCamera : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;

        // Face the camera
        transform.LookAt(Camera.main.transform);
    }
}
