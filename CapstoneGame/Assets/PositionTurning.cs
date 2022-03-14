using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionTurning : MonoBehaviour
{
    public GameObject CheckBox;

    public bool turnPlus;

    public Vector3 Rotation;
    public float Speed;
    public float HowMuchRotate;
    public bool IsTurning;
  


    // Start is called before the first frame update
    void Start()
    {
        
        IsTurning = false;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 angles = transform.rotation.eulerAngles;
        Debug.Log(angles);

        if (turnPlus)
        {
            if (IsTurning)
            {
               
                transform.Rotate(Rotation * Time.deltaTime * Speed);
                if (angles.y >= HowMuchRotate) { Speed = 0; IsTurning = false; }
            }
        }
        else
        {
            if (IsTurning)
            {

                transform.Rotate(Rotation * Time.deltaTime * Speed);
                if (angles.y <= HowMuchRotate) { Speed = 0; IsTurning = false; }
            }
        }



    }

    public void TurnOn() {
        IsTurning = true;
    
    }

}
