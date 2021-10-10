using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    public Vector3 BigScale;
    public float IncreaseSpeed;
    public bool TurnOn=false;


    void Bigger() {
        this.gameObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x + IncreaseSpeed, this.gameObject.transform.localScale.y + IncreaseSpeed, this.gameObject.transform.localScale.z + IncreaseSpeed);

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            
            if (this.gameObject.transform.localScale.x <= BigScale.x) {

                TurnOn = true;

               }
            
        
        }


        if (TurnOn) {
            Bigger();
            if (this.gameObject.transform.localScale.x > BigScale.x) {
                TurnOn = false;
            }

        }
    }
}
