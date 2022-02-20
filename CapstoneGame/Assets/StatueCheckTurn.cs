using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatueCheckTurn : MonoBehaviour
{

    public GameObject Player;

    public Vector3 Rotation;
    public float Speed;
    public float HowMuchRotate;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 angles = transform.rotation.eulerAngles;
        Debug.Log(angles.y);

        if (Player.GetComponent<CharacterController_Player>().instrumentStates[0] == true) {
            transform.Rotate(Rotation * Time.deltaTime*Speed);
        if (angles.y <= HowMuchRotate) { Speed = 0; }
        }
    }
}
