using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueCheckTurn : MonoBehaviour
{

    public GameObject Tutorial;

    public Vector3 Rotation;
    public float Speed;
    public float HowMuchRotate;
    public bool IsTurning;

    // Start is called before the first frame update
    void Start()
    {
        Tutorial = GameObject.Find("Instrument Tutorial");
        IsTurning = false;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 angles = transform.rotation.eulerAngles;
       

        if (Tutorial.GetComponent<TutorialSequence>().index > Tutorial.GetComponent<TutorialSequence>().tutorialPieces.Length) 
        {
            IsTurning = true;
            transform.Rotate(Rotation * Time.deltaTime*Speed);
        if (angles.y <= HowMuchRotate) { Speed = 0; IsTurning = false; }
        }
    }
}
