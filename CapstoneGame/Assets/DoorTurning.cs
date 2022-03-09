using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTurning : MonoBehaviour
{
    public Vector3 Rotation;
    public float Speed;
    public float HowMuchRotate;

    public bool OpenTheDoor;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 angles = transform.rotation.eulerAngles;
        Debug.Log(angles);

        Debug.Log(angles);
        if (OpenTheDoor)
        {
            transform.Rotate(Rotation * Time.deltaTime * Speed);
          
        }
        if (angles.y <= HowMuchRotate) { Speed = 0; OpenTheDoor = false; }

    }
}
