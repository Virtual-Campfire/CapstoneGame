using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueCheckTurn : MonoBehaviour
{

    public GameObject Tutorial;

    public Vector3 Rotation;
    public float Speed;
    public float HowMuchRotate;

    // Start is called before the first frame update
    void Start()
    {
        Tutorial = GameObject.Find("Instrument Tutorial");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 angles = transform.rotation.eulerAngles;
       

        if (Tutorial.GetComponent<TutorialSequence>().index > Tutorial.GetComponent<TutorialSequence>().tutorialPieces.Length) {
            transform.Rotate(Rotation * Time.deltaTime*Speed);
        if (angles.y <= HowMuchRotate) { Speed = 0; }
        }
    }
}
