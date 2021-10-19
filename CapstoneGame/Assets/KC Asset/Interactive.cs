using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactive : MonoBehaviour
{
    public Vector3 BigScale;
    public float IncreaseSpeed;
    bool TurnOn=false;
    public Vector3 OrginalSize;
    public bool Distory = false;

    private void Start()
    {
        OrginalSize = new Vector3(0, 0, 0);

        this.gameObject.transform.localScale = OrginalSize;
    }


    void Bigger() {

        this.gameObject.transform.localScale = new Vector3(this.gameObject.transform.localScale.x + IncreaseSpeed, this.gameObject.transform.localScale.y + IncreaseSpeed, this.gameObject.transform.localScale.z + IncreaseSpeed);

    }

    void CleanUp() {

        this.gameObject.transform.localScale = OrginalSize;


    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            
            if (this.gameObject.transform.localScale.x <= BigScale.x) {

                TurnOn = true;

               }
            
        
        }


        if (TurnOn)
        {
            Bigger();
            if (this.gameObject.transform.localScale.x > BigScale.x)
            {
                TurnOn = false;

            }

        }
        else { if (Distory) { CleanUp(); Distory = false; }  }
    }
}
